using System;
using System.Collections.Generic;

namespace GW2FOX
{
    public static class BossOverlayHelper
    {
        public static List<BossListItem> GetBossOverlayItems(List<BossEventRun> combinedBosses)
        {
            var overlayItems = new List<BossListItem>();

            foreach (var run in combinedBosses)
            {
                var timeRemaining = run.NextRunTime - DateTime.UtcNow;
                bool isPast = timeRemaining.TotalSeconds < 0;

                // ⏱ Wenn das Event älter als 14:59 ist, überspringen
                if (isPast && timeRemaining.TotalMinutes <= -15)
                    continue;

                if (isPast)
                {
                    timeRemaining = -timeRemaining;
                }

                string formattedTime = isPast
                    ? timeRemaining.ToString(@"mm\:ss")     // vergangen: nur Minuten+Sekunden
                    : timeRemaining.ToString(@"hh\:mm\:ss"); // zukünftig: volle Stundenanzeige

                overlayItems.Add(new BossListItem
                {
                    BossName = run.BossName,
                    Waypoint = run.Waypoint,
                    IsPastEvent = isPast,
                    TimeRemainingFormatted = isPast ? "-" + formattedTime : formattedTime
                });
            }

            return overlayItems;
        }
    }
}
