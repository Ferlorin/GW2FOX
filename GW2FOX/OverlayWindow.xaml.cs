using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using static GW2FOX.BossTimerService;
using static GW2FOX.OverlayWindow;
using static System.Windows.Forms.AxHost;
using Forms = System.Windows.Forms;
using WpfButton = System.Windows.Controls.Button;
using WpfClipboard = System.Windows.Clipboard;
// Aliases für eindeutige Verweise
using WpfImage = System.Windows.Controls.Image;
using WpfMessageBox = System.Windows.MessageBox;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using WpfMouseEventArgs = System.Windows.Input.MouseWheelEventArgs;
using WpfPoint = System.Windows.Point;
using WpfSize = System.Windows.Size;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private bool isResizing = false;
        private WpfPoint clickPosition;
        public readonly BossDataManager bossDataManager = new();
        private static OverlayWindow? _instance;
        private DispatcherTimer _bossTimer;
        private DateTime _lastResetDate = DateTime.MinValue;
        public static OverlayWindow GetInstance() => _instance ??= new OverlayWindow();
        private TreasureHunterMiniOverlay _miniOverlay;
        private Smilies _miniOverlay2;
        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();

        public OverlayWindow()
        {
            InitializeComponent();
            DataContext = this;
            _instance = this;

            this.Left = 1330;
            this.Top = 710;
            this.Width = 250;
            this.Height = 350;

            PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;
            StartBossTimer();
            StartResetTimer();
            _ = InitializeOverlayAsync();
        }

        private async Task InitializeOverlayAsync()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => DynamicEventManager.CleanupExpiredEvents();
            timer.Start();
            UpdateBossOverlayListAsync();
            bossDataManager.InitializeAsync();
        }



        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public class BossDataManager
        {
            public Dictionary<string, List<LootHelper.LootResult>> GroupedLoot { get; set; }
            public bool IsLootLoaded { get; set; }

            public readonly LootHelper lootHelper = new();

            public async Task InitializeAsync()
            {
                GroupedLoot = await lootHelper.LoadLootGroupedByBossAsync();
                IsLootLoaded = true;
            }
        }

        private void StartResetTimer()
        {
            DispatcherTimer resetTimer = new() { Interval = TimeSpan.FromMinutes(1) };
            resetTimer.Tick += (s, e) => CheckDailyChestReset();
            resetTimer.Start();
        }

        private void OverlayWindow_PreviewMouseWheel(object sender, WpfMouseEventArgs e)
        {
            var position = e.GetPosition(BossListView);
            var element = BossListView.InputHitTest(position) as DependencyObject;

            while (element != null && !(element is System.Windows.Controls.ListViewItem))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            if (element is System.Windows.Controls.ListViewItem)
            {
                double multiplier = 3.0;
                double offset = BossScrollViewer.VerticalOffset - e.Delta / 120.0 * multiplier;
                BossScrollViewer.ScrollToVerticalOffset(Math.Max(0, Math.Min(BossScrollViewer.ScrollableHeight, offset)));
                e.Handled = true;
            }
        }

        private void CustomScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BossScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

        public async Task UpdateTreasureDataAsync()
        {
            try
            {
                var lootHelper = new LootHelper(); // Liest IDs aus BossTimings.json
                var groupedLoot = await lootHelper.LoadLootGroupedByBossAsync();

                var jObject = new JObject();

                foreach (var bossGroup in groupedLoot)
                {
                    var lootArray = new JArray();
                    foreach (var loot in bossGroup.Value)
                    {
                        lootArray.Add(new JObject
                        {
                            ["Name"] = loot.Name,
                            ["ChatLink"] = loot.ChatLink,
                            ["Price"] = loot.FormattedPrice,
                            ["BossName"] = loot.BossName
                        });
                    }
                    jObject[bossGroup.Key] = lootArray;
                }

                string filePath = "BossTimings.json";
                JObject json;

                if (File.Exists(filePath))
                {
                    var text = await File.ReadAllTextAsync(filePath);
                    json = JObject.Parse(text);
                }
                else
                {
                    json = new JObject();
                }

                json["Treasures"] = jObject;
                await File.WriteAllTextAsync(filePath, json.ToString(Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Treasure-Update: {ex.Message}");
            }
        }



        private void AnimateScale(WpfImage img, double toScale, double durationMs = 100)
        {
            if (img.RenderTransform is not ScaleTransform scale)
                return;

            // Freeze fix
            if (scale.IsFrozen)
            {
                scale = scale.Clone();
                img.RenderTransform = scale;
            }

            var animX = new DoubleAnimation
            {
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            var animY = animX.Clone();

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
        }


        private async void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            if (!bossDataManager.IsLootLoaded)
            {
                return;
            }

            if (sender is FrameworkElement fe && fe.DataContext is BossListItem item)
            {
                string clipboardText;

                string levelText = item.Category switch
                {
                    "WBs" => $"☠ Level {item.Level} ",
                    "Treasures" => $"☠ Level {item.Level} ",
                    _ => string.Empty
                };

                if (item.IsPastEvent)
                {
                    int minutesAgo = (int)Math.Round((DateTime.Now - item.NextRunTime).TotalMinutes);

                    // Angepasste Ausgabe für Events, die vor weniger als einer Minute gestartet sind
                    if (minutesAgo == 0)
                    {
                        clipboardText = $"{levelText}\"{item.BossName}\" @ {item.Waypoint} started now";
                    }
                    else
                    {
                        clipboardText = $"{levelText}\"{item.BossName}\" @ {item.Waypoint} started before {minutesAgo}min";
                    }
                }
                else
                {
                    TimeSpan remaining = item.NextRunTime - DateTime.Now;
                    int displayMinutes = (int)Math.Ceiling(remaining.TotalMinutes);

                    if (displayMinutes >= -1 && displayMinutes <= 1)
                    {
                        clipboardText = $"{levelText}\"{item.BossName}\" @ {item.Waypoint} is about to start soon";
                    }
                    else
                    {
                        string timeFormatted = remaining.Hours > 0
                            ? $"{remaining.Hours}h {displayMinutes % 60}min"
                            : $"{displayMinutes}min";

                        string timePrefix = item.Category == "Treasures" ? "is running about the next " : "in ca ";
                        clipboardText = $"{levelText}\"{item.BossName}\" @ {item.Waypoint} {timePrefix}{timeFormatted}";
                    }
                }

                if (bossDataManager.GroupedLoot.TryGetValue(item.BossName, out var lootItems) && lootItems.Any())
                {
                    var lootInfo = lootItems.Select(l => $"{l.ChatLink} cost {l.FormattedPrice}");
                    clipboardText += " ♥♥♥ Loot here: " + string.Join(" ", lootInfo);
                }

                WpfClipboard.SetText(clipboardText);

                var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(gw2Proc.MainWindowHandle);
                }
            }
        }
        }


        private async void Chest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            if (e.ChangedButton != MouseButton.Left)
                return;

            if (sender is FrameworkElement fe && fe.DataContext is BossListItem item)
            {
                item.ChestOpened = !item.ChestOpened;

                BossTimings.SetChestState(item.BossName, item.ChestOpened);

                var linkedBosses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "LLA Timberline",
            "LLA Iron Marches",
            "LLA Gendarran"
        };

                if (linkedBosses.Contains(item.BossName))
                {
                    foreach (var otherItem in OverlayItems)
                    {
                        if (linkedBosses.Contains(otherItem.BossName))
                        {
                            otherItem.ChestOpened = item.ChestOpened;
                            otherItem.TriggerIconUpdate(); 
                        }
                    }
                }
                else
                {
                    item.TriggerIconUpdate();
                }
            }
        }
        }



        private void CheckDailyChestReset()
        {
            var now = DateTime.UtcNow;
            if (_lastResetDate.Date == now.Date)
                return;

            // Täglich um 00:00 Uhr UTC zurücksetzen
            if (now.Hour == 0 && now.Minute < 5)
            {
                BossTimings.ResetAllChestStates();
                _lastResetDate = now.Date;

                foreach (var item in OverlayItems)
                {
                    item.ChestOpened = false;
                }
            }
        }


        private async void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            if (e.ChangedButton == MouseButton.Left)
            {
                    var wbForm = BossTimerService.WorldbossesInstance;
                    if (wbForm == null || wbForm.IsDisposed)
                    {
                        wbForm = new Worldbosses();
                        BossTimerService.WorldbossesInstance = wbForm;
                        wbForm.Show();
                    }
                    else if (wbForm.Visible)
                    {
                        wbForm.Hide();
                    }
                    else
                    {
                        wbForm.Show();
                        wbForm.BringToFront();
                    }
                  
            }

            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }
        }

        private void FoXXy_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch (InvalidOperationException)
                {
                    // Falls das Fenster während des Drag-Vorgangs geschlossen wird.
                }
            }
        }

    

        private async void SchedulerIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            // Hier eine Instanz der Klasse erstellen
            var textboxesInstance = new Textboxes();
            textboxesInstance.button67_Click(null, EventArgs.Empty);

            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }
        }

        private async void SmilieIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            // Schließe das andere Overlay, wenn es geöffnet ist
            _miniOverlay?.Close();
                _miniOverlay = null;

                if (_miniOverlay2 == null || !_miniOverlay2.Visible)
                {
                    _miniOverlay2 = new Smilies(this);

                    // Bildschirmkoordinaten des Icons ermitteln
                    WpfPoint iconPosition = img.PointToScreen(new WpfPoint(0, 0));

                    double iconWidth = img.ActualWidth;
                    double overlayWidth = _miniOverlay2.Width;

                    double targetLeft = iconPosition.X + (iconWidth / 2) - (overlayWidth / 2);
                    double targetTop = iconPosition.Y - 215;

                    _miniOverlay2.Left = (int)targetLeft;
                    _miniOverlay2.Top = (int)targetTop;

                    _miniOverlay2.Show();

                    // Fokus auf das GW2-Fenster setzen
                    var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                    if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                    {
                        SetForegroundWindow(gw2Proc.MainWindowHandle);
                    }
                }
                else
                {
                    _miniOverlay2.Close();
                    _miniOverlay2 = null;

                    var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                    if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                    {
                        SetForegroundWindow(gw2Proc.MainWindowHandle);
                    }
                }
            }
        }

        private async void TreasureHunterIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            

            // Schließe das andere Overlay, wenn es geöffnet ist
            _miniOverlay2?.Close();
                _miniOverlay2 = null;

                if (_miniOverlay == null || !_miniOverlay.IsLoaded)
                {
                    _miniOverlay = new TreasureHunterMiniOverlay(this);
                    WpfPoint iconPosition = img.PointToScreen(new WpfPoint(0, 0));
                    double iconWidth = img.ActualWidth;
                    double overlayWidth = _miniOverlay.Width;

                    double targetLeft = iconPosition.X + (iconWidth / 2) - (overlayWidth / 2);
                    double targetTop = iconPosition.Y - 230;

                    _miniOverlay.Left = targetLeft;
                    _miniOverlay.Top = targetTop;

                    _miniOverlay.Show();

                    var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                    if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                    {
                        SetForegroundWindow(gw2Proc.MainWindowHandle);
                    }

                    await UpdateTreasureDataAsync();
                }
                else
                {
                    _miniOverlay.Close();
                    _miniOverlay = null;

                    var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                    if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                    {
                        SetForegroundWindow(gw2Proc.MainWindowHandle);
                    }
                }
            }
        }



        private async void GroupSearchIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            }
            try
            {
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonPath = Path.Combine(exeDir, "BossTimings.json");

                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    var jsonObject = JObject.Parse(jsonContent);

                    string? guildText = jsonObject["Runinfo"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(guildText))
                    {
                        WpfClipboard.SetText(guildText);


                        var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                        if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                        {
                            SetForegroundWindow(gw2Proc.MainWindowHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Error loading Guild info: " + ex.Message);
            }
        }



        private async void GuildAdvertisingIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            }
            try
            {
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonPath = Path.Combine(exeDir, "BossTimings.json");

                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    var jsonObject = JObject.Parse(jsonContent);

                    string? guildText = jsonObject["Guild"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(guildText))
                    {
                        WpfClipboard.SetText(guildText);


                        var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                        if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                        {
                            SetForegroundWindow(gw2Proc.MainWindowHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Error loading Guild info: " + ex.Message);
            }
        }


        private async void GuildWelcomeIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            }
            try
            {
                string exeDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonPath = Path.Combine(exeDir, "BossTimings.json");

                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    var jsonObject = JObject.Parse(jsonContent);

                    string? guildText = jsonObject["Welcome"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(guildText))
                    {
                        WpfClipboard.SetText(guildText);

                        var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                        if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                        {
                            SetForegroundWindow(gw2Proc.MainWindowHandle);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Error loading Guild info: " + ex.Message);
            }
        }


        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.10);
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.0);
        }

        private async Task Image_MouseDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img) // Ensure 'img' is assigned before use
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                await Task.Delay(100); // Short delay to complete the animation
                AnimateScale(img, 1.0);
            }
        }


        private void ResizeTriangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isResizing = true;
                clickPosition = e.GetPosition(this);
                ResizeTriangle.CaptureMouse();
            }
        }

        private void ResizeTriangle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isResizing)
            {
                WpfPoint currentPosition = e.GetPosition(this);
                double newWidth = Math.Max(this.MinWidth, this.Width + (currentPosition.X - clickPosition.X));
                double newHeight = Math.Max(this.MinHeight, this.Height + (currentPosition.Y - clickPosition.Y));

                this.Width = newWidth;
                this.Height = newHeight;

                clickPosition = currentPosition;
            }
        }

        private void ResizeTriangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            ResizeTriangle.ReleaseMouseCapture();
        }



        private void StartBossTimer()
        {
            _bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _bossTimer.Tick += (s, e) => RefreshTimesOnly();
            _bossTimer.Start();
        }

        private void RefreshTimesOnly()
        {
            var now = GlobalVariables.CURRENT_DATE_TIME;
            bool needsRefresh = false;

            foreach (var item in OverlayItems.ToList())
            {
                item.UpdateTimeProperties(now);

                // Dynamisches Event abgelaufen → trigger Refresh
                if (item.IsDynamicEvent && item.NextRunTime <= now)
                {
                    needsRefresh = true;
                }

                // Normales Event verjährt (> 15 Minuten in der Vergangenheit) → entfernen
                if (!item.IsDynamicEvent && item.NextRunTime < now.AddMinutes(-15))
                {
                    OverlayItems.Remove(item);
                    needsRefresh = true;
                }
            }

            if (needsRefresh)
            {
                _ = UpdateBossOverlayListAsync(); // Fire & Forget
            }
        }



        public async Task UpdateBossOverlayListAsync()
        {
            try
            {
                var runs = await Task.Run(() => GetBossRunsForOverlay());
                var items = await Task.Run(() => GetBossOverlayItems(runs, GlobalVariables.CURRENT_DATE_TIME));

                // Use a temporary list to avoid modifying the collection while enumerating
                var updatedItems = new List<BossListItem>();

                foreach (var item in items)
                {
                    var previousItem = OverlayItems.FirstOrDefault(x => x.BossName == item.BossName);
                    if (previousItem != null)
                    {
                        item.ChestOpened = previousItem.ChestOpened;
                    }
                    else
                    {
                        item.LoadChestState();
                    }

                    updatedItems.Add(item);
                }

                Dispatcher.Invoke(() =>
                {
                    double oldOffset = BossScrollViewer.VerticalOffset;

                    var previousItems = OverlayItems.ToList();
                    OverlayItems.Clear();

                    foreach (var item in items)
                    {
                        var previous = previousItems.FirstOrDefault(x => x.BossName == item.BossName);
                        if (previous != null)
                        {
                            item.ChestOpened = previous.ChestOpened;
                        }
                        else
                        {
                            item.LoadChestState();
                        }

                        OverlayItems.Add(item);
                    }

                    BossScrollViewer.ScrollToVerticalOffset(oldOffset);
                });


            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRÖR] Exception in UpdateBossOverlayListAsync: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
