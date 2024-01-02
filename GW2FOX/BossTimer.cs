namespace GW2FOX
{
    public class BossTimer : IDisposable
    {
        public void StopTimer()
        {
            Stop();
        }

        public static ListView CustomBossList;
        private readonly ListView bossList;
        private readonly TimeZoneInfo mezTimeZone;
        private System.Threading.Timer timer;
        private List<BossEvent> nextBosses;

        public BossTimer(ListView bossList)
        {
            this.bossList = bossList;
            this.nextBosses = new List<BossEvent>();
            this.mezTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
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
            UpdateBossList();
        }

        private void UpdateBossList()
        {
            try
            {
                if (bossList.IsHandleCreated)
                {
                    bossList.BeginInvoke((MethodInvoker)delegate
                    {
                        DateTime currentTimeUtc = DateTime.UtcNow;
                        DateTime currentTimeMez = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, mezTimeZone);
                        TimeSpan currentTime = currentTimeMez.TimeOfDay;

                        var upcomingBosses = BossTimings.Events
                            .Where(bossEvent =>
                                bossEvent.Timing > currentTime && bossEvent.Timing < currentTime.Add(new TimeSpan(2, 0, 0, 0)))
                            .ToList();

                        var pastBosses = BossTimings.Events
                            .Where(bossEvent =>
                                bossEvent.Timing >= currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                            .ToList();

                        var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                        var allBossesSnapshot = allBosses.ToList();

                        allBossesSnapshot.Sort((bossEvent1, bossEvent2) =>
                            bossEvent1.Timing.CompareTo(bossEvent2.Timing));

                        var listViewItems = new List<ListViewItem>();

                        foreach (var bossEvent in allBossesSnapshot)
                        {
                            TimeSpan remainingTime = bossEvent.Timing - currentTime;
                            string countdownFormat = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";

                            Color fontColor;

                            if (bossEvent.Category == "Maguuma")
                                fontColor = Color.LimeGreen;
                            else if (bossEvent.Category == "Desert")
                                fontColor = Color.DeepPink;
                            else if (bossEvent.Category == "WBs")
                                fontColor = Color.WhiteSmoke;
                            else if (bossEvent.Category == "Ice")
                                fontColor = Color.DeepSkyBlue;
                            else
                                fontColor = Color.Blue;

                            if (pastBosses.Any(pastBoss => pastBoss.BossName == bossEvent.BossName && pastBoss.Timing == bossEvent.Timing))
                            {
                                fontColor = Color.OrangeRed;
                            }

                            var listViewItem = new ListViewItem(new[] { bossEvent.BossName, countdownFormat });
                            listViewItem.ForeColor = fontColor;
                            listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                            listViewItems.Add(listViewItem);
                        }

                        bossList.BeginInvoke((MethodInvoker)delegate
                        {
                            if (bossList.IsHandleCreated)
                            {
                                bossList.BeginUpdate();
                                bossList.Items.Clear();
                                bossList.Items.AddRange(listViewItems.ToArray());
                                bossList.EndUpdate();
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateBossList: {ex}");
            }
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
                // Freigabe von verwalteten Ressourcen
                if (timer != null)
                {
                    timer.Dispose();
                }
            }

            // Freigabe von nicht verwalteten Ressourcen (falls vorhanden)
        }

        ~BossTimer()
        {
            Dispose(false);
        }
    }

    // ... (andere Klassen und Methoden)
}
