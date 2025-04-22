using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

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
            new DynamicEvent("The Eye of Zhaitan", TimeSpan.FromMinutes(20), "Treasures", "[&BPgCAAA=]"), // correct all 20min from point i click
            new DynamicEvent("Gates of Arah",        TimeSpan.FromMinutes(90), "Treasures", "[&BA8DAAA=]"), //???
            new DynamicEvent("Branded Generals",     TimeSpan.FromMinutes(90), "Treasures", "[&BIMLAAA=]"), //???
            new DynamicEvent("Dredge Commissar",     TimeSpan.FromMinutes(20), "Treasures", "[&BFYCAAA=]"), // correct same style like Eye
            new DynamicEvent("Captain Rotbeard",     TimeSpan.FromMinutes(35), "Treasures", "[&BOQGAAA=]"), // correct same style like Eye
            new DynamicEvent("Rhendak",              TimeSpan.FromMinutes(10), "Treasures", "[&BNwAAAA=]"), // correct same style like Eye
            new DynamicEvent("Ogrewars",             TimeSpan.FromMinutes(20), "Treasures", "[&BDwEAAA=]"), // correct same style like Eye
            new DynamicEvent("Statue of Dwanya",     TimeSpan.FromMinutes(50), "Treasures", "[&BLICAAA=]"), // correct same style like Eye
            new DynamicEvent("Priestess of Lyssa",   TimeSpan.FromMinutes(90), "Treasures", "[&BKsCAAA=]"), //???
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
            {
                Console.WriteLine($"[Trigger] No dynamic event found for {bossName}");
                return;
            }

            ev.Trigger();
            SavePersistedState();
            Console.WriteLine($"[Trigger] Triggered dynamic event: {bossName} at {DateTime.UtcNow}");
        }


        /// <summary>
        /// Get all currently running events as BossEventRuns.
        /// </summary>
        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            var running = Events
                .Where(e => e.IsRunning)
                .Select(e => e.BossName)
                .ToList();

            Console.WriteLine("Running dynamic events: " + string.Join(", ", running));

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
                    .Select(e => new PersistEntry { BossName = e.BossName, UtcStartTime = e.StartTime.Value.ToUniversalTime() })
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


