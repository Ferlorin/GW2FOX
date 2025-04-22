using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Globalization;
using System.Windows.Input;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } = new();
        public static List<string> BossList23 { get; private set; } = new();
        public static Dictionary<DateTime, List<string>> DoneBosses { get; private set; } = new();
        public static readonly List<BossEvent> BossEventsList = new();
        internal static readonly List<BossEventGroup> BossEventGroups = new();
        internal static System.Windows.Controls.ListView? BossListView { get; set; }
        public static List<BossEventRun> StaticBossEvents { get; set; } = new();
        public static List<BossEventRun> DynamicBossEvents { get; set; } = new();

        private static readonly char[] Separator = new[] { ',' };

        private static void Init()
        {
            BossEventsList.Clear();
            BossEventGroups.Clear();
        }

        public static void RegisterListView(System.Windows.Controls.ListView listView)
        {
            BossListView = listView;
        }

        public static void SetBossListFromConfig_Bosses()
        {
            try
            {
                var lines = File.ReadAllLines(GlobalVariables.FILE_PATH);

                var bossIndex = -1;
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("Bosses:")) continue;
                    bossIndex = i;
                    break;
                }

                if (bossIndex == -1 || bossIndex >= lines.Length)
                    return;

                var bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();
                var bossNames = bossLine
                    .Trim().Trim('"')
                    .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => name.Trim())
                    .ToArray();

                var newBossList = new List<string>(bossNames);

                for (var i = bossIndex + 1; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("Timings:")) continue;
                    var timingLine = lines[i].Replace("Timings:", "").Trim();
                    var timings = timingLine.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim())
                        .ToArray();

                    if (timings.Length == bossNames.Length)
                    {
                        for (var j = 0; j < bossNames.Length; j++)
                        {
                            AddBossEvent(bossNames[j], new[] { timings[j] }, "WBs");
                        }
                    }

                    break;
                }

                BossList23 = newBossList;

                UpdateBossOverlayList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetBossListFromConfig_Bosses: {ex.Message}");
            }
        }

        private static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "")
        {
            try
            {
                foreach (var timing in timings)
                {
                    var utcTime = ConvertToUtcFromConfigTime(timing);
                    BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBossEvent (multiple timings): {ex.Message}");
            }
        }

        public static DateTime ConvertToUtcFromConfigTime(string configTime)
        {
            var unspecifiedTime = DateTime.SpecifyKind(
                DateTime.ParseExact(configTime, "HH:mm:ss", CultureInfo.InvariantCulture),
                DateTimeKind.Unspecified
            );

            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedTime, GlobalVariables.TIMEZONE_TO_USE);
        }

        public static void UpdateBossOverlayList()
        {
            try
            {
                if (BossListView == null)
                    return;

                var now = GlobalVariables.CURRENT_DATE_TIME;
                var selectedBosses = BossList23 ?? new List<string>();

                var staticBosses = BossEventGroups
                    .Where(group => selectedBosses.Contains(group.BossName))
                    .SelectMany(group => group.GetNextRuns());

                var dynamicBosses = DynamicEventManager.GetActiveBossEventRuns();
                var combinedBosses = staticBosses.Concat(dynamicBosses)
                                                 .OrderBy(run => run.TimeToShow)
                                                 .ToList();

                var items = BossOverlayHelper.GetBossOverlayItems(combinedBosses, now);

                BossListView.Dispatcher.Invoke(() =>
                {
                    var window = OverlayWindow.GetInstance();
                    window.OverlayItems.Clear();

                    foreach (var item in items.OrderBy(i => i.TimeToShow))
                    {
                        item.UpdateCountdown();
                        window.OverlayItems.Add(item);
                    }
                });

                foreach (var boss in combinedBosses.OrderBy(b => b.TimeToShow))
                {
                    Console.WriteLine($"- {boss.BossName} @ {boss.NextRunTime} | Show: {boss.TimeToShow}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }

        public static List<BossEventRun> GetCombinedBossEvents()
        {
            return StaticBossEvents.Concat(DynamicBossEvents).ToList();
        }

        static BossTimings()
        {
            Init();
            SetBossListFromConfig_Bosses();

            AddBossEvent("The frozen Maw", new[] {
    "01:15:00", "03:15:00", "05:15:00", "07:15:00", "09:15:00", "11:15:00",
    "13:15:00", "15:15:00", "17:15:00", "19:15:00", "21:15:00", "23:15:00"
}, "WBs", "[&BMIDAAA=]");

            AddBossEvent("FireShaman", new[] {
    "01:10:00", "03:10:00", "05:10:00", "07:10:00", "09:10:00", "11:10:00",
    "13:10:00", "15:10:00", "17:10:00", "19:10:00", "21:10:00", "23:10:00"
}, "WBs", "[&BO4BAAA=]");

            AddBossEvent("LLA Timberline", new[] {
    "01:20:00", "07:20:00", "13:20:00", "19:20:00"
}, "WBs");

            AddBossEvent("LLA Iron Marches", new[] {
    "03:20:00", "09:20:00", "15:20:00", "21:20:00"
}, "WBs", "[&BOcBAAA=]");

            AddBossEvent("LLA Gendarran", new[] {
    "05:20:00", "11:20:00", "17:20:00", "23:20:00"
}, "WBs");

            AddBossEvent("Fire Elemental", new[] {
    "01:45:00", "03:45:00", "05:45:00", "07:45:00", "09:45:00", "11:45:00",
    "13:45:00", "15:45:00", "17:45:00", "19:45:00", "21:45:00", "23:45:00"
}, "WBs", "[&BEcAAAA=]");

            AddBossEvent("Great Jungle Wurm", new[] {
    "00:15:00", "02:15:00", "04:15:00", "06:15:00", "08:15:00", "10:15:00",
    "12:15:00", "14:15:00", "16:15:00", "18:15:00", "20:15:00", "22:15:00"
}, "WBs", "[&BEEFAAA=]");

            AddBossEvent("Ulgoth the Modniir", new[] {
    "02:30:00", "05:30:00", "08:30:00", "11:30:00", "14:30:00", "17:30:00",
    "20:30:00", "23:30:00"
}, "WBs", "[&BLAAAAA=]");

            AddBossEvent("Taidha Covington", new[] {
    "01:00:00", "04:00:00", "07:00:00", "10:00:00", "13:00:00", "16:00:00",
    "19:00:00", "22:00:00"
}, "WBs", "[&BKgBAAA=]");

            AddBossEvent("The Shatterer", new[] {
    "02:00:00", "05:00:00", "08:00:00", "11:00:00", "14:00:00", "17:00:00",
    "20:00:00", "23:00:00"
}, "WBs", "[&BE4DAAA=]");

            AddBossEvent("Shadow Behemoth", new[] {
    "00:45:00", "02:45:00", "04:45:00", "06:45:00", "08:45:00", "10:45:00",
    "12:45:00", "14:45:00", "16:45:00", "18:45:00", "20:45:00", "22:45:00"
}, "WBs", "[&BPcAAAA=]");

            AddBossEvent("Tequatl the Sunless", new[] {
    "01:00:00", "04:00:00", "08:00:00", "12:30:00", "17:00:00", "20:00:00"
}, "WBs", "[&BNABAAA=]");

            AddBossEvent("Megadestroyer", new[] {
    "01:30:00", "04:30:00", "07:30:00", "10:30:00", "13:30:00", "16:30:00",
    "19:30:00", "22:30:00"
}, "WBs", "[&BM0CAAA=]");

            AddBossEvent("Inquest Golem M2", new[] {
    "00:03:00", "03:03:00", "06:03:00", "09:03:00", "12:03:00", "15:03:00",
    "18:03:00", "21:03:00"
}, "WBs", "[&BNQCAAA=]");

            AddBossEvent("Karka Queen", new[] {
    "00:00:00", "03:00:00", "07:00:00", "11:30:00", "16:00:00", "19:00:00"
}, "WBs", "[&BNUGAAA=]");

            AddBossEvent("Claw of Jormag", new[] {
    "00:30:00", "03:30:00", "06:30:00", "09:30:00", "12:30:00", "15:30:00",
    "18:30:00", "21:30:00"
}, "WBs", "[&BHoCAAA=]");

            //LWS2
            AddBossEvent("Sandstorm", new[] {
    "00:40:00", "01:40:00", "02:40:00", "03:40:00", "04:40:00", "05:40:00",
    "06:40:00", "07:40:00", "08:40:00", "09:40:00", "10:40:00", "11:40:00",
    "12:40:00", "13:40:00", "14:40:00", "15:40:00", "16:40:00", "17:40:00",
    "18:40:00", "19:40:00", "20:40:00", "21:40:00", "22:40:00", "23:40:00"
}, "LWS2", "[&BIAHAAA=]");

            //LWS3
            AddBossEvent("Saidra's Haven", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "LWS3", "[&BK0JAAA=]");

            AddBossEvent("New Loamhurst", new[] {
    "00:45:00", "02:45:00", "04:45:00", "06:45:00", "08:45:00", "10:45:00",
    "12:45:00", "14:45:00", "16:45:00", "18:45:00", "20:45:00", "22:45:00"
}, "LWS3", "[&BLQJAAA=]");

            AddBossEvent("Noran's Homestead", new[] {
    "01:40:00", "03:40:00", "05:40:00", "07:40:00", "09:40:00", "11:40:00",
    "13:40:00", "15:40:00", "17:40:00", "19:40:00", "21:40:00", "23:40:00"
}, "LWS3", "[&BK8JAAA=]");

            //Ice
            AddBossEvent("Defend Jora's Keep", new[] {
    "00:45:00", "02:45:00", "04:45:00", "06:45:00", "08:45:00", "10:45:00",
    "12:45:00", "14:45:00", "16:45:00", "18:45:00", "20:45:00", "22:45:00"
}, "Ice", "[&BCcMAAA=]");

            AddBossEvent("Doomlore Shrine", new[] {
    "01:38:00", "03:38:00", "05:38:00", "07:38:00", "09:38:00", "11:38:00",
    "13:38:00", "15:38:00", "17:38:00", "19:38:00", "21:38:00", "23:38:00"
}, "Ice", "[&BA4MAAA=]");

            AddBossEvent("Storms of Winter", new[] {
    "01:00:00", "03:00:00", "05:00:00", "07:00:00", "09:00:00", "11:00:00",
    "13:00:00", "15:00:00", "17:00:00", "19:00:00", "21:00:00", "23:00:00"
}, "Ice", "[&BCcMAAA=]");

            AddBossEvent("Effigy", new[] {
    "01:10:00", "03:10:00", "05:10:00", "07:10:00", "09:10:00", "11:10:00",
    "13:10:00", "15:10:00", "17:10:00", "19:10:00", "21:10:00", "23:10:00"
}, "Ice", "[&BA4MAAA=]");

            AddBossEvent("Ooze Pits", new[] {
    "00:05:00", "02:05:00", "04:05:00", "06:05:00", "08:05:00", "10:05:00",
    "12:05:00", "14:05:00", "16:05:00", "18:05:00", "20:05:00", "22:05:00"
}, "Ice", "[&BPgLAAA=]");

            AddBossEvent("Dragonstorm", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "Ice", "[&BAkMAAA=]");

            AddBossEvent("Drakkar", new[] {
    "00:05:00", "02:05:00", "04:05:00", "06:05:00", "08:05:00", "10:05:00",
    "12:05:00", "14:05:00", "16:05:00", "18:05:00", "20:05:00", "22:05:00"
}, "Ice", "[&BDkMAAA=]");

            AddBossEvent("Metal Concert", new[] {
    "00:40:00", "02:40:00", "04:40:00", "06:40:00", "08:40:00", "10:40:00",
    "12:40:00", "14:40:00", "16:40:00", "18:40:00", "20:40:00", "22:40:00"
}, "Ice", "[&BPgLAAA=]");

            // Maguuma
            AddBossEvent("Chak Gerent", new[] {
    "01:30:00", "03:30:00", "05:30:00", "07:30:00", "09:30:00", "11:30:00",
    "13:30:00", "15:30:00", "17:30:00", "19:30:00", "21:30:00", "23:30:00"
}, "Maguuma", "[&BPUHAAA=]");

            AddBossEvent("Battle in Tarir", new[] {
    "01:45:00", "03:45:00", "05:45:00", "07:45:00", "09:45:00", "11:45:00",
    "13:45:00", "15:45:00", "17:45:00", "19:45:00", "21:45:00", "23:45:00"
}, "Maguuma", "[&BN0HAAA=][&BGwIAAA=][&BAIIAAA=][&BAYIAAA=]");

            AddBossEvent("Octovine", new[] {
    "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00", "12:00:00",
    "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00", "00:00:00"
}, "Maguuma", "[&BN0HAAA=][&BGwIAAA=][&BAIIAAA=][&BAYIAAA=]");

            AddBossEvent("Nightbosses", new[] {
    "01:10:00", "03:10:00", "05:10:00", "07:10:00", "09:10:00", "11:10:00",
    "13:10:00", "15:10:00", "17:10:00", "19:10:00", "21:10:00", "23:10:00"
}, "Maguuma", "[&BO8HAAA=]");

            AddBossEvent("Dragon's Stand", new[] {
    "00:30:00", "02:30:00", "04:30:00", "06:30:00", "08:30:00", "10:30:00",
    "12:30:00", "14:30:00", "16:30:00", "18:30:00", "20:30:00", "22:30:00"
}, "Maguuma", "[&BBAIAAA=]");

            // Desert
            AddBossEvent("The Oil Floes", new[] {
    "01:45:00", "03:45:00", "05:45:00", "07:45:00", "09:45:00", "11:45:00",
    "13:45:00", "15:45:00", "17:45:00", "19:45:00", "21:45:00", "23:45:00"
}, "Desert", "[&BKYLAAA=]");

            AddBossEvent("Maws of Torment", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "Desert", "[&BKMKAAA=]");

            AddBossEvent("Palawadan", new[] {
    "00:45:00", "02:45:00", "04:45:00", "06:45:00", "08:45:00", "10:45:00",
    "12:45:00", "14:45:00", "16:45:00", "18:45:00", "20:45:00", "22:45:00"
}, "Desert", "[&BAkLAAA=]");

            AddBossEvent("Thunderhead Keep", new[] {
    "00:45:00", "02:45:00", "04:45:00", "06:45:00", "08:45:00", "10:45:00",
    "12:45:00", "14:45:00", "16:45:00", "18:45:00", "20:45:00", "22:45:00"
}, "Desert", "[&BLsLAAA=]");

            AddBossEvent("Serpents' Ire", new[] {
    "01:30:00", "03:30:00", "05:30:00", "07:30:00", "09:30:00", "11:30:00",
    "13:30:00", "15:30:00", "17:30:00", "19:30:00", "21:30:00", "23:30:00"
}, "Desert", "[&BHQKAAA=]");

            AddBossEvent("DB Shatterer", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "Desert", "[&BJMLAAA=]");

            AddBossEvent("Junundu Rising", new[] {
    "00:30:00", "01:30:00", "02:30:00", "03:30:00", "04:30:00", "05:30:00",
    "06:30:00", "07:30:00", "08:30:00", "09:30:00", "10:30:00", "11:30:00",
    "12:30:00", "13:30:00", "14:30:00", "15:30:00", "16:30:00", "17:30:00",
    "18:30:00", "19:30:00", "20:30:00", "21:30:00", "22:30:00", "23:30:00"
}, "Desert", "[&BMEKAAA=]");

            AddBossEvent("Path to Ascension", new[] {
    "00:30:00", "02:30:00", "04:30:00", "06:30:00", "08:30:00", "10:30:00",
    "12:30:00", "14:30:00", "16:30:00", "18:30:00", "20:30:00", "22:30:00"
}, "Desert", "[&BFMKAAA=]");

            AddBossEvent("Doppelganger", new[] {
    "00:55:00", "02:55:00", "04:55:00", "06:55:00", "08:55:00", "10:55:00",
    "12:55:00", "14:55:00", "16:55:00", "18:55:00", "20:55:00", "22:55:00"
}, "Desert", "[&BFMKAAA=]");

            AddBossEvent("Forged with Fire", new[] {
    "01:00:00", "02:00:00", "03:00:00", "04:00:00", "05:00:00", "06:00:00",
    "07:00:00", "08:00:00", "09:00:00", "10:00:00", "11:00:00", "12:00:00",
    "13:00:00", "14:00:00", "15:00:00", "16:00:00", "17:00:00", "18:00:00",
    "19:00:00", "20:00:00", "21:00:00", "22:00:00", "23:00:00", "00:00:00"
}, "Desert", "[&BO0KAAA=]");

            AddBossEvent("Choya Piñata", new[] {
    "01:20:00", "03:20:00", "05:20:00", "07:20:00", "09:20:00", "11:20:00",
    "13:20:00", "15:20:00", "17:20:00", "19:20:00", "21:20:00", "23:20:00"
}, "Desert", "[&BLsKAAA=]");

            // Cantha
            AddBossEvent("Aetherblade Assault", new[] {
    "00:30:00", "02:30:00", "04:30:00", "06:30:00", "08:30:00", "10:30:00",
    "12:30:00", "14:30:00", "16:30:00", "18:30:00", "20:30:00", "22:30:00"
}, "Cantha", "[&BGUNAAA=]");

            AddBossEvent("Kaineng Blackout", new[] {
    "01:00:00", "03:00:00", "05:00:00", "07:00:00", "09:00:00", "11:00:00",
    "13:00:00", "15:00:00", "17:00:00", "19:00:00", "21:00:00", "23:00:00"
}, "Cantha", "[&BBkNAAA=]");

            AddBossEvent("Gang War", new[] {
    "01:30:00", "03:30:00", "05:30:00", "07:30:00", "09:30:00", "11:30:00",
    "13:30:00", "15:30:00", "17:30:00", "19:30:00", "21:30:00", "23:30:00"
}, "Cantha", "[&BMwMAAA=]");

            AddBossEvent("Aspenwood", new[] {
    "00:40:00", "02:40:00", "04:40:00", "06:40:00", "08:40:00", "10:40:00",
    "12:40:00", "14:40:00", "16:40:00", "18:40:00", "20:40:00", "22:40:00"
}, "Cantha", "[&BPkMAAA=]");

            AddBossEvent("Battle for Jade Sea", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "Cantha", "[&BKIMAAA=]");

            // SotO
            AddBossEvent("Wizard's Tower", new[] {
    "00:00:00", "02:00:00", "04:00:00", "06:00:00", "08:00:00", "10:00:00",
    "12:00:00", "14:00:00", "16:00:00", "18:00:00", "20:00:00", "22:00:00"
}, "SotO", "[&BL4NAAA=]");

            AddBossEvent("Fly by Night", new[] {
    "00:55:00", "02:55:00", "04:55:00", "06:55:00", "08:55:00", "10:55:00",
    "12:55:00", "14:55:00", "16:55:00", "18:55:00", "20:55:00", "22:55:00"
}, "SotO", "[&BB8OAAA=]");

            AddBossEvent("Defense of Amnytas", new[] {
    "01:00:00", "03:00:00", "05:00:00", "07:00:00", "09:00:00", "11:00:00",
    "13:00:00", "15:00:00", "17:00:00", "19:00:00", "21:00:00", "23:00:00"
}, "SotO", "[&BDQOAAA=]");

            AddBossEvent("Convergences", new[] {
    "02:30:00", "05:30:00", "08:30:00", "11:30:00", "14:30:00", "17:30:00",
    "20:30:00", "23:30:00"
}, "SotO", "[&BB8OAAA=]");

            BossEventGroups = BossEventsList.GroupBy(bossEvent =>
                               new { bossEvent.BossName, bossEvent.Category },
                           (key, g) =>
                               new BossEventGroup(key.BossName, g.ToList())
                       )
                       .ToList();
        }
    }
}
