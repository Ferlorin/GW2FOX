using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using static GW2FOX.BossTimerService;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private DispatcherTimer bossTimer;
        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();

        private double _scrollValue;
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
            List<BossListItem> testData = new List<BossListItem>();
            for (int i = 1; i <= 50; i++)
            {
                testData.Add(new BossListItem
                {
                    BossName = $"Test-Boss {i}",
                    TimeRemainingFormatted = $"00:{(i % 60):D2}",
                    Category = "WBs", // oder irgendwas Gültiges für deine Converter
                    IsPastEvent = false
                });
            }
            BossListView.ItemsSource = testData;


            this.Left = 1325;
            this.Top = 700;
            _instance = this;

            DataContext = this;
            this.PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;
            this.Loaded += Window_Loaded;

            BossTimings.RegisterListView(BossListView);

            StartBossTimer();
            UpdateBossOverlayListAsync(); // Initial async call
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollValue = BossScrollViewer.VerticalOffset;
        }

        private void OverlayWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (BossScrollViewer.IsMouseOver || this.IsActive)
            {
                double scrollMultiplier = 3.0;
                var newOffset = BossScrollViewer.VerticalOffset - (e.Delta / 120.0 * scrollMultiplier);
                BossScrollViewer.ScrollToVerticalOffset(newOffset);

                if (Math.Abs(ScrollValue - newOffset) > 0.1)
                    _scrollValue = newOffset;
                e.Handled = true;
            }
        }


        private void BossScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ScrollValue != e.VerticalOffset)
                ScrollValue = e.VerticalOffset;
        }

        private void StartBossTimer()
        {
            bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            bossTimer.Tick += (s, e) => UpdateBossOverlayListAsync();
            bossTimer.Start();
        }

        private async void UpdateBossOverlayListAsync()
        {
            try
            {
                var combinedRuns = await Task.Run(() => BossTimerService.GetBossRunsForOverlay());
                var overlayItems = await Task.Run(() => BossOverlayHelper.GetBossOverlayItems(combinedRuns, DateTime.Now));

                Dispatcher.Invoke(() =>
                {
                    BossListView.ItemsSource = null;
                    BossListView.ItemsSource = overlayItems;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }

        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint);
                System.Windows.Point position = img.TranslatePoint(new System.Windows.Point(0, img.ActualHeight), MainOverlayWindow);
                ShowCopiedMessage(position);
            }
        }

        private void ShowCopiedMessage(System.Windows.Point position)
        {
            CopiedMessage.Visibility = Visibility.Visible;
            Canvas.SetLeft(CopiedMessage, position.X);
            Canvas.SetTop(CopiedMessage, position.Y);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.8) };
            timer.Tick += (s, e) =>
            {
                CopiedMessage.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        private void ManualScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BossScrollViewer != null)
            {
                BossScrollViewer.ScrollToVerticalOffset(e.NewValue);
            }
        }


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public static OverlayWindow GetInstance()
        {
            return _instance ??= new OverlayWindow();
        }

        protected override void OnClosed(EventArgs e)
        {
            bossTimer?.Stop();
            bossTimer = null;
            _instance = null;
            base.OnClosed(e);
        }

        public void UpdateBossOverlayList()
        {
            try
            {
                var combinedRuns = BossTimerService.GetBossRunsForOverlay();
                var overlayItems = BossOverlayHelper.GetBossOverlayItems(combinedRuns, DateTime.Now);

                Dispatcher.Invoke(() =>
                {
                    BossListView.ItemsSource = null;
                    BossListView.ItemsSource = overlayItems;
                });

                foreach (var boss in overlayItems)
                {
                    //Console.WriteLine($"- {boss.BossName} | {boss.TimeRemainingFormatted} | Vergangen: {boss.IsPastEvent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void BossListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
