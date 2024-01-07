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

        private void UpdateBossList()
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
                            bossEvent.Timing >= currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                        .ToList();

                    var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                    var allBossesSnapshot = allBosses.ToList();

                    allBossesSnapshot.Sort((bossEvent1, bossEvent2) =>
                        bossEvent1.Timing.CompareTo(bossEvent2.Timing));

                    var listViewItems = new List<ListViewItem>();

                    // Use a HashSet to keep track of added boss names
                    HashSet<string> addedBossNames = new HashSet<string>();

                    foreach (var bossEvent in allBossesSnapshot)
                    {
                        // Check if the boss name is already added to avoid duplicates
                        if (addedBossNames.Add(bossEvent.BossName))
                        {
                            TimeSpan remainingTime = bossEvent.Timing - currentTime;
                            string countdownFormat = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";

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
