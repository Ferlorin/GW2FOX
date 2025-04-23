using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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
            Console.WriteLine($"[Trigger] {BossName} triggered at {StartTime.Value:HH:mm:ss} UTC");
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
            if (!StartTime.HasValue)
                throw new InvalidOperationException("Event not triggered");

            return new BossEventRun(
                bossName: BossName,
                timing: Duration,
                category: Category,
                nextRunTime: EndTime.Value,
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

        public static void Trigger(string bossName)
        {
            var ev = Events.FirstOrDefault(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase));
            if (ev == null)
            {
                Console.WriteLine($"[Trigger] No dynamic event found for {bossName}");
                return;
            }

            ev.Trigger();
            SavePersistedEvents();
        }

        /// <summary>
        /// Gibt nur aktive dynamische Events für das Overlay zurück.
        /// </summary>
        private static bool _choosenOnesLoaded = false;

        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            var result = new List<BossEventRun>();

            // 1. Dynamische Events
            var dynamicEvents = Events
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun())
                .ToList();

            if (dynamicEvents.Count > 0)
                result.AddRange(dynamicEvents);

            // 2. ChoosenOnes laden (nur einmal pro Anwendungslauf)
            if (!_choosenOnesLoaded)
            {
                BossTimings.ApplyBossGroupFromConfig("ChoosenOnes");
                _choosenOnesLoaded = true;
            }

            var choosen = BossTimings.BossEventsList
                .Where(b => BossTimings.BossList23.Contains(b.BossName, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (choosen.Count > 0)
            {
                var chosenRuns = choosen.Select(b => new BossEventRun(
                    bossName: b.BossName,
                    timing: b.Timing,
                    category: b.Category,
                    nextRunTime: GlobalVariables.CURRENT_DATE_TIME + TimeSpan.FromMinutes(1),
                    waypoint: b.Waypoint
                ));
                result.AddRange(chosenRuns);
            }

            // 3. Wenn wirklich nichts vorhanden ist, dann ALLE
            if (result.Count == 0)
            {
                result = BossTimings.BossEventsList
                    .Select(b => new BossEventRun(
                        bossName: b.BossName,
                        timing: b.Timing,
                        category: b.Category,
                        nextRunTime: GlobalVariables.CURRENT_DATE_TIME + TimeSpan.FromMinutes(1),
                        waypoint: b.Waypoint
                    ))
                    .ToList();
            }

            return result.OrderBy(r => r.TimeToShow);
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
                    Console.WriteLine("[Cleanup] Removed expired dynamic events (15+ min past EndTime).");
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
