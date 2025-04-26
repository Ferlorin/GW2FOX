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
using System;
using System.Windows.Forms;

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
            AddButtonAnimations(this);
            BossTimings.LoadBossConfigInfos("BossTimings.json");
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        private const int SW_RESTORE = 9;

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

        public static void BringGw2ToFront()
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
                        // Restore window if it's minimized
                        ShowWindow(mainWindowHandle, SW_RESTORE);

                        // Force focus
                        uint currentThreadId = GetCurrentThreadId();
                        uint gw2ThreadId = GetWindowThreadProcessId(mainWindowHandle, IntPtr.Zero);

                        // Temporarily attach input threads to force focus
                        AttachThreadInput(gw2ThreadId, currentThreadId, true);
                        SetForegroundWindow(mainWindowHandle);
                        AttachThreadInput(gw2ThreadId, currentThreadId, false);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Could not find the main window handle of Gw2-64.exe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Gw2-64.exe is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Failed to focus Gw2-64.exe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected static void SaveTextToFile(string textToSave, string sectionHeader, bool hideMessages = false)
        {
            string jsonPath = "BossTimings.json";

            try
            {
                BossConfigInfos config;

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);
                    config = JsonConvert.DeserializeObject<BossConfigInfos>(json) ?? new BossConfigInfos();
                }
                else
                {
                    config = new BossConfigInfos();
                }

                // 🪄 Hier ist die Magie: Normalize Line Breaks
                textToSave = textToSave.Replace(Environment.NewLine, "\n");

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
                    default:
                        if (!hideMessages)
                        {
                            System.Windows.Forms.MessageBox.Show($"Section '{sectionHeader}' not recognized.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        return;
                }

                // Speichern zurück in Datei
                var updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(jsonPath, updatedJson);

                BossTimings.LoadedConfigInfos = config;

                if (!hideMessages)
                {
                    System.Windows.Forms.MessageBox.Show($"{sectionHeader} saved.", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error {sectionHeader}: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        protected void LoadConfigText(
    System.Windows.Forms.TextBox runinfoBox,
    System.Windows.Forms.TextBox squadinfoBox,
    System.Windows.Forms.TextBox guildBox,
    System.Windows.Forms.TextBox welcomeBox,
    System.Windows.Forms.TextBox symbolsBox)
        {

            var config = BossTimings.LoadedConfigInfos;

            if (config == null)
            {
                return;
            }

            runinfoBox.Text = (config.Runinfo ?? "").Replace("\n", Environment.NewLine);
            squadinfoBox.Text = (config.Squadinfo ?? "").Replace("\n", Environment.NewLine);
            guildBox.Text = (config.Guild ?? "").Replace("\n", Environment.NewLine);
            welcomeBox.Text = (config.Welcome ?? "").Replace("\n", Environment.NewLine);
            symbolsBox.Text = (config.Symbols ?? "").Replace("\n", Environment.NewLine);
            Console.WriteLine("LoadedConfigInfos: " + (config != null ? "OK" : "NULL"));

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
