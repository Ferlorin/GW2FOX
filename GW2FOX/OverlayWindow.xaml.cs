using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using static GW2FOX.BossTimerService;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private bool _isDragging = false;
        private System.Windows.Point _clickPosition;
        private double _thumbStartTop;
        private double _startOffsetTop = 0;
        private DispatcherTimer _bossTimer;
        private double _scrollValue;
        private bool isDragging = false;
        private System.Windows.Point clickPosition;

        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();

        public double ScrollValue
        {
            get => _scrollValue;
            set
            {
                if (_scrollValue != value)
                {
                    _scrollValue = value;
                    BossScrollViewer.ScrollToVerticalOffset(value);
                    OnPropertyChanged(nameof(ScrollValue));
                }
            }
        }

        public OverlayWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Einmalige Bindung an die ObservableCollection
            BossListView.ItemsSource = OverlayItems;

            this.Left = 1320;
            this.Top = 710;
            _instance = this;
            Loaded += Window_Loaded;
            PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;

            BossTimings.RegisterListView(BossListView);
            StartBossTimer();
            UpdateBossOverlayListAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (BossListView.Items.Count > 0)
            {
                // Das erste ItemContainer besorgen (visuelles Objekt der ersten Zeile)
                var item = BossListView.ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement;
                if (item != null)
                {
                    var position = item.TranslatePoint(new System.Windows.Point(0, 0), BossListView);
                    _startOffsetTop = position.Y;
                }
            }

            UpdateThumbPosition();
        }


        private void OverlayWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (BossScrollViewer.IsMouseOver || IsActive)
            {
                double multiplier = 3.0;
                double offset = BossScrollViewer.VerticalOffset - (e.Delta / 120.0 * multiplier);
                offset = Math.Max(0, Math.Min(BossScrollViewer.ScrollableHeight, offset));
                BossScrollViewer.ScrollToVerticalOffset(offset);
                ScrollValue = offset;
                UpdateThumbPosition();
                e.Handled = true;
            }
        }

        private void BossScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateThumbPosition();
        }

        private void UpdateThumbPosition()
        {
            if (BossScrollViewer.ExtentHeight <= BossScrollViewer.ViewportHeight)
            {
                Canvas.SetTop(ScrollThumb, _startOffsetTop);
                return;
            }

            double trackHeight = BossListView.ActualHeight;
            double ratio = BossScrollViewer.ViewportHeight / BossScrollViewer.ExtentHeight;
            double thumbHeight = Math.Max(30, ratio * trackHeight);
            ScrollThumb.Height = thumbHeight;

            double maxMove = trackHeight - thumbHeight;
            double percent = BossScrollViewer.VerticalOffset / (BossScrollViewer.ExtentHeight - BossScrollViewer.ViewportHeight);
            double calculatedTop = _startOffsetTop + (percent * maxMove);

            // Nur wenn nicht gerade gezogen wird, neu positionieren
            if (!_isDragging)
                Canvas.SetTop(ScrollThumb, calculatedTop);
        }

        private void StartBossTimer()
        {
            _bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _bossTimer.Tick += (s, e) => UpdateBossOverlayListAsync();
            _bossTimer.Start();
        }


        private void Thumb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) // <-- Wichtig!
        {
            if (!isDragging)
            {
                isDragging = true;
                clickPosition = e.GetPosition(MainCanvas);
                ScrollThumb.CaptureMouse();
            }
        }

        private void Thumb_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) // <-- Wichtig!
        {
            if (isDragging)
            {
                System.Windows.Point currentPosition = e.GetPosition(MainCanvas);
                double deltaY = currentPosition.Y - clickPosition.Y;

                double newTop = Canvas.GetTop(ScrollThumb) + deltaY;

                double minTop = 0;
                double maxTop = MainCanvas.ActualHeight - ScrollThumb.ActualHeight;
                newTop = Math.Max(minTop, Math.Min(newTop, maxTop));

                Canvas.SetTop(ScrollThumb, newTop);

                double scrollableHeight = BossScrollViewer.ScrollableHeight;
                if (scrollableHeight > 0)
                {
                    double scrollOffset = (newTop / maxTop) * scrollableHeight;
                    BossScrollViewer.ScrollToVerticalOffset(scrollOffset);
                }

                clickPosition = currentPosition;
            }
        }

        private void Thumb_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) // <-- Wichtig!
        {
            if (isDragging)
            {
                isDragging = false;
                ScrollThumb.ReleaseMouseCapture();
            }
        }


        public async void UpdateBossOverlayListAsync()
        {
            try
            {
                var runs = await Task.Run(() => GetBossRunsForOverlay());
                var items = await Task.Run(() => BossOverlayHelper.GetBossOverlayItems(runs, DateTime.Now));
                Dispatcher.Invoke(() =>
                {
                    // 1) Aktuellen Scroll-Offset merken
                    double currentOffset = BossScrollViewer.VerticalOffset;

                    // 2) ObservableCollection neu befüllen
                    OverlayItems.Clear();
                    foreach (var item in items)
                        OverlayItems.Add(item);

                    // 3) Scroll-Offset wiederherstellen
                    BossScrollViewer.ScrollToVerticalOffset(currentOffset);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint);
                var pos = img.TranslatePoint(new System.Windows.Point(0, img.ActualHeight), this);
                ShowCopiedMessage(pos);
            }
        }

        private void ShowCopiedMessage(System.Windows.Point position)
        {
            CopiedMessage.Visibility = Visibility.Visible;
            Canvas.SetLeft(CopiedMessage, position.X);
            Canvas.SetTop(CopiedMessage, position.Y - 40);

            var t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
            t.Tick += (s, e) => { CopiedMessage.Visibility = Visibility.Collapsed; t.Stop(); };
            t.Start();
        }


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        public static OverlayWindow GetInstance() => _instance ??= new OverlayWindow();

        protected override void OnClosed(EventArgs e)
        {
            _bossTimer?.Stop();
            _bossTimer = null;
            _instance = null;
            base.OnClosed(e);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
