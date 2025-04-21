using GW2FOX;
using System.Collections.ObjectModel;

public static class BossOverlayHelper
{
    public static ObservableCollection<BossListItem> GetBossOverlayItems(IEnumerable<BossEventRun> bossRuns, DateTime now)

    {
        var overlayItems = new ObservableCollection<BossListItem>();

        var items = bossRuns
            .Select(run =>
            {
                var timeRemaining = run.NextRunTime - now;
                bool isPast = timeRemaining.TotalSeconds < 0;
                if (isPast && timeRemaining.TotalMinutes <= -15)
                    return null;
                if (!isPast && timeRemaining.TotalHours > 4)
                    return null;

                var remaining = isPast ? -timeRemaining : timeRemaining;
                string formatted = isPast
                    ? "-" + remaining.ToString(@"mm\:ss")
                    : remaining.ToString(@"hh\:mm\:ss");

                return new BossListItem
                {
                    BossName = run.BossName,
                    Waypoint = run.Waypoint,
                    Category = run.Category,
                    IsPastEvent = isPast,
                    TimeRemainingFormatted = formatted,
                    SecondsRemaining = (int)(isPast ? -remaining.TotalSeconds : remaining.TotalSeconds),
                    NextRunTime = run.NextRunTime
                };
            })
            .Where(item => item != null)
            .ToList();

        // Sortieren: Vergangenheit zuerst, dann Zukunft aufsteigend
        var past = items.Where(x => x.IsPastEvent).OrderBy(x => x.SecondsRemaining);
        var future = items.Where(x => !x.IsPastEvent).OrderBy(x => x.NextRunTime).ToList();

        // Kursive Markierung für gleichzeitige Events (innerhalb 1 Minute)
        for (int i = 0; i < future.Count; i++)
        {
            var current = future[i];
            current.IsConcurrentEvent = future.Any(other =>
                other != current &&
                Math.Abs((other.NextRunTime - current.NextRunTime).TotalSeconds) < 60);
        }

        foreach (var item in past.Concat(future))
            overlayItems.Add(item);

        return overlayItems;
    }



}