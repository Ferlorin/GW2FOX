using System.Runtime.InteropServices;

namespace GW2FOX;

public class GlobalVariables
{
    public static TimeZoneInfo TIMEZONE_TO_USE = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    public static DateTime CURRENT_DATE_TIME =>
        TimeZoneInfo
            .ConvertTimeFromUtc(
                DateTime.UtcNow, 
                // DateTime.UtcNow + TimeSpan.FromHours(11) - TimeSpan.FromMinutes(17),
                // DateTime.Parse("14:18:01"),
                TIMEZONE_TO_USE
            );
    
    public static bool IsDaylightSavingTimeActive()
    {
        return TIMEZONE_TO_USE.IsDaylightSavingTime(CURRENT_DATE_TIME);
    }

    public static TimeSpan CURRENT_TIME => CURRENT_DATE_TIME.TimeOfDay;
    public static DateTime CURRENT_DATE => CURRENT_DATE_TIME.Date;
    public static string FILE_PATH = "config.txt";

    // Constants for window handling
    public const int SW_RESTORE = 9;
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    public static string DEFAULT_BOSSES =
        "LLLA - Timberline Falls, LLLA - Iron Marches, LLLA - Gendarran Fields, The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem Mark II, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";
    public static string DEFAULT_META =
        "Chak Gerent, Battle in Tarir, Death-Branded Shatterer, Junundu Rising, Path to Ascension, Doppelganger, Forged with Fire, Choya Piñata, Nightbosses, Drakkar, Dragonstorm, Claw of Jormag";
    public static string DEFAULT_WORLD =
        "The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem Mark II, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";
    public static string DEFAULT_MIXED =
        "Battle in Tarir, Chak Gerent, Death-Branded Shatterer, Junundu Rising, Path to Ascension, Doppelganger, Forged with Fire, Choya Piñata, Nightbosses, Drakkar, Dragonstorm, The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem Mark II, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";
    public static string DEFAULT_SYMBOLS =
        "☠ ★ ☣ ☮ ☢ ♪ ☜ ☞ ┌ ∩ ┐ ( ●̮̃ • ) ۶ ( • ◡ • ) ♋ ☿ ♀ ♂ ☀ ☁ ☂ ☃ ☄ ☾ ☽ ☇ ☉ ☐ ☒ ☑ ☝ ☚ • ☟ ☆ ♔ ♕ ♖ ♗ ♘ ♙ ♚ ♛ ♜ ♝ ♞ ♟ † ☨ ☥ ☦ ☓ ☩ ☯ ☧ ☬ ☸ ♁ ♆ ☭ ✯ ☪ ☫ ✡ © ™ ® ☕ ☎ ☻ ♥ ⏰ 凸 ◇";
    public static string DEFAULT_WELCOME =
        "Welcome to the FOXhole. Read the Message of the Day for Infos - Questions, ask us! Guides & Tools on our Homepage: https://gw2fox.wixsite.com/about";
    public static string DEFAULT_GUILD =
        "☠ Young or old [FOX], we take every stray. Humor, respect and fun at the game are what distinguish us. No Obligations! Infos: wsp me or https://gw2fox.wixsite.com/about ☻";
    public static string DEFAULT_RUN_INFO =
        "«Meta-Train» with the old [FOX]";
    public static string DEFAULT_SQUAD_INFO =
        "\n• InstanceCheck:\n    - right click on me & join map\n• Don’t cancel invites!\n• No 3ple Trouble\n• https://gw2fox.wixsite.com/about";



}