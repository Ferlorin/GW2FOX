using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace GW2FOX;

public class GlobalVariables
{
    public static TimeZoneInfo TIMEZONE_TO_USE = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    public static DateTime ConvertUtcToLocal(DateTime utcDateTime)
    {
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TIMEZONE_TO_USE);
        return localTime;
    }

    public static DateTime CURRENT_LOCAL_TIME
    {
        get
        {
            DateTime currentUtcTime = DateTime.UtcNow;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, TIMEZONE_TO_USE);


            return localTime;
        }
    }

    public static DateTime CURRENT_DATE_TIME
    {
        get
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, TIMEZONE_TO_USE);
            return localDateTime;
        }
    }


    public static bool IsDaylightSavingTimeActive()
    {
        return TIMEZONE_TO_USE.IsDaylightSavingTime(CURRENT_LOCAL_TIME);
    }

 
    public static string FILE_PATH = "config.txt";

   
    public const int SW_RESTORE = 9;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    public static string DEFAULT_BOSSES =
       "FireShaman, LLA Timberline, LLA Iron Marches, LLA Gendarran, The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem M2, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";

    public static string DEFAULT_META =
        "Chak Gerent, Battle in Tarir, DB Shatterer, Junundu Rising, Path to Ascension, Doppelganger, Forged with Fire, Choya Piñata, Nightbosses, Drakkar, Dragonstorm, Claw of Jormag";

    public static string DEFAULT_WORLD =
        "FireShaman, LLA Timberline, LLA Iron Marches, LLA Gendarran, The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem M2, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";

    public static string DEFAULT_FIDO =
         "Tequatl the Sunless, Effigy, Karka Queen, LLA Timberline, Chak Gerent, Doomlore Shrine, The Oil Floes, Battle in Tarir, DB Shatterer, Path to Ascension, Doppelganger, Palawadan, Choya Piñata, Nightbosses, Dragonstorm, Forged with Fire, Ooze Pits, Aetherblade Assault";

    public static string DEFAULT_MIXED =
        "FireShaman, Battle in Tarir, Chak Gerent, DB Shatterer, Junundu Rising, Path to Ascension, Doppelganger, Forged with Fire, Choya Piñata, Nightbosses, Drakkar, Dragonstorm, The frozen Maw, Shadow Behemoth, Fire Elemental, Great Jungle Wurm, Ulgoth the Modniir, Taidha Covington, Megadestroyer, Inquest Golem M2, Tequatl the Sunless, The Shatterer, Karka Queen, Claw of Jormag";

    public static string DEFAULT_SYMBOLS =
        "\u2620 \u2605 \u2623 \u262e \u2622 \u266a \u261c \u261e \u250c \u2229 \u2510 ( \u25cf\u032e\u0303 • ) ۶ ( • \u25e1 • ) \u264b \u263f \u2640 \u2642 \u2600 \u2601 \u2602 \u2603 \u2604 \u263e \u263d \u2607 \u2609 \u2610 \u2612 \u2611 \u261d \u261a • \u261f \u2606 \u2654 \u2655 \u2656 \u2657 \u2658 \u2659 \u265a \u265b \u265c \u265d \u265e \u265f † \u2628 \u2625 \u2626 \u2613 \u2629 \u262f \u2627 \u262c \u2638 \u2641 \u2646 \u262d \u272f \u262a \u262b \u2721 \u00a9 \u2122 \u00ae \u2615 \u260e \u263b \u2665 \u23f0 凸";

    public static string DEFAULT_WELCOME =
        "Welcome to the FOXhole. Read the Message of the Day for Infos - Questions, ask us! Guides & Tools on our Homepage:\nhttps://gw2fox.wixsite.com/about";

    public static string DEFAULT_GUILD =
        "\u2620 Young or old [FOX], we take every stray. Humor, respect and fun at the game are what distinguish us. No Obligations! Infos: wsp me or\nhttps://gw2fox.wixsite.com/about \u263b";

    public static string DEFAULT_RUN_INFO =
        "«Meta-Train» with the old [FOX]";

    public static string DEFAULT_SQUAD_INFO =
        "\n• InstanceCheck:\n    - right click on me & join map\n• Don’t cancel invites!\n• No 3ple Trouble\n\n• https://gw2fox.wixsite.com/about";
}
