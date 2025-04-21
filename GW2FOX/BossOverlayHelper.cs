using System;
using System.Collections.Generic;

namespace GW2FOX
{
    using System;
    using System.Collections.Generic;

 
        public static class BossOverlayHelper
        {
            public static List<BossListItem> GetBossOverlayItems(List<BossEventRun> combinedBosses)
            {
                var overlayItems = new List<BossListItem>();
                var now = DateTime.UtcNow;

                foreach (var run in combinedBosses)
                {
                    var timeRemaining = run.NextRunTime - now;
                    bool isPast = timeRemaining.TotalSeconds < 0;

                    // Vergangene Events: nur anzeigen, wenn sie max. 15 Minuten her sind
                    if (isPast && timeRemaining.TotalMinutes <= -15)
                        continue;

                    // Zukünftige Events: nur anzeigen, wenn sie in den nächsten 4 Stunden sind
                    if (!isPast && timeRemaining.TotalHours > 4)
                        continue;

                    // Zeit korrekt formatieren
                    if (isPast)
                        timeRemaining = -timeRemaining;

                    string formattedTime = isPast
                        ? timeRemaining.ToString(@"mm\:ss") // vergangen: Minuten:Sekunden
                        : timeRemaining.ToString(@"hh\:mm\:ss"); // zukünftig: Stunden:Minuten:Sekunden

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


