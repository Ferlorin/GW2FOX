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
using System.Windows.Threading;

namespace GW2FOX
{
    public partial class TreasureHunterMiniOverlay : Window
    {
        private Worldbosses worldbosses = new Worldbosses();

        public TreasureHunterMiniOverlay(Window overlayWindow)
        {
            InitializeComponent();

            StartLoadingAnimation();

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
                bossName = bossName.TrimStart('☠', ' ');

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



        private void HandleEyeOfZhaitan()
        {
            worldbosses.Eye_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }


        private void HandleGatesOfArah()
        {
            worldbosses.button31_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleBrandedGenerals()
        {
            worldbosses.button42_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleDredgeCommissar()
        {
            worldbosses.button63_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleCaptainRotbeard()
        {
            worldbosses.button65_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleRhendak()
        {
            worldbosses.Rhendak_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleOgrewars()
        {
            worldbosses.Ogrewars_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandleStatueOfDwanya()
        {
            worldbosses.Dwayna_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }

        private void HandlePriestessOfLyssa()
        {
            worldbosses.Lyssa_Click(this, EventArgs.Empty);
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
        }
    }
}
