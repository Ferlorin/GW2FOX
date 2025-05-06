using System.Windows;
using System.Windows.Input;

// Aliases für eindeutige Verweise
using WpfImage = System.Windows.Controls.Image;
using WpfClipboard = System.Windows.Clipboard;
using WpfPoint = System.Windows.Point;
using WpfMouseEventArgs = System.Windows.Input.MouseWheelEventArgs;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using WpfSize = System.Windows.Size;
using WpfButton = System.Windows.Controls.Button;
using WpfMessageBox = System.Windows.MessageBox;
using WpfListView = System.Windows.Controls.ListView;
using System.Windows.Controls;

namespace GW2FOX
{
    public partial class TreasureHunterMiniOverlay : Window
    {
        private Worldbosses worldbosses = new Worldbosses();
        public TreasureHunterMiniOverlay()
        {
            InitializeComponent();
            BossListView.PreviewMouseLeftButtonDown += ListViewItem_MouseLeftButtonDown;
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is string bossName)
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

        // Platzhalter-Methoden (fülle sie später!)
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
