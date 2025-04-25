using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

public class GlobalKeyboardHook
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    
    private const int WM_KEYUP = 0x0101;       // Add key up message
    private const int WM_SYSKEYUP = 0x0105;   // Add system key up message
    private HashSet<Keys> _pressedKeys = new(); // Track currently pressed keys


    private readonly LowLevelKeyboardProc _proc;
    private IntPtr _hookId = IntPtr.Zero;

    public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    public GlobalKeyboardHook()
    {
        _proc = HookCallback;
        _hookId = SetHook(_proc);
    }

    ~GlobalKeyboardHook()
    {
        UnhookWindowsHookEx(_hookId);
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)  // Process only if there's no error
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Keys key = (Keys)vkCode;

            if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                // Key is pressed, add to the set
                _pressedKeys.Add(key);
                KeyPressed?.Invoke(this, new KeyPressedEventArgs(key));
            }

            if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                // Key is released, remove it from the set
                _pressedKeys.Remove(key);
            }

        }

        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    public IReadOnlyCollection<Keys> PressedKeys => _pressedKeys;

    #region WinAPI-Importe

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    #endregion
}


public class KeyPressedEventArgs : EventArgs
{
    public Keys Key { get; private set; }

    public KeyPressedEventArgs(Keys key)
    {
        Key = key;
    }
}
