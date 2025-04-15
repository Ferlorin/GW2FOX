using System.Diagnostics;
using System.Drawing.Drawing2D;
using static GW2FOX.BossTimings;
using static GW2FOX.GlobalVariables;

namespace GW2FOX
{
    public class BaseForm : Form
    {

        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook? _globalKeyboardHook;

        public static ListView CustomBossList { get; private set; } = new ListView();



        public BaseForm()
        {
            InitializeCustomBossList();
            InitializeGlobalKeyboardHook();
            SetFormTransparency();
        }

        protected void SetFormTransparency()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.Opacity = 0.90;
            this.TopMost = true;
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
            customBossList = new DoubleBufferedListView(); // statt new ListView()
            customBossList.View = View.Details;
            customBossList.Columns.Add("Meta", 145);
            customBossList.Columns.Add("Time", 78);
            customBossList.Location = new Point(0, 0);
            customBossList.ForeColor = Color.White;
            customBossList.FullRowSelect = true;
            customBossList.OwnerDraw = true;

            customBossList.Font = new Font("Segoe UI", 10, FontStyle.Bold);
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
            this.Hide(); // Aktuelles Fenster ausblenden
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
                string[] lines = File.ReadAllLines(FILE_PATH);

                int headerIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(headerToUse + ":"))
                    {
                        headerIndex = i;
                        break;
                    }
                }

                if (headerIndex != -1)
                {
                    lines[headerIndex] = $"{headerToUse}: \"{textToSave}\"";
                }
                else
                {
                    lines = lines.Concat(new[] { $"{headerToUse}: \"{textToSave}\"" }).ToArray();
                }

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
            string pattern = $@"{sectionHeader}\s*""([^""]*)""";

            var match = System.Text.RegularExpressions.Regex.Match(configText, pattern);

            if (match.Success)
            {
                textBox.Text = match.Groups[1].Value;
            }
            else
            {
                SaveTextToFile(defaultToInsert, sectionHeader, true);
                configText = File.ReadAllText(FILE_PATH);
                LoadTextFromConfig(sectionHeader, textBox, configText, defaultToInsert);
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
                        Console.WriteLine($"Error creating config file: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bossTimer.Dispose();
            base.OnFormClosing(e);
            Application.Exit();
        }

       

        protected void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }

        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //

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

                // OwnerDraw aktivieren + Handler zuweisen
                bossList.OwnerDraw = true;
                bossList.DrawItem += BossList_DrawItem;
                bossList.DrawSubItem += BossList_DrawSubItem;
                bossList.DrawColumnHeader += BossList_DrawColumnHeader;
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

            private void BossList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
            {
                e.DrawDefault = true;
            }

            private void BossList_DrawItem(object sender, DrawListViewItemEventArgs e)
            {
                // Muss existieren, aber wird leer gelassen, weil SubItems separat gezeichnet werden
               
            }

            private void BossList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
            {
                e.DrawBackground();
                e.DrawDefault = false;

                Font font = new Font("Segoe UI", 10, FontStyle.Bold);
                string text = e.SubItem.Text;
                Rectangle bounds = e.SubItem.Bounds;

                // 👉 Hier: Die Farbe des ersten SubItems (Bossname) verwenden
                Color textColor = e.Item.SubItems[0].ForeColor;

                using (Graphics g = e.Graphics)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };

                    Rectangle textBounds = new Rectangle(bounds.X + 2, bounds.Y, bounds.Width - 4, bounds.Height);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, textBounds, format);

                        using (Pen outlinePen = new Pen(Color.Black, 2) { LineJoin = LineJoin.Round })
                        {
                            g.DrawPath(outlinePen, path);
                        }

                        using (Brush textBrush = new SolidBrush(textColor))
                        {
                            g.FillPath(textBrush, path);
                        }
                    }
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

                        var bossEventGroups = BossEventGroups
                            .Where(bossEventGroup => bossNamesFromConfig.Contains(bossEventGroup.BossName))
                            .ToList();

                        var allBosses = bossEventGroups
                            .SelectMany(bossEventGroup => bossEventGroup.GetNextRuns())
                            .ToList();


                        var listViewItems = new List<ListViewItem>();

                        // Use a HashSet to keep track of added boss names
                        HashSet<string> addedBossNames = new HashSet<string>();

                        allBosses.Sort((bossEvent1, bossEvent2) =>
                        {
                            // Compare the adjusted timings for the next day
                            int adjustedTimingComparison = bossEvent1.NextRunTime.CompareTo(bossEvent2.NextRunTime);
                            if (adjustedTimingComparison != 0) return adjustedTimingComparison;


                            // If durations and timings are equal, sort by categories (if necessary)
                            int categoryComparison = String.Compare(bossEvent1.Category, bossEvent2.Category, StringComparison.Ordinal);
                            if (categoryComparison != 0) return categoryComparison;

                            return 0;
                        });

                        foreach (var bossEvent in allBosses)
                        {
                            // Calculate the end time of the boss event based on the current time

                            var listViewItem = new ListViewItem("btn", 0);
                            listViewItem.SubItems.Add(bossEvent.BossName); // Hier wird ein Unterelement hinzugefügt
                            listViewItem.SubItems.Add(bossEvent.getTimeRemainingFormatted()); // Hier wird ein Unterelement hinzugefügt
                            listViewItem.ForeColor = bossEvent.getForeColor();
                            // listViewItem.ToolTipText =
                            //     "Left Click to copy the Waypoint to clipboard\nRight Click to remove from the list";
                            listViewItem.Tag = bossEvent;
                            foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems)
                            {
                                subItem.ForeColor = listViewItem.ForeColor;
                            }


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
