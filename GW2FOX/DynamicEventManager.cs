using System;
using System.Collections.Generic;
using System.Linq;
using static GW2FOX.BossTimings;

namespace GW2FOX
{
    internal static class DynamicEventManager
    {
        /// <summary>
        /// Statically defined dynamic events with their respective delay and waypoint.
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

        /// <summary>
        /// Starts the countdown for the specified dynamic event.
        /// </summary>
        public static void Trigger(string bossName)
        {
            var ev = Events.FirstOrDefault(e => e.BossName == bossName);
            if (ev == null)
                return;

            ev.Trigger();
        }

        /// <summary>
        /// Returns all currently running dynamic events as BossEventRun instances.
        /// </summary>
        public static IEnumerable<BossEventRun> GetActiveBossEventRuns()
        {
            return Events
                .Where(e => e.IsRunning)
                .Select(e => e.ToBossEventRun());
        }

        // Hilfsmethode, wenn du je anders bauen möchtest
        private static string GetWaypoint(string bossName)
        {
            return bossName switch
            {
                "The Eye of Zhaitan" => "[&BPgCAAA=]",
                "Gates of Arah" => "[&BA8DAAA=]",
                "Branded Generals" => "[&BIMLAAA=]",
                "Dredge Commissar" => "[&BFYCAAA=]",
                "Captain Rotbeard" => "[&BOQGAAA=]",
                "Rhendak" => "[&BNwAAAA=]",
                "Ogrewars" => "[&BDwEAAA=]",
                "Statue of Dwanya" => "[&BLICAAA=]",
                "Priestess of Lyssa" => "[&BKsCAAA=]",
                "FireShaman" => "[&BKsCAAA=]",
                _ => string.Empty
            };
        }
    }
}

