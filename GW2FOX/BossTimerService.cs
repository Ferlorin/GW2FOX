using System.Collections.ObjectModel;
using System.Reflection;
using static GW2FOX.BossTimings;
using System.Windows.Controls; // WPF ListView
using System.Windows.Threading;
using System.Windows;

namespace GW2FOX
{
    public static class BossTimerService
    {
        private static readonly string TimeZoneId = "W. Europe Standard Time";

        public static OverlayWindow? _overlayWindow { get; set; }
        public static BossTimer? _bossTimer { get; set; }
        public static System.Windows.Controls.ListView CustomBossList { get; set; } = new(); // WPF ListView
        public static ObservableCollection<BossEventDisplay> BossListItems { get; set; } = new();

        static BossTimerService()
        {
            Initialize();
        }

        public static void UpdateCustomBossList(System.Windows.Controls.ListView updatedList) // WPF ListView
        {
            CustomBossList = updatedList;
            CustomBossList.ItemsSource = BossListItems;
        }

        private static void InitializeBossTimerAndOverlay()
        {
            _bossTimer = new BossTimer(CustomBossList);

            if (_overlayWindow == null)
            {
                _overlayWindow = new OverlayWindow();
                _overlayWindow.Closed += (s, e) => _overlayWindow = null;
                _overlayWindow.Show();
            }
        }

        private static void InitializeCustomBossList()
        {
            CustomBossList = new System.Windows.Controls.ListView // WPF ListView
            {
                // Set a basic template for the ListView to display BossName, TimeRemaining, and Category.
                ItemTemplate = (DataTemplate)System.Windows.Application.Current.Resources["BossListTemplate"]
            };
        }

        public static void Timer_Click(object sender, EventArgs e)
        {
            Update();
        }

        private static void Initialize()
        {
            InitializeCustomBossList();
            if (_overlayWindow == null)
            {
                InitializeBossTimerAndOverlay();
            }
        }

        public static void Update()
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
            }
        }

        public class BossTimer : IDisposable
        {
            public DispatcherTimer Timer { get; } = new DispatcherTimer();
            private bool _isRunning = false;
            public bool IsRunning => _isRunning;

            private readonly System.Windows.Controls.ListView _bossList; // WPF ListView

            public BossTimer(System.Windows.Controls.ListView bossList) // WPF ListView
            {
                this._bossList = bossList;
                Timer.Interval = TimeSpan.FromSeconds(1);
                Timer.Tick += TimerCallback;
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
                Console.WriteLine("Timer Tick: " + DateTime.Now);
                try
                {
                    UpdateBossList();
                }
                catch (Exception ex)
                {
                    HandleException(ex, "TimerCallback");
                }
            }

            public void UpdateBossList()
            {
                var allBosses = BossEventGroups
                    .SelectMany(bossEventGroup => bossEventGroup.GetNextRuns())
                    .ToList();

                // Sortieren nach nächster Laufzeit und Kategorie
                allBosses.Sort((bossEvent1, bossEvent2) =>
                {
                    int timeComparison = bossEvent1.NextRunTime.CompareTo(bossEvent2.NextRunTime);
                    if (timeComparison != 0) return timeComparison;
                    return string.Compare(bossEvent1.Category, bossEvent2.Category, StringComparison.Ordinal);
                });

                // Aktualisiere die ObservableCollection
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    BossListItems.Clear();
                    foreach (var bossEvent in allBosses)
                    {
                        BossListItems.Add(new BossEventDisplay
                        {
                            BossName = bossEvent.BossName,
                            TimeRemaining = bossEvent.getTimeRemainingFormatted(),
                            Category = bossEvent.Category
                        });
                    }
                });
            }

            private void HandleException(Exception ex, string methodName)
            {
                Console.WriteLine($"Exception in {methodName}: {ex}");
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                Timer.Stop();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposing) return;
                Timer.Stop();
            }
        }

        public class BossEventDisplay
        {
            public string BossName { get; set; }
            public string TimeRemaining { get; set; }
            public string Category { get; set; }
        }
    }
}
