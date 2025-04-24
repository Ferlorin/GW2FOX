using GW2FOX;
using System;
using System.Collections.ObjectModel;
using System.Linq;


public static class BossOverlayHelper
    {
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

                // Anzeige: bis 1h vergangen oder 8h voraus
                if (isPast && timeRemaining.TotalMinutes <= -60)
                    return null;
                if (!isPast && timeRemaining.TotalHours > 8)
                    return null;

                var remaining = isPast ? -timeRemaining : timeRemaining;
                string formatted = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";

                if (isPast)
                    formatted = $"-{remaining.Minutes:D2}:{remaining.Seconds:D2}";

                return new BossListItem
                {
                    BossName = run.BossName,
                    Waypoint = run.Waypoint,
                    Category = run.Category,
                    IsPastEvent = isPast,
                    TimeRemainingFormatted = formatted,
                    SecondsRemaining = (int)(isPast ? -remaining.TotalSeconds : remaining.TotalSeconds),
                    NextRunTime = eventTime
                };
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

        foreach (var boss in overlayItems)

        return overlayItems;
    }



}

