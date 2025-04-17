using System.Diagnostics;
using static GW2FOX.BossTimerService;
using static GW2FOX.GlobalVariables;

namespace GW2FOX
{
    public partial class BaseForm : Form
    {

        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook? _globalKeyboardHook;

        public static ListView CustomBossList { get; private set; } = new ListView();

        public BaseForm()
        {
            InitializeCustomBossList();
            InitializeGlobalKeyboardHook();
            SetFormTransparency();
        }

        protected void SetFormTransparency()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.Opacity = 0.90;
        }
        protected void InitializeBossTimerAndOverlay()
        {
            bossTimer = new BossTimer(customBossList);
            overlay = new Overlay(customBossList);
            overlay.WindowState = FormWindowState.Normal;
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
                if (this is Main)
                {
                    Timer_Click(sender, e);
                }
            }
        }
        protected void InitializeCustomBossList()
        {
            customBossList = new DoubleBufferedListView(); // statt new ListView()
            customBossList.View = View.Details;
            customBossList.Columns.Add("Meta", 145);
            customBossList.Columns.Add("Time", 78);
            customBossList.Location = new Point(0, 0);
            customBossList.ForeColor = Color.White;
            customBossList.FullRowSelect = true;
            customBossList.OwnerDraw = true;

            customBossList.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        protected void ShowAndHideForm(Form newForm)
        {
            newForm.Owner = this;
            newForm.Show();

            if (this is Worldbosses || this is Main)
            {
                this.Hide();
            }
            else
            {
                this.Close();
            }
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
