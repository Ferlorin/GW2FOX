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
            InitializeComponent(); // ✅ MUSS zuerst sein

            this.Left = 1315;
            this.Top = 700;
            _instance = this;

            UpdateBossOverlayList();
            StartBossTimer();

            DataContext = this;
            this.PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;

            BossTimings.RegisterListView(BossListView);
        }

        private void OverlayWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (BossScrollViewer.IsMouseOver || this.IsActive)
            {
                var newOffset = BossScrollViewer.VerticalOffset - e.Delta;
                BossScrollViewer.ScrollToVerticalOffset(newOffset);
                ScrollValue = newOffset;
                e.Handled = true;
            }
        }

        private void BossScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ScrollValue != e.VerticalOffset)
            {
                ScrollValue = e.VerticalOffset;
            }
        }

        private void StartBossTimer()
        {
            bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            bossTimer.Tick += BossTimer_Tick;
            bossTimer.Start();
        }

        private void BossTimer_Tick(object? sender, EventArgs e)
        {
            UpdateBossOverlayList();
        }

        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint);

                // Position relativ zum Fenster bestimmen
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
            BossScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }


        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public static OverlayWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OverlayWindow();
            }
            else
            {
                //Console.WriteLine("Verwende bestehende Instanz von OverlayWindow.");
            }
            return _instance;
        }

        protected override void OnClosed(EventArgs e)
        {
            _instance = null;
            base.OnClosed(e);
        }

        public void UpdateBossOverlayList()
        {
            try
            {
                var combinedRuns = BossTimerService.GetBossRunsForOverlay();

                // FIX: Übergabe von DateTime.Now als zweiter Parameter
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
    }
}