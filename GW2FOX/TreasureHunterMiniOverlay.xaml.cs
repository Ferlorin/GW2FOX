using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

namespace GW2FOX
{
    public partial class TreasureHunterMiniOverlay : Window
    {
        private Worldbosses worldbosses = new Worldbosses();

        public TreasureHunterMiniOverlay(Window overlayWindow)
        {
            InitializeComponent();

            // Positionieren: direkt oberhalb von OverlayWindow
            if (overlayWindow != null)
            {
                this.Left = overlayWindow.Left;
                this.Top = overlayWindow.Top - this.Height; // Mini-Overlay oberhalb von OverlayWindow
            }

            this.Show();

            // Sicherstellen, dass das Mini-Overlay bei Bewegung des Hauptfensters mitbewegt wird
            overlayWindow.LocationChanged += (s, e) =>
            {
                this.Left = overlayWindow.Left;
                this.Top = overlayWindow.Top - this.Height;
            };

        }

        private void ListViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is WpfListViewItem item && item.DataContext is string bossName)
            {
                // "☠" und Leerzeichen entfernen
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
        }


        private void HandleGatesOfArah()
        {
            worldbosses.button31_Click(this, EventArgs.Empty);
        }

        private void HandleBrandedGenerals()
        {
            worldbosses.button42_Click(this, EventArgs.Empty);
        }

        private void HandleDredgeCommissar()
        {
            worldbosses.button63_Click(this, EventArgs.Empty);
        }

        private void HandleCaptainRotbeard()
        {
            worldbosses.button65_Click(this, EventArgs.Empty);
        }

        private void HandleRhendak()
        {
            worldbosses.Rhendak_Click(this, EventArgs.Empty);
        }

        private void HandleOgrewars()
        {
            worldbosses.Ogrewars_Click(this, EventArgs.Empty);
        }

        private void HandleStatueOfDwanya()
        {
            worldbosses.Dwayna_Click(this, EventArgs.Empty);
        }

        private void HandlePriestessOfLyssa()
        {
            worldbosses.Lyssa_Click(this, EventArgs.Empty);
        }
    }
}
