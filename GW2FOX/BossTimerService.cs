using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GW2FOX
{
    public static class BossTimerService
    {
        public static Worldbosses? WorldbossesInstance { get; set; }
        public static OverlayWindow? _overlayWindow;
        public static BossTimer? _bossTimer;
        public static ObservableCollection<BossEventRun> BossListItems { get; private set; } = new();

        public static ObservableCollection<BossListItem> GetBossOverlayItems(IEnumerable<BossEventRun> bossRuns, DateTime _)
        {
            var overlayItems = new ObservableCollection<BossListItem>();
            var now = DateTime.Now;

            var items = bossRuns
                .Select(run =>
                {
                    var eventTime = run.NextRunTime;
                    var timeRemaining = eventTime - now;
                    bool isPast = timeRemaining.TotalSeconds < 0;

                    if (isPast && timeRemaining.TotalMinutes <= -15)
                        return null;
                    if (!isPast && timeRemaining.TotalHours > 8)
                        return null;

                    var remaining = isPast ? -timeRemaining : timeRemaining;
                    string formatted = isPast
                        ? $"-{remaining.Minutes:D2}:{remaining.Seconds:D2}"
                        : $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";

                    var item = new BossListItem
                    {
                        BossName = run.BossName,
                        Waypoint = run.Waypoint,
                        Category = run.Category,
                        Level = run.Level,
                        IsPastEvent = isPast,
                        TimeRemainingFormatted = formatted,
                        SecondsRemaining = (int)(isPast ? -remaining.TotalSeconds : remaining.TotalSeconds),
                        NextRunTime = eventTime,
                        ChestOpened = BossTimings.IsChestOpened(run.BossName)
                    };

                    return item;
                })
                .Where(item => item != null)
                .ToList();

            var past = items
                .Where(x => x.IsPastEvent)
                .OrderByDescending(x => x.SecondsRemaining);

            var future = items
                .Where(x => !x.IsPastEvent)
                .OrderBy(x => x.NextRunTime)
                .ToList();

            for (int i = 0; i < future.Count; i++)
            {
                var current = future[i];
                current.IsConcurrentEvent = future.Any(other =>
                    other != current &&
                    Math.Abs((other.NextRunTime - current.NextRunTime).TotalSeconds) < 899);
            }

            foreach (var item in past.Concat(future))
                overlayItems.Add(item);

            return overlayItems;
        }

        public static List<BossEventRun> GetBossRunsForOverlay()
        {
            var selectedBosses = BossTimings.BossList23 ?? new List<string>();

            var staticBosses = BossTimings.BossEventGroups
                .SelectMany(group => group.GetNextRuns())
                .Where(run => selectedBosses.Contains(run.BossName));

            var dynamicBosses = DynamicEventManager.GetActiveBossEventRuns();

            return staticBosses.Concat(dynamicBosses)
                               .OrderBy(run => run.TimeToShow)
                               .ToList();
        }

        public static void Timer_Click(object? sender, EventArgs e)
        {
            try
            {
                BossTimer.UpdateBossList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei Timer_Click: {ex.Message}");
            }
        }
    }

    public class BossListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _countdown = "";
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

        public string ChestImagePath => ChestOpened ? "/Resources/OpenChest.png" : "/Resources/Black_Lion_Chest.png";

        public string BossName { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Waypoint { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

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

        public DateTime NextRunTime { get; set; }

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

        public bool IsDynamicEvent { get; set; }

        private bool _isConcurrentEvent;
        public bool IsConcurrentEvent
        {
            get => _isConcurrentEvent;
            set
            {
                if (_isConcurrentEvent != value)
                {
                    _isConcurrentEvent = value;
                    OnPropertyChanged(nameof(IsConcurrentEvent));
                }
            }
        }

        public DateTime TimeToShow => IsPastEvent ? NextRunTime.AddMinutes(15) : NextRunTime;
        public List<LootHelper.LootResult> LootItems { get; set; } = new();

        private bool _chestOpened;
        public bool ChestOpened
        {
            get => _chestOpened;
            set
            {
                if (_chestOpened != value)
                {
                    _chestOpened = value;
                    OnPropertyChanged(nameof(ChestOpened));
                    OnPropertyChanged(nameof(ChestImagePath));
                }
            }
        }

        public void LoadChestState()
        {
            var value = BossTimings.IsChestOpened(BossName);

            if (ChestOpened != value)
            {
                ChestOpened = value;
            }
            else
            {
                OnPropertyChanged(nameof(ChestOpened));
                OnPropertyChanged(nameof(ChestImagePath));
            }
        }

        public void TriggerIconUpdate()
        {
            OnPropertyChanged(nameof(ChestImagePath));
        }

        public void UpdateCountdown()
        {
            var timeLeft = NextRunTime - GlobalVariables.CURRENT_DATE_TIME;
            Countdown = timeLeft > TimeSpan.Zero
                ? timeLeft.ToString(@"hh\:mm\:ss")
                : "Runs";
        }

        public void UpdateTimeProperties(DateTime now)
        {
            var remaining = NextRunTime - now;
            IsPastEvent = remaining.TotalSeconds < 0;

            var abs = remaining.Duration();
            SecondsRemaining = (int)(IsPastEvent ? -abs.TotalSeconds : abs.TotalSeconds);

            TimeRemainingFormatted = IsPastEvent
                ? $"-{(int)abs.TotalHours:D2}:{abs.Minutes:D2}:{abs.Seconds:D2}"
                : $"{(int)abs.TotalHours:D2}:{abs.Minutes:D2}:{abs.Seconds:D2}";
        }
    }
}