using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

// Aliases für eindeutige Verweise
using WpfImage = System.Windows.Controls.Image;
using WpfClipboard = System.Windows.Clipboard;
using WpfPoint = System.Windows.Point;
using WpfMouseEventArgs = System.Windows.Input.MouseWheelEventArgs;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using WpfSize = System.Windows.Size;
using WpfButton = System.Windows.Controls.Button;
using WpfMessageBox = System.Windows.MessageBox;
using WpfListViewItem = System.Windows.Controls.ListViewItem;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class TreasureHunterMiniOverlay : Window
    {
        private Worldbosses worldbosses = BossTimerService.WorldbossesInstance;


        public TreasureHunterMiniOverlay(Window overlayWindow)
        {
            InitializeComponent();

            StartLoadingAnimation();
            StartBossColorAnimation();

            if (overlayWindow != null)
            {
                this.Left = overlayWindow.Left;
                this.Top = overlayWindow.Top - this.Height;
            }

            this.Show();

            overlayWindow.LocationChanged += (s, e) =>
            {
                this.Left = overlayWindow.Left;
                this.Top = overlayWindow.Top - this.Height;
            };

        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private DispatcherTimer bossColorTimer;

        private void StartBossColorAnimation()
        {
            if (bossColorTimer == null)
            {
                bossColorTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
                bossColorTimer.Tick += (s, e) =>
                {
                    bossColorTimer.Stop();
                    BossListView.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                    BossListView.FontSize = 13;
                    BossListView.FontWeight = FontWeights.SemiBold;  // hier FontWeights.Normal verwenden
                };
            }

            bossColorTimer.Start();
        }


        private void StartLoadingAnimation()
        {
            // Timer für den Textwechsel
            var changeTextTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
            changeTextTimer.Tick += (s, e) =>
            {
                changeTextTimer.Stop();
                LoadingText.Text = "Data Loaded!";

                // Nach 1 Sekunde langsam ausblenden
                var fadeOut = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(1.5),
                    FillBehavior = FillBehavior.HoldEnd
                };
                fadeOut.Completed += (s3, e3) => LoadingText.Visibility = Visibility.Collapsed;
                LoadingText.BeginAnimation(OpacityProperty, fadeOut);
            };
            changeTextTimer.Start();
        }


        private void ListViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfListViewItem item && item.DataContext is string bossName)
            {
                switch (bossName)
                {
                    case "The Eye of Zhaitan":
                        HandleEyeOfZhaitan();
                        break;
                    case "Gates of Arah":
                        HandleGatesOfArah();
                        break;
                    case "Branded Generals":
                        HandleBrandedGenerals();
                        break;
                    case "Dredge Commissar":
                        HandleDredgeCommissar();
                        break;
                    case "Captain Rotbeard":
                        HandleCaptainRotbeard();
                        break;
                    case "Rhendak The Crazed":
                        HandleRhendak();
                        break;
                    case "Ogrewars":
                        HandleOgrewars();
                        break;
                    case "Statue of Dwanya":
                        HandleStatueOfDwanya();
                        break;
                    case "Priestess of Lyssa":
                        HandlePriestessOfLyssa();
                        break;
                }
            }
        }



        private async void HandleEyeOfZhaitan()
        {
            worldbosses.Eye_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }


        private async void HandleGatesOfArah()
        {
            worldbosses.button31_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleBrandedGenerals()
        {
            worldbosses.button42_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleDredgeCommissar()
        {
            worldbosses.button63_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleCaptainRotbeard()
        {
            worldbosses.button65_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleRhendak()
        {
            worldbosses.Rhendak_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleOgrewars()
        {
            worldbosses.Ogrewars_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandleStatueOfDwanya()
        {
            worldbosses.Dwayna_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private async void HandlePriestessOfLyssa()
        {
            worldbosses.Lyssa_Click(this, EventArgs.Empty);
            await OverlayWindow.GetInstance()
                               .UpdateBossOverlayListAsync();
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }


        public class BossItem
        {
            public string Name { get; set; }
            public string IconPath { get; set; } // optional, kann null sein
        }


    }
}
