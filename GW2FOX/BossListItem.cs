using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem
    {
        private static BitmapImage? _waypointImage;
        public static BitmapImage? WaypointImage
        {
            get
            {
                if (_waypointImage == null)
                {
                    try
                    {
                        _waypointImage = new BitmapImage(
                            new Uri("pack://application:,,,/GW2FOX;component/Waypoint.png", UriKind.Absolute));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fehler beim Laden von Waypoint.png: " + ex.Message);
                    }
                }
                return _waypointImage;
            }
        }



        public string BossName { get; set; }
        public string Waypoint { get; set; }
        public string Category { get; set; }
        public string TimeRemainingFormatted { get; set; }
        public int SecondsRemaining { get; set; }
        public DateTime NextRunTime { get; set; }
        public bool IsPastEvent { get; set; }
        public bool IsDynamicEvent { get; set; }
        public bool IsConcurrentEvent { get; set; }
        public string Countdown { get; set; } = string.Empty;
        public DateTime TimeToShow => NextRunTime;

        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Runs";
        }
    }
}
