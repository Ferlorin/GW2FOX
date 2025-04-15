using System.Diagnostics;
using static GW2FOX.BossTimerService;
using static GW2FOX.GlobalVariables;

namespace GW2FOX
{
    public partial class BaseForm : Form
    {



        public BaseForm()
        {
            InitializeComponent();

            // Setzen Sie hier Ihre gemeinsamen Eigenschaften
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = Properties.Resources.Background;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "GW2FOX";
            this.SizeGripStyle = SizeGripStyle.Show;
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Normal;
            this.DoubleBuffered = true;
            // Weitere gemeinsame Eigenschaften setzen...
        }


        protected void ShowAndHideForm(Form newForm)
        {
            // Speichern Sie die Größe und Position des aktuellen Formulars
            Size currentSize = this.Size;
            Point currentPosition = this.Location;

            // Setzen Sie die Größe und Position des neuen Formulars
            newForm.Size = currentSize;
            newForm.Location = currentPosition;

            // Setzen Sie das aktuelle Formular als Besitzer des neuen Formulars
            newForm.Owner = this;

            // Zeigen Sie das neue Formular an und verbergen Sie das aktuelle Formular
            newForm.Show();
            this.Hide();
        }

        protected static void SaveTextToFile(string textToSave, string sectionHeader, bool hideMessages = false)
        {
            var headerToUse = sectionHeader;
            if (headerToUse.EndsWith(':'))
            {
                headerToUse = headerToUse[..^1];
            }

            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(FILE_PATH);

                // Index der Zeile mit dem angegebenen Header finden
                int headerIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(headerToUse + ":"))
                    {
                        headerIndex = i;
                        break;
                    }
                }

                // Wenn der Header gefunden wird, den Text aktualisieren
                if (headerIndex != -1)
                {
                    lines[headerIndex] = $"{headerToUse}: \"{textToSave}\"";
                }
                else
                {
                    // Wenn der Header nicht gefunden wird, eine neue Zeile hinzufügen
                    lines = lines.Concat(new[] { $"{headerToUse}: \"{textToSave}\"" }).ToArray();
                }

                // Aktualisierten Inhalt zurück in die Datei schreiben
                File.WriteAllLines(FILE_PATH, lines);

                if (!hideMessages)
                {
                    MessageBox.Show($"{headerToUse} saved.", "Saved!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {headerToUse}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void AdjustWindowSize()
        {
            Screen currentScreen = Screen.FromControl(this);
            Rectangle workingArea = currentScreen.WorkingArea;

            if (Width > workingArea.Width || Height > workingArea.Height)
            {
                Size = new Size(
                    Math.Min(Width, workingArea.Width),
                    Math.Min(Height, workingArea.Height)
                );

                Location = new Point(
                    workingArea.Left + (workingArea.Width - Width) / 2,
                    workingArea.Top + (workingArea.Height - Height) / 2
                );
            }

            if (Height > workingArea.Height)
            {
                Height = workingArea.Height;
            }

            if (Bottom > workingArea.Bottom)
            {
                Location = new Point(
                    Left,
                    Math.Max(workingArea.Top, workingArea.Bottom - Height)
                );
            }
        }
        protected void HandleException(Exception ex, string methodName)
        {
            Console.WriteLine($"Exception in {methodName}: {ex}");
        }

        private void LoadTextFromConfig(string sectionHeader, TextBox textBox, string configText,
            string defaultToInsert)
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
                SaveTextToFile(defaultToInsert, sectionHeader, true);
                configText = File.ReadAllText(FILE_PATH);
                LoadTextFromConfig(sectionHeader, textBox, configText, defaultToInsert);
                // Muster wurde nicht gefunden
                // MessageBox.Show($"Das Muster '{sectionHeader}' wurde in der Konfigurationsdatei nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected void BringGw2ToFront()
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

        protected void LoadConfigText(TextBox Runinfo, TextBox Squadinfo, TextBox Guild, TextBox Welcome, TextBox Symbols)
        {
            try
            {

                // Überprüfen, ob die Datei existiert
                if (File.Exists(FILE_PATH))
                {
                    // Den gesamten Text aus der Datei lesen
                    string configText = File.ReadAllText(FILE_PATH);

                    // Laden von Runinfo
                    LoadTextFromConfig("Runinfo:", Runinfo, configText, DEFAULT_RUN_INFO);

                    // Laden von Squadinfo
                    LoadTextFromConfig("Squadinfo:", Squadinfo, configText, DEFAULT_SQUAD_INFO);

                    // Laden von Guild
                    LoadTextFromConfig("Guild:", Guild, configText, DEFAULT_GUILD);

                    // Laden von Welcome
                    LoadTextFromConfig("Welcome:", Welcome, configText, DEFAULT_WELCOME);

                    // Laden von Symbols
                    LoadTextFromConfig("Symbols:", Symbols, configText, DEFAULT_SYMBOLS);
                }
                else
                {

                    Console.WriteLine($"Config file does not exist. Will try to create it");
                    try
                    {
                        var fileStream = File.Create(FILE_PATH);
                        fileStream.Close();
                        LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception, but don't call ReadConfigFile recursively
                        Console.WriteLine($"Error creating config file: {ex.Message}");
                        throw; // Re-throw the exception to prevent infinite recursion
                    }
                    // // Die Konfigurationsdatei existiert nicht
                    // MessageBox.Show("Die Konfigurationsdatei 'config.txt' wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Fehler beim Laden der Konfigurationsdatei
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _bossTimer?.Dispose(); // Dispose of the BossTimer first
            base.OnFormClosing(e);
            Application.Exit();
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Owner?.Show();
            Dispose();
        }



    }
}
