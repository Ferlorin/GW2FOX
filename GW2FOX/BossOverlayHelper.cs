using GW2FOX;
using System;
using System.Collections.ObjectModel;
using System.Linq;

public static class BossOverlayHelper
{
    public static ObservableCollection<BossListItem> GetBossOverlayItems(IEnumerable<BossEventRun> bossRuns, DateTime _)
    {
        var overlayItems = new ObservableCollection<BossListItem>();

        // LOKALE Zeit, keine UTC!
        var now = DateTime.Now;

        var items = bossRuns
            .Select(run =>
            {
                // LOKAL bleiben: keine Konvertierung
                var eventTime = run.NextRunTime;

                var timeRemaining = eventTime - now;
                bool isPast = timeRemaining.TotalSeconds < 0;

                if (isPast && timeRemaining.TotalMinutes <= -15)
                    return null;
                if (!isPast && timeRemaining.TotalHours > 4)
                    return null;

                var remaining = isPast ? -timeRemaining : timeRemaining;
                string formatted = remaining.ToString(@"hh\:mm\:ss");
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
                    NextRunTime = eventTime // lokal, keine Konvertierung!
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

        return overlayItems;
    }

}
