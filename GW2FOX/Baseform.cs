using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GW2FOX
{
    public class BaseForm : Form
    {
        private Form previousPage;
        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook _globalKeyboardHook;

        public static ListView CustomBossList { get; private set; } = new ListView();

        public BaseForm()
        {
            InitializeCustomBossList();
            overlay = new Overlay(customBossList);
            bossTimer = new BossTimer(customBossList);
            InitializeGlobalKeyboardHook();
        }

        protected void InitializeBossTimerAndOverlay()
        {
            bossTimer = new BossTimer(customBossList);
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
            private readonly System.Windows.Forms.Timer timer;

            public BossTimer(ListView bossList)
            {
                this.bossList = bossList;
                this.mezTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                this.timer = new System.Windows.Forms.Timer();
                timer.Tick += TimerCallback;
                timer.Interval = 1000;
            }

            public void Start()
            {
                timer.Enabled = true;
            }

            public void Stop()
            {
                timer.Enabled = false;
            }

            private void TimerCallback(object? sender, EventArgs e)
            {
                try
                {
                    if (!bossList.IsHandleCreated) return;
                    bossList.BeginInvoke((MethodInvoker)delegate
                    {
                        UpdateBossList();

                      
                        if (CheckIfBossEventExpired())
                        {
                            // Wenn ja, die Liste neu einlesen und aktualisieren
                            ReloadAndRefreshBossList();
                        }
                    });
                }
                catch (Exception ex)
                {
                    HandleException(ex, "TimerCallback");
                }
            }

            private void ReloadAndRefreshBossList()
            {
            
                UpdateBossList(); 
            }

            private bool CheckIfBossEventExpired()
            {
                DateTime currentTimeUtc = DateTime.UtcNow;
                DateTime currentTimeMez = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, mezTimeZone);

                // Rufe die Liste der zukünftigen Boss-Events ab
                var futureBossEvents = BossTimings.GetFutureBossEvents();

                // Überprüfe, ob es abgelaufene Boss-Events gibt
                var expiredBosses = futureBossEvents
                    .Where(bossEvent =>
                        TimeSpan.Compare(bossEvent.Timing - currentTimeMez, TimeSpan.Zero) < 0)
                    .ToList();

                return expiredBosses.Any();
            }



            public void UpdateBossList()
            {
                if (!bossList.IsHandleCreated) return;

                bossList.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
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
                                bossEvent.Timing > currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                            .ToList();

                        var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                        var listViewItems = new List<ListViewItem>();
                        HashSet<string> addedBossNames = new HashSet<string>();

                        allBosses.Sort((bossEvent1, bossEvent2) =>
                        {
                            DateTime adjustedTiming1 = currentTimeMez.Date + currentTimeMez.TimeOfDay + bossEvent1.Timing;
                            DateTime adjustedTiming2 = currentTimeMez.Date + currentTimeMez.TimeOfDay + bossEvent2.Timing;

                            int adjustedTimingComparison = adjustedTiming1.CompareTo(adjustedTiming2);
                            if (adjustedTimingComparison != 0) return adjustedTimingComparison;

                            int durationComparison = bossEvent1.Duration.CompareTo(bossEvent2.Duration);
                            if (durationComparison != 0) return durationComparison;

                            int categoryComparison = bossEvent1.Category.CompareTo(bossEvent2.Category);
                            if (categoryComparison != 0) return categoryComparison;

                            return 0;
                        });

                        foreach (var bossEvent in allBosses)
                        {
                            DateTime adjustedTiming = GetAdjustedTiming(currentTimeMez, bossEvent.Timing);

                            if (addedBossNames.Add($"{bossEvent.BossName}_{adjustedTiming}"))
                            {
                                if (pastBosses.Contains(bossEvent) && currentTimeMez - adjustedTiming < new TimeSpan(0, 14, 59))
                                {
                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName });
                                    listViewItem.ForeColor = GetFontColor(bossEvent, pastBosses);
                                    listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    listViewItems.Add(listViewItem);
                                }
                                else
                                {
                                    TimeSpan elapsedTime = adjustedTiming - currentTimeMez;
                                    TimeSpan countdownTime = elapsedTime;

                                    string elapsedTimeFormat = $"{(int)elapsedTime.TotalHours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";

                                    Color fontColor = GetFontColor(bossEvent, pastBosses);

                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName, elapsedTimeFormat });
                                    listViewItem.ForeColor = fontColor;

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


            private bool HasSameTimeAndCategory(List<BossTimings.BossEvent> allBosses, BossTimings.BossEvent currentBossEvent)
            {
                return allBosses.Any(bossEvent =>
                    bossEvent != currentBossEvent &&
                    bossEvent.Timing == currentBossEvent.Timing &&
                    bossEvent.Category == currentBossEvent.Category &&
                    bossEvent.BossName != currentBossEvent.BossName);
            }

            private DateTime GetAdjustedTiming(DateTime currentTimeMez, TimeSpan bossTiming)
            {
                DateTime adjustedTiming = currentTimeMez.Add(bossTiming);

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
                        HandleException(ex, "UpdateBossList");
                    }
                });
            }

            private void HandleException(Exception ex, string methodName)
            {
                Console.WriteLine($"Exception in {methodName}: {ex}");
            }

            private TimeSpan GetRemainingTime(DateTime currentTimeMez, DateTime adjustedTiming, BossTimings.BossEvent bossEvent)
            {
                return adjustedTiming - currentTimeMez;
            }

            private Color GetFontColor(BossTimings.BossEvent bossEvent, List<BossTimings.BossEvent> pastBosses)
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
