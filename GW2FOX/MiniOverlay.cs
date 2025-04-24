using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class MiniOverlay : BaseForm
    {
        private Form lastOpenedForm;
        private Form lastOpenedBoss = null;
        private OverlayWindow _overlayWindow;

        public MiniOverlay(Worldbosses worldbosses)
        {
            InitializeComponent();
            this.TopMost = true;
            this.Load += MiniOverlay_Load;
        }

        private void MiniOverlay_Load(object sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (screen.Width - this.Width) / 2,
                0
            );
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void button1_Click(object sender, EventArgs e)
        {
            var excludedTypes = new[] { typeof(MiniOverlay) };
            var topMostStates = new Dictionary<Form, bool>();

            // 1. TopMost merken und ausschalten
            foreach (Form f in Application.OpenForms)
            {
                topMostStates[f] = f.TopMost;
                f.TopMost = false;
            }

            // 2. Fokus ermitteln
            IntPtr activeHandle = GetForegroundWindow();

            // 3. Form finden & toggeln
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Handle == activeHandle && !excludedTypes.Contains(openForm.GetType()))
                {
                    if (openForm.Visible)
                    {
                        openForm.Hide();
                    }
                    else
                    {
                        openForm.Show();
                        openForm.BringToFront();
                        openForm.Activate();
                        SetForegroundWindow(openForm.Handle);
                    }

                    // 4. TopMost zurücksetzen und fertig
                    foreach (var kvp in topMostStates)
                        kvp.Key.TopMost = kvp.Value;

                    return;
                }
            }

            // 5. Kein gültiges Fenster → TopMost zurücksetzen
            foreach (var kvp in topMostStates)
                kvp.Key.TopMost = kvp.Value;

            MessageBox.Show("Kein gültiges aktives Fenster deines Programms gefunden.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            LaunchExternalTool("Blish HUD.exe");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LaunchExternalTool("GW2TacO.exe");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Discord-Ordner und Pfad zur Update.exe
                string discordFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Discord");

                string updaterPath = Path.Combine(discordFolder, "Update.exe");
                string shortcutPath = Path.Combine(discordFolder, "StartDiscord.lnk");

                // Prüfen ob Update.exe existiert
                if (!File.Exists(updaterPath))
                {
                    MessageBox.Show("Install Discord first!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Falls keine Verknüpfung vorhanden ist, erstellen
                if (!File.Exists(shortcutPath))
                {
                    CreateShortcut(shortcutPath, updaterPath, "--processStart \"Discord.exe\"", "Start Discord");
                }

                // Verknüpfung starten
                Process.Start("explorer.exe", $"\"{shortcutPath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting Dissi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateShortcut(string shortcutPath, string targetPath, string arguments, string description)
        {
            // COM-Verweis nötig: "Windows Script Host Object Model"
            var wsh = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(shortcutPath);
            shortcut.Description = description;
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.Save();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _overlayWindow = OverlayWindow.GetInstance(); // Hole Singleton

            if (_overlayWindow.IsVisible)
            {
                _overlayWindow.Hide();
            }
            else
            {
                _overlayWindow.Show();
                _overlayWindow.Activate(); // Fokus und bringt es in den Vordergrund
            }
        }
        private void LaunchExternalTool(string executableName)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, executableName);

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Starten von {executableName}:\n{ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"{executableName} wurde nicht gefunden im Verzeichnis:\n{exeDirectory}", "Datei nicht gefunden", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
