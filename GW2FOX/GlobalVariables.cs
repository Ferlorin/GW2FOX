using System.Runtime.InteropServices;

namespace GW2FOX;

public class GlobalVariables
{
    public static readonly DateTime CURRENT_DATE_TIME =
        TimeZoneInfo
            .ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
            );

    public static readonly TimeSpan CURRENT_TIME = CURRENT_DATE_TIME.TimeOfDay;
    public static string FILE_PATH = "config.txt";
    
    // Constants for window handling
    public const int SW_RESTORE = 9;
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
}