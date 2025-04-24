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
        public DateTime? StartTime { get; set; } // UTC
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
        public DateTime? StartTime { get; private set; }

        public DynamicEvent(string bossName, TimeSpan duration, string category, string waypoint)
        {
            BossName = bossName;
            Duration = duration;
            Category = category;
            Waypoint = waypoint;
        }

        /// <summary>
        /// Event wurde ausgelöst – Startzeit wird gesetzt.
        /// </summary>
        public void Trigger()
        {
            StartTime = DateTime.UtcNow;
        }


        /// <summary>
        /// Endzeit = Startzeit + Dauer (falls getriggert).
        /// </summary>
        public DateTime? EndTime => StartTime?.Add(Duration);

        /// <summary>
        /// Ob das Event aktuell aktiv ist (d.h. nach Trigger & innerhalb der geschätzten Dauer).
        /// </summary>
        public bool IsRunning => StartTime.HasValue && DateTime.UtcNow < EndTime;

        /// <summary>
        /// Wandelt in ein darstellbares Objekt für das Overlay um.
        /// </summary>
        public BossEventRun ToBossEventRun()
        {
            return new BossEventRun(
                bossName: BossName,
                timing: Duration,
                category: Category,
                nextRunTime: EndTime.Value.ToLocalTime(), // ✅ NUR HIER lokale Zeit berechnen!
                waypoint: Waypoint
            );
        }


        public DynamicEventState ToState() => new()
        {
            BossName = BossName,
            Duration = Duration,
            Category = Category,
            Waypoint = Waypoint,
            StartTime = StartTime
        };

        public static DynamicEvent FromState(DynamicEventState state)
        {
            var ev = new DynamicEvent(state.BossName, state.Duration, state.Category, state.Waypoint);
            if (state.StartTime.HasValue)
                ev.StartTime = state.StartTime;
            return ev;
        }
    }

    /// <summary>
    /// Zentrale Verwaltung aller dynamischen Events (inkl. Laden/Speichern).
    /// </summary>
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
            // Zuerst die Event-Instanz für das Triggern erstellen
            var dynamicEvent = Events.FirstOrDefault(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase));

            if (dynamicEvent == null)
            {
                // Falls das Event noch nicht existiert, erstelle es
                dynamicEvent = new DynamicEvent(bossName, TimeSpan.FromMinutes(30), "Unknown", "[&UNKNOWN=]");
                Events.Add(dynamicEvent);
            }

            // Event auslösen
            dynamicEvent.Trigger();

            // In BossTimings.json -> ChoosenOnes eintragen
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            var json = File.ReadAllText(configPath);
            var jObj = JObject.Parse(json);

            var choosen = jObj["ChoosenOnes"] as JArray ?? new JArray();
            if (!choosen.Any(x => x.ToString().Equals(bossName, StringComparison.OrdinalIgnoreCase)))
            {
                choosen.Add(bossName);
                jObj["ChoosenOnes"] = choosen;
                File.WriteAllText(configPath, jObj.ToString());
            }

            // Auch die aktuelle Event-Liste speichern
            SavePersistedEvents();
        }





        /// <summary>
        /// Gibt nur aktive dynamische Events für das Overlay zurück.
        /// </summary>
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
                if (!File.Exists(PersistFile))
                {
                    LoadDefaultEvents();
                    return;
                }

                var json = File.ReadAllText(PersistFile);
                var states = JsonConvert.DeserializeObject<List<DynamicEventState>>(json) ?? new();

                DateTime now = DateTime.UtcNow;
                var allEvents = states.Select(DynamicEvent.FromState).ToList();

                Events = allEvents
                    .Where(e => !e.StartTime.HasValue || e.EndTime >= now - TimeSpan.FromMinutes(15))
                    .ToList();

                if (Events.Count != allEvents.Count)
                {
                    SavePersistedEvents();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Load] Failed to load dynamic events: {ex.Message}");
                LoadDefaultEvents();
            }
        }

        public static void SavePersistedEvents()
        {
            try
            {
                var states = Events.Select(e => e.ToState()).ToList();
                var json = JsonConvert.SerializeObject(states, Formatting.Indented);
                File.WriteAllText(PersistFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Save] Failed to persist dynamic events: {ex.Message}");
            }
        }

        /// <summary>
        /// Optional: Dynamische Events initial befüllen (z. B. für Test oder Defaults).
        /// </summary>
        private static void LoadDefaultEvents()
        {
            Events = new List<DynamicEvent>
            {
                new("The Eye of Zhaitan", TimeSpan.FromMinutes(20), "Treasures", "[&BPgCAAA=]"),
                new("Gates of Arah", TimeSpan.FromMinutes(90), "Treasures", "[&BA8DAAA=]"),
                new("Branded Generals", TimeSpan.FromMinutes(90), "Treasures", "[&BIMLAAA=]"),
                new("Dredge Commissar", TimeSpan.FromMinutes(20), "Treasures", "[&BFYCAAA=]"),
                new("Captain Rotbeard", TimeSpan.FromMinutes(35), "Treasures", "[&BOQGAAA=]"),
                new("Rhendak", TimeSpan.FromMinutes(10), "Treasures", "[&BNwAAAA=]"),
                new("Ogrewars", TimeSpan.FromMinutes(20), "Treasures", "[&BDwEAAA=]"),
                new("Statue of Dwanya", TimeSpan.FromMinutes(50), "Treasures", "[&BLICAAA=]"),
                new("Priestess of Lyssa", TimeSpan.FromMinutes(90), "Treasures", "[&BKsCAAA=]")
            };
        }
    }
}
