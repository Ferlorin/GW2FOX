using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    /// <summary>
    /// Zustand eines dynamischen Events – wird als JSON gespeichert.
    /// </summary>
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




    /// <summary>
    /// Ein dynamisches Event – beginnt erst nach Trigger().
    /// </summary>
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
        private const string PersistFile = "dynamic_events.json";

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

            // 3. Entfernen oder Hinzufügen des Bosses aus/in "ChoosenOnes"
            var choosen = jObj["ChoosenOnes"] as JArray ?? new JArray();
            var existing = choosen.FirstOrDefault(x => x.ToString().Equals(bossName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                // Boss wurde abgewählt → aus ChoosenOnes entfernen
                choosen.Remove(existing);
            }
            else
            {
                // Boss wurde ausgewählt → zu ChoosenOnes hinzufügen
                choosen.Add(bossName);
            }

            jObj["ChoosenOnes"] = choosen;
            File.WriteAllText(configPath, jObj.ToString());

            // 4. Entfernen des Events, wenn Boss abgewählt wurde
            if (existing != null)
            {
                Events.RemoveAll(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase));
            }
        }


        private static bool _choosenOnesLoaded = false;

        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            // 🕒 Zeitdefinitionen
            DateTime nowLocal = GlobalVariables.CURRENT_DATE_TIME;
            DateTime nowUtc = DateTime.UtcNow;
            DateTime minUtc = nowUtc - TimeSpan.FromMinutes(15);
            DateTime minLocal = nowLocal - TimeSpan.FromMinutes(15);

            var result = new List<BossEventRun>();

            // 1️⃣ Dynamische Events (nur wenn aktiv und im sichtbaren Zeitfenster)
            var dynamicEvents = Events
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun())
                .Where(e => e.NextRunTime.ToUniversalTime() >= minUtc)
                .ToList();

            result.AddRange(dynamicEvents);

            // 2️⃣ ChoosenOnes aus JSON-Konfiguration
            var config = BossTimings.LoadBossConfigFromFile("BossTimings.json");
            var choosenBossNames = config.ChoosenOnes
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var choosenRuns = BossTimings.BossEventGroups
                .Where(g => choosenBossNames.Contains(g.BossName))
                .SelectMany(g => g.GetNextRuns())
                .Where(r => r.NextRunTime >= minLocal)
                .ToList();

            result.AddRange(choosenRuns);

            // 3️⃣ Fallback: Alle Bosse, falls keine Ergebnisse vorhanden
            if (result.Count == 0)
            {
                result = BossTimings.BossEventGroups
                    .SelectMany(g => g.GetNextRuns())
                    .Where(r => r.NextRunTime >= minLocal)
                    .ToList();
            }

            // 4️⃣ Sortierung und Duplikat-Entfernung
            return result
                .GroupBy(r => new { r.BossName, r.NextRunTime })
                .Select(g => g.First())
                .OrderBy(r => r.NextRunTime)
                .ToList();
        }




        /// <summary>
        /// Lädt Events aus JSON-Datei, entfernt dabei abgelaufene (15+ Minuten vergangen).
        /// </summary>
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

        /// <summary>
        /// Optional: Dynamische Events initial befüllen (z. B. für Test oder Defaults).
        /// </summary>
        private static void LoadDefaultEvents()
        {
            const string configPath = "BossTimings.json";

            if (!File.Exists(configPath))
            {
                Console.WriteLine("[LoadDefaultEvents] BossTimings.json not found.");
                Events = new List<DynamicEvent>();
                return;
            }

            try
            {
                var json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);
                var bossesArray = jObj["Bosses"] as JArray;

                if (bossesArray == null)
                {
                    Console.WriteLine("[LoadDefaultEvents] No 'Bosses' array found in BossTimings.json.");
                    Events = new List<DynamicEvent>();
                    return;
                }

                Events = bossesArray
                    .Select(b =>
                    {
                        string name = b["Name"]?.ToString() ?? "Unknown";
                        string category = b["Category"]?.ToString() ?? "Unknown";
                        string waypoint = b["Waypoint"]?.ToString() ?? "[&UNKNOWN=]";
                        string level = b["Level"]?.ToString() ?? "";

                        // Default duration if none provided
                        TimeSpan defaultDuration = TimeSpan.FromMinutes(15);

                        return new DynamicEvent(name, defaultDuration, category, waypoint, level);
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadDefaultEvents] Failed to parse BossTimings.json: {ex.Message}");
                Events = new List<DynamicEvent>();
            }
        }

    }
}
