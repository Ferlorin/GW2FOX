namespace GW2FOX
{
    
    public static class BossTimings
    {
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } = new Dictionary<string, List<BossEvent>>();
        public static List<string> BossList23 { get; set; } = new List<string>();
        internal static List<BossEvent> Events = new List<BossEvent>();
        internal static List<BossEventGroup> BossEventGroups = new List<BossEventGroup>();

        internal static int NEXT_RUNS_TO_SHOW = 2;
        internal static int PREVIOUS_RUNS_TO_SHOW = 1;

        static BossTimings()
        {
            SetBossListFromConfig_Bosses();


            foreach (var bossName in BossList23)
            {


                AddBossEvent("The frozen Maw", "02:15:00", "WBs");
                AddBossEvent("The frozen Maw", "04:15:00", "WBs");
                AddBossEvent("The frozen Maw", "06:15:00", "WBs");
                AddBossEvent("The frozen Maw", "08:15:00", "WBs");
                AddBossEvent("The frozen Maw", "10:15:00", "WBs");
                AddBossEvent("The frozen Maw", "12:15:00", "WBs");
                AddBossEvent("The frozen Maw", "14:15:00", "WBs");
                AddBossEvent("The frozen Maw", "16:15:00", "WBs");
                AddBossEvent("The frozen Maw", "18:15:00", "WBs");
                AddBossEvent("The frozen Maw", "20:15:00", "WBs");
                AddBossEvent("The frozen Maw", "22:15:00", "WBs");
                AddBossEvent("The frozen Maw", "00:15:00", "WBs");

                AddBossEvent("LLLA - Timberline Falls", "02:20:00", "WBs");
                AddBossEvent("LLLA - Iron Marches", "04:20:00", "WBs");
                AddBossEvent("LLLA - Gendarran Fields", "06:20:00", "WBs");
                AddBossEvent("LLLA - Timberline Falls", "08:20:00", "WBs");
                AddBossEvent("LLLA - Iron Marches", "10:20:00", "WBs");
                AddBossEvent("LLLA - Gendarran Fields", "12:20:00", "WBs");
                AddBossEvent("LLLA - Timberline Falls", "14:20:00", "WBs");
                AddBossEvent("LLLA - Iron Marches", "16:20:00", "WBs");
                AddBossEvent("LLLA - Gendarran Fields", "18:20:00", "WBs");
                AddBossEvent("LLLA - Timberline Falls", "20:20:00", "WBs");
                AddBossEvent("LLLA - Iron Marches", "22:20:00", "WBs");
                AddBossEvent("LLLA - Gendarran Fields", "00:20:00", "WBs");

                AddBossEvent("Fire Elemental", "02:45:00", "WBs");
                AddBossEvent("Fire Elemental", "04:45:00", "WBs");
                AddBossEvent("Fire Elemental", "06:45:00", "WBs");
                AddBossEvent("Fire Elemental", "08:45:00", "WBs");
                AddBossEvent("Fire Elemental", "10:45:00", "WBs");
                AddBossEvent("Fire Elemental", "12:45:00", "WBs");
                AddBossEvent("Fire Elemental", "14:45:00", "WBs");
                AddBossEvent("Fire Elemental", "16:45:00", "WBs");
                AddBossEvent("Fire Elemental", "18:45:00", "WBs");
                AddBossEvent("Fire Elemental", "20:45:00", "WBs");
                AddBossEvent("Fire Elemental", "22:45:00", "WBs");
                AddBossEvent("Fire Elemental", "00:45:00", "WBs");

                AddBossEvent("Great Jungle Wurm", "01:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "03:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "05:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "07:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "09:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "11:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "13:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "15:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "17:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "19:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "21:15:00", "WBs");
                AddBossEvent("Great Jungle Wurm", "23:15:00", "WBs");

                AddBossEvent("Ulgoth the Modniir", "03:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "06:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "09:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "12:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "15:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "18:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "21:30:00", "WBs");
                AddBossEvent("Ulgoth the Modniir", "00:30:00", "WBs");

                AddBossEvent("Taidha Covington", "02:00:00", "WBs");
                AddBossEvent("Taidha Covington", "05:00:00", "WBs");
                AddBossEvent("Taidha Covington", "08:00:00", "WBs");
                AddBossEvent("Taidha Covington", "11:00:00", "WBs");
                AddBossEvent("Taidha Covington", "14:00:00", "WBs");
                AddBossEvent("Taidha Covington", "17:00:00", "WBs");
                AddBossEvent("Taidha Covington", "20:00:00", "WBs");
                AddBossEvent("Taidha Covington", "23:00:00", "WBs");

                AddBossEvent("The Shatterer", "03:00:00", "WBs");
                AddBossEvent("The Shatterer", "06:00:00", "WBs");
                AddBossEvent("The Shatterer", "09:00:00", "WBs");
                AddBossEvent("The Shatterer", "12:00:00", "WBs");
                AddBossEvent("The Shatterer", "15:00:00", "WBs");
                AddBossEvent("The Shatterer", "18:00:00", "WBs");
                AddBossEvent("The Shatterer", "21:00:00", "WBs");
                AddBossEvent("The Shatterer", "00:00:00", "WBs");

                AddBossEvent("Shadow Behemoth", "03:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "05:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "07:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "09:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "11:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "13:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "15:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "17:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "19:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "21:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "23:45:00", "WBs");
                AddBossEvent("Shadow Behemoth", "01:45:00", "WBs");

                AddBossEvent("Tequatl the Sunless", "02:00:00", "WBs");
                AddBossEvent("Tequatl the Sunless", "05:00:00", "WBs");
                AddBossEvent("Tequatl the Sunless", "09:00:00", "WBs");
                AddBossEvent("Tequatl the Sunless", "13:30:00", "WBs");
                AddBossEvent("Tequatl the Sunless", "18:00:00", "WBs");
                AddBossEvent("Tequatl the Sunless", "21:00:00", "WBs");

                AddBossEvent("Megadestroyer", "02:30:00", "WBs");
                AddBossEvent("Megadestroyer", "05:30:00", "WBs");
                AddBossEvent("Megadestroyer", "08:30:00", "WBs");
                AddBossEvent("Megadestroyer", "11:30:00", "WBs");
                AddBossEvent("Megadestroyer", "14:30:00", "WBs");
                AddBossEvent("Megadestroyer", "17:30:00", "WBs");
                AddBossEvent("Megadestroyer", "20:30:00", "WBs");
                AddBossEvent("Megadestroyer", "23:30:00", "WBs");

                AddBossEvent("Inquest Golem Mark II", "01:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "04:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "07:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "10:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "13:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "16:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "19:03:00", "WBs");
                AddBossEvent("Inquest Golem Mark II", "22:03:00", "WBs");

                AddBossEvent("Karka Queen", "01:00:00", "WBs");
                AddBossEvent("Karka Queen", "04:00:00", "WBs");
                AddBossEvent("Karka Queen", "08:00:00", "WBs");
                AddBossEvent("Karka Queen", "12:30:00", "WBs");
                AddBossEvent("Karka Queen", "17:00:00", "WBs");
                AddBossEvent("Karka Queen", "20:00:00", "WBs");

                AddBossEvent("Claw of Jormag", "04:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "07:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "10:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "13:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "16:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "19:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "22:30:00", "WBs");
                AddBossEvent("Claw of Jormag", "01:30:00", "WBs");


                //LWS2
                AddBossEvent("Sandstorm", "01:40:00", "LWS2");
                AddBossEvent("Sandstorm", "02:40:00", "LWS2");
                AddBossEvent("Sandstorm", "03:40:00", "LWS2");
                AddBossEvent("Sandstorm", "04:40:00", "LWS2");
                AddBossEvent("Sandstorm", "05:40:00", "LWS2");
                AddBossEvent("Sandstorm", "06:40:00", "LWS2");
                AddBossEvent("Sandstorm", "07:40:00", "LWS2");
                AddBossEvent("Sandstorm", "08:40:00", "LWS2");
                AddBossEvent("Sandstorm", "09:40:00", "LWS2");
                AddBossEvent("Sandstorm", "10:40:00", "LWS2");
                AddBossEvent("Sandstorm", "11:40:00", "LWS2");
                AddBossEvent("Sandstorm", "12:40:00", "LWS2");
                AddBossEvent("Sandstorm", "13:40:00", "LWS2");
                AddBossEvent("Sandstorm", "14:40:00", "LWS2");
                AddBossEvent("Sandstorm", "15:40:00", "LWS2");
                AddBossEvent("Sandstorm", "16:40:00", "LWS2");
                AddBossEvent("Sandstorm", "17:40:00", "LWS2");
                AddBossEvent("Sandstorm", "18:40:00", "LWS2");
                AddBossEvent("Sandstorm", "19:40:00", "LWS2");
                AddBossEvent("Sandstorm", "20:40:00", "LWS2");
                AddBossEvent("Sandstorm", "21:40:00", "LWS2");
                AddBossEvent("Sandstorm", "22:40:00", "LWS2");
                AddBossEvent("Sandstorm", "23:40:00", "LWS2");
                AddBossEvent("Sandstorm", "00:40:00", "LWS2");

                //LWS3
                AddBossEvent("Saidra's Haven", "01:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "03:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "05:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "07:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "09:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "11:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "13:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "15:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "17:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "19:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "21:00:00", "LWS3");
                AddBossEvent("Saidra's Haven", "23:00:00", "LWS3");

                AddBossEvent("New Loamhurst", "01:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "03:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "05:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "07:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "09:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "11:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "13:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "15:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "17:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "19:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "21:45:00", "LWS3");
                AddBossEvent("New Loamhurst", "23:45:00", "LWS3");

                AddBossEvent("Noran's Homestead", "02:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "04:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "06:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "08:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "10:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "12:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "14:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "16:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "18:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "20:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "22:40:00", "LWS3");
                AddBossEvent("Noran's Homestead", "00:40:00", "LWS3");
                // Ice
                AddBossEvent("Defend Jora's Keep", "03:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "05:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "07:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "09:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "11:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "13:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "15:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "17:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "19:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "21:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "23:45:00", "Ice");
                AddBossEvent("Defend Jora's Keep", "01:45:00", "Ice");

                AddBossEvent("Doomlore Shrine", "02:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "04:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "06:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "08:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "10:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "12:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "14:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "16:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "18:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "20:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "22:38:00", "Ice");
                AddBossEvent("Doomlore Shrine", "00:38:00", "Ice");

                AddBossEvent("Storms of Winter", "02:00:00", "Ice");
                AddBossEvent("Storms of Winter", "04:00:00", "Ice");
                AddBossEvent("Storms of Winter", "06:00:00", "Ice");
                AddBossEvent("Storms of Winter", "08:00:00", "Ice");
                AddBossEvent("Storms of Winter", "10:00:00", "Ice");
                AddBossEvent("Storms of Winter", "12:00:00", "Ice");
                AddBossEvent("Storms of Winter", "14:00:00", "Ice");
                AddBossEvent("Storms of Winter", "16:00:00", "Ice");
                AddBossEvent("Storms of Winter", "18:00:00", "Ice");
                AddBossEvent("Storms of Winter", "20:00:00", "Ice");
                AddBossEvent("Storms of Winter", "22:00:00", "Ice");
                AddBossEvent("Storms of Winter", "00:00:00", "Ice");

                AddBossEvent("Effigy", "02:10:00", "Ice");
                AddBossEvent("Effigy", "04:10:00", "Ice");
                AddBossEvent("Effigy", "06:10:00", "Ice");
                AddBossEvent("Effigy", "08:10:00", "Ice");
                AddBossEvent("Effigy", "10:10:00", "Ice");
                AddBossEvent("Effigy", "12:10:00", "Ice");
                AddBossEvent("Effigy", "14:10:00", "Ice");
                AddBossEvent("Effigy", "16:10:00", "Ice");
                AddBossEvent("Effigy", "18:10:00", "Ice");
                AddBossEvent("Effigy", "20:10:00", "Ice");
                AddBossEvent("Effigy", "22:10:00", "Ice");
                AddBossEvent("Effigy", "00:10:00", "Ice");

                AddBossEvent("Ooze Pits", "01:05:00", "Ice");
                AddBossEvent("Ooze Pits", "03:05:00", "Ice");
                AddBossEvent("Ooze Pits", "05:05:00", "Ice");
                AddBossEvent("Ooze Pits", "07:05:00", "Ice");
                AddBossEvent("Ooze Pits", "09:05:00", "Ice");
                AddBossEvent("Ooze Pits", "11:05:00", "Ice");
                AddBossEvent("Ooze Pits", "13:05:00", "Ice");
                AddBossEvent("Ooze Pits", "15:05:00", "Ice");
                AddBossEvent("Ooze Pits", "17:05:00", "Ice");
                AddBossEvent("Ooze Pits", "19:05:00", "Ice");
                AddBossEvent("Ooze Pits", "21:05:00", "Ice");
                AddBossEvent("Ooze Pits", "23:05:00", "Ice");

                AddBossEvent("Dragonstorm", "01:00:00", "Ice");
                AddBossEvent("Dragonstorm", "03:00:00", "Ice");
                AddBossEvent("Dragonstorm", "05:00:00", "Ice");
                AddBossEvent("Dragonstorm", "07:00:00", "Ice");
                AddBossEvent("Dragonstorm", "09:00:00", "Ice");
                AddBossEvent("Dragonstorm", "11:00:00", "Ice");
                AddBossEvent("Dragonstorm", "13:00:00", "Ice");
                AddBossEvent("Dragonstorm", "15:00:00", "Ice");
                AddBossEvent("Dragonstorm", "17:00:00", "Ice");
                AddBossEvent("Dragonstorm", "19:00:00", "Ice");
                AddBossEvent("Dragonstorm", "21:00:00", "Ice");
                AddBossEvent("Dragonstorm", "23:00:00", "Ice");

                AddBossEvent("Drakkar", "01:05:00", "Ice");
                AddBossEvent("Drakkar", "03:05:00", "Ice");
                AddBossEvent("Drakkar", "05:05:00", "Ice");
                AddBossEvent("Drakkar", "07:05:00", "Ice");
                AddBossEvent("Drakkar", "09:05:00", "Ice");
                AddBossEvent("Drakkar", "11:05:00", "Ice");
                AddBossEvent("Drakkar", "13:05:00", "Ice");
                AddBossEvent("Drakkar", "15:05:00", "Ice");
                AddBossEvent("Drakkar", "17:05:00", "Ice");
                AddBossEvent("Drakkar", "19:05:00", "Ice");
                AddBossEvent("Drakkar", "21:05:00", "Ice");
                AddBossEvent("Drakkar", "23:05:00", "Ice");

                AddBossEvent("Metal Concert", "01:40:00", "Ice");
                AddBossEvent("Metal Concert", "03:40:00", "Ice");
                AddBossEvent("Metal Concert", "05:40:00", "Ice");
                AddBossEvent("Metal Concert", "07:40:00", "Ice");
                AddBossEvent("Metal Concert", "09:40:00", "Ice");
                AddBossEvent("Metal Concert", "11:40:00", "Ice");
                AddBossEvent("Metal Concert", "13:40:00", "Ice");
                AddBossEvent("Metal Concert", "15:40:00", "Ice");
                AddBossEvent("Metal Concert", "17:40:00", "Ice");
                AddBossEvent("Metal Concert", "19:40:00", "Ice");
                AddBossEvent("Metal Concert", "21:40:00", "Ice");
                AddBossEvent("Metal Concert", "23:40:00", "Ice");


                // Maguuma
                AddBossEvent("Chak Gerent", "02:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "04:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "06:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "08:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "10:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "12:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "14:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "16:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "18:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "20:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "22:30:00", "Maguuma");
                AddBossEvent("Chak Gerent", "00:30:00", "Maguuma");

                AddBossEvent("Battle in Tarir", "02:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "04:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "06:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "08:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "10:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "12:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "14:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "16:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "18:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "20:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "22:45:00", "Maguuma");
                AddBossEvent("Battle in Tarir", "00:45:00", "Maguuma");

                AddBossEvent("Spellmaster Macsen", "02:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "04:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "06:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "08:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "10:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "12:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "14:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "16:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "18:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "20:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "22:10:00", "Maguuma");
                AddBossEvent("Spellmaster Macsen", "00:10:00", "Maguuma");

                AddBossEvent("Dragon's Stand", "01:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "03:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "05:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "07:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "09:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "11:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "13:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "15:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "17:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "19:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "21:30:00", "Maguuma");
                AddBossEvent("Dragon's Stand", "23:30:00", "Maguuma");


                //Desert
                AddBossEvent("The Oil Floes", "02:45:00", "Desert");
                AddBossEvent("The Oil Floes", "04:45:00", "Desert");
                AddBossEvent("The Oil Floes", "06:45:00", "Desert");
                AddBossEvent("The Oil Floes", "08:45:00", "Desert");
                AddBossEvent("The Oil Floes", "10:45:00", "Desert");
                AddBossEvent("The Oil Floes", "12:45:00", "Desert");
                AddBossEvent("The Oil Floes", "14:45:00", "Desert");
                AddBossEvent("The Oil Floes", "16:45:00", "Desert");
                AddBossEvent("The Oil Floes", "18:45:00", "Desert");
                AddBossEvent("The Oil Floes", "20:45:00", "Desert");
                AddBossEvent("The Oil Floes", "22:45:00", "Desert");
                AddBossEvent("The Oil Floes", "00:45:00", "Desert");

                AddBossEvent("Maws of Torment", "01:00:00", "Desert");
                AddBossEvent("Maws of Torment", "03:00:00", "Desert");
                AddBossEvent("Maws of Torment", "05:00:00", "Desert");
                AddBossEvent("Maws of Torment", "07:00:00", "Desert");
                AddBossEvent("Maws of Torment", "09:00:00", "Desert");
                AddBossEvent("Maws of Torment", "11:00:00", "Desert");
                AddBossEvent("Maws of Torment", "13:00:00", "Desert");
                AddBossEvent("Maws of Torment", "15:00:00", "Desert");
                AddBossEvent("Maws of Torment", "17:00:00", "Desert");
                AddBossEvent("Maws of Torment", "19:00:00", "Desert");
                AddBossEvent("Maws of Torment", "21:00:00", "Desert");
                AddBossEvent("Maws of Torment", "23:00:00", "Desert");

                AddBossEvent("Palawadan", "01:45:00", "Desert");
                AddBossEvent("Palawadan", "03:45:00", "Desert");
                AddBossEvent("Palawadan", "05:45:00", "Desert");
                AddBossEvent("Palawadan", "07:45:00", "Desert");
                AddBossEvent("Palawadan", "09:45:00", "Desert");
                AddBossEvent("Palawadan", "11:45:00", "Desert");
                AddBossEvent("Palawadan", "13:45:00", "Desert");
                AddBossEvent("Palawadan", "15:45:00", "Desert");
                AddBossEvent("Palawadan", "17:45:00", "Desert");
                AddBossEvent("Palawadan", "19:45:00", "Desert");
                AddBossEvent("Palawadan", "21:45:00", "Desert");
                AddBossEvent("Palawadan", "23:45:00", "Desert");

                AddBossEvent("Thunderhead Keep", "01:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "03:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "05:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "07:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "09:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "11:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "13:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "15:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "17:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "19:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "21:45:00", "Desert");
                AddBossEvent("Thunderhead Keep", "23:45:00", "Desert");

                AddBossEvent("Serpents' Ire", "02:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "04:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "06:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "08:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "10:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "12:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "14:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "16:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "18:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "20:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "22:30:00", "Desert");
                AddBossEvent("Serpents' Ire", "00:30:00", "Desert");

                AddBossEvent("Death-Branded Shatterer", "03:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "05:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "07:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "09:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "11:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "13:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "15:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "17:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "19:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "21:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "23:00:00", "Desert");
                AddBossEvent("Death-Branded Shatterer", "01:00:00", "Desert");

                AddBossEvent("Junundu Rising", "01:30:00", "Desert");
                AddBossEvent("Junundu Rising", "02:30:00", "Desert");
                AddBossEvent("Junundu Rising", "03:30:00", "Desert");
                AddBossEvent("Junundu Rising", "04:30:00", "Desert");
                AddBossEvent("Junundu Rising", "05:30:00", "Desert");
                AddBossEvent("Junundu Rising", "06:30:00", "Desert");
                AddBossEvent("Junundu Rising", "07:30:00", "Desert");
                AddBossEvent("Junundu Rising", "08:30:00", "Desert");
                AddBossEvent("Junundu Rising", "09:30:00", "Desert");
                AddBossEvent("Junundu Rising", "10:30:00", "Desert");
                AddBossEvent("Junundu Rising", "11:30:00", "Desert");
                AddBossEvent("Junundu Rising", "12:30:00", "Desert");
                AddBossEvent("Junundu Rising", "13:30:00", "Desert");
                AddBossEvent("Junundu Rising", "14:30:00", "Desert");
                AddBossEvent("Junundu Rising", "15:30:00", "Desert");
                AddBossEvent("Junundu Rising", "16:30:00", "Desert");
                AddBossEvent("Junundu Rising", "17:30:00", "Desert");
                AddBossEvent("Junundu Rising", "18:30:00", "Desert");
                AddBossEvent("Junundu Rising", "19:30:00", "Desert");
                AddBossEvent("Junundu Rising", "20:30:00", "Desert");
                AddBossEvent("Junundu Rising", "21:30:00", "Desert");
                AddBossEvent("Junundu Rising", "22:30:00", "Desert");
                AddBossEvent("Junundu Rising", "23:30:00", "Desert");
                AddBossEvent("Junundu Rising", "00:30:00", "Desert");

                AddBossEvent("Path to Ascension", "01:30:00", "Desert");
                AddBossEvent("Path to Ascension", "03:30:00", "Desert");
                AddBossEvent("Path to Ascension", "05:30:00", "Desert");
                AddBossEvent("Path to Ascension", "07:30:00", "Desert");
                AddBossEvent("Path to Ascension", "09:30:00", "Desert");
                AddBossEvent("Path to Ascension", "11:30:00", "Desert");
                AddBossEvent("Path to Ascension", "13:30:00", "Desert");
                AddBossEvent("Path to Ascension", "15:30:00", "Desert");
                AddBossEvent("Path to Ascension", "17:30:00", "Desert");
                AddBossEvent("Path to Ascension", "19:30:00", "Desert");
                AddBossEvent("Path to Ascension", "21:30:00", "Desert");
                AddBossEvent("Path to Ascension", "23:30:00", "Desert");

                AddBossEvent("Doppelganger", "01:55:00", "Desert");
                AddBossEvent("Doppelganger", "03:55:00", "Desert");
                AddBossEvent("Doppelganger", "05:55:00", "Desert");
                AddBossEvent("Doppelganger", "07:55:00", "Desert");
                AddBossEvent("Doppelganger", "09:55:00", "Desert");
                AddBossEvent("Doppelganger", "11:55:00", "Desert");
                AddBossEvent("Doppelganger", "13:55:00", "Desert");
                AddBossEvent("Doppelganger", "15:55:00", "Desert");
                AddBossEvent("Doppelganger", "17:55:00", "Desert");
                AddBossEvent("Doppelganger", "19:55:00", "Desert");
                AddBossEvent("Doppelganger", "21:55:00", "Desert");
                AddBossEvent("Doppelganger", "23:55:00", "Desert");

                AddBossEvent("Forged with Fire", "01:00:00", "Desert");
                AddBossEvent("Forged with Fire", "02:00:00", "Desert");
                AddBossEvent("Forged with Fire", "03:00:00", "Desert");
                AddBossEvent("Forged with Fire", "04:00:00", "Desert");
                AddBossEvent("Forged with Fire", "05:00:00", "Desert");
                AddBossEvent("Forged with Fire", "06:00:00", "Desert");
                AddBossEvent("Forged with Fire", "07:00:00", "Desert");
                AddBossEvent("Forged with Fire", "08:00:00", "Desert");
                AddBossEvent("Forged with Fire", "09:00:00", "Desert");
                AddBossEvent("Forged with Fire", "10:00:00", "Desert");
                AddBossEvent("Forged with Fire", "11:00:00", "Desert");
                AddBossEvent("Forged with Fire", "12:00:00", "Desert");
                AddBossEvent("Forged with Fire", "13:00:00", "Desert");
                AddBossEvent("Forged with Fire", "14:00:00", "Desert");
                AddBossEvent("Forged with Fire", "15:00:00", "Desert");
                AddBossEvent("Forged with Fire", "16:00:00", "Desert");
                AddBossEvent("Forged with Fire", "17:00:00", "Desert");
                AddBossEvent("Forged with Fire", "18:00:00", "Desert");
                AddBossEvent("Forged with Fire", "19:00:00", "Desert");
                AddBossEvent("Forged with Fire", "20:00:00", "Desert");
                AddBossEvent("Forged with Fire", "21:00:00", "Desert");
                AddBossEvent("Forged with Fire", "22:00:00", "Desert");
                AddBossEvent("Forged with Fire", "23:00:00", "Desert");
                AddBossEvent("Forged with Fire", "00:00:01", "Desert");
                

                AddBossEvent("Choya Piñata", "02:20:00", "Desert");
                AddBossEvent("Choya Piñata", "04:20:00", "Desert");
                AddBossEvent("Choya Piñata", "06:20:00", "Desert");
                AddBossEvent("Choya Piñata", "08:20:00", "Desert");
                AddBossEvent("Choya Piñata", "10:20:00", "Desert");
                AddBossEvent("Choya Piñata", "12:20:00", "Desert");
                AddBossEvent("Choya Piñata", "14:20:00", "Desert");
                AddBossEvent("Choya Piñata", "16:20:00", "Desert");
                AddBossEvent("Choya Piñata", "18:20:00", "Desert");
                AddBossEvent("Choya Piñata", "20:20:00", "Desert");
                AddBossEvent("Choya Piñata", "22:20:00", "Desert");
                AddBossEvent("Choya Piñata", "00:20:00", "Desert");


                // Cantha
                AddBossEvent("Aetherblade Assault", "01:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "03:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "05:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "07:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "09:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "11:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "13:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "15:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "17:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "19:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "21:30:00", "Cantha");
                AddBossEvent("Aetherblade Assault", "23:30:00", "Cantha");

                AddBossEvent("Kaineng Blackout", "02:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "04:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "06:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "08:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "10:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "12:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "14:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "16:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "18:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "20:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "22:00:00", "Cantha");
                AddBossEvent("Kaineng Blackout", "00:00:00", "Cantha");

                AddBossEvent("Gang War", "02:30:00", "Cantha");
                AddBossEvent("Gang War", "04:30:00", "Cantha");
                AddBossEvent("Gang War", "06:30:00", "Cantha");
                AddBossEvent("Gang War", "08:30:00", "Cantha");
                AddBossEvent("Gang War", "10:30:00", "Cantha");
                AddBossEvent("Gang War", "12:30:00", "Cantha");
                AddBossEvent("Gang War", "14:30:00", "Cantha");
                AddBossEvent("Gang War", "16:30:00", "Cantha");
                AddBossEvent("Gang War", "18:30:00", "Cantha");
                AddBossEvent("Gang War", "20:30:00", "Cantha");
                AddBossEvent("Gang War", "22:30:00", "Cantha");
                AddBossEvent("Gang War", "00:30:00", "Cantha");

                AddBossEvent("Aspenwood", "01:40:00", "Cantha");
                AddBossEvent("Aspenwood", "03:40:00", "Cantha");
                AddBossEvent("Aspenwood", "05:40:00", "Cantha");
                AddBossEvent("Aspenwood", "07:40:00", "Cantha");
                AddBossEvent("Aspenwood", "09:40:00", "Cantha");
                AddBossEvent("Aspenwood", "11:40:00", "Cantha");
                AddBossEvent("Aspenwood", "13:40:00", "Cantha");
                AddBossEvent("Aspenwood", "15:40:00", "Cantha");
                AddBossEvent("Aspenwood", "17:40:00", "Cantha");
                AddBossEvent("Aspenwood", "19:40:00", "Cantha");
                AddBossEvent("Aspenwood", "21:40:00", "Cantha");
                AddBossEvent("Aspenwood", "23:40:00", "Cantha");

                AddBossEvent("Battle for Jade Sea", "01:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "03:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "05:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "07:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "09:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "11:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "13:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "15:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "17:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "19:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "21:00:00", "Cantha");
                AddBossEvent("Battle for Jade Sea", "23:00:00", "Cantha");


                //SotO
                //SotO
                AddBossEvent("Unlock'Wizard's Tower", "01:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "03:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "05:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "07:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "09:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "11:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "13:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "15:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "17:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "19:00:00", "SotO");
                AddBossEvent("Unlock'Wizard's Tower", "23:00:00", "SotO");

                AddBossEvent("Fly by Night", "01:55:00", "SotO");
                AddBossEvent("Fly by Night", "03:55:00", "SotO");
                AddBossEvent("Fly by Night", "05:55:00", "SotO");
                AddBossEvent("Fly by Night", "07:55:00", "SotO");
                AddBossEvent("Fly by Night", "09:55:00", "SotO");
                AddBossEvent("Fly by Night", "11:55:00", "SotO");
                AddBossEvent("Fly by Night", "13:55:00", "SotO");
                AddBossEvent("Fly by Night", "15:55:00", "SotO");
                AddBossEvent("Fly by Night", "17:55:00", "SotO");
                AddBossEvent("Fly by Night", "19:55:00", "SotO");
                AddBossEvent("Fly by Night", "21:55:00", "SotO");
                AddBossEvent("Fly by Night", "23:55:00", "SotO");

                AddBossEvent("Defense of Amnytas", "02:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "04:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "06:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "08:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "10:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "12:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "14:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "16:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "18:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "20:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "22:00:00", "SotO");
                AddBossEvent("Defense of Amnytas", "00:00:00", "SotO");

                AddBossEvent("Convergences", "03:30:00", "SotO");
                AddBossEvent("Convergences", "06:30:00", "SotO");
                AddBossEvent("Convergences", "09:30:00", "SotO");
                AddBossEvent("Convergences", "12:30:00", "SotO");
                AddBossEvent("Convergences", "15:30:00", "SotO");
                AddBossEvent("Convergences", "18:30:00", "SotO");
                AddBossEvent("Convergences", "21:30:00", "SotO");
                AddBossEvent("Convergences", "00:30:00", "SotO");



                BossEventGroups = Events.GroupBy(bossEvent =>
                            new { bossEvent.BossName, bossEvent.Category },
                        (key, g) =>
                            new BossEventGroup(key.BossName, key.Category, g.ToList())
                    )
                    .ToList();
            }
        }



        public static void SetBossListFromConfig_Bosses()
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(GlobalVariables.FILE_PATH);

                // Index der Zeile mit dem Bossnamen finden
                int bossIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Bosses:"))
                    {
                        bossIndex = i; // Die aktuelle Zeile enthält den Namen
                        break;
                    }
                }

                // Wenn der Bossname gefunden wird, setze die BossList23
                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    // Entferne die äußeren Anführungszeichen und teile die Bosse
                    string[] bossNames = bossLine
                        .Trim('"')  // Entferne äußere Anführungszeichen
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())  // Entferne führende und abschließende Leerzeichen
                        .ToArray();

                    // Erstelle eine neue List, um BossList23 zu ersetzen
                    List<string> newBossList = new List<string>();

                    // Füge jeden Bossnamen zur neuen Liste hinzu
                    newBossList.AddRange(bossNames);

                    // Iteriere durch die Zeilen, um Timings zu extrahieren
                    for (int i = bossIndex + 1; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("Timings:"))
                        {
                            string timingLine = lines[i].Replace("Timings:", "").Trim();
                            string[] timings = timingLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(timing => timing.Trim())
                                .ToArray();

                            // Überprüfe, ob die Anzahl der Timings mit der Anzahl der Bosse übereinstimmt
                            if (timings.Length == bossNames.Length)
                            {
                                for (int j = 0; j < bossNames.Length; j++)
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
                    }

                    // Jetzt kannst du die alte BossList23 durch die neue Liste ersetzen
                    BossList23 = newBossList;
                }
            }
            catch (Exception ex)
            {
                // Hier kann eine Fehlermeldung protokolliert oder geloggt werden, wenn gewünscht
            }
        }


        public static void AddBossEvent(string bossName, string timing, string category)
        {
            Events.Add(new BossEvent(bossName, timing, category));
        }

        public class BossEventGroup
        {

            public string BossName { get; }
            public string Category { get; }
            public TimeSpan Duration { get; set; } 
            public List<BossEvent> Timings = new List<BossEvent>();
            
            

            public BossEventGroup(string bossName, string bossCategory, List<BossEvent> events)
            {
                BossName = bossName;
                Category = bossCategory;
                Timings = events
                    .Where(bossEvent => bossEvent.BossName.Equals(BossName))
                    .OrderBy(span => span.Timing)
                    .ToList();
                // BossEvent? firstEvent = events.FirstOrDefault();
                // if (firstEvent != null)
                // {
                //     Category = firstEvent.Category;
                // }
            }

            public List<BossEventRun> GetNextRuns()
            {
                List<BossEvent> nextTimings = Timings
                    .Where(bossEvent => bossEvent.Timing > GlobalVariables.CURRENT_TIME)
                    .ToList();
                if (nextTimings.Count == 0)
                {
                    return Timings
                        .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category, GlobalVariables.CURRENT_DATE_TIME.Date.Add(new TimeSpan(24, 0, 0)) + bossEvent.Timing))
                        .Take(NEXT_RUNS_TO_SHOW)
                        .ToList();
                }

                return nextTimings
                    .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category, GlobalVariables.CURRENT_DATE_TIME.Date + bossEvent.Timing))
                    .Take(NEXT_RUNS_TO_SHOW)
                    .ToList();
            }

            public List<BossEventRun> GetPreviousRuns()
            {
                return Timings
                    .Where(bossEvent => bossEvent.Timing > GlobalVariables.CURRENT_TIME.Subtract(new TimeSpan(0, 14, 59)) && bossEvent.Timing < GlobalVariables.CURRENT_TIME)
                    .Select(bossEvent => new BossEventRun(bossEvent.BossName, bossEvent.Timing, bossEvent.Category, GlobalVariables.CURRENT_DATE_TIME.Date + bossEvent.Timing))
                    .Take(PREVIOUS_RUNS_TO_SHOW)
                    .ToList();
            }
            
        }

        public class BossEvent
        {
            public string BossName { get; }
            public TimeSpan Timing { get; }
            public string Category { get; }
            public TimeSpan Duration { get; set; }

            public BossEvent(string bossName, string timing, string category) : this(bossName, TimeSpan.Parse(timing),
                category)
            {
            }

            public BossEvent(string bossName, TimeSpan timing, string category)
            {
                BossName = bossName;
                Timing = timing;
                Category = category;
            }
        }


        public class BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime)
            : BossEvent(bossName, timing, category)
        {
            public DateTime NextRunTime { get; set; } = nextRunTime;
            
        }
    }
}