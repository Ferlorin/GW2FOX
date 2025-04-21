using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        private static OverlayWindow? _instance;
        private DispatcherTimer bossTimer;
        private DispatcherTimer scrollBarTimer;
        private bool isMouseOver = false;

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
            
        }

      



        private void MainGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOver = true;

            // Fokus aufs Fenster erzwingen
            if (!this.IsActive)
            {
                this.Activate();
                this.Focus();
            }
        }

        private void MainGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOver = false;
        }


        private void StartBossTimer()
        {
            bossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            bossTimer.Tick += BossTimer_Tick;
            bossTimer.Start();
        }

        private void OverlayWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (BossScrollViewer.IsMouseOver || this.IsActive)
            {
                BossScrollViewer.ScrollToVerticalOffset(BossScrollViewer.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }


        private void BossTimer_Tick(object? sender, EventArgs e)
        {
            UpdateBossOverlayList();
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

        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint); // Kopiert die Wegmarke in die Zwischenablage
                ShowCopiedMessage();
            }
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

        protected override void OnClosed(EventArgs e)
        {
            _instance = null;
            base.OnClosed(e);
        }

        public void UpdateBossOverlayList()
        {
            try
            {
                var filteredBosses = BossTimings.BossEventGroups
                    .Where(group => BossTimings.BossList23.Contains(group.BossName))
                    .SelectMany(group => group.GetNextRuns())
                    .OrderBy(run => run.NextRunTime)
                    .ThenBy(run => run.Category)
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    BossListView.ItemsSource = null;
                    BossListView.ItemsSource = filteredBosses;
                });
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
