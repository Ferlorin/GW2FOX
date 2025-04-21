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
        private DateTime _nextRunTime;
        public DateTime NextRunTime
        {
            get => _nextRunTime;
            set
            {
                if (_nextRunTime != value)
                {
                    _nextRunTime = value;
                    OnPropertyChanged(nameof(NextRunTime));
                    UpdateCountdown(); // ← wichtig, damit sich alles bei Änderung aktualisiert!
                }
            }
        }

        public void UpdateCountdown()
        {
            var now = DateTime.UtcNow;
            var nextEndTime = NextRunTime.AddMinutes(14).AddSeconds(59);
            var timeToShow = NextRunTime < now ? nextEndTime : NextRunTime;

            var remaining = timeToShow - now;

            // Update Formatted String
            TimeRemainingFormatted = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            SecondsRemaining = (int)remaining.TotalSeconds;
            IsPastEvent = NextRunTime < now;
            Console.WriteLine($"[{BossName}] Updated countdown: {TimeRemainingFormatted}");
            Console.WriteLine($"[{BossName}] now: {now}, NextRunTime: {NextRunTime}, TimeToShow: {timeToShow}");

        }

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsConcurrentEvent { get; set; } = false;

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
                    OnPropertyChanged(nameof(CategoryBrush)); // Brush auch updaten
                }
            }
        }


        public System.Windows.Media.Brush CategoryBrush =>
   Category switch
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

    }
}
