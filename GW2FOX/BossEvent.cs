using System;

namespace GW2FOX
{
    public class BossEvent
    {
        public string BossName { get; }
        public string Waypoint { get; }
        public TimeSpan Timing { get; }
        public string Category { get; }

        public BossEvent(string bossName, TimeSpan timing, string category, string waypoint = "")
        {
            BossName = bossName;
            Timing = GlobalVariables.IsDaylightSavingTimeActive()
                ? timing.Add(TimeSpan.FromHours(1))
                : timing;
            Category = category;
            Waypoint = waypoint;
        }

        public BossEvent(string bossName, string timing, string category, string waypoint = "")
            : this(bossName, TimeSpan.Parse(timing), category, waypoint) { }
    }
}
