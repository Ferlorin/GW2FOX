// Main.cs

using System.Diagnostics;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace GW2FOX
{
    public partial class Main : BaseForm
    {


        private GlobalKeyboardHook? _globalKeyboardHook; // Füge dies hinzu

        public Main()
        {
            InitializeComponent();

            InitializeGlobalKeyboardHook();

            // Updater.CheckForUpdates(Worldbosses.getConfigLineForItem("Version"));
        }


        private void InitializeGlobalKeyboardHook()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyPressed += GlobalKeyboardHook_KeyPressed;
        }

        private void GlobalKeyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (ModifierKeys == Keys.Alt && e.Key == Keys.T)
            {
                Timer_Click(sender, e);
            }
        }


        private void Timer_Click(object sender, EventArgs e)
        {
            BossTimerService.Timer_Click(sender, e);
        }


        private void HandleException(Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OpenForm(Form newForm)
        {
            newForm.Owner = this;
            newForm.Show();
            // this.Dispose();
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
                MessageBox.Show($"GREAT - you deletet the INTERNET!: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath); // Setzen Sie das Arbeitsverzeichnis auf den Ordner der ausf?hrbaren Datei
                shortcut.Arguments = commandLineParameters;
                shortcut.Save(); // Speichern Sie die Verkn?pfung, um sie zu erstellen
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



        private void Leading_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Worldbosses());
            BossTimerService.Timer_Click(sender, e);
        }


        private void CloseAll_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop the Timer and dispose of the BossTimer
                BossTimerService._bossTimer?.Stop();
                BossTimerService._bossTimer?.Dispose(); // Dispose of the BossTimer

                // Close the Overlay and dispose of it
                BossTimerService._overlay?.Close();
                BossTimerService._overlay?.Dispose(); // Dispose of the Overlay

                // Close the program
                Application.Exit();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BlishHUD_Click(object sender, EventArgs e)
        {
            // Verzeichnis der ausführbaren Datei erhalten
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Pfad zur Datei "Blish HUD.exe" im Verzeichnis "data"
            string filePath = Path.Combine(exeDirectory, "data2", "Blish HUD.exe");

            // Überprüfen, ob die Datei existiert, bevor sie geöffnet wird
            if (File.Exists(filePath))
            {
                try
                {
                    // Öffne die Datei
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Öffnen der Datei: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Die Datei wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Verzeichnis der ausführbaren Datei erhalten
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Pfad zur Datei "Blish HUD.exe" im Verzeichnis "data"
            string filePath = Path.Combine(exeDirectory, "data", "GW2TacO.exe");

            // Überprüfen, ob die Datei existiert, bevor sie geöffnet wird
            if (File.Exists(filePath))
            {
                try
                {
                    // Öffne die Datei
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Öffnen der Datei: " + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Die Datei wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArcDPSInstall_Click(object sender, EventArgs e)
        {
            string gw2Verzeichnis = GetGw2Verzeichnis();

            if (string.IsNullOrEmpty(gw2Verzeichnis))
            {
                MessageBox.Show("Das Guild Wars 2-Verzeichnis wurde nicht ausgewählt.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Pfade zu den zu kopierenden Dateien
            string d3d11DllQuelle = Path.Combine("data", "d3d11.dll");
            string d3d11Md5SumQuelle = Path.Combine("data", "d3d11.dll.md5sum");

            // Ziel Pfade im Guild Wars 2-Verzeichnis
            string d3d11DllZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll");
            string d3d11Md5SumZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll.md5sum");

            try
            {
                // Dateien kopieren
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
                // Pfade zu den zu löschenden Dateien
                string d3d11DllZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll");
                string d3d11Md5SumZiel = Path.Combine(gw2Verzeichnis, "d3d11.dll.md5sum");

                // Dateien löschen, wenn sie existieren
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


    }
}
