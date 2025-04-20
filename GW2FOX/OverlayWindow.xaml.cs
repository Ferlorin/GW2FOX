using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Windows; // Clipboard für WPF

namespace GW2FOX
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
            InitializeBossList();
        }

        public class BossListItem
        {
            public string BossName { get; set; }
            public string Waypoint { get; set; }
            public string Time { get; set; } // z.B. "15:30"
            public BitmapImage WaypointImage => new BitmapImage(new Uri("pack://application:,,,/Icons/waypoint.png"));
        }

        private void InitializeBossList()
        {
            // Alle zukünftigen Runs sammeln
            var upcomingRuns = BossTimings.BossEvents
                .SelectMany(kvp => new BossTimings.BossEventGroup(kvp.Key, kvp.Value).GetNextRuns())
                .OrderBy(run => run.NextRunTime) // Verwende NextRunTime statt EventTime
                .ToList();

            var bossList = upcomingRuns.Select(run => new BossListItem
            {
                BossName = run.BossName,
                Waypoint = run.Waypoint,
                Time = run.NextRunTime.ToString("HH:mm") // Verwende NextRunTime statt EventTime
            }).ToList();

            BossListView.ItemsSource = bossList;
        }


        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image img && img.DataContext is BossListItem boss)
            {
                System.Windows.Clipboard.SetText(boss.Waypoint); // Verwende den WPF Clipboard-Namespace
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

        // Event-Handler, um das Fenster zu verschieben
        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Verschiebe das Fenster
            }
        }

        // Event-Handler für den Doppelklick auf ein ListView-Item
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Hier könnten weitere Interaktionen mit der Liste eingefügt werden
        }
    }
}
