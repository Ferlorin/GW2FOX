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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<BossListItem> OverlayItems { get; set; } = new();
        private static OverlayWindow? _instance;

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

            _copiedTimer?.Stop(); // Verhindert mehrere Timer gleichzeitig
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
            if (_instance == null)
            {
                _instance = new OverlayWindow();
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
                var now = GlobalVariables.CURRENT_DATE_TIME; // <--- Zeit einmal definieren
                var combinedRuns = BossTimerService.GetBossRunsForOverlay();
                var newItems = BossOverlayHelper.GetBossOverlayItems(combinedRuns, now); // <--- Zeit übergeben
                newItems = new ObservableCollection<BossListItem>(newItems.OrderBy(b => b.SecondsRemaining));

                for (int i = OverlayItems.Count - 1; i >= 0; i--)
                {
                    var existing = OverlayItems[i];
                    if (!newItems.Any(x => x.BossName == existing.BossName))
                    {
                        OverlayItems.RemoveAt(i);
                    }
                }

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
