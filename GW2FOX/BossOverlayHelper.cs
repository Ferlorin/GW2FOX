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

                // 👇 Wenn das Event mehr als 15 Minuten vorbei ist, skippen
                if (isPast && timeRemaining.TotalMinutes <= -15)
                    continue;

                if (isPast)
                    timeRemaining = -timeRemaining;

                // ⏱ Zeit schön formatieren (z. B. 04:36)
                string formattedTime = timeRemaining.ToString(@"mm\:ss");

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
