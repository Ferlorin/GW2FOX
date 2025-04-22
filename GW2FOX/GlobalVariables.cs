using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace GW2FOX;

public class GlobalVariables
{
    public static TimeZoneInfo TIMEZONE_TO_USE = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    public static DateTime CURRENT_LOCAL_TIME =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TIMEZONE_TO_USE);

    public static DateTime CURRENT_DATE_TIME =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TIMEZONE_TO_USE);

    public static bool IsDaylightSavingTimeActive() =>
        TIMEZONE_TO_USE.IsDaylightSavingTime(CURRENT_LOCAL_TIME);


    public const int SW_RESTORE = 9;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

}