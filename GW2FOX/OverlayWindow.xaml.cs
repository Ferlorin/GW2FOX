using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace GW2FOX
{
    public partial class OverlayWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private DispatcherTimer? _copiedTimer;
        private DispatcherTimer? _countdownTimer;

        private static OverlayWindow? _instance;

        public ObservableCollection<BossListItem> OverlayItems { get; set; } = new();

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
                    OnPropertyChanged(nameof(ScrollValue));   // HIER IST DER 2te FEHLER
                }
            }
        }

        public OverlayWindow()
        {
            this.Left = 1315;
            this.Top = 700;
            _instance = this;

            InitializeComponent();
            DataContext = this;

            UpdateBossOverlayList();

            this.PreviewMouseWheel += OverlayWindow_PreviewMouseWheel;
            BossTimings.RegisterListView(BossListView);

            _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _countdownTimer.Tick += (s, e) =>
            {
                foreach (var item in OverlayItems)
                    item.UpdateCountdown();

                SortOverlayItems();
            };
            _countdownTimer.Start();
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

        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint);
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

            _copiedTimer?.Stop();
            _copiedTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.8) };
            _copiedTimer.Tick += (s, e) =>
            {
                CopiedMessage.Visibility = Visibility.Collapsed;
                _copiedTimer?.Stop();
            };
            _copiedTimer.Start();
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
            _instance ??= new OverlayWindow();
            return _instance;
        }

        protected override void OnClosed(EventArgs e)
        {
            _countdownTimer?.Stop();
            _instance = null;
            base.OnClosed(e);
        }

        public void UpdateBossOverlayList()
        {
            try
            {
                var now = GlobalVariables.CURRENT_DATE_TIME;
                Console.WriteLine($"Overlay: Starte UpdateBossOverlayList()");
                Console.WriteLine($"GlobalVariables.CURRENT_DATE_TIME: {now}");

                var combinedRuns = BossTimerService.GetBossRunsForOverlay();
                Console.WriteLine($"Overlay: combinedRuns.Count = {combinedRuns.Count}");

                var newItems = BossOverlayHelper.GetBossOverlayItems(combinedRuns, now);
                Console.WriteLine($"Overlay: newItems.Count = {newItems.Count}");

                // Entferne nicht mehr vorhandene Einträge
                for (int i = OverlayItems.Count - 1; i >= 0; i--)
                {
                    var existing = OverlayItems[i];
                    if (!newItems.Any(x => x.BossName == existing.BossName))
                    {
                        OverlayItems.RemoveAt(i);
                    }
                }

                // Ergänze oder aktualisiere Einträge
                foreach (var newItem in newItems)
                {
                    var match = OverlayItems.FirstOrDefault(x => x.BossName == newItem.BossName);
                    if (match == null)
                    {
                        newItem.UpdateCountdown();
                        OverlayItems.Add(newItem);
                    }
                    else
                    {
                        match.NextRunTime = newItem.NextRunTime;
                        match.Waypoint = newItem.Waypoint;
                        match.UpdateCountdown();
                    }
                }

                SortOverlayItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }

        private void SortOverlayItems()
        {
            var sorted = OverlayItems.OrderBy(b => b.SecondsRemaining).ToList();
            OverlayItems.Clear();
            foreach (var item in sorted)
                OverlayItems.Add(item);
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
                    OnPropertyChanged(nameof(TimeRemainingFormatted)); // HIER IST DER ERSTE FEHLER
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class ItalicConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value is bool b && b ? FontStyles.Italic : FontStyles.Normal;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }
    }
}
