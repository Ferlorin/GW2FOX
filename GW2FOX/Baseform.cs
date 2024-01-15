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
            // Überprüfe, ob "ALT + T" gedrückt wurde
            if (Control.ModifierKeys == Keys.Alt && e.Key == Keys.T)
            {
                // Rufe die Timer_Click-Methode auf
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
            bossTimer.Dispose(); // Dispose of the BossTimer first
            base.OnFormClosing(e);
            Application.Exit();
        }

        public class BossTimer : IDisposable
        {
            private static readonly Color DefaultFontColor = Color.Blue;
            private static readonly Color PastBossFontColor = Color.OrangeRed;
            private readonly ListView bossList;
            private readonly System.Windows.Forms.Timer timer;

            public BossTimer(ListView bossList)
            {
                this.bossList = bossList;
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
                    });
                }
                catch (Exception ex)
                {
                    HandleException(ex, "TimerCallback");
                }
            }

            public void UpdateBossList()
            {
                // Überprüfen, ob das Handle der bossList erstellt wurde
                if (!bossList.IsHandleCreated) return;

                // Asynchroner Aufruf, um auf das UI-Handle zuzugreifen
                bossList.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        // Boss-Namen aus der Konfiguration abrufen
                        List<string> bossNamesFromConfig = BossTimings.BossList23;

                        // Aktuelle Zeit (UTC) abrufen
                        DateTime currentTimeUtc = DateTime.UtcNow;
                        TimeSpan currentTime = currentTimeUtc.TimeOfDay;

                        // Zukünftige Boss-Events filtern
                        var upcomingBosses = BossTimings.Events
                            .Where(bossEvent =>
                                bossNamesFromConfig.Contains(bossEvent.BossName) &&
                                (bossEvent.Timing > currentTime && bossEvent.Timing < currentTime.Add(new TimeSpan(8, 0, 0)) ||
                                 bossEvent.Timing.Add(bossEvent.RepeatInterval) > currentTime && bossEvent.Timing.Add(bossEvent.RepeatInterval) < currentTime.Add(new TimeSpan(8, 0, 0))))
                            .ToList();

                        // Vergangene Boss-Events filtern
                        var pastBosses = BossTimings.Events
                            .Where(bossEvent =>
                                bossNamesFromConfig.Contains(bossEvent.BossName) &&
                                bossEvent.Timing > currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                            .ToList();

                        // Alle Boss-Events (Vergangene + Zukünftige) zusammenführen
                        var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                        // Liste für ListView-Elemente erstellen
                        var listViewItems = new List<ListViewItem>();
                        HashSet<string> addedBossNames = new HashSet<string>();

                        // Boss-Events nach Zeitpunkt, Dauer und Kategorie sortieren
                        allBosses.Sort((bossEvent1, bossEvent2) =>
                        {
                            DateTime adjustedTiming1 = DateTime.Today + bossEvent1.Timing;
                            DateTime adjustedTiming2 = DateTime.Today + bossEvent2.Timing;

                            int adjustedTimingComparison = adjustedTiming1.CompareTo(adjustedTiming2);
                            if (adjustedTimingComparison != 0) return adjustedTimingComparison;

                            int durationComparison = bossEvent1.Duration.CompareTo(bossEvent2.Duration);
                            if (durationComparison != 0) return durationComparison;

                            int categoryComparison = bossEvent1.Category.CompareTo(bossEvent2.Category);
                            if (categoryComparison != 0) return categoryComparison;

                            return 0;
                        });

                        // Durch alle Boss-Events iterieren
                        foreach (var bossEvent in allBosses)
                        {
                            // Adjustierte Zeit für das Boss-Event erhalten
                            DateTime adjustedTiming = GetAdjustedTiming(bossEvent.Timing, bossEvent.RepeatInterval);

                            // Überprüfen, ob der Boss bereits hinzugefügt wurde
                            if (addedBossNames.Add($"{bossEvent.BossName}_{adjustedTiming}"))
                            {
                                // Überprüfen, ob es sich um ein vergangenes Event handelt und es weniger als 15 Minuten her ist
                                if (pastBosses.Contains(bossEvent) && DateTime.Now - adjustedTiming < new TimeSpan(0, 14, 59))
                                {
                                    // ListViewItem für vergangene Events erstellen
                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName });
                                    listViewItem.ForeColor = GetFontColor(bossEvent, pastBosses);
                                    listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    listViewItems.Add(listViewItem);
                                }
                                else
                                {
                                    // Verbleibende Zeit bis zum Event berechnen
                                    TimeSpan elapsedTime = adjustedTiming - DateTime.Now;
                                    TimeSpan countdownTime = elapsedTime;

                                    // Formatieren der verbleibenden Zeit
                                    string elapsedTimeFormat = $"{(int)elapsedTime.TotalHours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";

                                    // Farbe des Textes basierend auf der Kategorie des Boss-Events erhalten
                                    Color fontColor = GetFontColor(bossEvent, pastBosses);

                                    // ListViewItem für zukünftige Events erstellen
                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName, elapsedTimeFormat });
                                    listViewItem.ForeColor = fontColor;

                                    // Wenn mehrere Events zur gleichen Zeit und Kategorie, dann Schrift kursiv setzen
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

                        // ListView mit den aktualisierten Items aktualisieren
                        UpdateListViewItems(listViewItems);
                    }
                    catch (Exception ex)
                    {
                        // Fehlerbehandlung, falls eine Ausnahme auftritt
                        HandleException(ex, "UpdateBossList");
                    }
                });
            }


            private bool HasSameTimeAndCategory(List<BossEvent> allBosses, BossEvent currentBossEvent)
            {
                return allBosses.Any(bossEvent =>
                    bossEvent != currentBossEvent &&
                    bossEvent.Timing == currentBossEvent.Timing &&
                    bossEvent.Category == currentBossEvent.Category &&
                    bossEvent.BossName != currentBossEvent.BossName);
            }

            private DateTime GetAdjustedTiming(TimeSpan bossTiming, TimeSpan repeatInterval)
            {
                DateTime adjustedTiming = DateTime.Today + bossTiming;

                while (adjustedTiming < DateTime.Now)
                {
                    adjustedTiming = adjustedTiming.Add(repeatInterval);
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

            private Color GetFontColor(BossEvent bossEvent, List<BossEvent> pastBosses)
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
