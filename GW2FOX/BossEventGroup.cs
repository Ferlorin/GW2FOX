using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2FOX
{
    public class BossEventGroup
    {
        public string BossName { get; }
        public TimeSpan Duration { get; set; }
        private readonly List<BossEvent> _timings;
        private const int NextRunsToShow = 2;
        private const int DaysExtraToCalculate = 1;

        public BossEventGroup(string bossName, IEnumerable<BossEvent> events)
        {
            try
            {
                Console.WriteLine($"Creating BossEventGroup for: {bossName}");

                BossName = bossName;
                _timings = events
                    .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                    .OrderBy(span => span.Timing)
                    .ToList();

                Console.WriteLine($"BossEventGroup created for {BossName} with {_timings.Count} events.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BossEventGroup constructor: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public IEnumerable<BossEventRun> GetNextRuns()
        {
            try
            {
                Console.WriteLine($"Getting next runs for {BossName}");

                List<BossEventRun> toReturn = new List<BossEventRun>();

                for (var i = -1; i <= DaysExtraToCalculate; i++)
                {
                    toReturn.AddRange(
                        _timings
                            .Select(bossEvent => new BossEventRun(
                                bossEvent.BossName,
                                bossEvent.Timing,
                                bossEvent.Category,
                                GlobalVariables.CURRENT_DATE_TIME.Date
                                    .Add(new TimeSpan(0, i * 24, 0, 0, 0)) + bossEvent.Timing,
                                bossEvent.Waypoint))
                            .ToList()
                    );
                }

                var result = toReturn
                    .Where(bossEvent =>
                        bossEvent.TimeToShow >= GlobalVariables.CURRENT_DATE_TIME &&
                        bossEvent.TimeToShow <= GlobalVariables.CURRENT_DATE_TIME.AddHours(3))
                    .OrderBy(bossEvent => bossEvent.TimeToShow)
                    .Take(NextRunsToShow)
                    .ToList();

                Console.WriteLine($"{result.Count} next runs found for {BossName}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNextRuns for {BossName}: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<BossEventRun>();
            }
        }

        public IEnumerable<BossEventRun> GetPreviousRuns()
        {
            try
            {
                Console.WriteLine($"Getting previous runs for {BossName}");

                var result = _timings
                    .Where(bossEvent =>
                        bossEvent.Timing > GlobalVariables.CURRENT_TIME.Subtract(new TimeSpan(0, 15, 0)) &&
                        bossEvent.Timing < GlobalVariables.CURRENT_TIME)
                    .Select(bossEvent => new BossEventRun(
                        bossEvent.BossName,
                        bossEvent.Timing,
                        bossEvent.Category,
                        GlobalVariables.CURRENT_DATE_TIME.Date + bossEvent.Timing,
                        bossEvent.Waypoint))
                    .ToList();

                Console.WriteLine($"{result.Count} previous runs found for {BossName}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPreviousRuns for {BossName}: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Enumerable.Empty<BossEventRun>();
            }
        }
    }


    public class BossEvent(string bossName, TimeSpan timing, string category, string waypoint = "")
    {
        public string BossName { get; } = bossName;
        public string Waypoint { get; } = waypoint;
        public TimeSpan Timing { get; } = GlobalVariables.IsDaylightSavingTimeActive() ? timing.Add(new TimeSpan(1, 0, 0)) : timing;
        public string Category { get; } = category;


        public BossEvent(string bossName, string timing, string category, string waypoint = "") : this(bossName,
            TimeSpan.Parse(timing),
            category, waypoint)
        {
        }
    }

    
}
