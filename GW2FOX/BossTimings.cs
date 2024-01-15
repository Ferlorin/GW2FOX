namespace GW2FOX
{
    public static class BossTimings
    {
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } = new Dictionary<string, List<BossEvent>>();
        private static string filePath = "config.txt";
        public static List<string> BossList23 { get; set; } = new List<string>();
        internal static List<BossEvent> Events = new List<BossEvent>();

        static BossTimings()
        {
            SetBossListFromConfig_Bosses();
            InitializeBossEvents();
        }

        private static void InitializeBossEvents()
        {
            foreach (var bossName in BossList23)
            {
                AddBossEvent(bossName, "01:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "03:15:00", "WBs", TimeSpan.FromHours(24));        
                AddBossEvent(bossName, "05:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "07:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "09:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "11:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "13:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "15:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "17:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "19:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "21:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent(bossName, "23:15:00", "WBs", TimeSpan.FromHours(24));


                AddBossEvent("Fireshaman & minions", "01:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "03:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "04:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "07:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "09:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "11:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "13:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "15:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "17:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "19:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "21:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fireshaman & minions", "23:15:00", "WBs", TimeSpan.FromHours(24));


                AddBossEvent("LLLA - Timberline Falls", "01:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Iron Marches", "03:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Gendarran Fields", "05:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Timberline Falls", "07:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Iron Marches", "09:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Gendarran Fields", "11:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Timberline Falls", "13:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Iron Marches", "15:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Gendarran Fields", "17:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Timberline Falls", "19:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Iron Marches", "21:20:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("LLLA - Gendarran Fields", "23:20:00", "WBs", TimeSpan.FromHours(24));


                AddBossEvent("Fire Elemental", "01:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "03:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "05:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "07:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "09:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "11:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "13:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "15:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "17:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "19:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "21:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Fire Elemental", "23:45:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Great Jungle Wurm", "00:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "02:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "04:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "06:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "08:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "10:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "12:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "14:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "16:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "18:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "20:15:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Great Jungle Wurm", "22:15:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Ulgoth the Modniir", "02:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "05:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "08:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "11:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "14:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "17:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "20:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Ulgoth the Modniir", "23:30:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Taidha Covington", "01:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "04:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "07:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "10:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "13:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "16:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "19:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Taidha Covington", "22:00:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("The Shatterer", "02:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "05:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "08:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "11:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "14:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "17:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "20:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("The Shatterer", "23:00:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Shadow Behemoth", "02:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "04:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "06:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "08:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "10:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "12:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "14:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "16:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "18:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "20:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "22:45:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Shadow Behemoth", "00:45:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Tequatl the Sunless", "01:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Tequatl the Sunless", "04:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Tequatl the Sunless", "08:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Tequatl the Sunless", "12:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Tequatl the Sunless", "17:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Tequatl the Sunless", "20:00:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Megadestroyer", "01:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "04:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "07:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "10:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "13:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "16:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "19:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Megadestroyer", "22:30:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Inquest Golem Mark II", "00:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "03:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "06:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "09:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "12:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "15:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "18:03:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Inquest Golem Mark II", "21:03:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Karka Queen", "00:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Karka Queen", "03:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Karka Queen", "07:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Karka Queen", "11:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Karka Queen", "16:00:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Karka Queen", "19:00:00", "WBs", TimeSpan.FromHours(24));

                AddBossEvent("Claw of Jormag", "03:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "06:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "09:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "12:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "15:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "18:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "21:30:00", "WBs", TimeSpan.FromHours(24));
                AddBossEvent("Claw of Jormag", "00:30:00", "WBs", TimeSpan.FromHours(24));

                //LWS2
                AddBossEvent("Sandstorm", "00:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "01:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "02:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "03:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "04:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "05:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "06:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "07:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "08:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "09:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "10:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "11:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "12:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "13:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "14:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "15:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "16:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "17:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "18:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "19:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "20:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "21:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "22:40:00", "LWS2", TimeSpan.FromHours(24));
                AddBossEvent("Sandstorm", "23:40:00", "LWS2", TimeSpan.FromHours(24));

                //LWS3
                AddBossEvent("Saidra's Haven", "00:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "02:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "04:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "06:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "08:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "10:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "12:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "14:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "16:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "18:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "20:00:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Saidra's Haven", "22:00:00", "LWS3", TimeSpan.FromHours(24));

                AddBossEvent("New Loamhurst", "00:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "02:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "04:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "06:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "08:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "10:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "12:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "14:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "16:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "18:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "20:45:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("New Loamhurst", "22:45:00", "LWS3", TimeSpan.FromHours(24));

                AddBossEvent("Noran's Homestead", "01:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "03:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "05:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "07:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "09:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "11:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "13:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "15:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "17:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "19:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "21:40:00", "LWS3", TimeSpan.FromHours(24));
                AddBossEvent("Noran's Homestead", "23:40:00", "LWS3", TimeSpan.FromHours(24));


                //Ice
                AddBossEvent("Defend Jora's Keep", "02:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "04:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "06:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "08:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "10:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "12:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "14:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "16:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "18:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "20:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "22:45:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Defend Jora's Keep", "00:45:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Doomlore Shrine", "01:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "03:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "05:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "07:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "09:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "11:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "13:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "15:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "17:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "19:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "21:38:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Doomlore Shrine", "23:38:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Storms of Winter", "01:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "03:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "05:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "07:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "09:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "11:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "13:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "15:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "17:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "19:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "21:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Storms of Winter", "23:00:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Effigy", "01:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "03:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "05:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "07:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "09:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "11:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "13:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "15:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "17:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "19:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "21:10:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Effigy", "23:10:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Ooze Pits", "00:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "02:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "04:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "06:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "08:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "10:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "12:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "14:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "16:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "18:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "20:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Ooze Pits", "22:05:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Dragonstorm", "00:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "02:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "04:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "06:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "08:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "10:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "12:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "14:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "16:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "18:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "20:00:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Dragonstorm", "22:00:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Drakkar", "00:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "02:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "04:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "06:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "08:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "10:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "12:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "14:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "16:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "18:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "20:05:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Drakkar", "22:05:00", "Ice", TimeSpan.FromHours(24));

                AddBossEvent("Metal Concert", "00:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "02:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "04:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "06:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "08:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "10:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "12:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "14:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "16:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "18:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "20:40:00", "Ice", TimeSpan.FromHours(24));
                AddBossEvent("Metal Concert", "22:40:00", "Ice", TimeSpan.FromHours(24));

                // Maguuma
                AddBossEvent("Chak Gerent", "01:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "03:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "05:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "07:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "09:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "11:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "13:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "15:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "17:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "19:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "21:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Chak Gerent", "23:30:00", "Maguuma", TimeSpan.FromHours(24));

                AddBossEvent("Battle in Tarir", "01:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "03:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "05:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "07:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "09:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "11:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "13:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "15:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "17:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "19:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "21:45:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Battle in Tarir", "23:45:00", "Maguuma", TimeSpan.FromHours(24));

                AddBossEvent("Spellmaster Macsen", "01:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "03:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "05:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "07:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "09:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "11:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "13:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "15:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "17:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "19:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "21:10:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Spellmaster Macsen", "23:10:00", "Maguuma", TimeSpan.FromHours(24));

                AddBossEvent("Dragon's Stand", "00:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "02:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "04:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "06:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "08:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "10:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "12:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "14:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "16:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "18:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "20:30:00", "Maguuma", TimeSpan.FromHours(24));
                AddBossEvent("Dragon's Stand", "22:30:00", "Maguuma", TimeSpan.FromHours(24));

                //Desert
                AddBossEvent("The Oil Floes", "01:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "03:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "05:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "07:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "09:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "11:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "13:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "15:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "17:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "19:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "21:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("The Oil Floes", "23:45:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Maws of Torment", "00:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "02:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "04:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "06:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "08:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "10:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "12:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "14:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "16:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "18:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "20:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Maws of Torment", "22:00:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Palawadan", "00:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "02:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "04:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "06:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "08:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "10:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "12:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "14:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "16:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "18:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "20:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Palawadan", "22:45:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Thunderhead Keep", "00:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "02:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "04:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "06:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "08:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "10:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "12:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "14:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "16:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "18:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "20:45:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Thunderhead Keep", "22:45:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Serpents' Ire", "01:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "03:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "05:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "07:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "09:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "11:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "13:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "15:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "17:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "19:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "21:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Serpents' Ire", "23:30:00", "Desert", TimeSpan.FromHours(24));


                AddBossEvent("Death-Branded Shatterer", "02:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "04:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "06:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "08:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "10:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "12:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "14:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "16:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "18:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "20:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "22:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Death-Branded Shatterer", "00:00:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Junundu Rising", "00:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "01:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "02:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "03:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "04:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "05:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "06:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "07:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "08:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "09:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "10:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "12:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "13:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "14:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "15:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "16:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "17:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "18:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "19:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "20:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "21:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "22:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Junundu Rising", "23:30:00", "Desert", TimeSpan.FromHours(24));
              


                AddBossEvent("Path to Ascension", "00:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "02:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "04:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "06:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "08:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "10:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "12:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "14:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "16:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "18:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "20:30:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Path to Ascension", "22:30:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Doppelganger", "00:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "02:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "04:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "06:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "08:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "10:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "12:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "14:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "16:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "18:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "20:55:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Doppelganger", "22:55:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Forged with Fire", "01:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "02:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "03:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "04:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "05:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "06:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "07:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "08:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "09:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "10:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "11:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "12:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "13:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "14:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "15:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "16:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "17:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "18:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "19:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "20:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "21:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "22:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "23:00:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Forged with Fire", "24:00:00", "Desert", TimeSpan.FromHours(24));

                AddBossEvent("Choya Piñata", "01:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "03:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "05:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "07:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "09:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "11:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "13:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "15:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "17:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "19:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "21:20:00", "Desert", TimeSpan.FromHours(24));
                AddBossEvent("Choya Piñata", "23:20:00", "Desert", TimeSpan.FromHours(24));

                // Cantha
                AddBossEvent("Aetherblade Assault", "00:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "02:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "04:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "06:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "08:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "10:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "12:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "14:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "16:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "18:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "20:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aetherblade Assault", "22:30:00", "Cantha", TimeSpan.FromHours(24));

                AddBossEvent("Kaineng Blackout", "01:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "03:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "05:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "07:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "09:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "11:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "13:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "15:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "17:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "19:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "21:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Kaineng Blackout", "23:00:00", "Cantha", TimeSpan.FromHours(24));

                AddBossEvent("Gang War", "01:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "03:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "05:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "07:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "09:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "11:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "13:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "15:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "17:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "19:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "21:30:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Gang War", "23:30:00", "Cantha", TimeSpan.FromHours(24));

                AddBossEvent("Aspenwood", "00:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "02:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "04:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "06:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "08:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "10:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "12:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "14:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "16:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "18:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "20:40:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Aspenwood", "22:40:00", "Cantha", TimeSpan.FromHours(24));

                AddBossEvent("Battle for Jade Sea", "00:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "02:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "04:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "06:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "08:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "10:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "12:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "14:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "16:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "18:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "20:00:00", "Cantha", TimeSpan.FromHours(24));
                AddBossEvent("Battle for Jade Sea", "22:00:00", "Cantha", TimeSpan.FromHours(24));

                //SotO
                AddBossEvent("Unlock'Wizard's Tower", "00:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "02:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "04:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "06:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "08:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "10:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "12:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "14:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "16:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "18:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Unlock'Wizard's Tower", "22:00:00", "SotO", TimeSpan.FromHours(24));


                AddBossEvent("Fly by Night", "00:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "02:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "04:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "06:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "08:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "10:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "12:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "14:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "16:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "18:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "20:55:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Fly by Night", "22:55:00", "SotO", TimeSpan.FromHours(24));

                AddBossEvent("Defense of Amnytas", "01:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "03:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "05:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "07:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "09:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "11:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "13:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "15:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "17:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "19:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "21:00:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Defense of Amnytas", "23:00:00", "SotO", TimeSpan.FromHours(24));

                AddBossEvent("Convergences", "02:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "05:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "08:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "11:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "14:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "17:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "20:30:00", "SotO", TimeSpan.FromHours(24));
                AddBossEvent("Convergences", "23:30:00", "SotO", TimeSpan.FromHours(24));

            }
        }

        

        private static void SetBossListFromConfig_Bosses()
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(filePath);

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
                                    AddBossEvent(bossNames[j], timings[j], "WBs", TimeSpan.FromHours(24));
                                }
                            }
                            else
                            {
                           
                            }
                            break;
                        }
                    }
                    BossList23 = newBossList;
                }
            }
            catch (Exception ex)
            {
                // Hier kann eine Fehlermeldung protokolliert oder geloggt werden, wenn gewünscht
            }
        }

        private static void AddBossEvent(string bossName, string timing, string category, TimeSpan repeatInterval)
        {
           
            BossEvent bossEvent = new BossEvent
            {
                BossName = bossName,
                Timing = ParseTiming(timing),
                Category = category,
                RepeatInterval = repeatInterval
            };
            Events.Add(bossEvent);
        }


        private static TimeSpan ParseTiming(string timing)
        {
            // Implement parsing logic based on your requirements
            // Example: return TimeSpan.Parse(timing);
            return TimeSpan.Zero; // Placeholder, replace with actual logic
        }
    }

    public class BossEvent
    {
        public string BossName { get; set; }
        public TimeSpan Timing { get; set; }
        public string Category { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan RepeatInterval { get; set; }

        // Parameterloser Konstruktor für den Objektinitialisierer
        public BossEvent() { }

     
        public BossEvent(string bossName, TimeSpan timing, string category)
        {
            BossName = bossName;
            Timing = timing;
            Category = category;
        }

           public void SetEventDetails(TimeSpan duration, TimeSpan repeatInterval)
        {
            Duration = duration;
            RepeatInterval = repeatInterval;
        }
    }
}

