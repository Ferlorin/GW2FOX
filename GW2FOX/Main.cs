using System.Diagnostics;
using IWshRuntimeLibrary;
using File = System.IO.File;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace GW2FOX
{
    public partial class Main : BaseForm
    {
        private Worldbosses _worldbossesForm;
        private MiniOverlay _miniOverlay;
        private OverlayWindow _overlayWindow;

        public Main()
        {
            InitializeComponent();
            InitializeWorldbosses();
        }

        private void InitializeWorldbosses()
        {
            _worldbossesForm = new Worldbosses();
            _worldbossesForm.FormClosed += (s, args) => _worldbossesForm = null;
            BossTimerService.WorldbossesInstance = _worldbossesForm;
        }

        private void HandleException(Exception ex)
        {
            System.Windows.Forms.MessageBox.Show($"An error occurred: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OpenForm(Form newForm)
        {
            newForm.Owner = this;
            newForm.Show();
        }

        private void Fox_Click(object sender, EventArgs e)
        {
            try
            {
                string homepageUrl = "https://gw2fox.wixsite.com/about";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = homepageUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"GREAT - you deleted the INTERNET!: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Repair_Click(object sender, EventArgs e)
        {
            string gw2ExecutablePath = GetGw2ExecutablePath();
            if (string.IsNullOrEmpty(gw2ExecutablePath))
            {
                System.Windows.Forms.MessageBox.Show("No Gw2-64.exe selected! Please choose the Guild Wars 2 executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string gw2Directory = Path.GetDirectoryName(gw2ExecutablePath);
            string shortcutPath = Path.Combine(gw2Directory, "RepairGW2.lnk");

            CreateShortcut(gw2ExecutablePath, shortcutPath, "-repair");

            LaunchWithRepair(shortcutPath);
        }

        private string GetGw2ExecutablePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Guild Wars 2 Executable|Gw2-64.exe|All Files|*.*";
                openFileDialog.Title = "Select Guild Wars 2 Executable (Gw2-64.exe)";

                return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
            }
        }

        private void CreateShortcut(string targetPath, string shortcutPath, string commandLineParameters)
        {
            try
            {
                if (File.Exists(shortcutPath))
                {
                    System.Windows.Forms.MessageBox.Show("Shortcut already exists. Launching existing shortcut.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                shortcut.Arguments = commandLineParameters;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error creating the shortcut: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchWithRepair(string shortcutPath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = shortcutPath,
                    UseShellExecute = true,
                    Verb = "open"
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error launching the shortcut: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAll_Click(object sender, EventArgs e)
        {
            try
            {
                BossTimerService._bossTimer?.Stop();
                BossTimerService._bossTimer?.Dispose();
                BossTimerService._overlayWindow?.Close();

                System.Windows.Forms.Application.Exit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LaunchExternalTool(string relativePath)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                System.Windows.Forms.MessageBox.Show("Verknüpfung nicht gefunden: " + fullPath);
                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true 
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Fehler beim Starten: " + ex.Message);
            }
        }


        private string GetGw2Verzeichnis()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Please select the directory of Guild Wars 2 where the .exe file is located.";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }

            return null;
        }


        protected void ShowAndHideForm(Form newForm)
        {
            newForm.BringToFront();
            newForm.Activate();
            this.Hide();
        }

        private void Leading_Click(object sender, EventArgs e)
        {
            // 1. MiniOverlay öffnen und anzeigen (WPF)
            if (_miniOverlay == null || !_miniOverlay.IsLoaded)
            {
                _miniOverlay = new MiniOverlay(_worldbossesForm);
                _miniOverlay.Closed += (s, args) =>
                {
                    _miniOverlay = null;
                };
                _miniOverlay.Show();
            }
            else if (_miniOverlay.Visibility != Visibility.Visible)
            {
                _miniOverlay.Show();
                _miniOverlay.Activate();
            }

            // 3. OverlayWindow starten (WPF Singleton)
            _overlayWindow = OverlayWindow.GetInstance();
            if (!_overlayWindow.IsVisible)
            {
                _overlayWindow.Show();
            }
            else
            {
                _overlayWindow.Activate();
            }

            // 4. Main-Fenster ausblenden
            this.Hide();
        }

    }
}
