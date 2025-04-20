using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GW2FOX
{
    public static class BossTimings
    {
        
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } =
            new Dictionary<string, List<BossEvent>>();

        public static List<string> BossList23 { get; private set; } = [];
        public static Dictionary<DateTime, List<string>> DoneBosses { get; private set; } = [];
        public static readonly List<BossEvent> Events = [];
        internal static readonly List<BossEventGroup> BossEventGroups = [];

        

        // internal static int PREVIOUS_RUNS_TO_SHOW = 1;
        private static readonly char[] Separator = new char[] { ',' };

        private static void Init()
        {
            Events.Clear();
            BossEventGroups.Clear();
        }

        static BossTimings()
        {
            SetBossListFromConfig_Bosses();
            Init();

            AddBossEvent("The frozen Maw", "01:15:00", 2, "WBs", "[&BMIDAAA=]");
            AddBossEvent("FireShaman", "01:10:00", 2, "WBs", "[&BO4BAAA=]");
            AddBossEvent("LLA Timberline", "01:20:00", 6, "WBs");
            AddBossEvent("LLA Iron Marches", "03:20:00", 6, "WBs", "[&BOcBAAA=]");
            AddBossEvent("LLA Gendarran", "05:20:00", 6, "WBs");
            AddBossEvent("Fire Elemental", "01:45:00", 2, "WBs", "[&BEcAAAA=]");
            AddBossEvent("Great Jungle Wurm", "00:15:00", 2, "WBs", "[&BEEFAAA=]");
            AddBossEvent("Ulgoth the Modniir", "02:30:00", 3, "WBs", "[&BLAAAAA=]");
            AddBossEvent("Taidha Covington", "01:00:00", 3, "WBs", "[&BKgBAAA=]");
            AddBossEvent("The Shatterer", "02:00:00", 3, "WBs", "[&BE4DAAA=]");
            AddBossEvent("Shadow Behemoth", "00:45:00", 2, "WBs", "[&BPcAAAA=]");
            AddBossEvent("Tequatl the Sunless", [
                "01:00:00",
                "04:00:00",
                "08:00:00",
                "12:30:00",
                "17:00:00",
                "20:00:00"
            ], "WBs", "[&BNABAAA=]");
            AddBossEvent("Megadestroyer", "01:30:00", 3, "WBs", "[&BM0CAAA=]");
            AddBossEvent("Inquest Golem M2", "00:03:00", 3, "WBs", "[&BNQCAAA=]");
            AddBossEvent("Karka Queen", [
                "00:00:00",
                "03:00:00",
                "07:00:00",
                "11:30:00",
                "16:00:00",
                "19:00:00"
            ], "WBs", "[&BNUGAAA=]");
            AddBossEvent("Claw of Jormag", "00:30:00", 3, "WBs", "[&BHoCAAA=]");


            //LWS2
            AddBossEvent("Sandstorm", "00:40:00", 1, "LWS2", "[&BIAHAAA=]");

            //LWS3
            AddBossEvent("Saidra's Haven", "00:00:00", 2, "LWS3", "[&BK0JAAA=]");
            AddBossEvent("New Loamhurst", "00:45:00", 2, "LWS3", "[&BLQJAAA=]");
            AddBossEvent("Noran's Homestead", "01:40:00", 2, "LWS3", "[&BK8JAAA=]");


            //Ice
            AddBossEvent("Defend Jora's Keep", "00:45:00", 2, "Ice", "[&BCcMAAA=]");
            AddBossEvent("Doomlore Shrine", "01:38:00", 2, "Ice", "[&BA4MAAA=]");
            AddBossEvent("Storms of Winter", "01:00:00", 2, "Ice", "[&BCcMAAA=]");
            AddBossEvent("Effigy", "01:10:00", 2, "Ice", "[&BA4MAAA=]");
            AddBossEvent("Ooze Pits", "00:05:00", 2, "Ice", "[&BPgLAAA=]");
            AddBossEvent("Dragonstorm", "00:00:00", 2, "Ice", "[&BAkMAAA=]");
            AddBossEvent("Drakkar", "00:05:00", 2, "Ice", "[&BDkMAAA=]");
            AddBossEvent("Metal Concert", "00:40:00", 2, "Ice", "[&BPgLAAA=]");

            // Maguuma
            AddBossEvent("Chak Gerent", "01:30:00", 2, "Maguuma", "[&BPUHAAA=]");
            AddBossEvent("Battle in Tarir", "01:45:00", 2, "Maguuma", "[&BN0HAAA=][&BGwIAAA=][&BAIIAAA=][&BAYIAAA=]");
            AddBossEvent("Octovine", "02:00:00", 2, "Maguuma", "[&BN0HAAA=][&BGwIAAA=][&BAIIAAA=][&BAYIAAA=]");
            AddBossEvent("Nightbosses", "01:10:00", 2, "Maguuma", "[&BO8HAAA=]");
            AddBossEvent("Dragon's Stand", "00:30:00", 2, "Maguuma", "[&BBAIAAA=]");

            //Desert
            AddBossEvent("The Oil Floes", "01:45:00", 2, "Desert", "[&BKYLAAA=]");
            AddBossEvent("Maws of Torment", "00:00:00", 2, "Desert", "[&BKMKAAA=]");
            AddBossEvent("Palawadan", "00:45:00", 2, "Desert", "[&BAkLAAA=]");
            AddBossEvent("Thunderhead Keep", "00:45:00", 2, "Desert", "[&BLsLAAA=]");
            AddBossEvent("Serpents' Ire", "01:30:00", 2, "Desert", "[&BHQKAAA=]");
            AddBossEvent("DB Shatterer", "00:00:00", 2, "Desert", "[&BJMLAAA=]");
            AddBossEvent("Junundu Rising", "00:30:00", 1, "Desert", "[&BMEKAAA=]");
            AddBossEvent("Path to Ascension", "00:30:00", 2, "Desert", "[&BFMKAAA=]");
            AddBossEvent("Doppelganger", "00:55:00", 2, "Desert", "[&BFMKAAA=]");
            AddBossEvent("Forged with Fire", "01:00:00", 1, "Desert", "[&BO0KAAA=]");
            AddBossEvent("Choya Piñata", "01:20:00", 2, "Desert", "[&BLsKAAA=]");

            // Cantha
            AddBossEvent("Aetherblade Assault", "00:30:00", 2, "Cantha", "[&BGUNAAA=]");
            AddBossEvent("Kaineng Blackout", "01:00:00", 2, "Cantha", "[&BBkNAAA=]");
            AddBossEvent("Gang War", "01:30:00", 2, "Cantha", "[&BMwMAAA=]");
            AddBossEvent("Aspenwood", "00:40:00", 2, "Cantha", "[&BPkMAAA=]");
            AddBossEvent("Battle for Jade Sea", "00:00:00", 2, "Cantha", "[&BKIMAAA=]");

            //SotO
            AddBossEvent("Wizard's Tower", "00:00:00", 2, "SotO", "[&BL4NAAA=]");
            AddBossEvent("Fly by Night", "00:55:00", 2, "SotO", "[&BB8OAAA=]");
            AddBossEvent("Defense of Amnytas", "01:00:00", 2, "SotO", "[&BDQOAAA=]");
            AddBossEvent("Convergences", "02:30:00", 3, "SotO", "[&BB8OAAA=]");


            BossEventGroups = Events.GroupBy(bossEvent =>
                        new { bossEvent.BossName, bossEvent.Category },
                    (key, g) =>
                        new BossEventGroup(key.BossName, g.ToList())
                )
                .ToList();
        }


        public static void SetBossListFromConfig_Bosses()
        {
            try
            {

                // Vorhandenen Inhalt aus der Datei lesen
                var lines = File.ReadAllLines(GlobalVariables.FILE_PATH);
                Console.WriteLine("File read successfully.");

                // Index der Zeile mit dem Bossnamen finden
                var bossIndex = -1;
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("Bosses:")) continue;
                    bossIndex = i; // Die aktuelle Zeile enthält den Namen
                    break;
                }

                if (bossIndex == -1 || bossIndex >= lines.Length)
                {
                    Console.WriteLine("No 'Bosses:' line found or invalid index.");
                    return;
                }

                Console.WriteLine("Bosses found at line: " + bossIndex);

                // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                var bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();
                var bossNames = bossLine
                    .Trim('"') // Entferne äußere Anführungszeichen
                    .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => name.Trim()) // Entferne führende und abschließende Leerzeichen
                    .ToArray();

               

                var newBossList = new List<string>();
                newBossList.AddRange(bossNames);

                // Iteriere durch die Zeilen, um Timings zu extrahieren
                for (var i = bossIndex + 1; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("Timings:")) continue;
                    var timingLine = lines[i].Replace("Timings:", "").Trim();
                    var timings = timingLine.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(timing => timing.Trim())
                        .ToArray();

                    Console.WriteLine($"Timings found: {string.Join(", ", timings)}");

                    if (timings.Length == bossNames.Length)
                    {
                        for (var j = 0; j < bossNames.Length; j++)
                        {
                            AddBossEvent(bossNames[j], timings[j], "WBs");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Number of timings does not match the number of bosses.");
                    }

                    break; // Da wir die Timings gefunden haben, können wir die Schleife beenden
                }

                // Jetzt kannst du die alte BossList23 durch die neue Liste ersetzen
                BossList23 = newBossList;
                Console.WriteLine("Boss list updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetBossListFromConfig_Bosses: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


        private static void AddBossEvent(string bossName, string firstTiming, int happensEveryInHours, string category,
            string waypoint = "")
        {
            var timeSpan = TimeSpan.Parse(firstTiming);
            while (timeSpan <= TimeSpan.Parse("23:59:59"))
            {
                Events.Add(new BossEvent(bossName, timeSpan, category, waypoint));
                timeSpan = timeSpan.Add(TimeSpan.FromHours(happensEveryInHours));
            }
        }


        private static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "")
        {
            try
            {
                Console.WriteLine($"Adding BossEvents for: {bossName}, Category: {category}, Waypoint: {waypoint}");

                foreach (var timing in timings)
                {
                    Events.Add(new BossEvent(bossName, timing, category, waypoint));
                }

                Console.WriteLine("Boss events added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBossEvent (multiple timings): {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


        private static void AddBossEvent(string bossName, string timing, string category, string waypoint = "")
        {
            try
            {
                Console.WriteLine($"Adding BossEvent: {bossName}, Timing: {timing}, Category: {category}, Waypoint: {waypoint}");

                Events.Add(new BossEvent(bossName, timing, category, waypoint));

                Console.WriteLine("Boss event added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBossEvent (single timing): {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public static void UpdateBossList(System.Windows.Controls.ListView listView)
        {
            Console.WriteLine($"Number of events: {Events.Count}");

            var currentTime = GlobalVariables.CURRENT_DATE_TIME;

            var upcomingBosses = Events
                .SelectMany(bossEvent => new BossEventGroup(bossEvent.BossName, Events).GetNextRuns())
                .OrderBy(boss => boss.TimeToShow)
                .ToList();

            var items = new List<BossListItem>();

            foreach (var boss in upcomingBosses)
            {
                items.Add(new BossListItem
                {
                    BossName = boss.BossName,
                    TimeRemainingFormatted = boss.TimeRemainingFormatted,
                    Waypoint = boss.Waypoint
                });
            }

            listView.ItemsSource = items;
        }
    }
}