using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;
using System.IO;


namespace GW2FOX
{
    public partial class OverlayWindow : Window
    {
        private System.Windows.Controls.ListView myListView;
        private static OverlayWindow? _instance;
        private DispatcherTimer bossTimer;

        public OverlayWindow()
        {
            this.Left = 1315;
            this.Top = 700;
            _instance = this;
            InitializeComponent(); // Stelle sicher, dass dies korrekt ausgeführt wird
            myListView = new System.Windows.Controls.ListView();
            BossListView.ItemsSource = BossTimerService.BossListItems;
            StartBossTimer();
        }

        private void StartBossTimer()
        {
            bossTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            bossTimer.Tick += BossTimer_Tick;
            bossTimer.Start();
        }

        private void BossTimer_Tick(object? sender, EventArgs e)
        {
            BossTimerService.Timer_Click(sender, e);
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
        protected override void OnClosed(EventArgs e)
        {
            _instance = null;
            base.OnClosed(e);
        }
    }
}
