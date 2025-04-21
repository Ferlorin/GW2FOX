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
                // Konvertiere UTC-Zeit des Boss-Events in lokale Zeit
                var localBossEventTime = GlobalVariables.ConvertUtcToLocal(run.NextRunTime);

                // Berechne verbleibende Zeit bis zum Event
                var timeRemaining = localBossEventTime - now;
                bool isPast = timeRemaining.TotalSeconds < 0;

                // Ignoriere Events, die mehr als 15 Minuten in der Vergangenheit liegen oder mehr als 4 Stunden in der Zukunft
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
                    NextRunTime = localBossEventTime // Lokale Zeit für Anzeige
                };
            })
            .Where(item => item != null)
            .ToList();

        // Vergangene Events (absteigend) und zukünftige Events (aufsteigend) sortieren
        var past = items
            .Where(x => x.IsPastEvent)
            .OrderByDescending(x => x.SecondsRemaining);

        var future = items
            .Where(x => !x.IsPastEvent)
            .OrderBy(x => x.NextRunTime)
            .ToList();

        // Events innerhalb von 60 Sekunden markieren als gleichzeitig
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

    public static string FormatRemainingTime(TimeSpan remaining)
    {
        int totalHours = (int)remaining.TotalHours;
        return $"{totalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
    }
}
