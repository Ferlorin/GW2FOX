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


        // Scroll-Sync zwischen ScrollViewer und Slider
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

        // Konstruktor
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

        // ▓▓▓ Scroll per Mausrad ▓▓▓
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

        // ▓▓▓ Scroll-Sync ▓▓▓
        private void BossScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ScrollValue != e.VerticalOffset)
            {
                ScrollValue = e.VerticalOffset;
            }
        }

        // ▓▓▓ Startet den Timer für Sekundentakt ▓▓▓
        private void StartBossTimer()
        {
            bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            bossTimer.Tick += BossTimer_Tick;
            bossTimer.Start();
        }

        // ▓▓▓ Tick-Ereignis (1 Sekunde) ▓▓▓
        private void BossTimer_Tick(object? sender, EventArgs e)
        {
            UpdateBossOverlayList();
        }

        // ▓▓▓ Klick auf Waypoint-Symbol ▓▓▓
        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint);
                ShowCopiedMessage();
            }
        }

        // ▓▓▓ Zeige "Kopiert"-Nachricht ▓▓▓
        private void ShowCopiedMessage()
        {
            CopiedMessage.Visibility = Visibility.Visible;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.8) };
            timer.Tick += (s, e) =>
            {
                CopiedMessage.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        // ▓▓▓ Scrollbar-Wertänderung ▓▓▓
        private void ManualScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BossScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

        // ▓▓▓ Drag der Fensterleiste ▓▓▓
        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // ▓▓▓ Singleton-Instanz ▓▓▓
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

        // ▓▓▓ PropertyChanged für DataBinding ▓▓▓
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}