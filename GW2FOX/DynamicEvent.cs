using System;
using static GW2FOX.BossTimings;

namespace GW2FOX
{
    internal class DynamicEvent
    {
        public string BossName { get; }
        public TimeSpan Delay { get; }
        public string Category { get; }
        public string Waypoint { get; }
        private DateTime? startTime;

        public DynamicEvent(string bossName, TimeSpan delay, string category, string waypoint)
        {
            BossName = bossName;
            Delay = delay;
            Category = category;
            Waypoint = waypoint;
        }

        public void Trigger() => startTime = DateTime.Now;

        public bool IsRunning => startTime.HasValue
                                 && DateTime.Now < startTime.Value + Delay;

        public BossEventRun ToBossEventRun()
        {
            if (!startTime.HasValue)
                throw new InvalidOperationException("Event not triggered");

            DateTime nextRun = startTime.Value + Delay;
            return new BossEventRun(
                bossName: BossName,
                timing: Delay,              // TimeSpan
                category: Category,
                nextRunTime: nextRun,       // DateTime
                waypoint: Waypoint
            );
        }
    }
}
