using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Imaging = System.Drawing.Imaging;

namespace GW2FOX
{
    public class BossListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string BossName { get; set; } = string.Empty;

        private string _countdown = string.Empty;
        public string Countdown
        {
            get => _countdown;
            set
            {
                if (_countdown != value)
                {
                    _countdown = value;
                    OnPropertyChanged(nameof(Countdown));
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

        private string _category = string.Empty;
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                    OnPropertyChanged(nameof(CategoryBrush));
                }
            }
        }

        public System.Windows.Media.Brush CategoryBrush => Category switch
        {
            "Maguuma" => System.Windows.Media.Brushes.LimeGreen,
            "WBs" => System.Windows.Media.Brushes.WhiteSmoke,
            "Ice" => System.Windows.Media.Brushes.DeepSkyBlue,
            "Cantha" => System.Windows.Media.Brushes.Blue,
            "SotO" => System.Windows.Media.Brushes.Yellow,
            "LWS2" => System.Windows.Media.Brushes.LightYellow,
            "LWS3" => System.Windows.Media.Brushes.ForestGreen,
            "Desert" => System.Windows.Media.Brushes.DeepPink,
            _ => System.Windows.Media.Brushes.White
        };


        private static BitmapImage? _waypointImage;
        public static BitmapImage WaypointImage
        {
            get
            {
                if (_waypointImage == null)
                {
                    using var bitmap = Properties.Resources.Waypoint;
                    using var ms = new MemoryStream();
                    bitmap.Save(ms, Imaging.ImageFormat.Png);
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

        public bool IsConcurrentEvent { get; set; }

        private double _opacity = 1.0;
        public double Opacity
        {
            get => _opacity;
            set
            {
                if (_opacity != value)
                {
                    _opacity = value;
                    OnPropertyChanged(nameof(Opacity));
                }
            }
        }

        public DateTime NextRunTime { get; set; }

        public DateTime TimeToShow => IsPastEvent ? NextRunTime.AddMinutes(15) : NextRunTime;

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

        public void UpdateCountdown()
        {
            var now = DateTime.Now;
            var remaining = TimeToShow - now;

            TimeRemainingFormatted = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            SecondsRemaining = (int)remaining.TotalSeconds;
            IsPastEvent = NextRunTime < now;
            Countdown = TimeRemainingFormatted;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
