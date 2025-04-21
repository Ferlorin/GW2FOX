using GW2FOX;
using System.Collections.ObjectModel;

public static class BossOverlayHelper
{
    public static ObservableCollection<BossListItem> GetBossOverlayItems(IEnumerable<BossEventRun> bossRuns)
    {
        var overlayItems = new ObservableCollection<BossListItem>();
        var now = DateTime.UtcNow;

        foreach (var run in bossRuns)
        {
            TimeSpan timeRemaining = run.NextRunTime - now;
            bool isPast = timeRemaining.TotalSeconds < 0;

            if (isPast)
            {
                if (timeRemaining.TotalMinutes <= -15)
                    continue;

                timeRemaining = -timeRemaining;
            }
            else
            {
                if (timeRemaining.TotalHours > 4)
                    continue;
            }

            string formattedTime = isPast
                ? timeRemaining.ToString(@"mm\:ss")
                : timeRemaining.ToString(@"hh\:mm\:ss");

            overlayItems.Add(new BossListItem
            {
                BossName = run.BossName,
                Waypoint = run.Waypoint,
                IsPastEvent = isPast,
                TimeRemainingFormatted = isPast ? "-" + formattedTime : formattedTime,
                SecondsRemaining = (int)(isPast ? -timeRemaining.TotalSeconds : timeRemaining.TotalSeconds),
                NextRunTime = run.NextRunTime // falls du das für Updates brauchst
            });
        }

        return overlayItems;
    }


}