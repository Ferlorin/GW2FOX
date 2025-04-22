using System.Diagnostics;
using System.Runtime.InteropServices;
using static GW2FOX.BossTimerService;
using static GW2FOX.GlobalVariables;
using System.Threading;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using WinFormsButton = System.Windows.Forms.Button;
using System.Windows;
using System.Text.Json;
using Newtonsoft.Json;


namespace GW2FOX
{
    public partial class BaseForm : Form
    {
        private readonly Dictionary<System.Windows.Forms.Button, System.Drawing.Size> originalSizes = new();
        protected OverlayWindow overlayWindow; // Ersetzt Overlay durch OverlayWindow
        protected System.Windows.Controls.ListView customBossList;
        protected BossTimer bossTimer;
        private GlobalKeyboardHook? _globalKeyboardHook;
        protected Form lastOpenedBoss = null;
        public static System.Windows.Controls.ListView CustomBossList { get; private set; } = new System.Windows.Controls.ListView();

        public BaseForm()
        {
            InitializeGlobalKeyboardHook();
            SetFormTransparency();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddButtonAnimations(this); // 💥 wird auf jeder Form aufgerufen
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        protected const int SW_RESTORE = 9;

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

       

        protected void ShowAndHideForm(Form newForm)
        {
            if (lastOpenedBoss != null && !lastOpenedBoss.IsDisposed)
            {
                lastOpenedBoss.Hide();
            }

            lastOpenedBoss = newForm;

            newForm.Owner = this;
            ShowFormWithoutActivation(newForm);

            if (newForm.WindowState == FormWindowState.Minimized)
                newForm.WindowState = FormWindowState.Normal;

            newForm.BringToFront();
            newForm.Activate();

            SetForegroundWindow(newForm.Handle);

            if (this is Worldbosses || this is Main)
            {
                this.Hide();
            }
        }

        protected void SetFormTransparency()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            this.Opacity = 0.90;
            this.TopMost = true;
        }

        private void ShowFormWithoutActivation(Form form)
        {
            var style = NativeMethods.GetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE);
            NativeMethods.SetWindowLong(form.Handle, NativeMethods.GWL_EXSTYLE, style | NativeMethods.WS_EX_NOACTIVATE);
            form.Show();
        }

        internal static class NativeMethods
        {
            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_NOACTIVATE = 0x08000000;

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }

        protected void BringGw2ToFront()
        {
            try
            {
                string processName = "Gw2-64";
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    IntPtr mainWindowHandle = processes[0].MainWindowHandle;

                    if (mainWindowHandle != IntPtr.Zero)
                    {
                        // Falls das Fenster minimiert ist, stelle es wieder her
                        ShowWindow(mainWindowHandle, SW_RESTORE);

                        // Setze das Fenster in den Vordergrund und fokussiere es
                        SetForegroundWindow(mainWindowHandle);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Fensterhandle von Gw2-64.exe nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Gw2-64.exe läuft nicht.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Fehler beim Fokussieren von Gw2-64.exe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected static void SaveTextToFile(string textToSave, string sectionHeader, bool hideMessages = false)
        {
            string jsonPath = "bosses_config.json";

            try
            {
                Console.WriteLine($"[SaveTextToFile] Versuche '{sectionHeader}' zu speichern...");

                var config = BossTimings.LoadedConfig ?? new BossConfig();

                switch (sectionHeader)
                {
                    case "Runinfo":
                        config.Runinfo = textToSave;
                        break;
                    case "Squadinfo":
                        config.Squadinfo = textToSave;
                        break;
                    case "Guild":
                        config.Guild = textToSave;
                        break;
                    case "Welcome":
                        config.Welcome = textToSave;
                        break;
                    case "Symbols":
                        config.Symbols = textToSave;
                        break;
                    case "Meta":
                        config.Meta = textToSave;
                        break;
                    case "Mixed":
                        config.Mixed = textToSave;
                        break;
                    case "World":
                        config.World = textToSave;
                        break;
                    case "Fido":
                        config.Fido = textToSave;
                        break;
                    default:
                        Console.WriteLine($"[SaveTextToFile] Abschnitt '{sectionHeader}' wird nicht unterstützt.");
                        if (!hideMessages)
                        {
                            System.Windows.Forms.MessageBox.Show($"Section '{sectionHeader}' not recognized.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        return;
                }

                // Schreiben und global aktualisieren
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                BossTimings.LoadedConfig = config;

                Console.WriteLine($"[SaveTextToFile] '{sectionHeader}' erfolgreich gespeichert.");

                if (!hideMessages)
                {
                    System.Windows.Forms.MessageBox.Show($"{sectionHeader} gespeichert.", "Gespeichert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SaveTextToFile] Fehler: {ex.Message}");
                System.Windows.Forms.MessageBox.Show($"Fehler beim Speichern von {sectionHeader}: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadTextFromConfig(string sectionHeader, System.Windows.Forms.TextBox textBox, string defaultToInsert)
        {
            string jsonPath = "bosses_config.json";

            try
            {
                Console.WriteLine($"[LoadTextFromConfig] Versuche '{sectionHeader}' aus {jsonPath} zu laden...");

                if (!File.Exists(jsonPath))
                {
                    Console.WriteLine($"[LoadTextFromConfig] Datei '{jsonPath}' existiert nicht – wird mit Defaultwert neu erstellt.");
                    SaveTextToFile(defaultToInsert, sectionHeader, true);
                    LoadTextFromConfig(sectionHeader, textBox, defaultToInsert); // Retry
                    return;
                }

                string jsonContent = File.ReadAllText(jsonPath);
                Console.WriteLine($"[LoadTextFromConfig] JSON geladen. Länge: {jsonContent.Length} Zeichen.");

                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent) ?? new();

                Console.WriteLine($"[LoadTextFromConfig] JSON deserialisiert. Einträge: {config.Count}");

                if (config.TryGetValue(sectionHeader, out string value))
                {
                    Console.WriteLine($"[LoadTextFromConfig] Eintrag '{sectionHeader}' gefunden, wird in TextBox gesetzt.");
                    textBox.Text = value;
                }
                else
                {
                    Console.WriteLine($"[LoadTextFromConfig] Eintrag '{sectionHeader}' fehlt – wird neu gespeichert und erneut geladen.");
                    SaveTextToFile(defaultToInsert, sectionHeader, true);
                    LoadTextFromConfig(sectionHeader, textBox, defaultToInsert); // Retry
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadTextFromConfig] Fehler: {ex.Message}");
                System.Windows.Forms.MessageBox.Show($"Error loading {sectionHeader}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected void LoadConfigText(
            System.Windows.Forms.TextBox runinfoBox,
            System.Windows.Forms.TextBox squadinfoBox,
            System.Windows.Forms.TextBox guildBox,
            System.Windows.Forms.TextBox welcomeBox,
            System.Windows.Forms.TextBox symbolsBox)
        {
            var config = BossTimings.LoadedConfig;

            runinfoBox.Text = config?.Runinfo ?? "«Meta-Train» with the old [FOX]";
            squadinfoBox.Text = config?.Squadinfo ?? "• InstanceCheck:\n    - right click on me & join map\n• Don’t cancel invites!\n• No 3ple Trouble\n\n• https://gw2fox.wixsite.com/about";
            guildBox.Text = config?.Guild ?? "☠ Young or old [FOX], we take every stray. Humor, respect and fun at the game are what distinguish us. No Obligations! Infos: wsp me or https://gw2fox.wixsite.com/about ☻";
            welcomeBox.Text = config?.Welcome ?? "Welcome to the FOXhole. Read the Message of the Day for Infos - Questions, ask us! Guides & Tools on our Homepage: https://gw2fox.wixsite.com/about";
            symbolsBox.Text = config?.Symbols ?? "☠ ★ ☣ ☮ ☢ ♪ ☜ ☞ ┌ ∩ ┐ ( ●̮̃ • ) ۶ ( • ◡ • ) ☿ ♀ ♂ ☀ ☁ ☂ ☃ ☄ ☾ ☽ ☇ ☉ ☐ ☒ ☑ ☝ ☚ • ☟ ☆ ♕ ♖ ♘ ♙ ♛ ♜ ♞ ♟ † ☨ ☥ ☦ ☓ ☩ ☯ ☧ ☬ ☸ ♁ ♆ ☭ ☪ ☫ © ™ ® ☎ ♥ 凸";
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bossTimer?.Dispose();
            overlayWindow?.Close(); // Schließt das OverlayWindow
            base.OnFormClosing(e);

            if (this is Main)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Owner?.Show();
            Dispose();
        }


        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is WinFormsButton button)
            {
                if (!originalSizes.ContainsKey(button))
                {
                    originalSizes[button] = button.Size;
                }

                // Vergrößere die Größe um 10%
                int newWidth = (int)(originalSizes[button].Width * 1.03);
                int newHeight = (int)(originalSizes[button].Height * 1.03);
                button.Size = new System.Drawing.Size(newWidth, newHeight);
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms. Button button && originalSizes.TryGetValue(button, out System.Drawing.Size originalSize))
            {
                // Setze die Originalgröße wieder her
                button.Size = originalSize;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Forms.Button button)
            {
                if (!originalSizes.ContainsKey(button))
                {
                    originalSizes[button] = button.Size;
                }

                // Verkleinere die Größe um 3%
                int newWidth = (int)(originalSizes[button].Width * 0.97);
                int newHeight = (int)(originalSizes[button].Height * 0.97);
                button.Size = new System.Drawing.Size(newWidth, newHeight);
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Forms.Button button && originalSizes.TryGetValue(button, out System.Drawing.Size originalSize))
            {
                button.Size = originalSize;
            }
        }

        private void AddButtonAnimations(System.Windows.Forms.Control control)
        {
            foreach (System.Windows.Forms.Control c in control.Controls)
            {
                if (c is System.Windows.Forms.Button  button)
                {
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                    button.MouseDown += Button_MouseDown;
                    button.MouseUp += Button_MouseUp;
                }

                // Rekursiv alle Child-Controls durchgehen
                if (c.HasChildren)
                {
                    AddButtonAnimations(c);
                }
            }
        }

    }
}
