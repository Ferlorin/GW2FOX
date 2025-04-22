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

        protected static void SaveTextToFile(string textToSave, string sectionHeader, bool hideMessages = false)
        {
            string jsonPath = "bosses_config.json";

            try
            {
                Dictionary<string, string> config = File.Exists(jsonPath)
                    ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonPath)) ?? new()
                    : new();

                config[sectionHeader] = textToSave;

                File.WriteAllText(jsonPath, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));

                if (!hideMessages)
                {
                    System.Windows.Forms.MessageBox.Show($"{sectionHeader} saved.", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error {sectionHeader}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTextFromConfig(string sectionHeader, System.Windows.Forms.TextBox textBox, string defaultToInsert)
        {
            string jsonPath = "bosses_config.json";

            try
            {
                Dictionary<string, string> config = File.Exists(jsonPath)
                    ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonPath)) ?? new()
                    : new();

                if (config.TryGetValue(sectionHeader, out string value))
                {
                    textBox.Text = value;
                }
                else
                {
                    SaveTextToFile(defaultToInsert, sectionHeader, true);
                    LoadTextFromConfig(sectionHeader, textBox, defaultToInsert); // Retry
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error loading {sectionHeader}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        protected void LoadConfigText(
    System.Windows.Forms.TextBox Runinfo,
    System.Windows.Forms.TextBox Squadinfo,
    System.Windows.Forms.TextBox Guild,
    System.Windows.Forms.TextBox Welcome,
    System.Windows.Forms.TextBox Symbols)
        {
            string jsonPath = "bosses_config.json";

            try
            {
                // JSON laden oder leeres Dictionary erzeugen
                Dictionary<string, string> config = File.Exists(jsonPath)
                    ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(jsonPath)) ?? new()
                    : new();

                // Helper-Funktion zum Einlesen + Erstellen falls nicht vorhanden
                void LoadOrInsert(string key, System.Windows.Forms.TextBox box, string fallback)
                {
                    if (config.TryGetValue(key, out string value))
                    {
                        box.Text = value;
                    }
                    else
                    {
                        config[key] = fallback;
                        box.Text = fallback;
                    }
                }

                // Standardtexte direkt hier definieren
                LoadOrInsert("Runinfo", Runinfo, "«Meta-Train» with the old [FOX]");
                LoadOrInsert("Squadinfo", Squadinfo, "• InstanceCheck:\n    - right click on me & join map\n• Don’t cancel invites!\n• No 3ple Trouble\n\n• https://gw2fox.wixsite.com/about");
                LoadOrInsert("Guild", Guild, "☠ Young or old [FOX], we take every stray. Humor, respect and fun at the game are what distinguish us. No Obligations! Infos: wsp me or https://gw2fox.wixsite.com/about ☻");
                LoadOrInsert("Welcome", Welcome, "Welcome to the FOXhole. Read the Message of the Day for Infos - Questions, ask us! Guides & Tools on our Homepage: https://gw2fox.wixsite.com/about");
                LoadOrInsert("Symbols", Symbols, "☠ ★ ☣ ☮ ☢ ♪ ☜ ☞ ┌ ∩ ┐ ( ●̮̃ • ) ۶ ( • ◡ • ) ☿ ♀ ♂ ☀ ☁ ☂ ☃ ☄ ☾ ☽ ☇ ☉ ☐ ☒ ☑ ☝ ☚ • ☟ ☆ ♕ ♖ ♘ ♙ ♛ ♜ ♞ ♟ † ☨ ☥ ☦ ☓ ☩ ☯ ☧ ☬ ☸ ♁ ♆ ☭ ☪ ☫ © ™ ® ☎ ♥ 凸");

                // Gespeicherte Änderungen zurückschreiben
                File.WriteAllText(jsonPath, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
