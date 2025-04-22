using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GW2FOX
{
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

        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            return Events
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun());
        }

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
                var states = JsonConvert.DeserializeObject<List<DynamicEventState>>(json);

                Events = states?.Select(DynamicEvent.FromState).ToList() ?? new List<DynamicEvent>();
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

        private static void LoadDefaultEvents()
        {
            Events = new List<DynamicEvent>
            {
                new("The Eye of Zhaitan", TimeSpan.FromMinutes(20), "Treasures", "[&BPgCAAA=]"),
                new("Gates of Arah",        TimeSpan.FromMinutes(90), "Treasures", "[&BA8DAAA=]"),
                new("Branded Generals",     TimeSpan.FromMinutes(90), "Treasures", "[&BIMLAAA=]"),
                new("Dredge Commissar",     TimeSpan.FromMinutes(20), "Treasures", "[&BFYCAAA=]"),
                new("Captain Rotbeard",     TimeSpan.FromMinutes(35), "Treasures", "[&BOQGAAA=]"),
                new("Rhendak",              TimeSpan.FromMinutes(10), "Treasures", "[&BNwAAAA=]"),
                new("Ogrewars",             TimeSpan.FromMinutes(20), "Treasures", "[&BDwEAAA=]"),
                new("Statue of Dwanya",     TimeSpan.FromMinutes(50), "Treasures", "[&BLICAAA=]"),
                new("Priestess of Lyssa",   TimeSpan.FromMinutes(90), "Treasures", "[&BKsCAAA=]")
            };
        }
    }
}
