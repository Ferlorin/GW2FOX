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

// Aliases für eindeutige Verweise
using WpfPoint = System.Windows.Point;
using WpfImage = System.Windows.Controls.Image;
using WpfClipboard = System.Windows.Clipboard;
using WpfMouseEventArgs = System.Windows.Input.MouseEventArgs;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private DispatcherTimer _bossTimer;
        private double _scrollValue;

        // Thumb-Drag State
        private bool _isDragging = false;
        private WpfPoint _dragStartPoint;
        private double _thumbOriginalTop;

        // Scroll Offset for initial thumb position
        private double _startOffsetTop = 0;

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
            // Initiale Position des Thumbs hinter dem ersten Eintrag
            if (BossListView.Items.Count > 0)
            {
                if (BossListView.ItemContainerGenerator.ContainerFromIndex(0) is FrameworkElement item)
                {
                    WpfPoint position = item.TranslatePoint(new WpfPoint(0, 0), BossListView);
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

            if (!_isDragging)
                Canvas.SetTop(ScrollThumb, calculatedTop);
        }

        private void StartBossTimer()
        {
            _bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _bossTimer.Tick += (s, e) => UpdateBossOverlayListAsync();
            _bossTimer.Start();
        }

        // Thumb Drag Handlers:
        private void Thumb_MouseLeftButtonDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (!_isDragging)
            {
                _isDragging = true;
                Canvas canvas = (Canvas)ScrollThumb.Parent;
                _dragStartPoint = e.GetPosition(canvas);
                _thumbOriginalTop = Canvas.GetTop(ScrollThumb);
                ScrollThumb.CaptureMouse();
                e.Handled = true;
            }
        }

        private void Thumb_MouseMove(object sender, WpfMouseEventArgs e)
        {
            if (!_isDragging) return;

            Canvas canvas = (Canvas)ScrollThumb.Parent;
            WpfPoint pos = e.GetPosition(canvas);
            double deltaY = pos.Y - _dragStartPoint.Y;

            double maxTop = canvas.ActualHeight - ScrollThumb.ActualHeight;
            double newTop = Math.Max(0, Math.Min(_thumbOriginalTop + deltaY, maxTop));
            Canvas.SetTop(ScrollThumb, newTop);

            double scrollable = BossScrollViewer.ScrollableHeight;
            if (scrollable > 0)
            {
                double percent = newTop / maxTop;
                BossScrollViewer.ScrollToVerticalOffset(percent * scrollable);
            }

            e.Handled = true;
        }

        private void Thumb_MouseLeftButtonUp(object sender, WpfMouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                ScrollThumb.ReleaseMouseCapture();
                e.Handled = true;
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
                    double currentOffset = BossScrollViewer.VerticalOffset;
                    OverlayItems.Clear();
                    foreach (var item in items)
                        OverlayItems.Add(item);
                    BossScrollViewer.ScrollToVerticalOffset(currentOffset);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void Waypoint_MouseLeftButtonDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img && img.DataContext is BossListItem boss)
            {
                WpfClipboard.SetText(boss.Waypoint);
                WpfPoint pos = img.TranslatePoint(new WpfPoint(0, img.ActualHeight), this);
                ShowCopiedMessage(pos);
            }
        }

        private void ShowCopiedMessage(WpfPoint position)
        {
            CopiedMessage.Visibility = Visibility.Visible;
            Canvas.SetLeft(CopiedMessage, position.X);
            Canvas.SetTop(CopiedMessage, position.Y - 40);

            DispatcherTimer t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.2) };
            t.Tick += (s, e) => { CopiedMessage.Visibility = Visibility.Collapsed; t.Stop(); };
            t.Start();
        }

        private void Icon_MouseLeftButtonDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        public static OverlayWindow GetInstance() => _instance ??= new OverlayWindow();

        protected override void OnClosed(EventArgs e)
        {
            _bossTimer?.Stop();
            _bossTimer = null!;
            _instance = null;
            base.OnClosed(e);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}