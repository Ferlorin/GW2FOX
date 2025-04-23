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
    
       public System.Windows.Media.Brush CategoryBrush =>
 Category switch
 {
     "Maguuma" => System.Windows.Media.Brushes.LimeGreen,
     "Desert" => System.Windows.Media.Brushes.DeepPink,
     "WBs" => System.Windows.Media.Brushes.WhiteSmoke,
     "Ice" => System.Windows.Media.Brushes.DeepSkyBlue,
     "Cantha" => System.Windows.Media.Brushes.Blue,
     "SotO" => System.Windows.Media.Brushes.Yellow,
     "LWS2" => System.Windows.Media.Brushes.LightYellow,
     "LWS3" => System.Windows.Media.Brushes.ForestGreen,
     _ => System.Windows.Media.Brushes.White
 };
    }
}
