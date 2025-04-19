using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace GW2FOX
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
            InitializeBossList();
        }

        private void InitializeBossList()
        {
            // Beispielhafte Datenquelle für Boss-Daten
            var bosses = new[]
            {
                new { Waypoint = "Way1", BossName = "Boss 1", Time = "12:00", WaypointImage = "/Images/Waypoint.png" },
                new { Waypoint = "Way2", BossName = "Boss 2", Time = "14:00", WaypointImage = "/Images/Waypoint.png" }
            };

            // Setzen der ItemsSource für die ListView
            BossListView.ItemsSource = bosses;
        }

        // Event-Handler, wenn auf das Waypoint-Icon geklickt wird
        private void Waypoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                var boss = image.DataContext as dynamic;
                if (boss != null)
                {
                    Clipboard.SetText(boss.Waypoint); // Kopiere den Wegpunkt in die Zwischenablage
                    ShowCopiedMessage(); // Zeige "Copied!" Nachricht an
                }
            }
        }

        // Methode, um die "Copied!" Nachricht anzuzeigen
        private void ShowCopiedMessage()
        {
            var message = new TextBlock
            {
                Text = "Copied!",
                Foreground = Brushes.White,
                Background = Brushes.Black,
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                Visibility = Visibility.Visible
            };

            // Zeige die Nachricht im Overlay
            this.Content = message;

            // Timer, um die Nachricht nach 0.5 Sekunden auszublenden
            var dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };
            dispatcherTimer.Tick += (s, e) =>
            {
                message.Visibility = Visibility.Collapsed;
                dispatcherTimer.Stop();
            };
            dispatcherTimer.Start();
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
