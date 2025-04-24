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
                    Console.WriteLine("Versuche Waypoint.png zu laden...");

                    try
                    {
                        // URI zur eingebetteten Resource
                        var uri = new Uri("pack://application:,,,/GW2FOX;component/Waypoint.png", UriKind.Absolute);
                        Console.WriteLine("Verwende URI: " + uri);

                        // Optional: Test, ob die Resource überhaupt eingebettet ist
                        var info = System.Windows.Application.GetResourceStream(uri);
                        if (info == null)
                        {
                            Console.WriteLine("ResourceStream: Waypoint.png wurde NICHT gefunden.");
                        }
                        else
                        {
                            Console.WriteLine("ResourceStream: Waypoint.png wurde erfolgreich gefunden.");
                        }

                        // Bild laden
                        _waypointImage = new BitmapImage(uri);
                        Console.WriteLine("Waypoint.png erfolgreich als BitmapImage geladen.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fehler beim Laden von Waypoint.png: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Waypoint.png wurde bereits geladen (aus Cache).");
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
