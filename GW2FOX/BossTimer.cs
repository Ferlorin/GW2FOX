using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GW2FOX
{
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
        }

        public void Start()
        {
            if (!_isRunning)
            {
                Timer.Start();
                _isRunning = true;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                Timer.Stop();
                _isRunning = false;
            }
        }

        private void TimerCallback(object? sender, EventArgs e)
        {
            try
            {
                var staticBosses = BossTimings.BossEventGroups
                    .SelectMany(group => group.GetAllRuns());

                var dynamicBosses = DynamicEventManager.GetActiveBossEventRuns();

                var combinedBosses = staticBosses
                    .Concat(dynamicBosses)
                    .ToList();

                combinedBosses.Sort((a, b) =>
                {
                    int timeComparison = a.NextRunTime.CompareTo(b.NextRunTime);
                    return timeComparison != 0 ? timeComparison : string.Compare(a.Category, b.Category, StringComparison.Ordinal);
                });

                var limitedBosses = combinedBosses.Take(50).ToList();

                foreach (var boss in limitedBosses)
                {
                    var timeRemaining = boss.NextRunTime - DateTime.UtcNow;
                    var formattedTime = $"{(int)timeRemaining.TotalHours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler im TimerCallback: {ex.Message}");
            }
        }

        public static void UpdateBossList()
        {
            try
            {

                var selectedBosses = BossTimings.BossList23?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new();

                var staticBosses = BossTimings.BossEventGroups
                    .Where(group => selectedBosses.Contains(group.BossName))
                    .SelectMany(group => group.GetAllRuns())
                    .ToList();


                var dynamicBosses = DynamicEventManager.GetActiveBossEventRuns().ToList();

                var combinedBosses = staticBosses
                    .Concat(dynamicBosses)
                    .ToList();

                combinedBosses.Sort((a, b) =>
                {
                    int timeComparison = a.NextRunTime.CompareTo(b.NextRunTime);
                    return timeComparison != 0 ? timeComparison : string.Compare(a.Category, b.Category, StringComparison.Ordinal);
                });

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    BossTimerService.BossListItems.Clear();
                    foreach (var boss in combinedBosses)
                    {
                        BossTimerService.BossListItems.Add(boss);
                    }

                    OverlayWindow.GetInstance().UpdateBossOverlayList();
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei UpdateBossList: {ex.Message}");
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
            Timer.Tick -= TimerCallback;
        }
    }
}
