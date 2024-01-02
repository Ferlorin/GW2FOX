﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Dragonstorm : BaseForm
    {
        public Dragonstorm()
        {
            InitializeComponent();
            LoadConfigText();
        }

        // Variable zur Speicherung des Ursprungs der Seite
        private string originPage;

        // Konstruktor, der den Ursprung der Seite als Parameter akzeptiert
        public Dragonstorm(string origin)
        {
            InitializeComponent();
            LoadConfigText();

            // Setze den Ursprung der Seite
            originPage = origin;
        }

        private void LoadConfigText()
        {
            try
            {
                // Pfad zur Konfigurationsdatei
                string configFilePath = "config.txt";

                // Überprüfen, ob die Datei existiert
                if (File.Exists(configFilePath))
                {
                    // Den gesamten Text aus der Datei lesen
                    string configText = File.ReadAllText(configFilePath);

                    // Laden von Runinfo
                    LoadTextFromConfig("Runinfo:", Runinfo, configText);

                    // Laden von Squadinfo
                    LoadTextFromConfig("Squadinfo:", Dragonstorminstance, configText);

                    // Laden von Guild
                    LoadTextFromConfig("Guild:", Guild, configText);

                    // Laden von Welcome
                    LoadTextFromConfig("Welcome:", Welcome, configText);

                    // Laden von Symbols
                    LoadTextFromConfig("Symbols:", Symbols, configText);
                }
                else
                {
                    // Die Konfigurationsdatei existiert nicht
                    MessageBox.Show("Die Konfigurationsdatei 'config.txt' wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Fehler beim Laden der Konfigurationsdatei
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTextFromConfig(string sectionHeader, TextBox textBox, string configText)
        {
            // Suchmuster für den Abschnitt und den eingeschlossenen Text in Anführungszeichen
            string pattern = $@"{sectionHeader}\s*""([^""]*)""";

            // Mit einem regulären Ausdruck nach dem Muster suchen
            var match = System.Text.RegularExpressions.Regex.Match(configText, pattern);

            // Überprüfen, ob ein Treffer gefunden wurde
            if (match.Success)
            {
                // Den extrahierten Text in das Textfeld einfügen
                textBox.Text = match.Groups[1].Value;
            }
            else
            {
                // Muster wurde nicht gefunden
                MessageBox.Show($"Das Muster '{sectionHeader}' wurde in der Konfigurationsdatei nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BringGw2ToFront()
        {
            try
            {
                // Specify the process name without the file extension
                string processName = "Gw2-64";

                // Get the processes by name
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    // Bring the first instance to the foreground
                    IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                    ShowWindow(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                }
                else
                {
                    MessageBox.Show("Gw2-64.exe is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error bringing Gw2-64.exe to the foreground: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Constants for window handling
        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void Back_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Worldbosses());
        }

        private void Runinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Runinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Instancestorm_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Dragonstormstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Guild_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Guild.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Welcome_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Welcome.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Storminfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Dragonstorminfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Squadinfo(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Dragonstorminstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Attentionstorm_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Attentiondragonstorm.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Dragonstorminfo1_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Dragonstorminfo1.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Dragonstorminfo2_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Dragonstorminfo2.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }
    }
}