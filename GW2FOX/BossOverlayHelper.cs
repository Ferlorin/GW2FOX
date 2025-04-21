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
                Console.WriteLine($"[Overlay] {run.BossName,-25} NextRun: {run.NextRunTime:HH:mm:ss}, Now: {now:HH:mm:ss}, Diff: {(run.NextRunTime - now).TotalMinutes:F1} min");

                var timeRemaining = run.NextRunTime - now;
                bool isPast = timeRemaining.TotalSeconds < 0;

                // Exclude events older than 15 minutes in the past or further than 4h in the future
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

        // Sort: past (newest first), future (soonest first)
        var past = items
            .Where(x => x.IsPastEvent)
            .OrderByDescending(x => x.SecondsRemaining);

        var future = items
            .Where(x => !x.IsPastEvent)
            .OrderBy(x => x.NextRunTime)
            .ToList();

        // Detect concurrent events (within 60s of each other)
        for (int i = 0; i < future.Count; i++)
        {
            var current = future[i];
            current.IsConcurrentEvent = future.Any(other =>
                other != current &&
                Math.Abs((other.NextRunTime - current.NextRunTime).TotalSeconds) < 60);
        }

        foreach (var item in past.Concat(future))
            overlayItems.Add(item);

        // DEBUG: Finale Reihenfolge ausgeben
        Console.WriteLine("Finale Reihenfolge:");
        foreach (var item in overlayItems)
            Console.WriteLine($"{item.BossName,-25} {item.TimeRemainingFormatted} {(item.IsPastEvent ? "(past)" : "")}");

        return overlayItems;
    }



}