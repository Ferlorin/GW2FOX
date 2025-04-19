﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class MiniOverlay : BaseForm
    {
        private Form lastOpenedForm;
        private Form lastOpenedBoss = null;

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

        private void button1_Click(object sender, EventArgs e)
        {
            var allowedTypes = new[] { typeof(Main), typeof(MiniOverlay), typeof(Overlay), typeof(Worldbosses) };

            foreach (Form openForm in Application.OpenForms)
            {
                if (!allowedTypes.Contains(openForm.GetType()))
                {
                    // Toggle Verhalten für "andere" Fenster
                    openForm.ShowInTaskbar = false; // Verhindert Taskleisten-Eintrag
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
                    return;
                }
            }

            if (lastOpenedBoss == null || lastOpenedBoss.IsDisposed)
            {
                lastOpenedBoss = new Worldbosses
                {
                    ShowInTaskbar = false // Wichtig: keine Taskleiste
                };
                lastOpenedBoss.Show();
            }
            else if (lastOpenedBoss.Visible)
            {
                lastOpenedBoss.Hide();
            }
            else
            {
                lastOpenedBoss.Show();
                lastOpenedBoss.BringToFront();
                lastOpenedBoss.Activate();
                SetForegroundWindow(lastOpenedBoss.Handle);
            }
        }





        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
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
            var overlayForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f is Overlay);

            if (overlayForm != null)
            {
                // If the overlay is visible, hide it
                if (overlayForm.Visible)
                {
                    overlayForm.Hide();
                }
                // If the overlay is hidden, show it
                else
                {
                    overlayForm.Show();
                }
            }
        }
    }
}
