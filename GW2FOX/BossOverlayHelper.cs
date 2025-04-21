using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GW2FOX
{
    public static class BossOverlayHelper
    {
        public static ObservableCollection<BossListItem> GetBossOverlayItems(List<BossEventRun> combinedBosses)
        {
            var overlayItems = new ObservableCollection<BossListItem>();
            var now = DateTime.UtcNow;

            foreach (var run in combinedBosses)
            {
                TimeSpan timeRemaining = run.NextRunTime - now;
                bool isPast = timeRemaining.TotalSeconds < 0;

                if (isPast)
                {
                    // Vergangene Events nur anzeigen, wenn max. 15 Minuten her
                    if (timeRemaining.TotalMinutes <= -15)
                        continue;

                    timeRemaining = -timeRemaining; // Absoluten Wert berechnen
                }
                else
                {
                    // Zukünftige Events nur anzeigen, wenn sie in den nächsten 4 Stunden sind
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
                    SecondsRemaining = (int)(isPast ? -timeRemaining.TotalSeconds : timeRemaining.TotalSeconds)
                });
            }

            return overlayItems;
        }
    }
}


