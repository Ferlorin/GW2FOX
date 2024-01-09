using static GW2FOX.BossTimings;

namespace GW2FOX
{
    public class BossTimer : IDisposable
    {
        private static readonly string TimeZoneId = "W. Europe Standard Time";
        private static readonly Color DefaultFontColor = Color.Blue;
        private static readonly Color PastBossFontColor = Color.OrangeRed;

        private readonly ListView bossList;
        private readonly TimeZoneInfo mezTimeZone;
        private readonly System.Threading.Timer timer;


        public BossTimer(ListView bossList)
        {
            this.bossList = bossList;
            this.mezTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
            this.timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);

        }

        public void Start()
        {
            timer.Change(0, 1000);
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void TimerCallback(object? state)
        {
            try
            {
                UpdateBossList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in TimerCallback: {ex}");
                // Consider logging the exception
            }
        }

        public void UpdateBossList()
        {
            if (!bossList.IsHandleCreated) return;

            bossList.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    // Read the boss names from the configuration file
                    List<string> bossNamesFromConfig = BossTimings.BossList23;

                    DateTime currentTimeUtc = DateTime.UtcNow;
                    DateTime currentTimeMez = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, mezTimeZone);
                    TimeSpan currentTime = currentTimeMez.TimeOfDay;

                    var upcomingBosses = BossTimings.Events
                        .Where(bossEvent =>
                            bossNamesFromConfig.Contains(bossEvent.BossName) &&
                            bossEvent.Timing > currentTime && bossEvent.Timing < currentTime.Add(new TimeSpan(8, 0, 0)))
                        .ToList();

                    var pastBosses = BossTimings.Events
                        .Where(bossEvent =>
                            bossNamesFromConfig.Contains(bossEvent.BossName) &&
                            bossEvent.Timing > currentTime.Add(new TimeSpan(1, 0, 0, 0)))
                        .ToList();

                    // Find bosses in the past 14 minutes and 59 seconds
                    var past14MinBosses = BossTimings.Events
                        .Where(bossEvent =>
                            bossNamesFromConfig.Contains(bossEvent.BossName) &&
                            bossEvent.Timing >= currentTime.Subtract(new TimeSpan(0, 14, 59)) &&
                            bossEvent.Timing < currentTime)
                        .ToList();

                    // Combine all bosses
                    var allBosses = past14MinBosses.Concat(upcomingBosses).Concat(pastBosses).ToList();

                    var listViewItems = new List<ListViewItem>();

                    // Use a HashSet to keep track of added boss names
                    HashSet<string> addedBossNames = new HashSet<string>();

                    allBosses.Sort((bossEvent1, bossEvent2) =>
                    {
                        DateTime adjustedTiming1 = currentTimeMez.Date + bossEvent1.Timing;
                        DateTime adjustedTiming2 = currentTimeMez.Date + bossEvent2.Timing;

                        // Vergleiche die angepassten Timings für den nächsten Tag
                        int adjustedTimingComparison = adjustedTiming1.CompareTo(adjustedTiming2);
                        if (adjustedTimingComparison != 0) return adjustedTimingComparison;

                        // Falls die Timings gleich sind, sortiere nach aufsteigenden Dauern
                        int durationComparison = bossEvent1.Duration.CompareTo(bossEvent2.Duration);
                        if (durationComparison != 0) return durationComparison;

                        // Falls die Dauern und Timings gleich sind, sortiere nach Kategorien (falls erforderlich)
                        int categoryComparison = bossEvent1.Category.CompareTo(bossEvent2.Category);
                        if (categoryComparison != 0) return categoryComparison;

                        return 0; // Gleichstand, die Reihenfolge bleibt unverändert
                    });

                    foreach (var bossEvent in allBosses)
                    {
                        // Hier wird die Zeit auf die nächste Runde angepasst, wenn die aktuelle Zeit überschritten wird
                        DateTime adjustedTiming = currentTimeMez.Date + bossEvent.Timing;
                        while (adjustedTiming < currentTimeMez)
                        {
                            adjustedTiming = adjustedTiming.AddDays(1);
                        }

                        // Check if the boss with the adjusted timing is already added to avoid duplicates
                        if (addedBossNames.Add($"{bossEvent.BossName}_{adjustedTiming}"))
                        {
                            TimeSpan remainingTime = adjustedTiming - currentTimeMez;
                            // Änderung: Negiere die verbleibende Zeit für Bosse der vergangenen 14 Minuten und 49 Sekunden
                            if (IsPast14MinBoss(bossEvent, currentTimeMez, TimeSpan.FromMinutes(14), TimeSpan.FromSeconds(49)))
                            {
                                remainingTime = -remainingTime;
                            }

                            string countdownFormat = $"{(int)remainingTime.TotalHours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";

                            Color fontColor = GetFontColor(bossEvent, pastBosses);

                            var listViewItem = new ListViewItem(new[] { bossEvent.BossName, countdownFormat });
                            listViewItem.ForeColor = fontColor;
                            listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                            listViewItems.Add(listViewItem);
                        }
                    }

                    bossList.BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            if (bossList.IsHandleCreated)
                            {
                                bossList.BeginUpdate();
                                bossList.Items.Clear();
                                bossList.Items.AddRange(listViewItems.ToArray());
                                bossList.EndUpdate();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception in bossList.BeginInvoke: {ex}");
                            // Consider logging the exception
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in UpdateBossList: {ex}");
                    // Consider logging the exception
                }
            });
        }



        private static bool IsPast14MinBoss(BossEvent bossEvent, DateTime currentTime, TimeSpan pastDuration, TimeSpan pastThreshold)
        {
            DateTime bossStartTime = currentTime.Date + bossEvent.Timing;
            DateTime pastThresholdTime = currentTime - pastDuration;

            // Check if the boss appeared in the last 14 minutes and 59 seconds
            if (bossStartTime > pastThresholdTime && bossStartTime < currentTime)
            {
                // Calculate the elapsed time since the boss appeared
                TimeSpan elapsedTime = currentTime - bossStartTime;

                // Calculate the remaining time until "00.14.59" in descending order
                TimeSpan remainingTime = pastDuration - elapsedTime;

                // Use the remaining time for display
                string countdownFormat = $"{(int)remainingTime.TotalHours:D2}.{remainingTime.Minutes:D2}.{remainingTime.Seconds:D2}";

                Console.WriteLine($"Boss {bossEvent.BossName} appeared {countdownFormat} ago.");

                // Implement the timer logic here using System.Threading.Timer
                System.Threading.Timer timer = null;
                timer = new System.Threading.Timer((state) =>
                {
                    remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1)); // Decrease time by 1 second
                    Console.WriteLine($"Time remaining: {remainingTime}");

                    // Stop and dispose the timer when needed
                    if (remainingTime.TotalSeconds <= 0)
                    {
                        timer?.Change(Timeout.Infinite, Timeout.Infinite);
                        timer?.Dispose();
                    }
                }, null, 0, 1000);

                return true;
            }

            return false;
        }












        private static Color GetFontColor(BossEvent bossEvent, List<BossEvent> pastBosses)
        {
            Color fontColor;

            switch (bossEvent.Category)
            {
                case "Maguuma":
                    fontColor = Color.LimeGreen;
                    break;
                case "Desert":
                    fontColor = Color.DeepPink;
                    break;
                case "WBs":
                    fontColor = Color.WhiteSmoke;
                    break;
                case "Ice":
                    fontColor = Color.DeepSkyBlue;
                    break;
                case "Cantha":
                    fontColor = Color.Blue;
                    break;
                case "SotO":
                    fontColor = Color.Yellow;
                    break;
                case "LWS2":
                    fontColor = Color.LightYellow;
                    break;
                case "LWS3":
                    fontColor = Color.ForestGreen;
                    break;
                default:
                    fontColor = DefaultFontColor;
                    break;
            }

            if (pastBosses.Any(pastBoss => pastBoss.BossName == bossEvent.BossName && pastBoss.Timing == bossEvent.Timing))
            {
                fontColor = PastBossFontColor;
            }

            return fontColor;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
            }
        }

        ~BossTimer()
        {
            Dispose(false);
        }
    }
}
