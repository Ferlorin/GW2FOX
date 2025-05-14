using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    public class DynamicEventState
    {
        public string BossName { get; set; } = "";
        public TimeSpan Duration { get; set; }
        public string Category { get; set; } = "";
        public string Waypoint { get; set; } = "";
        public DateTime? StartTime { get; set; }
        public string Level { get; set; } = "";
        public List<int> LootItemId { get; set; } = new(); // ✅ NEU
    }

    public class DynamicEvent
    {
        public string BossName { get; }
        public TimeSpan Duration { get; }
        public string Category { get; }
        public string Waypoint { get; }
        public string Level { get; }
        public DateTime? StartTime { get; private set; }
        public List<int> LootItemId { get; } = new(); // ✅ NEU

        public DynamicEvent(string bossName, TimeSpan duration, string category, string waypoint, string level = "", List<int>? lootItemId = null)
        {
            BossName = bossName;
            Duration = duration;
            Category = category;
            Waypoint = waypoint;
            Level = level;
            if (lootItemId != null)
                LootItemId = lootItemId;
        }

        public void Trigger()
        {
            StartTime = DateTime.UtcNow;
        }

        public DateTime? EndTime => StartTime?.Add(Duration);
        public bool IsRunning => StartTime.HasValue && DateTime.UtcNow < EndTime;

        public BossEventRun ToBossEventRun()
        {
            return new BossEventRun(
    bossName: BossName,
    timing: Duration,
    category: Category,
    nextRunTime: EndTime.Value.ToLocalTime(),
    waypoint: Waypoint,
    level: Level // ✅ falls vorhanden
);

        }

        public DynamicEventState ToState() => new()
        {
            BossName = BossName,
            Duration = Duration,
            Category = Category,
            Waypoint = Waypoint,
            StartTime = StartTime,
            Level = Level,
            LootItemId = LootItemId // ✅ NEU
        };

        public static DynamicEvent FromState(DynamicEventState state)
        {
            var ev = new DynamicEvent(state.BossName, state.Duration, state.Category, state.Waypoint, state.Level, state.LootItemId);
            if (state.StartTime.HasValue)
                ev.StartTime = state.StartTime;
            return ev;
        }

    }
    internal static class DynamicEventManager
    {
        public static List<DynamicEvent> Events { get; private set; } = new();

        static DynamicEventManager()
        {
            LoadPersistedEvents();
        }

        public static void TriggerIt(string bossName)
        {
            // 1. Holen oder Erstellen der Event-Instanz
            var dynamicEvent = Events.FirstOrDefault(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase));

            if (dynamicEvent == null)
            {
                // Bossdaten aus Konfigurationsdatei laden
                var bossData = BossTimings.LoadBossConfigFromFile("BossTimings.json")
                    .Bosses.FirstOrDefault(b => b.Name.Equals(bossName, StringComparison.OrdinalIgnoreCase));

                string level = bossData?.Level ?? "";
                string category = bossData?.Category ?? "Unknown";
                string waypoint = bossData?.Waypoint ?? "[&UNKNOWN=]";
                List<int> lootItemId = bossData?.LootItemId?.ToList() ?? new List<int>(); // ✅ LootItemId berücksichtigen

                dynamicEvent = new DynamicEvent(bossName, TimeSpan.FromMinutes(15), category, waypoint, level, lootItemId);
                Events.Add(dynamicEvent);
            }

            // 2. Event auslösen
            dynamicEvent.Trigger();

            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            var json = File.ReadAllText(configPath);
            var jObj = JObject.Parse(json);
            var choosen = jObj["ChoosenOnes"] as JArray ?? new JArray();
            var existing = choosen.FirstOrDefault(x => x.ToString().Equals(bossName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                choosen.Remove(existing);
            }
            else
            {
                choosen.Add(bossName);
            }

            jObj["ChoosenOnes"] = choosen;
            File.WriteAllText(configPath, jObj.ToString());

            if (existing != null)
            {
                Events.RemoveAll(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase));
            }
        }


        private static bool _choosenOnesLoaded = false;

        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            DateTime nowLocal = GlobalVariables.CURRENT_DATE_TIME;
            DateTime nowUtc = DateTime.UtcNow;
            DateTime minUtc = nowUtc - TimeSpan.FromMinutes(15);
            DateTime minLocal = nowLocal - TimeSpan.FromMinutes(15);

            // 1) Snapshot aller DynamicEvents
            List<DynamicEvent> eventSnapshot;
            lock (Events)  // optional, falls andere Threads Events hinzufügen/entfernen
            {
                eventSnapshot = Events.ToList();
            }

            // 2) Laufende DynamicEvents zu BossEventRuns konvertieren & filtern
            var dynamicRuns = eventSnapshot
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun())
                .Where(run => run.NextRunTime.ToUniversalTime() >= minUtc)
                .ToList();

            // 3) Snapshot aller statischen Gruppen
            var groupSnapshot = BossTimings.BossEventGroups.ToList();

            // 4) ChoosenOnes aus Config holen
            var config = BossTimings.LoadBossConfigFromFile("BossTimings.json");
            var choosenNames = config.ChoosenOnes
                                      .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 5) Runs aus den gewählten Gruppen
            var chosenRuns = groupSnapshot
                .Where(g => choosenNames.Contains(g.BossName))
                .SelectMany(g => g.GetNextRuns())
                .Where(run => run.NextRunTime >= minLocal)
                .ToList();

            // 6) Fallback: wenn nichts gewählt, alle Gruppen
            if (!dynamicRuns.Any() && !chosenRuns.Any())
            {
                chosenRuns = groupSnapshot
                    .SelectMany(g => g.GetNextRuns())
                    .Where(run => run.NextRunTime >= minLocal)
                    .ToList();
            }

            // 7) Ergebnisse zusammenführen, Duplikate aussortieren und sortieren
            var result = dynamicRuns
                .Concat(chosenRuns)
                .GroupBy(r => new { r.BossName, r.NextRunTime })
                .Select(g => g.First())
                .OrderBy(r => r.NextRunTime)
                .ToList();

            return result;
        }


        public static void LoadPersistedEvents()
        {
            try
            {
                var config = BossTimings.LoadBossConfigFromFile("BossTimings.json");

                Events = config.ChoosenOnes
                    .Select(name => config.Bosses.FirstOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    .Where(b => b != null)
                    .Select(b => new DynamicEvent(
                        b.Name,
                        TimeSpan.FromMinutes(15),
                        b.Category,
                        b.Waypoint,
                        b.Level,
                        b.LootItemId?.ToList() ?? new List<int>()
                    ))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadFromChoosenOnes] Error: {ex.Message}");
                Events = new List<DynamicEvent>();
            }
        }

    }
}
