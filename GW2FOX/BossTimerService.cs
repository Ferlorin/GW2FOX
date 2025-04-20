using System.Collections.ObjectModel;
using System.Reflection;
using static GW2FOX.BossTimings;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;

namespace GW2FOX
{
    public static class BossTimerService
    {
        private static readonly string TimeZoneId = "W. Europe Standard Time";

        public static OverlayWindow? _overlayWindow { get; set; }
        public static BossTimer? _bossTimer { get; set; }
        public static System.Windows.Controls.ListView CustomBossList { get; set; } = new();
        public static ObservableCollection<BossEventDisplay> BossListItems { get; set; } = new();

        static BossTimerService()
        {
            Initialize();
        }

        public static List<BossEventRun> GetBossEvents()
        {
            // Beispiel: Boss-Events aus der internen Liste abrufen
            return BossEventGroups
                .SelectMany(group => group.GetNextRuns())
                .ToList();
        }

        public static void UpdateCustomBossList(System.Windows.Controls.ListView updatedList)
        {
            try
            {
                CustomBossList = updatedList;
                CustomBossList.ItemsSource = BossListItems;
                Console.WriteLine("CustomBossList updated successfully.");
            }
            catch (Exception ex)
            {
                LogError("UpdateCustomBossList", ex);
            }
        }

        private static void InitializeBossTimerAndOverlay()
        {
            try
            {
                _bossTimer = new BossTimer(CustomBossList);

                // Verwende die GetInstance-Methode, um die Singleton-Instanz zu erhalten
                _overlayWindow = OverlayWindow.GetInstance();

                if (!_overlayWindow.IsVisible)
                {
                    _overlayWindow.Closed += (s, e) => _overlayWindow = null;
                    _overlayWindow.Show();
                    Console.WriteLine("Overlay window initialized.");
                }
            }
            catch (Exception ex)
            {
                LogError("InitializeBossTimerAndOverlay", ex);
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

        public static void Timer_Click(object sender, EventArgs e)
        {
            try
            {
                Update();
            }
            catch (Exception ex)
            {
                LogError("Timer_Click", ex);
            }
        }

        private static void Initialize()
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
                Console.WriteLine("BossTimer initialized.");
            }

            public void Start()
            {
                try
                {
                    if (!_isRunning)
                    {
                        Console.WriteLine("Starting Timer");
                        Timer.Start();
                        _isRunning = true;
                    }
                }
                catch (Exception ex)
                {
                    LogError("Start", ex);
                }
            }

            public void Stop()
            {
                try
                {
                    if (_isRunning)
                    {
                        Console.WriteLine("Stopping Timer");
                        Timer.Stop();
                        _isRunning = false;
                    }
                }
                catch (Exception ex)
                {
                    LogError("Stop", ex);
                }
            }

            private void TimerCallback(object? sender, EventArgs e)
            {
                try
                {
                    Console.WriteLine("Timer Tick: " + DateTime.Now);
                    UpdateBossList();
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
                                TimeRemaining = bossEvent.TimeRemainingFormatted,
                                Category = bossEvent.Category
                            });
                        }
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
                Timer.Stop();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposing) return;
                Timer.Stop();
                Console.WriteLine("Disposed.");
            }
        }

        public class BossEventDisplay
        {
            public string BossName { get; set; }
            public string TimeRemaining { get; set; }
            public string Category { get; set; }
        }

        private static void LogError(string methodName, Exception ex)
        {
            Console.WriteLine($"Error in {methodName}: {ex.Message}");
            // Du kannst auch die Exception-Details loggen, wenn nötig:
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}
