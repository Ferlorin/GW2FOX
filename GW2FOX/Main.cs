using System.Diagnostics;
using IWshRuntimeLibrary;
using File = System.IO.File;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

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
            // Updater.CheckForUpdates(Worldbosses.getConfigLineForItem("Version"));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BossTimings.LoadBossConfig("bosses_config.json");
        }


        private void HandleException(Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"GREAT - you deleted the INTERNET!: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Repair_Click(object sender, EventArgs e)
        {
            string gw2ExecutablePath = GetGw2ExecutablePath();
            if (string.IsNullOrEmpty(gw2ExecutablePath))
            {
                MessageBox.Show("No Gw2-64.exe selected! Please choose the Guild Wars 2 executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Shortcut already exists. Launching existing shortcut.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show($"Error creating the shortcut: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Error launching the shortcut: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAll_Click(object sender, EventArgs e)
        {
            try
            {
                BossTimerService._bossTimer?.Stop();
                BossTimerService._bossTimer?.Dispose();
                BossTimerService._overlayWindow?.Close();

                Application.Exit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BlishHUD_Click(object sender, EventArgs e)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, "data2", "Blish HUD.exe");

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, "data", "GW2TacO.exe");

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArcDPSInstall_Click(object sender, EventArgs e)
        {
            string gw2Verzeichnis = GetGw2Verzeichnis();

            if (string.IsNullOrEmpty(gw2Verzeichnis))
            {
                MessageBox.Show("Please select the directory of Guild Wars 2 where the .exe file is located.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InstallArcDPS(gw2Verzeichnis);
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

        private void InstallArcDPS(string gw2Verzeichnis)
        {
            string d3d11DllQuelle = Path.Combine("data", "d3d11.dll");
            string d3d11Md5SumQuelle = Path.Combine("data", "d3d11.dll.md5sum");

            string d3d11DllZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll");
            string d3d11Md5SumZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll.md5sum");

            try
            {
                File.Copy(d3d11DllQuelle, d3d11DllZiel, true);
                File.Copy(d3d11Md5SumQuelle, d3d11Md5SumZiel, true);

                MessageBox.Show("Done.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArcDPSDeinstall_Click(object sender, EventArgs e)
        {
            string gw2Verzeichnis = GetGw2Verzeichnis();

            if (string.IsNullOrEmpty(gw2Verzeichnis))
            {
                MessageBox.Show("The Guild Wars 2 directory was not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string d3d11DllZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll");
                string d3d11Md5SumZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll.md5sum");

                if (File.Exists(d3d11DllZiel))
                    File.Delete(d3d11DllZiel);

                if (File.Exists(d3d11Md5SumZiel))
                    File.Delete(d3d11Md5SumZiel);

                MessageBox.Show("Done.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Tutorial());
            ShowAndHideForm(new MiniOverlay2(_worldbossesForm));
            BossTimerService.Timer_Click(sender, e);
        }

        protected void ShowAndHideForm(Form newForm)
        {
            newForm.BringToFront();
            newForm.Activate();
            this.Hide();
        }

        private void Leading_Click(object sender, EventArgs e)
        {
            // 1. MiniOverlay öffnen und anzeigen
            if (_miniOverlay == null || _miniOverlay.IsDisposed)
            {
                Console.WriteLine("MiniOverlay: Neue Instanz wird erstellt und angezeigt.");
                _miniOverlay = new MiniOverlay(_worldbossesForm);
                _miniOverlay.FormClosed += (s, args) =>
                {
                    Console.WriteLine("MiniOverlay: Instanz geschlossen.");
                    _miniOverlay = null;
                };
                _miniOverlay.Show();
            }
            else if (!_miniOverlay.Visible)
            {
                Console.WriteLine("MiniOverlay: Wird angezeigt.");
                _miniOverlay.Show();
                _miniOverlay.BringToFront();
            }

            // 2. Worldbosses öffnen und anzeigen
            if (_worldbossesForm == null || _worldbossesForm.IsDisposed)
            {
                Console.WriteLine("Worldbosses: Neue Instanz wird erstellt und angezeigt.");
                _worldbossesForm = new Worldbosses();
                _worldbossesForm.FormClosed += (s, args) =>
                {
                    Console.WriteLine("Worldbosses: Instanz geschlossen.");
                    _worldbossesForm = null;
                };
                _worldbossesForm.Show();
            }
            else if (!_worldbossesForm.Visible)
            {
                Console.WriteLine("Worldbosses: Wird angezeigt.");
                _worldbossesForm.Show();
                _worldbossesForm.BringToFront();
            }

            // 3. OverlayWindow starten (Singleton verwenden)
            _overlayWindow = OverlayWindow.GetInstance();
            if (!_overlayWindow.IsVisible)
            {
                Console.WriteLine("OverlayWindow: Wird angezeigt.");
                _overlayWindow.Show();
            }
            else
            {
                Console.WriteLine("OverlayWindow: Wird aktiviert.");
                _overlayWindow.Activate();
            }
        }
    }
}
