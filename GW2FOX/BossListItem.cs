using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem
    {
        public string BossName { get; set; }

        public string Waypoint { get; set; }         // <-- erforderlich für Button-Click oder Anzeige
        public string Category { get; set; }         // z. B. "Meta", "Worldboss", etc.
        public string TimeRemainingFormatted { get; set; }  // z. B. "08:13"
        public int SecondsRemaining { get; set; }    // für Sortierung oder Farben
        public DateTime NextRunTime { get; set; }    // Uhrzeit des nächsten Spawns
        public bool IsPastEvent { get; set; }        // für graue Schrift/Durchstreichung
        public bool IsDynamicEvent { get; set; }     // für kursiven Text
        public bool IsConcurrentEvent { get; set; }
        public static readonly ImageSource WaypointImage = new BitmapImage(new Uri("pack://application:,,,/GW2FOX;component/Resources/Waypoint.png"));
    }

}
