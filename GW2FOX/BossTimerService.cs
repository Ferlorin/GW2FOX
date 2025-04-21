using System.Collections.ObjectModel;
using System.Reflection;
using static GW2FOX.BossTimings;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace GW2FOX
{
    public static class BossTimerService
    {
        private static readonly string TimeZoneId = "W. Europe Standard Time";

        public static OverlayWindow? _overlayWindow { get; set; }
        public static BossTimer? _bossTimer { get; private set; } // Punkt 3: Singleton artiger Zugriff
        public static System.Windows.Controls.ListView CustomBossList { get; set; } = new();

        // Punkt 4: set privat machen
        public static ObservableCollection<BossEventRun> BossListItems { get; private set; } = new();

        static BossTimerService()
        {
            Initialize();
        }

        public static void Initialize()
        {
            try
            {
                InitializeCustomBossList();
                if (_overlayWindow == null)
                {
                    InitializeBossTimerAndOverlay();
                }
                Console.WriteLine("Initialization complete.");
            }
            catch (Exception ex)
            {
                LogError("Initialize", ex);
            }
        }

        public static void InitializeCustomBossList()
        {
            try
            {
                CustomBossList = new System.Windows.Controls.ListView
                {
                    ItemTemplate = (DataTemplate)System.Windows.Application.Current.Resources["BossListTemplate"]
                };
                Console.WriteLine("CustomBossList initialized.");
            }
            catch (Exception ex)
            {
                LogError("InitializeCustomBossList", ex);
            }
        }

        private static void InitializeBossTimerAndOverlay()
        {
            try
            {
                _bossTimer ??= new BossTimer(CustomBossList);

                _overlayWindow = OverlayWindow.GetInstance();

                if (_overlayWindow == null)
                {
                    Console.WriteLine("OverlayWindow.GetInstance() returned null!");
                    return;
                }

                // Punkt 2: Closed-Handler nicht mehrfach registrieren
                _overlayWindow.Closed -= OverlayWindow_Closed;
                _overlayWindow.Closed += OverlayWindow_Closed;

                if (!_overlayWindow.IsVisible)
                {
                    _overlayWindow.Show();
                    Console.WriteLine("Overlay window initialized.");
                }
            }
            catch (Exception ex)
            {
                LogError("InitializeBossTimerAndOverlay", ex);
            }
        }


        private static void OverlayWindow_Closed(object? sender, EventArgs e)
        {
            _overlayWindow = null;
        }

        public static void Update()
        {
            try
            {
                if (_bossTimer == null)
                {
                    Initialize();
                }

                _bossTimer?.Start();
                GC.KeepAlive(_bossTimer);

                if (_overlayWindow != null && !_overlayWindow.IsVisible)
                {
                    _overlayWindow.Show();
                    Console.WriteLine("Overlay window shown.");
                }
            }
            catch (Exception ex)
            {
                LogError("Update", ex);
            }
        }

        public static void Timer_Click(object? sender, EventArgs e)
        {
            try
            {
                _bossTimer?.UpdateBossList();
            }
            catch (Exception ex)
            {
                LogError("Timer_Click", ex);
            }
        }

        public class BossTimer : IDisposable
        {
            public DispatcherTimer Timer { get; } = new DispatcherTimer();
            private bool _isRunning = false;
            public bool IsRunning => _isRunning;

            private readonly System.Windows.Controls.ListView _bossList;

            public BossTimer(System.Windows.Controls.ListView bossList)
            {
                this._bossList = bossList;
                Timer.Interval = TimeSpan.FromSeconds(1);
                Timer.Tick += TimerCallback;
                Console.WriteLine("BossTimer initialized.");
            }

            public void Start()
            {
                if (!_isRunning)
                {
                    Console.WriteLine("Starting Timer");
                    Timer.Start();
                    _isRunning = true;
                }
            }

            public void Stop()
            {
                if (_isRunning)
                {
                    Console.WriteLine("Stopping Timer");
                    Timer.Stop();
                    _isRunning = false;
                }
            }

            private void TimerCallback(object? sender, EventArgs e)
            {
                try
                {
                    Console.WriteLine("Timer Tick: " + DateTime.Now);
                    UpdateBossList();

                    // Punkt 1: Nur hier PropertyChanged triggern
                    foreach (var boss in BossListItems)
                    {
                        boss.TriggerTimeRemainingChanged();
                    }
                }
                catch (Exception ex)
                {
                    LogError("TimerCallback", ex);
                }
            }

            public void UpdateBossList()
            {
                try
                {
                    var allBosses = BossEventGroups
                        .SelectMany(group => group.GetNextRuns())
                        .ToList();

                    allBosses.Sort((a, b) =>
                    {
                        int timeComparison = a.NextRunTime.CompareTo(b.NextRunTime);
                        return timeComparison != 0 ? timeComparison : string.Compare(a.Category, b.Category, StringComparison.Ordinal);
                    });

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        BossListItems.Clear();
                        foreach (var boss in allBosses)
                        {
                            BossListItems.Add(boss);
                           
                        }
                        _overlayWindow?.RefreshBossList();
                    });

                    Console.WriteLine("Boss list updated.");
                }
                catch (Exception ex)
                {
                    LogError("UpdateBossList", ex);
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposing) return;
                Timer.Stop();
                Console.WriteLine("Disposed.");
            }
        }

        private static void LogError(string method, Exception ex)
        {
            Console.WriteLine($"Error in {method}: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

    public class BossEvent
    {
        public string BossName { get; }
        public string Waypoint { get; }
        public TimeSpan Timing { get; }
        public string Category { get; }


        public BossEvent(string bossName, TimeSpan timing, string category, string waypoint = "")
        {
            BossName = bossName;
            Waypoint = waypoint;
            Timing = GlobalVariables.IsDaylightSavingTimeActive() ? timing.Add(new TimeSpan(1, 0, 0)) : timing;
            Category = category;
        }

        public BossEvent(string bossName, string timing, string category, string waypoint = "")
            : this(bossName, TimeSpan.Parse(timing), category, waypoint) { }
    }

    public class BossEventRun : BossEvent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime NextRunTime { get; set; }

        public bool IsPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;

        public BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime, string waypoint = "")
            : base(bossName, timing, category, waypoint)
        {
            NextRunTime = nextRunTime;
        }

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Neue Methode für externes PropertyChanged
        public void TriggerTimeRemainingChanged() =>
            OnPropertyChanged(nameof(TimeRemainingFormatted));

        public DateTime TimeToShow => IsPreviousBoss ? NextRunTimeEnding : NextRunTime;
        public DateTime NextRunTimeEnding => NextRunTime.AddMinutes(14).AddSeconds(59);

        public TimeSpan TimeRemaining =>
            !IsPreviousBoss
                ? TimeToShow - GlobalVariables.CURRENT_DATE_TIME
                : GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow;

        public string TimeRemainingFormatted =>
            $"{(int)TimeRemaining.TotalHours:D2}:{TimeRemaining.Minutes:D2}:{TimeRemaining.Seconds:D2}";



        public System.Drawing.Color ForeColor =>
            Category switch
            {
                "Maguuma" => System.Drawing.Color.LimeGreen,
                "Desert" => System.Drawing.Color.DeepPink,
                "WBs" => System.Drawing.Color.WhiteSmoke,
                "Ice" => System.Drawing.Color.DeepSkyBlue,
                "Cantha" => System.Drawing.Color.Blue,
                "SotO" => System.Drawing.Color.Yellow,
                "LWS2" => System.Drawing.Color.LightYellow,
                "LWS3" => System.Drawing.Color.ForestGreen,
                _ => System.Drawing.Color.White
            };
    }

    public class BossEventGroup
    {
        public string BossName { get; }
        private readonly List<BossEvent> _timings;
        private const int NextRunsToShow = 2;
        private const int DaysExtraToCalculate = 1;

        public BossEventGroup(string bossName, IEnumerable<BossEvent> events)
        {
            BossName = bossName;
            _timings = events
                .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                .OrderBy(span => span.Timing)
                .ToList();
        }

        public IEnumerable<BossEventRun> GetNextRuns()
        {
            List<BossEventRun> toReturn = new List<BossEventRun>();

            for (var i = -1; i <= DaysExtraToCalculate; i++)
            {
                toReturn.AddRange(
                    _timings
                        .Select(bossEvent => new BossEventRun(
                            bossEvent.BossName,
                            bossEvent.Timing,
                            bossEvent.Category,
                            GlobalVariables.CURRENT_DATE_TIME.Date
                                .Add(new TimeSpan(0, i * 24, 0, 0, 0)) + bossEvent.Timing,
                            bossEvent.Waypoint))
                        .ToList()
                );
            }

            return toReturn
                .Where(bossEvent =>
                    bossEvent.TimeToShow >= GlobalVariables.CURRENT_DATE_TIME &&
                    bossEvent.TimeToShow <= GlobalVariables.CURRENT_DATE_TIME.AddHours(3))
                .OrderBy(bossEvent => bossEvent.TimeToShow)
                .Take(NextRunsToShow)
                .ToList();
        }
    }
    public class BossListItem
    {
        public string BossName { get; set; } = string.Empty;
        public string TimeRemainingFormatted { get; set; } = string.Empty;
        public string Waypoint { get; set; } = string.Empty;

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

