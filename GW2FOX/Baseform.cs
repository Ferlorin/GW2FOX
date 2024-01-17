using System.Diagnostics;
using static GW2FOX.BossTimings;
using static GW2FOX.GlobalVariables;

namespace GW2FOX
{
    public class BaseForm : Form
    {

        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook? _globalKeyboardHook; // Füge dies hinzu

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
            if (ModifierKeys == Keys.Alt && e.Key == Keys.T)
            {
                if (this is Main)
                {
                    Timer_Click(sender, e);
                }
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
            newForm.Owner = this;
            newForm.Show();
            if (this is not Worldbosses)
            {
                Dispose();
            }
        }

        protected static void SaveTextToFile(string textToSave, string sectionHeader, bool hideMessages = false)
        {
            var headerToUse = sectionHeader;
            if (headerToUse.EndsWith(':'))
            {
                headerToUse = headerToUse[..^1];
            }

            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(FILE_PATH);

                // Index der Zeile mit dem angegebenen Header finden
                int headerIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(headerToUse + ":"))
                    {
                        headerIndex = i;
                        break;
                    }
                }

                // Wenn der Header gefunden wird, den Text aktualisieren
                if (headerIndex != -1)
                {
                    lines[headerIndex] = $"{headerToUse}: \"{textToSave}\"";
                }
                else
                {
                    // Wenn der Header nicht gefunden wird, eine neue Zeile hinzufügen
                    lines = lines.Concat(new[] { $"{headerToUse}: \"{textToSave}\"" }).ToArray();
                }

                // Aktualisierten Inhalt zurück in die Datei schreiben
                File.WriteAllLines(FILE_PATH, lines);

                if (!hideMessages)
                {
                    MessageBox.Show($"{headerToUse} saved.", "Saved!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {headerToUse}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void AdjustWindowSize()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle workingArea = currentScreen.WorkingArea;

            if (Width > workingArea.Width || Height > workingArea.Height)
            {
                Size = new Size(
                    Math.Min(Width, workingArea.Width),
                    Math.Min(Height, workingArea.Height)
                );

                Location = new Point(
                    workingArea.Left + (workingArea.Width - Width) / 2,
                    workingArea.Top + (workingArea.Height - Height) / 2
                );
            }

            if (Height > workingArea.Height)
            {
                Height = workingArea.Height;
            }

            if (Bottom > workingArea.Bottom)
            {
                Location = new Point(
                    Left,
                    Math.Max(workingArea.Top, workingArea.Bottom - Height)
                );
            }
        }
        protected void HandleException(Exception ex, string methodName)
        {
            Console.WriteLine($"Exception in {methodName}: {ex}");
        }

        private void LoadTextFromConfig(string sectionHeader, TextBox textBox, string configText,
            string defaultToInsert)
        {
            // Suchmuster für den Abschnitt und den eingeschlossenen Text in Anführungszeichen
            string pattern = $@"{sectionHeader}\s*""([^""]*)""";

            // Mit einem regulären Ausdruck nach dem Muster suchen
            var match = System.Text.RegularExpressions.Regex.Match(configText, pattern);

            // Überprüfen, ob ein Treffer gefunden wurde
            if (match.Success)
            {
                // Den extrahierten Text in das Textfeld einfügen
                textBox.Text = match.Groups[1].Value;
            }
            else
            {
                SaveTextToFile(defaultToInsert, sectionHeader, true);
                configText = File.ReadAllText(FILE_PATH);
                LoadTextFromConfig(sectionHeader, textBox, configText, defaultToInsert);
                // Muster wurde nicht gefunden
                // MessageBox.Show($"Das Muster '{sectionHeader}' wurde in der Konfigurationsdatei nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
        protected void BringGw2ToFront()
        {
            try
            { 
                // Specify the process name without the file extension
                string processName = "Gw2-64";

                // Get the processes by name
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    // Bring the first instance to the foreground
                    IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                    ShowWindow(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                }
                else
                {
                    MessageBox.Show("Gw2-64.exe is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error bringing Gw2-64.exe to the foreground: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        protected void LoadConfigText(TextBox Runinfo, TextBox Squadinfo, TextBox Guild, TextBox Welcome, TextBox Symbols)
        {
            try
            {

                // Überprüfen, ob die Datei existiert
                if (File.Exists(FILE_PATH))
                {
                    // Den gesamten Text aus der Datei lesen
                    string configText = File.ReadAllText(FILE_PATH);

                    // Laden von Runinfo
                    LoadTextFromConfig("Runinfo:", Runinfo, configText, DEFAULT_RUN_INFO);

                    // Laden von Squadinfo
                    LoadTextFromConfig("Squadinfo:", Squadinfo, configText, DEFAULT_SQUAD_INFO);

                    // Laden von Guild
                    LoadTextFromConfig("Guild:", Guild, configText, DEFAULT_GUILD);

                    // Laden von Welcome
                    LoadTextFromConfig("Welcome:", Welcome, configText, DEFAULT_WELCOME);

                    // Laden von Symbols
                    LoadTextFromConfig("Symbols:", Symbols, configText, DEFAULT_SYMBOLS);
                }
                else
                {
                    
                    Console.WriteLine($"Config file does not exist. Will try to create it");
                    try
                    {
                        var fileStream = File.Create(FILE_PATH);
                        fileStream.Close();
                        LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception, but don't call ReadConfigFile recursively
                        Console.WriteLine($"Error creating config file: {ex.Message}");
                        throw; // Re-throw the exception to prevent infinite recursion
                    }
                    // // Die Konfigurationsdatei existiert nicht
                    // MessageBox.Show("Die Konfigurationsdatei 'config.txt' wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Fehler beim Laden der Konfigurationsdatei
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bossTimer.Dispose(); // Dispose of the BossTimer first
            base.OnFormClosing(e);
            Application.Exit();
        }
        
        protected void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }


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
                mezTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
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
                    HandleException(ex, "TimerCallback");
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
                        List<string> bossNamesFromConfig = BossList23;

                        DateTime currentTimeUtc = DateTime.UtcNow;
                        DateTime currentTimeMez = TimeZoneInfo.ConvertTimeFromUtc(currentTimeUtc, mezTimeZone);
                        TimeSpan currentTime = currentTimeMez.TimeOfDay;


                        var upcomingBosses = BossEventGroups
                            .Where(bossEventGroup => bossNamesFromConfig.Contains(bossEventGroup.BossName))
                            .SelectMany(bossEventGroup => bossEventGroup.GetNextRuns())
                            .ToList();
                            
                            // Events
                            // .Where(bossEvent =>
                            //     bossNamesFromConfig.Contains(bossEvent.BossName) &&
                            //     bossEvent.Timing > currentTime && bossEvent.Timing < currentTime.Add(new TimeSpan(24, 0, 0)))
                            // .ToList();

                        var pastBosses =  BossEventGroups
                                .Where(bossEventGroup => bossNamesFromConfig.Contains(bossEventGroup.BossName))
                                .SelectMany(bossEventGroup => bossEventGroup.GetPreviousRuns())
                                .ToList();
                        // BossTimings.Events
                        //     .Where(bossEvent =>
                        //         bossNamesFromConfig.Contains(bossEvent.BossName) &&
                        //         bossEvent.Timing > currentTime.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < currentTime)
                        //     .ToList();

                        // Combine all bosses
                        var allBosses = upcomingBosses.Concat(pastBosses).ToList();

                        var listViewItems = new List<ListViewItem>();

                        // Use a HashSet to keep track of added boss names
                        HashSet<string> addedBossNames = new HashSet<string>();

                        allBosses.Sort((bossEvent1, bossEvent2) =>
                        {
                            // DateTime adjustedTiming1 = currentTimeMez.Date + bossEvent1.Timing;
                            // DateTime adjustedTiming2 = currentTimeMez.Date + bossEvent2.Timing;

                            // Compare the adjusted timings for the next day
                            int adjustedTimingComparison = bossEvent1.NextRunTime.CompareTo(bossEvent2.NextRunTime);
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
                            // DateTime adjustedTiming = GetAdjustedTiming(currentTimeMez, bossEvent.Timing);

                            // Check if the boss with the adjusted timing is already added to avoid duplicates
                            if (addedBossNames.Add($"{bossEvent.BossName}_{bossEvent.NextRunTime}"))
                            {
                                if (pastBosses.Contains(bossEvent) && currentTimeMez - bossEvent.NextRunTime < new TimeSpan(0, 14, 59))
                                {
                                    // Display only the boss name for past events within the time span of 00:14:59
                                    var listViewItem = new ListViewItem(new[] { bossEvent.BossName });
                                    listViewItem.ForeColor = GetFontColor(bossEvent, pastBosses);
                                    listViewItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    listViewItems.Add(listViewItem);
                                }
                                else
                                {
                                    TimeSpan elapsedTime = bossEvent.NextRunTime - currentTimeMez;

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

            private bool HasSameTimeAndCategory(List<BossEventRun> allBosses, BossEventRun currentBossEvent)
            {
                return allBosses.Any(bossEvent =>
                    bossEvent != currentBossEvent &&
                    bossEvent.NextRunTime == currentBossEvent.NextRunTime &&
                    bossEvent.Category == currentBossEvent.Category &&
                    bossEvent.BossName != currentBossEvent.BossName);
            }



            private DateTime GetAdjustedTiming(DateTime currentTimeMez, TimeSpan bossTiming)
            {
                DateTime adjustedTiming = currentTimeMez.Date + bossTiming;
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

            private TimeSpan GetRemainingTime(DateTime currentTimeMez, DateTime adjustedTiming, BossEventRun bossEvent)
            {
                return adjustedTiming - currentTimeMez;
            }

            private static Color GetFontColor(BossEventRun bossEvent, List<BossEventRun> pastBosses)
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

                if (pastBosses.Any(pastBoss => pastBoss.BossName == bossEvent.BossName && pastBoss.NextRunTime == bossEvent.NextRunTime))
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
