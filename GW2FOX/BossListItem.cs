using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem
    {
        public static readonly ImageSource? WaypointImage;

        // Statischer Konstruktor zum Initialisieren der statischen Ressource
        static BossListItem()
        {
            try
            {
                WaypointImage = new BitmapImage(new Uri("pack://application:,,,/GW2FOX;component/Waypoint.png"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler beim Laden des Waypoint-Bildes: " + ex.Message);
                WaypointImage = null;
            }
        }

        public string BossName { get; set; }
        public string Waypoint { get; set; }         // z. B. [&BEEAAAA=]
        public string Category { get; set; }         
        public string TimeRemainingFormatted { get; set; }  // z. B. "08:13"
        public int SecondsRemaining { get; set; }    // für Sortierung oder Farben
        public DateTime NextRunTime { get; set; }    // Uhrzeit des nächsten Spawns
        public bool IsPastEvent { get; set; }        // für graue Schrift/Durchstreichung
        public bool IsDynamicEvent { get; set; }     // für kursiven Text
        public bool IsConcurrentEvent { get; set; }
        public string Countdown { get; set; } = string.Empty;
        public DateTime TimeToShow => NextRunTime;

        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Läuft";
        }
    }
}
