using System;
using System.Collections.Generic;
using System.Linq;

namespace GW2FOX
{
    public class BossEventGroup
    {
        public string BossName { get; }
        private readonly List<BossEvent> _timings;
        private const int DaysExtraToCalculate = 1;

        public BossEventGroup(string bossName, IEnumerable<BossEvent> bossEvents)
        {
            BossName = bossName;
            _timings = bossEvents
                .Where(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.Timing)
                .ToList();
        }

        public IEnumerable<BossEventRun> GetNextRuns()
        {
            List<BossEventRun> result = new();

            for (int i = -1; i <= DaysExtraToCalculate; i++)
            {
                result.AddRange(_timings.Select(e => new BossEventRun(
                    e.BossName,
                    e.Timing,
                    e.Category,
                    GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                    e.Waypoint
                )));
            }

            return result
                .Where(run =>
                    run.TimeToShow >= GlobalVariables.CURRENT_DATE_TIME &&
                    run.TimeToShow <= GlobalVariables.CURRENT_DATE_TIME.AddHours(5))
                .OrderBy(run => run.TimeToShow);
        }

        public IEnumerable<BossEventRun> GetAllRuns()
        {
            List<BossEventRun> result = new();

            for (int i = -1; i <= DaysExtraToCalculate; i++)
            {
                result.AddRange(_timings.Select(e => new BossEventRun(
                    e.BossName,
                    e.Timing,
                    e.Category,
                    GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                    e.Waypoint
                )));
            }

            return result.OrderBy(run => run.TimeToShow);
        }
    }
}
