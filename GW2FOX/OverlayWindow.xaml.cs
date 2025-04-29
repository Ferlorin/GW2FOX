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

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private DispatcherTimer _bossTimer;

        public static OverlayWindow GetInstance() => _instance ??= new OverlayWindow();

        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();

        public OverlayWindow()
        {
            InitializeComponent();
            DataContext = this;
            _instance = this;

            this.Left = 1320;
            this.Top = 710;

            Loaded += (s, e) => Console.WriteLine("[DEBUG] Window Loaded.");
            BossTimings.RegisterListView(BossListView);

            PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;
            StartBossTimer();
            UpdateBossOverlayListAsync();
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
                    OverlayItems.Clear();
                    foreach (var item in items)
                        OverlayItems.Add(item);
                    BossScrollViewer.ScrollToVerticalOffset(oldOffset);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }

        private void Waypoint_MouseLeftButtonDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img && img.DataContext is BossListItem boss)
            {
                WpfClipboard.SetText(boss.Waypoint);
                var pos = img.TranslatePoint(new WpfPoint(0, img.ActualHeight), this);
                ShowCopiedMessage(pos);
            }
        }

        private void ShowCopiedMessage(WpfPoint position)
        {
            CopiedMessage.Visibility = Visibility.Visible;
            Canvas.SetLeft(CopiedMessage, position.X);
            Canvas.SetTop(CopiedMessage, position.Y - 40);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Tick += (s, e) =>
            {
                CopiedMessage.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
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
