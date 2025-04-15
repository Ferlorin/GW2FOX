using TimeSpan = System.TimeSpan;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } =
            new Dictionary<string, List<BossEvent>>();

        public static List<string> BossList23 { get; private set; } = [];
        public static Dictionary<DateTime, List<string>> DoneBosses { get; private set; } = [];
        private static readonly List<BossEvent> Events = [];
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
            AddBossEvent("Fireshaman", "01:15:00", 2, "WBs", "[&BO4BAAA=]");
            AddBossEvent("LLLA Timberline", "01:20:00", 6, "WBs");
            AddBossEvent("LLLA Iron Marches", "03:20:00", 6, "WBs", "[&BOcBAAA=]");
            AddBossEvent("LLLA Gendarran", "05:20:00", 6, "WBs");
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
            AddBossEvent("Spellmaster Macsen", "01:10:00", 2, "Maguuma", "[&BO8HAAA=]");
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

                // Index der Zeile mit dem Bossnamen finden
                var bossIndex = -1;
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("Bosses:")) continue;
                    bossIndex = i; // Die aktuelle Zeile enthält den Namen
                    break;
                }

                // Wenn der Bossname gefunden wird, setze die BossList23
                if (bossIndex == -1 || bossIndex >= lines.Length) return;
                {
                    // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                    var bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    // Entferne die äußeren Anführungszeichen und teile die Bosse
                    var bossNames = bossLine
                        .Trim('"') // Entferne äußere Anführungszeichen
                        .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim()) // Entferne führende und abschließende Leerzeichen
                        .ToArray();

                    // Erstelle eine neue List, um BossList23 zu ersetzen
                    var newBossList = new List<string>();

                    // Füge jeden Bossnamen zur neuen Liste hinzu
                    newBossList.AddRange(bossNames);

                    // Iteriere durch die Zeilen, um Timings zu extrahieren
                    for (var i = bossIndex + 1; i < lines.Length; i++)
                    {
                        if (!lines[i].StartsWith("Timings:")) continue;
                        var timingLine = lines[i].Replace("Timings:", "").Trim();
                        var timings = timingLine.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                            .Select(timing => timing.Trim())
                            .ToArray();

                        // Überprüfe, ob die Anzahl der Timings mit der Anzahl der Bosse übereinstimmt
                        if (timings.Length == bossNames.Length)
                        {
                            for (var j = 0; j < bossNames.Length; j++)
                            {
                                // Füge das BossEvent zur neuen Liste hinzu
                                AddBossEvent(bossNames[j], timings[j], "WBs");
                            }
                        }
                        else
                        {
                            // Handle den Fall, wenn die Anzahl der Timings nicht mit der Anzahl der Bosse übereinstimmt
                        }

                        break; // Da wir die Timings gefunden haben, können wir die Schleife beenden
                    }

                    // Jetzt kannst du die alte BossList23 durch die neue Liste ersetzen
                    BossList23 = newBossList;
                }
            }
            catch (Exception)
            {
                // Hier kann eine Fehlermeldung protokolliert oder geloggt werden, wenn gewünscht
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
            foreach (var timing in timings)
            {
                Events.Add(new BossEvent(bossName, timing, category, waypoint));
            }
        }


        private static void AddBossEvent(string bossName, string timing, string category, string waypoint = "")
        {
            Events.Add(new BossEvent(bossName, timing, category, waypoint));
        }

        public class BossEventGroup
        {
            public string BossName { get; }
            public TimeSpan Duration { get; set; }
            private readonly List<BossEvent> _timings;


            public BossEventGroup(string bossName, IEnumerable<BossEvent> events)
            {
                BossName = bossName;
                _timings = events
                    .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                    .OrderBy(span => span.Timing)
                    .ToList();
                // BossEvent? firstEvent = events.FirstOrDefault();
                // if (firstEvent != null)
                // {
                //     Category = firstEvent.Category;
                // }
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
                //changed the logic to always calculate more than one day
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

               return toReturn
                    .Where(bossEvent => bossEvent.getTimeToShow() >= GlobalVariables.CURRENT_DATE_TIME)
                    .OrderBy(bossEvent => bossEvent.getTimeToShow())
                    .Take(NextRunsToShow)
                    .ToList()
                    ;
                
                
            }

            public IEnumerable<BossEventRun> GetPreviousRuns()
            {
                return _timings
                    .Where(bossEvent =>
                        bossEvent.Timing > GlobalVariables.CURRENT_TIME.Subtract(new TimeSpan(0, 14, 59)) &&
                        bossEvent.Timing < GlobalVariables.CURRENT_TIME)
                    .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category,
                        GlobalVariables.CURRENT_DATE_TIME.Date + bossEvent.Timing
                        , bossEvent.Waypoint))
                    // .Where(bossEventRun => !DoneBosses.ContainsKey(bossEventRun.NextRunTime.Date) || !DoneBosses[bossEventRun.NextRunTime.Date].Contains(bossEventRun.BossName))
                    .ToList();
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


        public class BossEventRun(
            string bossName,
            TimeSpan timing,
            string category,
            DateTime nextRunTime,
            string waypoint = "")
            : BossEvent(bossName, timing, category, waypoint)
        {

            private static readonly Color DefaultFontColor = Color.White;
            private static readonly Color PastBossFontColor = Color.OrangeRed;

            public DateTime NextRunTime { get; set; } = nextRunTime;

            public bool isPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;
            public DateTime NextRunTimeEnding => NextRunTime.AddMinutes(14).AddSeconds(59);

            public DateTime getTimeToShow()
            {
                if (isPreviousBoss)
                {
                    return NextRunTimeEnding;
                }

                return NextRunTime;
            }
            public TimeSpan getTimeRemaining()
            {
                if (!isPreviousBoss)
                {
                    return getTimeToShow() - GlobalVariables.CURRENT_DATE_TIME;
                }
                else
                {
                    return GlobalVariables.CURRENT_DATE_TIME
                        .Add(new TimeSpan(0, 0, 15, 0, 0))
                        // .Subtract(new TimeSpan(0, 0,0, 2, 0))
                        .Subtract(getTimeToShow());
                }
            }
            public string getTimeRemainingFormatted()
            {
                var remainingTime = getTimeRemaining();
                return $"{(int)remainingTime.TotalHours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }

            public Color getForeColor()
            {
                if (isPreviousBoss)
                {
                    return PastBossFontColor; // Setzen Sie die Farbe auf OrangeRed für PreviewBosses
                }
                else
                {
                    // Setzen Sie die Farbe basierend auf der Kategorie des BossEvents
                    switch (Category)
                    {
                        case "Maguuma":
                            return Color.LimeGreen;
                        case "Desert":
                            return Color.DeepPink;
                        case "WBs":
                            return Color.WhiteSmoke;
                        case "Ice":
                            return Color.DeepSkyBlue;
                        case "Cantha":
                            return Color.Blue;
                        case "SotO":
                            return Color.Yellow;
                        case "LWS2":
                            return Color.LightYellow;
                        case "LWS3":
                            return Color.ForestGreen;
                        default:
                            return DefaultFontColor;
                    }
                }
            }
        }
    }


}