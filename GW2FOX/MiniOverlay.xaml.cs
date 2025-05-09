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
            Left = 323;
            Top = 0;

            foreach (var img in FindVisualChildren<WpfImage>(this))
            {
                AnimateScale(img, 0.8, 150);
                img.Opacity = 0.7;
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
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private void FocusGw2Window()
        {
            var gw2Proc = Process.GetProcessesByName("Gw2-64").FirstOrDefault();
            if (gw2Proc != null && gw2Proc.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(gw2Proc.MainWindowHandle);
            }
            // Else: GW2 not running → do nothing, keep focus where it is
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


        private void Button_MouseEnter(object sender, WpfMouseEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 1.0, 150);
            img.Opacity = 1.0;
            }
        }

        private void Button_MouseLeave(object sender, WpfMouseEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 0.8, 150);
            img.Opacity = 0.7;
            }
        }

        private void Button_MouseDown(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 0.9, 80);
                img.Opacity = 0.5;
            }
        }

        private void Button_MouseUp(object sender, WpfMouseButtonEventArgs e)
        {
            if (sender is WpfImage img)
            {
                AnimateScale(img, 1.0, 100);
                img.Opacity = 1.0;
            }
        }


    }
}
