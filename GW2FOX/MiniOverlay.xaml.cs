using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace GW2FOX
{
    public partial class MiniOverlay : Window
    {
        private readonly Forms.Form lastOpenedForm;
        private readonly Worldbosses _worldbossesForm;
        private OverlayWindow _overlayWindow;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public MiniOverlay(Worldbosses worldbossesForm)
        {
            InitializeComponent();
            _worldbossesForm = worldbossesForm;
            Topmost = true;
            Loaded += MiniOverlay_Load;
        }

        private void MiniOverlay_Load(object sender, RoutedEventArgs e)
        {
            var screen = Forms.Screen.PrimaryScreen.WorkingArea;
            Left = (screen.Width - Width) / 2;
            Top = 0;
        }

        private void BloodyCom_Click(object sender, MouseButtonEventArgs e)
        {
            // Original C# logic adapted for WPF host
            var excludedTypes = new[] { typeof(MiniOverlay), typeof(Main) };
            var topMostStates = new Dictionary<Forms.Form, bool>();

            // Remember and disable TopMost on all WinForms
            foreach (Forms.Form f in Forms.Application.OpenForms)
            {
                topMostStates[f] = f.TopMost;
                f.TopMost = false;
            }

            // Determine focus
            IntPtr activeHandle = GetForegroundWindow();

            // Find & toggle
            foreach (Forms.Form openForm in Forms.Application.OpenForms)
            {
                if (openForm.Handle == activeHandle && !excludedTypes.Contains(openForm.GetType()))
                {
                    if (openForm.Visible)
                    {
                        openForm.Hide();
                        // Hide Main only if active window wasn't Main
                        if (Forms.Application.OpenForms.OfType<Main>().FirstOrDefault() is Forms.Form mainForm && mainForm.Visible)
                            mainForm.Hide();
                    }
                    else
                    {
                        openForm.Show();
                        openForm.BringToFront();
                        openForm.Activate();
                        SetForegroundWindow(openForm.Handle);
                    }

                    // Restore TopMost states
                    foreach (var kvp in topMostStates)
                        kvp.Key.TopMost = kvp.Value;

                    return;
                }
            }

            // No valid window found → restore TopMost
            foreach (var kvp in topMostStates)
                kvp.Key.TopMost = kvp.Value;

            Forms.MessageBox.Show("Kein gültiges aktives Fenster deines Programms gefunden.", "Hinweis", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Information);
        }

        private void BlishHUD_Click(object sender, MouseButtonEventArgs e)
        {
            LaunchExternalTool("Blish HUD.exe");
        }

        private void GW2TacO_Click(object sender, MouseButtonEventArgs e)
        {
            LaunchExternalTool("GW2TacO.exe");
        }

        private void Discord_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string discordFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Discord");
                string updaterPath = Path.Combine(discordFolder, "Update.exe");
                string shortcutPath = Path.Combine(discordFolder, "StartDiscord.lnk");

                if (!File.Exists(updaterPath))
                {
                    Forms.MessageBox.Show("Install Discord first!", "Fehler", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(shortcutPath))
                {
                    CreateShortcut(shortcutPath, updaterPath, "--processStart \"Discord.exe\"", "Start Discord");
                }

                Process.Start("explorer.exe", $"\"{shortcutPath}\"");
            }
            catch (Exception ex)
            {
                Forms.MessageBox.Show($"Error starting Dissi: {ex.Message}", "Error", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
            }
        }

        private void CreateShortcut(string shortcutPath, string targetPath, string arguments, string description)
        {
            var wsh = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(shortcutPath);
            shortcut.Description = description;
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.Save();
        }

        private void Clock_Click(object sender, MouseButtonEventArgs e)
        {
            _overlayWindow = OverlayWindow.GetInstance();
            if (_overlayWindow.IsVisible)
                _overlayWindow.Hide();
            else
            {
                _overlayWindow.Show();
                _overlayWindow.Activate();
            }
        }

        private void LaunchExternalTool(string executableName)
        {
            string exeDirectory = Path.GetDirectoryName(Forms.Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, executableName);

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    Forms.MessageBox.Show($"Fehler beim Starten von {executableName}:\n{ex.Message}", "Fehler", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                Forms.MessageBox.Show($"{executableName} wurde nicht gefunden im Verzeichnis:\n{exeDirectory}", "Datei nicht gefunden", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
            }
        }
    }
}
