using static GW2FOX.BossTimings;

namespace GW2FOX
{
    public class BaseForm : Form
    {
        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook _globalKeyboardHook;

        public static ListView CustomBossList { get; private set; } = new ListView();

        public BaseForm()
        {
            InitializeCustomBossList();
            overlay = new Overlay(customBossList);
            bossTimer = new BossTimer(customBossList, "config.txt");

            InitializeGlobalKeyboardHook();
            

        }

        protected void InitializeBossTimerAndOverlay()
        {
            bossTimer = new BossTimer(customBossList, "config.txt");
            overlay = new Overlay(customBossList);
            overlay.WindowState = FormWindowState.Normal;
        }

        private void InitializeGlobalKeyboardHook()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyPressed += GlobalKeyboardHook_KeyPressed;
        }

        private void GlobalKeyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Alt && e.Key == Keys.T)
            {
                Timer_Click(sender, e);
            }
        }

        protected void InitializeCustomBossList()
        {
            customBossList = new ListView();
            customBossList.View = View.Details;
            customBossList.Columns.Add("Boss Name", 145);
            customBossList.Columns.Add("Time", 78);
            customBossList.Location = new Point(0, 0);
            customBossList.ForeColor = Color.White;
            new Font("Segoe UI", 10, FontStyle.Bold);
        }

        public void UpdateCustomBossList(ListView updatedList)
        {
            CustomBossList = updatedList;
        }

        public void Timer_Click(object sender, EventArgs e)
        {
            InitializeCustomBossList();
            InitializeBossTimerAndOverlay();

            bossTimer.Start();
            overlay.Show();
        }

        protected void ShowAndHideForm(Form newForm)
        {
            newForm.Show();
            this.Hide();
        }

        protected void AdjustWindowSize()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle workingArea = currentScreen.WorkingArea;

            if (this.Width > workingArea.Width || this.Height > workingArea.Height)
            {
                this.Size = new Size(
                    Math.Min(this.Width, workingArea.Width),
                    Math.Min(this.Height, workingArea.Height)
                );

                this.Location = new Point(
                    workingArea.Left + (workingArea.Width - this.Width) / 2,
                    workingArea.Top + (workingArea.Height - this.Height) / 2
                );
            }

            if (this.Height > workingArea.Height)
            {
                this.Height = workingArea.Height;
            }

            if (this.Bottom > workingArea.Bottom)
            {
                this.Location = new Point(
                    this.Left,
                    Math.Max(workingArea.Top, workingArea.Bottom - this.Height)
                );
            }
        }

        protected void HandleException(Exception ex, string methodName)
        {
            Console.WriteLine($"Exception in {methodName}: {ex}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bossTimer.Dispose();
            base.OnFormClosing(e);
            Application.Exit();
        }

        public class BossTimer : IDisposable
        {
            private static readonly string TimeZoneId = "W. Europe Standard Time";
            private static readonly Color DefaultFontColor = Color.Blue;
            private static readonly Color PastBossFontColor = Color.OrangeRed;

            private readonly ListView bossList;
            private readonly TimeZoneInfo mezTimeZone;
            private readonly System.Threading.Timer timer;
            private string configFilePath;

            public BossTimer(ListView bossList, string configFilePath)
            {
                this.bossList = bossList;
                this.configFilePath = configFilePath; 
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
                    UpdateBossList(configFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in TimerCallback: {ex}");
                    // Consider logging the exception
                }
            }


            public void UpdateBossList(string configFilePath)
            {
                if (!bossList.IsHandleCreated) return;

                bossList.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        // Read the boss names from the configuration file
                        List<string> bossNamesFromConfig = ReadBossNamesFromConfig(configFilePath);

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
                                bossEvent.Timing > currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                            .ToList();

                        // Combine all bosses
                        var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                        var listViewItems = new List<ListViewItem>();

                        // Use a HashSet to keep track of added boss names
                        HashSet<string> addedBossNames = new HashSet<string>();

                        allBosses.Sort((bossEvent1, bossEvent2) =>
                        {
                            DateTime adjustedTiming1 = currentTimeMez.Date + bossEvent1.Timing;
                            DateTime adjustedTiming2 = currentTimeMez.Date + bossEvent2.Timing;

                            // Compare the adjusted timings for the next day
                            int adjustedTimingComparison = adjustedTiming1.CompareTo(adjustedTiming2);
                            if (adjustedTimingComparison != 0) return adjustedTimingComparison;

                            // If timings are equal, sort by ascending durations
                            int durationComparison = bossEvent1.Duration.CompareTo(bossEvent2.Duration);
                            if (durationComparison != 0) return durationComparison;

                            // If durations and timings are equal, sort by categories (if necessary)
                            int categoryComparison = bossEvent1.Category.CompareTo(bossEvent2.Category);
                            if (categoryComparison != 0) return categoryComparison;

                            return 0; // Tie, maintain the order unchanged
                        });

                        foreach (var bossEvent in allBosses)
                        {
                            DateTime adjustedTiming = GetAdjustedTiming(currentTimeMez, bossEvent.Timing);

                            // Check if the boss with the adjusted timing is already added to avoid duplicates
                            if (addedBossNames.Add($"{bossEvent.BossName}_{adjustedTiming}"))
                            {
                                if (pastBosses.Contains(bossEvent) && currentTimeMez - adjustedTiming < new TimeSpan(0, 14, 59))
                                {
                                    // Display only the boss name for past events within the time span of 00:14:59
                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName });
                                    listViewItem.ForeColor = GetFontColor(bossEvent, pastBosses);
                                    listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    listViewItems.Add(listViewItem);
                                }
                                else
                                {
                                    TimeSpan elapsedTime = adjustedTiming - currentTimeMez;

                                    // Änderung der Formatierung für den Countdown basierend auf der vergangenen Zeitspanne
                                    TimeSpan countdownTime = elapsedTime;

                                    string elapsedTimeFormat = $"{(int)elapsedTime.TotalHours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";

                                    Color fontColor = GetFontColor(bossEvent, pastBosses);

                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName, elapsedTimeFormat });
                                    listViewItem.ForeColor = fontColor;

                                    // Neue Bedingung hinzufügen, um zu prüfen, ob ein Bossevent zur selben Zeit stattfindet wie ein anderes Bossevent derselben Kategorie
                                    if (HasSameTimeAndCategory(allBosses, bossEvent))
                                    {
                                        listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Italic | FontStyle.Bold);
                                    }
                                    else
                                    {
                                        listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    }

                                    listViewItems.Add(listViewItem);
                                }
                            }
                        }

                        UpdateListViewItems(listViewItems);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex, "UpdateBossList");
                    }
                });
            }

            private List<string> ReadBossNamesFromConfig(string filePath)
            {
                List<string> bossNames = new List<string>();

                try
                {
                    // Vorhandenen Inhalt aus der Datei lesen
                    string[] lines = File.ReadAllLines(filePath);

                    // Index der Zeile mit dem Bossnamen finden
                    int bossIndex = -1;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Bosses:"))
                        {
                            bossIndex = i; // Die aktuelle Zeile enthält den Namen
                            break;
                        }
                    }

                    // Bossnamen extrahieren
                    if (bossIndex != -1 && bossIndex + 1 < lines.Length)
                    {
                        string bossNamesLine = lines[bossIndex + 1];
                        bossNames = bossNamesLine.Split(',').Select(name => name.Trim()).ToList();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex, "ReadBossNamesFromConfig");
                }

                return bossNames;
            }

            private bool HasSameTimeAndCategory(List<BossEvent> allBosses, BossEvent currentBossEvent)
            {
                return allBosses.Any(bossEvent =>
                    bossEvent != currentBossEvent &&
                    bossEvent.Timing == currentBossEvent.Timing &&
                    bossEvent.Category == currentBossEvent.Category &&
                    bossEvent.BossName != currentBossEvent.BossName);
            }

            private DateTime GetAdjustedTiming(DateTime currentTimeMez, TimeSpan bossTiming)
            {
                DateTime adjustedTiming = currentTimeMez.Date + bossTiming;

                while (adjustedTiming < currentTimeMez)
                {
                    adjustedTiming = adjustedTiming.AddDays(1);
                }

                return adjustedTiming;
            }

            private void UpdateListViewItems(List<ListViewItem> listViewItems)
            {
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
                        HandleException(ex, "UpdateListViewItems");
                    }
                });
            }

            private void HandleException(Exception ex, string methodName)
            {
                Console.WriteLine($"Exception in {methodName}: {ex}");
                // Consider logging the exception with more details
            }

            private TimeSpan GetRemainingTime(DateTime currentTimeMez, DateTime adjustedTiming, BossEvent bossEvent)
            {
                return adjustedTiming - currentTimeMez;
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
}
