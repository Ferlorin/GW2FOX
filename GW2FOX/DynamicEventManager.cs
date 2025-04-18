using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static GW2FOX.BossTimings;

namespace GW2FOX
{
    internal static class DynamicEventManager
    {
        private const string PersistFile = "dynamicEvents.json";

        /// <summary>
        /// Statically defined dynamic events.
        /// </summary>
        public static List<DynamicEvent> Events { get; } = new List<DynamicEvent>
        {
            new DynamicEvent("The Eye of Zhaitan", TimeSpan.FromMinutes(20), "Treasures", "[&BPgCAAA=]"),
            new DynamicEvent("Gates of Arah",        TimeSpan.FromMinutes(90), "Treasures", "[&BA8DAAA=]"),
            new DynamicEvent("Branded Generals",     TimeSpan.FromMinutes(90), "Treasures", "[&BIMLAAA=]"),
            new DynamicEvent("Dredge Commissar",     TimeSpan.FromMinutes(20), "Treasures", "[&BFYCAAA=]"),
            new DynamicEvent("Captain Rotbeard",     TimeSpan.FromMinutes(35), "Treasures", "[&BOQGAAA=]"),
            new DynamicEvent("Rhendak",              TimeSpan.FromMinutes(10), "Treasures", "[&BNwAAAA=]"),
            new DynamicEvent("Ogrewars",             TimeSpan.FromMinutes(20), "Treasures", "[&BDwEAAA=]"),
            new DynamicEvent("Statue of Dwanya",     TimeSpan.FromMinutes(50), "Treasures", "[&BLICAAA=]"),
            new DynamicEvent("Priestess of Lyssa",   TimeSpan.FromMinutes(90), "Treasures", "[&BKsCAAA=]"),
            new DynamicEvent("FireShaman",           TimeSpan.FromMinutes(75), "Treasures", "[&BKsCAAA=]")
        };

        static DynamicEventManager()
        {
            LoadPersistedState();
        }

        /// <summary>
        /// Trigger the event and persist state.
        /// </summary>
        public static void Trigger(string bossName)
        {
            var ev = Events.FirstOrDefault(e => e.BossName == bossName);
            if (ev == null)
                return;

            ev.Trigger();
            SavePersistedState();
        }

        /// <summary>
        /// Get all currently running events as BossEventRuns.
        /// </summary>
        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            return Events
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun());
        }

        private static void LoadPersistedState()
        {
            try
            {
                if (!File.Exists(PersistFile))
                    return;

                var json = File.ReadAllText(PersistFile);
                var saved = JsonSerializer.Deserialize<List<PersistEntry>>(json);
                if (saved == null) return;

                foreach (var entry in saved)
                {
                    var ev = Events.FirstOrDefault(e => e.BossName == entry.BossName);
                    if (ev != null)
                        ev.SetStartTime(entry.UtcStartTime);
                }
            }
            catch
            {
                // ignore errors or log
            }
        }

        private static void SavePersistedState()
        {
            try
            {
                var entries = Events
                    .Where(e => e.StartTime.HasValue)
                    .Select(e => new PersistEntry { BossName = e.BossName, UtcStartTime = e.StartTime.Value })
                    .ToList();

                var json = JsonSerializer.Serialize(entries);
                File.WriteAllText(PersistFile, json);
            }
            catch
            {
                // ignore errors or log
            }
        }

        private class PersistEntry
        {
            public string BossName { get; set; } = string.Empty;
            public DateTime UtcStartTime { get; set; }
        }
    }
}


