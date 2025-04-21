using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<BossListItem> OverlayItems { get; } = new ObservableCollection<BossListItem>();
        private static OverlayWindow? _instance;
        private DispatcherTimer bossTimer;

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
            this.Left = 1315;
            this.Top = 700;
            _instance = this;
            InitializeComponent();
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
                ScrollValue = newOffset; // Update auch manuell setzen
                e.Handled = true;
            }
        }

        private void BossScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Sync ScrollValue wenn manuell oder per Mausrad gescrollt wird
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
            Console.WriteLine("Waypoint wurde angeklickt."); // Debug-Test

            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint); // <- explizit
                ShowCopiedMessage();
            }
        }

        private void ManualScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BossScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

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

        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public static OverlayWindow GetInstance()
        {
            if (_instance == null)
            {
                Console.WriteLine("Erstelle neue Instanz von OverlayWindow.");
                _instance = new OverlayWindow();
            }
            else
            {
                Console.WriteLine("Verwende bestehende Instanz von OverlayWindow.");
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
                var newItems = BossOverlayHelper.GetBossOverlayItems(combinedRuns);

                // Sortiere optional nach verbleibender Zeit
                newItems = newItems.OrderBy(b => b.SecondsRemaining).ToList();

                // Sync: entferne alte, aktualisiere bestehende, füge neue hinzu
                for (int i = OverlayItems.Count - 1; i >= 0; i--)
                {
                    var existing = OverlayItems[i];
                    if (!newItems.Any(x => x.BossName == existing.BossName))
                    {
                        OverlayItems.RemoveAt(i); // Entfernt nicht mehr vorhandene Bosse
                    }
                }

                foreach (var newItem in newItems)
                {
                    var match = OverlayItems.FirstOrDefault(x => x.BossName == newItem.BossName);
                    if (match == null)
                    {
                        OverlayItems.Add(newItem); // Neuer Boss
                    }
                    else
                    {
                        // Optional: bestehende Properties aktualisieren
                        match.TimeRemainingFormatted = newItem.TimeRemainingFormatted;
                        match.SecondsRemaining = newItem.SecondsRemaining;
                        match.IsPastEvent = newItem.IsPastEvent;
                        match.Waypoint = newItem.Waypoint;
                        // OnPropertyChanged innerhalb von BossListItem nicht vergessen!
                    }
                }

                Console.WriteLine("Overlay updated. Combined entries:");
                foreach (var boss in OverlayItems)
                {
                    Console.WriteLine($"- {boss.BossName} | {boss.TimeRemainingFormatted} | Vergangen: {boss.IsPastEvent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }



        private string _timeRemainingFormatted;
        public string TimeRemainingFormatted
        {
            get => _timeRemainingFormatted;
            set
            {
                if (_timeRemainingFormatted != value)
                {
                    _timeRemainingFormatted = value;
                    OnPropertyChanged(nameof(TimeRemainingFormatted));
                }
            }
        }
    }
}
