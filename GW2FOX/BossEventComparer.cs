using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2FOX
{
    public class BossEventComparer : IEqualityComparer<BossEvent>
    {
        public bool Equals(BossEvent? x, BossEvent? y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.BossName == y.BossName && x.Timing == y.Timing;
        }

        public int GetHashCode(BossEvent? obj)
        {
            if (obj == null)
                return 0;

            var hashBossName = obj.BossName.GetHashCode();
            var hashTiming = obj.Timing.GetHashCode();

            return hashBossName ^ hashTiming;
        }
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

        public DateTime TimeToShow =>
            IsPreviousBoss ? NextRunTimeEnding : NextRunTime;

        public DateTime NextRunTimeEnding => NextRunTime.AddMinutes(14).AddSeconds(59);

        public TimeSpan TimeRemaining =>
            !IsPreviousBoss
                ? TimeToShow - GlobalVariables.CURRENT_DATE_TIME
                : GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow;

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
}
