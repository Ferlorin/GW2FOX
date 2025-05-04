using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using static GW2FOX.BossTimerService;

// Aliases für eindeutige Verweise
using WpfImage = System.Windows.Controls.Image;
using WpfClipboard = System.Windows.Clipboard;
using WpfPoint = System.Windows.Point;
using WpfMouseEventArgs = System.Windows.Input.MouseWheelEventArgs;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using WpfSize = System.Windows.Size;
using Forms = System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private DispatcherTimer _bossTimer;
        private DateTime _lastResetDate = DateTime.MinValue;
        public static OverlayWindow GetInstance() => _instance ??= new OverlayWindow();

        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();

        public OverlayWindow()
        {
            InitializeComponent();
            DataContext = this;
            _instance = this;

            this.Left = 1320;
            this.Top = 710;

            PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;
            StartBossTimer();
            UpdateBossOverlayListAsync();
            StartResetTimer();
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


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

        public async void UpdateBossOverlayListAsync()
        {
            try
            {
                var runs = await Task.Run(() => GetBossRunsForOverlay());
                var items = await Task.Run(() => BossOverlayHelper.GetBossOverlayItems(runs, DateTime.Now));

                Dispatcher.Invoke(() =>
                {
                    double oldOffset = BossScrollViewer.VerticalOffset;

                    // Vorherige Einträge merken
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
                            item.LoadChestState(); // 🔥 Hier: Zustand aus JSON laden und UI zwingen zu aktualisieren
                        }

                        OverlayItems.Add(item);
                    }

                    BossScrollViewer.ScrollToVerticalOffset(oldOffset);
                    
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }


        private void Waypoint_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.10);
        }

        private void Waypoint_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.0);
        }

        private void Waypoint_MouseDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;
            }
        }

        private void Waypoint_MouseUp(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;
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


        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is BossListItem item)
            {
                string clipboardText;

                if (item.IsPastEvent)
                {
                    int minutesAgo = (int)Math.Round((DateTime.Now - item.NextRunTime).TotalMinutes);
                    clipboardText = $"\"{item.BossName}\" at {item.Waypoint} started before {minutesAgo}min";
                }
                else
                {
                    TimeSpan remaining = item.NextRunTime - DateTime.Now;
                    string timeFormatted = remaining.Hours > 0
                        ? $"{remaining.Hours}h {remaining.Minutes}min"
                        : $"{remaining.Minutes}min";

                    clipboardText = $"\"{item.BossName}\" at {item.Waypoint} in {timeFormatted}";
                }

                WpfClipboard.SetText(clipboardText);

                var position = fe.TransformToAncestor(this).Transform(new WpfPoint(0, 0));
                CopiedMessage.Measure(new WpfSize(double.PositiveInfinity, double.PositiveInfinity));

                double left = position.X + (fe.ActualWidth / 2) - (CopiedMessage.DesiredSize.Width / 2);
                double top = position.Y - CopiedMessage.DesiredSize.Height - 18;

                Canvas.SetLeft(CopiedMessage, left);
                Canvas.SetTop(CopiedMessage, top);
                CopiedMessage.Visibility = Visibility.Visible;

                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
                timer.Tick += (s, _) =>
                {
                    CopiedMessage.Visibility = Visibility.Collapsed;
                    timer.Stop();
                };
                timer.Start();
            }
        }



        private void Chest_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 0.90);
                img.Opacity = 0.7;
            }
        }

        private void Chest_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 1.10);
                img.Opacity = 1.0;

                if (e.ChangedButton == MouseButton.Right)
                {
                    var result = System.Windows.MessageBox.Show("Reset all chests?", "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        BossTimings.ResetAllChestStates();
                        foreach (var item in OverlayItems)
                        {
                            item.ChestOpened = false;
                        }
                    }
                }
            }
        }


        private void Chest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            if (sender is FrameworkElement fe && fe.DataContext is BossListItem item)
            {
                item.ChestOpened = !item.ChestOpened;

                BossTimings.SetChestState(item.BossName, item.ChestOpened);

                // 🔥 Wenn LLA-Boss: alle 3 LLA-Einträge im Overlay updaten
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
                            otherItem.TriggerIconUpdate(); // optional, falls UI-Iconwechsel notwendig
                        }
                    }
                }
                else
                {
                    item.TriggerIconUpdate();
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

        private void Chest_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.10);
        }

        private void Chest_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is WpfImage img)
                AnimateScale(img, 1.0);
        }


        private void BloodyCom_Click(object sender, MouseButtonEventArgs e)
        {
            _overlayWindow = OverlayWindow.GetInstance();
            _overlayWindow.ToggleAllWindows();
        }


        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                BloodyCom_Click(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    DragMove();
                }
                catch (InvalidOperationException)
                {
                    // Optional: Log oder ignorieren
                }
            }
        }



        public void ToggleAllWindows()
        {
            var excludedTypes = new[] { typeof(MiniOverlay), typeof(Main) };
            var topMostStates = new Dictionary<Forms.Form, bool>();
            bool anyToggled = false;

            // Alle offenen Forms entsperren (TopMost deaktivieren)
            foreach (Forms.Form f in Forms.Application.OpenForms)
            {
                topMostStates[f] = f.TopMost;
                f.TopMost = false;
            }

            // Alle Forms durchgehen und anzeigen/verstecken
            foreach (Forms.Form openForm in Forms.Application.OpenForms)
            {
                if (!excludedTypes.Contains(openForm.GetType()))
                {
                    if (openForm.Visible)
                    {
                        openForm.Hide();
                        anyToggled = true;
                    }
                    else
                    {
                        openForm.Show();
                        openForm.BringToFront();
                        openForm.Activate();
                        anyToggled = true;
                    }
                }
            }

            // Spezielle Behandlung für Worldbosses, falls noch nie angezeigt
            if (BossTimerService.WorldbossesInstance is { } wb && !excludedTypes.Contains(wb.GetType()))
            {
                if (!Forms.Application.OpenForms.Cast<Forms.Form>().Contains(wb))
                {
                    if (wb.Visible)
                    {
                        wb.Hide();
                        anyToggled = true;
                    }
                    else
                    {
                        wb.Show();
                        wb.BringToFront();
                        wb.Activate();
                        anyToggled = true;
                    }
                }
            }

            // Ursprüngliche TopMost-Werte wiederherstellen
            foreach (var kvp in topMostStates)
                kvp.Key.TopMost = kvp.Value;

            // GW2 in den Vordergrund bringen, falls etwas getoggelt wurde
            if (anyToggled)
            {
                var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
                if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(gw2Proc.MainWindowHandle);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "No toggleable windows found.\nMaybe they're already gone? 🧐",
                    "Nothing To Toggle", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }




        private void StartBossTimer()
        {
            _bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _bossTimer.Tick += (s, e) => UpdateBossOverlayListAsync();
            _bossTimer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
