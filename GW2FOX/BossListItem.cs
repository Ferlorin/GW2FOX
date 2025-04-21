using System.IO;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem
    {
        public string BossName { get; set; } = string.Empty;
        public string TimeRemainingFormatted { get; set; } = string.Empty;
        public string Waypoint { get; set; } = string.Empty;
        public bool IsPastEvent { get; set; }

        private static BitmapImage? _waypointImage;

        public static BitmapImage WaypointImage
        {
            get
            {
                if (_waypointImage == null)
                {
                    using var bitmap = Properties.Resources.Waypoint;
                    using var ms = new MemoryStream();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    image.Freeze();

                    _waypointImage = image;
                }

                return _waypointImage;
            }
        }
    }
}
