using System.Reflection;
using static GW2FOX.BossTimings;

namespace GW2FOX
{
    public static class BossTimerService
    {
        private static readonly string TimeZoneId = "W. Europe Standard Time";

        public static Overlay? _overlay { get; set; }
        public static BossTimer? _bossTimer { get; set; }
        private static ListView CustomBossList { get; set; } = new();
        public static ListViewExtender extender { get; private set; }

        private static ToolTip toolTip = new ToolTip();






        static BossTimerService()
        {
            Initialize();
        }

        public static void UpdateCustomBossList(ListView updatedList)
        {
            CustomBossList = updatedList;
        }

        private static void InitializeBossTimerAndOverlay()
        {
            _bossTimer = new BossTimer(CustomBossList);

            if (_overlay is { IsDisposed: false }) return;
            _overlay = new Overlay(CustomBossList);
            _overlay.WindowState = FormWindowState.Normal;
        }

        private static void InitializeCustomBossList()
        {
            CustomBossList = new ListView();

            // Activate double buffering for the ListView
            SetDoubleBuffered(CustomBossList);

            // CustomBossList.ShowItemToolTips = true;
            CustomBossList.View = View.Details;
            CustomBossList.Location = new Point(0, 0);
            CustomBossList.ForeColor = Color.White;
            CustomBossList.FullRowSelect = true;
            CustomBossList.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            CustomBossList.Columns.Add("Button", 20, HorizontalAlignment.Left);
            CustomBossList.Columns.Add("Boss Name", 138, HorizontalAlignment.Left);
            var timeHeader = CustomBossList.Columns.Add("Time");
            timeHeader.TextAlign = HorizontalAlignment.Right;
            CustomBossList.FullRowSelect = true;
            extender = new ListViewExtender(CustomBossList);
            var listViewButtonColumn = new ListViewButtonColumn(0);
            listViewButtonColumn.Click += ListView_MouseClick;
            extender.AddColumn(listViewButtonColumn);
        }


        private static void ListView_MouseClick(object? sender, ListViewColumnMouseEventArgs e)
        {
            var listViewButtonColumn = sender as ListViewButtonColumn;
            var listView = listViewButtonColumn.ListView;
            var selectedItem = listView?.GetItemAt(e.X, e.Y);
            if (listView == null) return;
            if (selectedItem is not { Tag: BossEventRun bossEvent }) return;

            if (e.Button == MouseButtons.Left)
            {
                // Assuming each ListViewItem.Tag holds the corresponding BossEventRun
                if (bossEvent.Waypoint.Equals("")) return;
                var textToCopy = bossEvent.Waypoint; // Use 'waypoint' property of BossEventRun instead.
                Clipboard.SetText(textToCopy);

                // Display "Copied" message
                ShowCopiedMessage();
            }
            else if (e.Button == MouseButtons.Right)
            {
                var dialogResult = MessageBox.Show("Delete " + bossEvent.BossName + "?",
                                                   "Confirm Delete",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);
                if (dialogResult != DialogResult.Yes) return;
                Worldbosses.RemoveBossNameFromConfig(bossEvent.BossName);
            }
        }

        private static void ShowCopiedMessage()
        {
            // Erstelle ein kleines, transparentes Popup-Fenster
            Form toast = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost = true,
                BackColor = Color.Black,
                Opacity = 0.8,
                Size = new Size(50, 20),
                Location = new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10)
            };

            Label label = new Label
            {
                Text = "Copied",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            toast.Controls.Add(label);

            // Schließe das Fenster nach 1 Sekunde
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 300 };

            timer.Tick += (s, e) =>
            {
                toast.Close();
                timer.Dispose();
            };

            // Zeige das Fenster
            toast.Load += (s, e) => timer.Start();
            toast.Show();
        }




        public static void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, control, new object[] { true });
        }

        private static void MyMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var contextMenu = (ContextMenuStrip)menuItem.Owner!;
            var owner = contextMenu.SourceControl;
            var listView = (ListView)owner!;

            if (listView.SelectedItems.Count <= 0) return;
            var listViewItem = listView.SelectedItems[0];
            var bossEvent = (BossEventRun)listViewItem.Tag!;

            if (DoneBosses.ContainsKey(bossEvent.NextRunTime.Date))
            {
                DoneBosses[bossEvent.NextRunTime.Date].Add(bossEvent.BossName);
            }
            else
            {
                DoneBosses.Add(bossEvent.NextRunTime.Date, [bossEvent.BossName]);
            }

        }

        public static void Timer_Click(object sender, EventArgs e)
        {

            Update();
        }


        private static void Initialize()
        {
            InitializeCustomBossList();
            if (_overlay == null || _overlay.IsDisposed)
            {
                InitializeBossTimerAndOverlay();
            }
        }

        public static void Update()
        {
            if (_bossTimer == null)
            {
                Initialize();
            }

            _bossTimer?.Start();

            GC.KeepAlive(_bossTimer);

            if (_overlay != null && !_overlay.Visible)
            {
                _overlay.Show();
            }
        }




        public class BossTimer : IDisposable
        {
            public System.Threading.Timer Timer => _timer;

            private bool _isRunning = false;

            public bool IsRunning => _isRunning;

            private readonly ListView _bossList;
            private readonly TimeZoneInfo _mezTimeZone;
            private readonly System.Threading.Timer _timer;

            public BossTimer(ListView bossList)
            {
                this._bossList = bossList;
                _mezTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                _timer = new System.Threading.Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            }

            public void Start()
            {
                if (!_isRunning)
                {
                    Console.WriteLine("Starting Timer");
                    _timer.Change(0, 1000);  // Start the timer
                    _isRunning = true;
                }
            }

            public void Stop()
            {
                if (_isRunning)
                {
                    Console.WriteLine("Stopping Timer");
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);  // Stop the timer
                    _isRunning = false;
                }
            }

            private void TimerCallback(object? state)
            {
                Console.WriteLine("Timer Tick: " + DateTime.Now);
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
                if (!_bossList.IsHandleCreated) return;

                _bossList.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate
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



            private void UpdateListViewItems(List<ListViewItem> listViewItems)
            {
                _bossList.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate

                {
                    try
                    {
                        if (_bossList.IsHandleCreated)
                        {
                            _bossList.BeginUpdate();
                            _bossList.Items.Clear();
                            _bossList.Items.AddRange(listViewItems.ToArray());
                            _bossList.EndUpdate();
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

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _timer.Dispose();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposing) return;
                // _bossTimer = null;
                _timer.Dispose();
            }
        }
    }

}