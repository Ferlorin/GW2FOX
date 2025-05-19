using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Forms = System.Windows.Forms;
using WpfImage = System.Windows.Controls.Image;
using WpfMouseEventArgs = System.Windows.Input.MouseEventArgs;
using WpfMouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using WpfPoint = System.Windows.Point;
using System.Windows.Controls;

namespace GW2FOX
{
    public partial class MenuOverlay : Window
    {
        private Worldbosses _worldbossesForm;
        private Textboxes _textboxesForm;
        private readonly Forms.Form lastOpenedForm;
        private OverlayWindow _overlayWindow;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();


        public MenuOverlay()
        {
            InitializeComponent();
            Topmost = true;
            _worldbossesForm = new Worldbosses();
            _textboxesForm = new Textboxes();
            Loaded += MiniOverlay_Load;
        }

        private void MiniOverlay_Load(object sender, RoutedEventArgs e)
        {
            var screen = Forms.Screen.PrimaryScreen.WorkingArea;
            Left = 323;
            Top = 30;

            foreach (var img in FindVisualChildren<WpfImage>(this))
            {
                AnimateScale(img, 0.8, 150);
                img.Opacity = 0.6;
            }
        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private void CreateShortcut(string shortcutPath, string targetPath, string arguments, string description)
        {
            var wsh = new IWshRuntimeLibrary.WshShell();
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(shortcutPath);
            shortcut.Description = description;
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.Save();
        }

        public void HideMenuAndChildren()
        {
            this.Hide();

            if (BossTimerService.WorldbossesInstance != null && !BossTimerService.WorldbossesInstance.IsDisposed)
                BossTimerService.WorldbossesInstance.Hide();

            if (_textboxesForm != null && !_textboxesForm.IsDisposed)
                _textboxesForm.Hide();
        }


        private void BossNameText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                switch (textBlock.Text)
                {
                    case "• Hide Timer":
                        Clock_Click(sender, e);
                        break;

                    case "• Timer Menu":
                        var wbForm = BossTimerService.WorldbossesInstance;
                        if (wbForm == null || wbForm.IsDisposed)
                        {
                            wbForm = new Worldbosses();
                            BossTimerService.WorldbossesInstance = wbForm;
                            wbForm.Show();
                        }
                        else if (wbForm.Visible)
                        {
                            wbForm.Hide();
                        }
                        else
                        {
                            wbForm.Show();
                            wbForm.BringToFront();
                        }
                        break;


                    case "• Text Menu":
                        if (_textboxesForm == null || _textboxesForm.IsDisposed)
                        {
                            _textboxesForm = new Textboxes();
                            _textboxesForm.Show();
                        }
                        else if (_textboxesForm.Visible)
                        {
                            _textboxesForm.Hide();
                        }
                        else
                        {
                            _textboxesForm.Show();
                            _textboxesForm.BringToFront();
                        }
                        break;
                    case "• Close":
                        {
                            Main.CloseAll_Click(sender, e);
                        }
                        break;

                    case "• Main Menu":
                        Worldbosses.RestartApplication();
                        return; 
                }
                FocusGw2Window();
            }
        }


        private void FocusGw2Window()
        {
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
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
                    Forms.MessageBox.Show($"Error {executableName}:\n{ex.Message}", "Error", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                Forms.MessageBox.Show($"{executableName} not found in directory:\n{exeDirectory}", "File not found", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Error);
            }
        }


        private void AnimateScale(WpfImage image, double toScale, double durationMs = 100)
        {
            if (image.RenderTransform is not ScaleTransform scaleTransform)
            {
                scaleTransform = new ScaleTransform(1.0, 1.0);
                image.RenderTransform = scaleTransform;
                image.RenderTransformOrigin = new WpfPoint(0.5, 0.5);
            }

            var animX = new DoubleAnimation
            {
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var animY = new DoubleAnimation
            {
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animY);
        }

        public void HideChildForms()
        {
            if (_textboxesForm != null && !_textboxesForm.IsDisposed && _textboxesForm.Visible)
                _textboxesForm.Hide();

            if (_worldbossesForm != null && !_worldbossesForm.IsDisposed && _worldbossesForm.Visible)
                _worldbossesForm.Hide();
        }

    }
}
