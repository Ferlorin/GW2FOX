using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

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

        private const int NextRunsToShow = 2;
        private const int DaysExtraToCalculate = 1;

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
                Debug.WriteLine("Start of SetBossListFromConfig_Bosses");

                // Vorhandenen Inhalt aus der Datei lesen
                var lines = File.ReadAllLines(GlobalVariables.FILE_PATH);
                Debug.WriteLine("File read successfully.");

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
                    Debug.WriteLine("No 'Bosses:' line found or invalid index.");
                    return;
                }

                Debug.WriteLine("Bosses found at line: " + bossIndex);

                // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                var bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();
                var bossNames = bossLine
                    .Trim('"') // Entferne äußere Anführungszeichen
                    .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => name.Trim()) // Entferne führende und abschließende Leerzeichen
                    .ToArray();

                Debug.WriteLine($"Boss names extracted: {string.Join(", ", bossNames)}");

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

                    Debug.WriteLine($"Timings found: {string.Join(", ", timings)}");

                    if (timings.Length == bossNames.Length)
                    {
                        for (var j = 0; j < bossNames.Length; j++)
                        {
                            AddBossEvent(bossNames[j], timings[j], "WBs");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Number of timings does not match the number of bosses.");
                    }

                    break; // Da wir die Timings gefunden haben, können wir die Schleife beenden
                }

                // Jetzt kannst du die alte BossList23 durch die neue Liste ersetzen
                BossList23 = newBossList;
                Debug.WriteLine("Boss list updated successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SetBossListFromConfig_Bosses: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
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
                Debug.WriteLine($"Adding BossEvents for: {bossName}, Category: {category}, Waypoint: {waypoint}");

                foreach (var timing in timings)
                {
                    Events.Add(new BossEvent(bossName, timing, category, waypoint));
                }

                Debug.WriteLine("Boss events added successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddBossEvent (multiple timings): {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


        private static void AddBossEvent(string bossName, string timing, string category, string waypoint = "")
        {
            try
            {
                Debug.WriteLine($"Adding BossEvent: {bossName}, Timing: {timing}, Category: {category}, Waypoint: {waypoint}");

                Events.Add(new BossEvent(bossName, timing, category, waypoint));

                Debug.WriteLine("Boss event added successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddBossEvent (single timing): {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public class BossEventGroup
        {
            public string BossName { get; }
            public TimeSpan Duration { get; set; }
            private readonly List<BossEvent> _timings;

            public BossEventGroup(string bossName, IEnumerable<BossEvent> events)
            {
                try
                {
                    Debug.WriteLine($"Creating BossEventGroup for: {bossName}");

                    BossName = bossName;
                    _timings = events
                        .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                        .OrderBy(span => span.Timing)
                        .ToList();

                    Debug.WriteLine($"BossEventGroup created for {BossName} with {_timings.Count} events.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in BossEventGroup constructor: {ex.Message}");
                    Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }


            /// <summary>
            /// Calculates Next Runs based on DaysExtraToCalculate and NextRunsToShow.
            /// </summary>
            /// <remarks>
            /// <b><code>DaysExtraToCalculate</code></b> is how many more days it should calculate after the current date (to calculate also tomorrow insert 1
            /// <b><code>NextRunsToShow</code></b> is how many more runs of the same Category/Boss it will show. I have inserted the default 2 but it could be more
            /// </remarks>
            public IEnumerable<BossEventRun> GetNextRuns()
            {
                try
                {
                    Debug.WriteLine($"Getting next runs for {BossName}");

                    List<BossEventRun> toReturn = new List<BossEventRun>();

                    for (var i = -1; i <= DaysExtraToCalculate; i++)
                    {
                        toReturn.AddRange(
                            _timings
                                .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category,
                                    GlobalVariables
                                        .CURRENT_DATE_TIME
                                        .Date
                                        .Add(new TimeSpan(0, i * 24, 0, 0, 0))
                                    + bossEvent.Timing,
                                    bossEvent.Waypoint))
                                .ToList()
                        );
                    }

                    var result = toReturn
                        .Where(bossEvent => bossEvent.TimeToShow >= GlobalVariables.CURRENT_DATE_TIME)
                        .OrderBy(bossEvent => bossEvent.TimeToShow)
                        .Take(NextRunsToShow)
                        .ToList();

                    Debug.WriteLine($"{result.Count} next runs found for {BossName}");

                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in GetNextRuns for {BossName}: {ex.Message}");
                    Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Enumerable.Empty<BossEventRun>(); // Falls ein Fehler auftritt, gibt eine leere Liste zurück
                }
            }

            public IEnumerable<BossEventRun> GetPreviousRuns()
            {
                try
                {
                    Debug.WriteLine($"Getting previous runs for {BossName}");

                    var result = _timings
                        .Where(bossEvent =>
                            bossEvent.Timing > GlobalVariables.CURRENT_TIME.Subtract(new TimeSpan(0, 14, 59)) &&
                            bossEvent.Timing < GlobalVariables.CURRENT_TIME)
                        .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category,
                            GlobalVariables.CURRENT_DATE_TIME.Date + bossEvent.Timing, bossEvent.Waypoint))
                        .ToList();

                    Debug.WriteLine($"{result.Count} previous runs found for {BossName}");

                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in GetPreviousRuns for {BossName}: {ex.Message}");
                    Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Enumerable.Empty<BossEventRun>(); // Falls ein Fehler auftritt, gibt eine leere Liste zurück
                }
            }
        }

        public class BossEvent(string bossName, TimeSpan timing, string category, string waypoint = "")
        {
            public string BossName { get; } = bossName;
            public string Waypoint { get; } = waypoint;
            public TimeSpan Timing { get; } = GlobalVariables.IsDaylightSavingTimeActive() ? timing.Add(new TimeSpan(1, 0, 0)) : timing;
            public string Category { get; } = category;


            public BossEvent(string bossName, string timing, string category, string waypoint = "") : this(bossName,
                TimeSpan.Parse(timing),
                category, waypoint)
            {
            }
        }

        private class BossEventComparer : IEqualityComparer<BossEvent>
        {
            public bool Equals(BossEvent? x, BossEvent? y)
            {
                if (x == null && y == null)
                    return true;
                else if (x == null || y == null)
                    return false;
                else
                    return x.BossName == y.BossName && x.Timing == y.Timing;
            }

            public int GetHashCode(BossEvent? obj)
            {
                if (obj == null)
                    return 0;

                var hashBossName = obj.BossName.GetHashCode();
                var hashTiming = obj.Timing.GetHashCode();

                return hashBossName ^ hashTiming;
            }
        }
        public class BossEventRun : BossEvent
        {
            private static readonly Color DefaultFontColor = Color.White;
            private static readonly Color PastBossFontColor = Color.OrangeRed;

            public DateTime NextRunTime { get; set; }

            public bool IsPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;

            public BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime, string waypoint = "")
                : base(bossName, timing, category, waypoint)
            {
                NextRunTime = nextRunTime;
            }

            public DateTime TimeToShow =>
                IsPreviousBoss ? NextRunTimeEnding : NextRunTime;

            public DateTime NextRunTimeEnding => NextRunTime.AddMinutes(14).AddSeconds(59);

            public TimeSpan TimeRemaining =>
                !IsPreviousBoss
                    ? TimeToShow - GlobalVariables.CURRENT_DATE_TIME
                    : GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow;

            public string TimeRemainingFormatted =>
                $"{(int)TimeRemaining.TotalHours:D2}:{TimeRemaining.Minutes:D2}:{TimeRemaining.Seconds:D2}";

            public Color ForeColor =>
                IsPreviousBoss ? PastBossFontColor : Category switch
                {
                    "Maguuma" => Color.LimeGreen,
                    "Desert" => Color.DeepPink,
                    "WBs" => Color.WhiteSmoke,
                    "Ice" => Color.DeepSkyBlue,
                    "Cantha" => Color.Blue,
                    "SotO" => Color.Yellow,
                    "LWS2" => Color.LightYellow,
                    "LWS3" => Color.ForestGreen,
                    _ => DefaultFontColor
                };
        }
    }
}