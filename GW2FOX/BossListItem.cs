using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public class BossListItem : INotifyPropertyChanged
    {
        public string BossName { get; set; } = string.Empty;

        private string _timeRemainingFormatted = string.Empty;
        public string TimeRemainingFormatted
        {
            get => _timeRemainingFormatted;
            set
            {
                if (_timeRemainingFormatted != value)
                {
                    _timeRemainingFormatted = value;
                    OnPropertyChanged(nameof(TimeRemainingFormatted));
                }
            }
        }

        private int _secondsRemaining;
        public int SecondsRemaining
        {
            get => _secondsRemaining;
            set
            {
                if (_secondsRemaining != value)
                {
                    _secondsRemaining = value;
                    OnPropertyChanged(nameof(SecondsRemaining));
                }
            }
        }

        private string _waypoint = string.Empty;
        public string Waypoint
        {
            get => _waypoint;
            set
            {
                if (_waypoint != value)
                {
                    _waypoint = value;
                    OnPropertyChanged(nameof(Waypoint));
                }
            }
        }

        private bool _isPastEvent;
        public bool IsPastEvent
        {
            get => _isPastEvent;
            set
            {
                if (_isPastEvent != value)
                {
                    _isPastEvent = value;
                    OnPropertyChanged(nameof(IsPastEvent));
                }
            }
        }

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
