using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

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
            Waypoint = waypoint;
            Timing = GlobalVariables.IsDaylightSavingTimeActive() ? timing.Add(new TimeSpan(1, 0, 0)) : timing;
            Category = category;
        }

        public BossEvent(string bossName, string timing, string category, string waypoint = "")
            : this(bossName, TimeSpan.Parse(timing), category, waypoint) { }
    }

    public class BossEventRun : BossEvent
    {
        private static readonly Color DefaultFontColor = Color.White;
        private static readonly Color PastBossFontColor = Color.OrangeRed;

        public DateTime NextRunTime { get; set; }

        public bool IsPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;

        public BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime, string waypoint = "")
            : base(bossName, timing, category, waypoint)
        {
            NextRunTime = nextRunTime;
        }

        public DateTime TimeToShow => IsPreviousBoss ? NextRunTimeEnding : NextRunTime;

        public DateTime NextRunTimeEnding => NextRunTime.AddMinutes(14).AddSeconds(59);

        public TimeSpan TimeRemaining
        {
            get
            {
                var remainingTime = !IsPreviousBoss
                    ? TimeToShow - GlobalVariables.CURRENT_DATE_TIME
                    : GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow;

                Console.WriteLine($"Boss: {BossName}, TimeToShow: {TimeToShow}, CurrentTime: {GlobalVariables.CURRENT_DATE_TIME}, RemainingTime: {remainingTime}");
                return remainingTime;
            }
        }

        public string TimeRemainingFormatted =>
            $"{(int)TimeRemaining.TotalHours:D2}:{TimeRemaining.Minutes:D2}:{TimeRemaining.Seconds:D2}";

        public Color ForeColor =>
            IsPreviousBoss ? PastBossFontColor : Category switch
            {
                "Maguuma" => Color.LimeGreen,
                "Desert" => Color.DeepPink,
                "WBs" => Color.WhiteSmoke,
                "Ice" => Color.DeepSkyBlue,
                "Cantha" => Color.Blue,
                "SotO" => Color.Yellow,
                "LWS2" => Color.LightYellow,
                "LWS3" => Color.ForestGreen,
                _ => DefaultFontColor
            };
    }

    public class BossEventGroup
    {
        public string BossName { get; }
        private readonly List<BossEvent> _timings;
        private const int NextRunsToShow = 2;
        private const int DaysExtraToCalculate = 1;

        public BossEventGroup(string bossName, IEnumerable<BossEvent> events)
        {
            BossName = bossName;
            _timings = events
                .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                .OrderBy(span => span.Timing)
                .ToList();
        }

        public IEnumerable<BossEventRun> GetNextRuns()
        {
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

            return toReturn
                .Where(bossEvent =>
                    bossEvent.TimeToShow >= GlobalVariables.CURRENT_DATE_TIME &&
                    bossEvent.TimeToShow <= GlobalVariables.CURRENT_DATE_TIME.AddHours(3))
                .OrderBy(bossEvent => bossEvent.TimeToShow)
                .Take(NextRunsToShow)
                .ToList();
        }
    }
}
