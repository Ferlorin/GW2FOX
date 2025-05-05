using System;
using System.Collections.ObjectModel;
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
                    if (!isPast && timeRemaining.TotalHours > 4)
                        return null;

                    var remaining = isPast ? -timeRemaining : timeRemaining;
                    string formatted = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                    if (isPast)
                        formatted = $"-{remaining.Minutes:D2}:{remaining.Seconds:D2}";

                    var item = new BossListItem
                    {
                        BossName = run.BossName,
                        Waypoint = run.Waypoint,
                        Category = run.Category,
                        IsPastEvent = isPast,
                        TimeRemainingFormatted = formatted,
                        SecondsRemaining = (int)(isPast ? -remaining.TotalSeconds : remaining.TotalSeconds),
                        NextRunTime = eventTime,
                        Level = run.Level
                    };


                    // Setze ChestOpened korrekt
                    item.ChestOpened = BossTimings.IsChestOpened(run.BossName);

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
                .Where(group => selectedBosses.Contains(group.BossName))
                .SelectMany(group => group.GetNextRuns());

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


        public class BossListItem
        {
            public string BossName { get; set; } = string.Empty;
            public string Level { get; set; } = string.Empty; // <-- HINZUGEFÜGT
            public string Waypoint { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public bool IsPastEvent { get; set; }
            public string TimeRemainingFormatted { get; set; } = string.Empty;
            public int SecondsRemaining { get; set; }
            public DateTime NextRunTime { get; set; }
            public bool IsConcurrentEvent { get; set; }
            public DateTime TimeToShow => IsPastEvent ? NextRunTime.AddMinutes(15) : NextRunTime;
            public bool ChestOpened { get; set; }

            public void UpdateCountdown()
            {
                var timeLeft = TimeToShow - DateTime.Now;
                TimeRemainingFormatted = timeLeft.ToString(@"hh\:mm\:ss");
            }
        }

    }
}
