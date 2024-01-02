using System;
using System.Collections.Generic;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static List<BossEvent> Events { get; } = new List<BossEvent>();

        static BossTimings()
        {
            AddBossEvent("The frozen Maw", "01:15:00", "WBs");
            AddBossEvent("The frozen Maw", "03:15:00", "WBs");
            AddBossEvent("The frozen Maw", "04:15:00", "WBs");
            AddBossEvent("The frozen Maw", "07:15:00", "WBs");
            AddBossEvent("The frozen Maw", "09:15:00", "WBs");
            AddBossEvent("The frozen Maw", "11:15:00", "WBs");
            AddBossEvent("The frozen Maw", "13:15:00", "WBs");
            AddBossEvent("The frozen Maw", "15:15:00", "WBs");
            AddBossEvent("The frozen Maw", "17:15:00", "WBs");
            AddBossEvent("The frozen Maw", "19:15:00", "WBs");
            AddBossEvent("The frozen Maw", "21:15:00", "WBs");
            AddBossEvent("The frozen Maw", "23:15:00", "WBs");

            AddBossEvent("Fireshaman and his minions", "01:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "03:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "04:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "07:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "09:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "11:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "13:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "15:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "17:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "19:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "21:15:00", "WBs");
            AddBossEvent("Fireshaman and his minions", "23:15:00", "WBs");


            AddBossEvent("LLLA - Timberline Falls", "01:20:00", "WBs");
            AddBossEvent("LLLA - Iron Marches", "03:20:00", "WBs");
            AddBossEvent("LLLA - Gendarran Fields", "05:20:00", "WBs");
            AddBossEvent("LLLA - Timberline Falls", "07:20:00", "WBs");
            AddBossEvent("LLLA - Iron Marches", "09:20:00", "WBs");
            AddBossEvent("LLLA - Gendarran Fields", "11:20:00", "WBs");
            AddBossEvent("LLLA - Timberline Falls", "13:20:00", "WBs");
            AddBossEvent("LLLA - Iron Marches", "15:20:00", "WBs");
            AddBossEvent("LLLA - Gendarran Fields", "17:20:00", "WBs");
            AddBossEvent("LLLA - Timberline Falls", "19:20:00", "WBs");
            AddBossEvent("LLLA - Iron Marches", "21:20:00", "WBs");
            AddBossEvent("LLLA - Gendarran Fields", "23:20:00", "WBs");


            AddBossEvent("Fire Elemental", "01:45:00", "WBs");
            AddBossEvent("Fire Elemental", "03:45:00", "WBs");
            AddBossEvent("Fire Elemental", "05:45:00", "WBs");
            AddBossEvent("Fire Elemental", "07:45:00", "WBs");
            AddBossEvent("Fire Elemental", "09:45:00", "WBs");
            AddBossEvent("Fire Elemental", "11:45:00", "WBs");
            AddBossEvent("Fire Elemental", "13:45:00", "WBs");
            AddBossEvent("Fire Elemental", "15:45:00", "WBs");
            AddBossEvent("Fire Elemental", "17:45:00", "WBs");
            AddBossEvent("Fire Elemental", "19:45:00", "WBs");
            AddBossEvent("Fire Elemental", "21:45:00", "WBs");
            AddBossEvent("Fire Elemental", "23:45:00", "WBs");

            AddBossEvent("Great Jungle Wurm", "00:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "02:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "04:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "06:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "08:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "10:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "12:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "14:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "16:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "18:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "20:15:00", "WBs");
            AddBossEvent("Great Jungle Wurm", "22:15:00", "WBs");

            AddBossEvent("Ulgoth the Modniir", "02:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "05:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "08:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "11:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "14:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "17:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "20:30:00", "WBs");
            AddBossEvent("Ulgoth the Modniir", "23:30:00", "WBs");
            AddBossEvent("Taidha Covington", "01:00:00", "WBs");
            AddBossEvent("Taidha Covington", "04:00:00", "WBs");
            AddBossEvent("Taidha Covington", "07:00:00", "WBs");
            AddBossEvent("Taidha Covington", "10:00:00", "WBs");
            AddBossEvent("Taidha Covington", "13:00:00", "WBs");
            AddBossEvent("Taidha Covington", "16:00:00", "WBs");
            AddBossEvent("Taidha Covington", "19:00:00", "WBs");
            AddBossEvent("Taidha Covington", "22:00:00", "WBs");

            AddBossEvent("The Shatterer", "02:00:00", "WBs");
            AddBossEvent("The Shatterer", "05:00:00", "WBs");
            AddBossEvent("The Shatterer", "08:00:00", "WBs");
            AddBossEvent("The Shatterer", "11:00:00", "WBs");
            AddBossEvent("The Shatterer", "14:00:00", "WBs");
            AddBossEvent("The Shatterer", "17:00:00", "WBs");
            AddBossEvent("The Shatterer", "20:00:00", "WBs");
            AddBossEvent("The Shatterer", "23:00:00", "WBs");

            AddBossEvent("Shadow Behemoth", "02:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "04:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "06:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "08:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "10:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "12:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "14:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "16:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "18:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "20:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "22:45:00", "WBs");
            AddBossEvent("Shadow Behemoth", "00:45:00", "WBs");

            AddBossEvent("Tequatl the Sunless", "01:01:00", "WBs");
            AddBossEvent("Tequatl the Sunless", "04:01:00", "WBs");
            AddBossEvent("Tequatl the Sunless", "08:01:00", "WBs");
            AddBossEvent("Tequatl the Sunless", "12:31:00", "WBs");
            AddBossEvent("Tequatl the Sunless", "16:01:00", "WBs");
            AddBossEvent("Tequatl the Sunless", "20:01:00", "WBs");

            AddBossEvent("Megadestroyer", "01:30:00", "WBs");
            AddBossEvent("Megadestroyer", "04:30:00", "WBs");
            AddBossEvent("Megadestroyer", "07:30:00", "WBs");
            AddBossEvent("Megadestroyer", "10:30:00", "WBs");
            AddBossEvent("Megadestroyer", "13:30:00", "WBs");
            AddBossEvent("Megadestroyer", "16:30:00", "WBs");
            AddBossEvent("Megadestroyer", "19:30:00", "WBs");
            AddBossEvent("Megadestroyer", "22:30:00", "WBs");

            AddBossEvent("Inquest Golem Mark II", "00:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "03:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "06:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "09:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "12:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "15:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "18:03:00", "WBs");
            AddBossEvent("Inquest Golem Mark II", "21:03:00", "WBs");

            AddBossEvent("Karka Queen", "03:00:00", "WBs");
            AddBossEvent("Karka Queen", "07:00:00", "WBs");
            AddBossEvent("Karka Queen", "11:30:00", "WBs");
            AddBossEvent("Karka Queen", "15:00:00", "WBs");
            AddBossEvent("Karka Queen", "19:00:00", "WBs");
            AddBossEvent("Karka Queen", "23:00:00", "WBs");

            AddBossEvent("Claw of Jormag", "03:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "06:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "09:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "12:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "15:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "18:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "21:30:00", "WBs");
            AddBossEvent("Claw of Jormag", "00:30:00", "WBs");

            //Ice
            AddBossEvent("Dragonstorm", "00:00:00", "Ice");
            AddBossEvent("Dragonstorm", "02:00:00", "Ice");
            AddBossEvent("Dragonstorm", "04:00:00", "Ice");
            AddBossEvent("Dragonstorm", "06:00:00", "Ice");
            AddBossEvent("Dragonstorm", "08:00:00", "Ice");
            AddBossEvent("Dragonstorm", "10:00:00", "Ice");
            AddBossEvent("Dragonstorm", "12:00:00", "Ice");
            AddBossEvent("Dragonstorm", "14:00:00", "Ice");
            AddBossEvent("Dragonstorm", "16:00:00", "Ice");
            AddBossEvent("Dragonstorm", "18:00:00", "Ice");
            AddBossEvent("Dragonstorm", "20:00:00", "Ice");
            AddBossEvent("Dragonstorm", "22:00:00", "Ice");

            AddBossEvent("Drakkar", "00:05:00", "Ice");
            AddBossEvent("Drakkar", "02:05:00", "Ice");
            AddBossEvent("Drakkar", "04:05:00", "Ice");
            AddBossEvent("Drakkar", "06:05:00", "Ice");
            AddBossEvent("Drakkar", "08:05:00", "Ice");
            AddBossEvent("Drakkar", "10:05:00", "Ice");
            AddBossEvent("Drakkar", "12:05:00", "Ice");
            AddBossEvent("Drakkar", "14:05:00", "Ice");
            AddBossEvent("Drakkar", "16:05:00", "Ice");
            AddBossEvent("Drakkar", "18:05:00", "Ice");
            AddBossEvent("Drakkar", "20:05:00", "Ice");
            AddBossEvent("Drakkar", "22:05:00", "Ice");

            AddBossEvent("Metal Concert", "00:40:00", "Ice");
            AddBossEvent("Metal Concert", "02:40:00", "Ice");
            AddBossEvent("Metal Concert", "04:40:00", "Ice");
            AddBossEvent("Metal Concert", "06:40:00", "Ice");
            AddBossEvent("Metal Concert", "08:40:00", "Ice");
            AddBossEvent("Metal Concert", "10:40:00", "Ice");
            AddBossEvent("Metal Concert", "12:40:00", "Ice");
            AddBossEvent("Metal Concert", "14:40:00", "Ice");
            AddBossEvent("Metal Concert", "16:40:00", "Ice");
            AddBossEvent("Metal Concert", "18:40:00", "Ice");
            AddBossEvent("Metal Concert", "20:40:00", "Ice");
            AddBossEvent("Metal Concert", "22:40:00", "Ice");

            // Maguuma
            AddBossEvent("Chak Gerent", "01:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "03:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "05:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "07:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "09:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "11:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "13:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "15:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "17:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "19:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "21:30:00", "Maguuma");
            AddBossEvent("Chak Gerent", "23:30:00", "Maguuma");

            AddBossEvent("Battle in Tarir", "01:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "03:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "05:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "07:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "09:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "11:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "13:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "15:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "17:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "19:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "21:45:00", "Maguuma");
            AddBossEvent("Battle in Tarir", "23:45:00", "Maguuma");

            AddBossEvent("Spellmaster Macsen", "01:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "03:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "05:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "07:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "09:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "11:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "13:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "15:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "17:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "19:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "21:10:00", "Maguuma");
            AddBossEvent("Spellmaster Macsen", "23:10:00", "Maguuma");

            //Desert
            AddBossEvent("Death-Branded Shatterer", "02:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "04:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "06:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "08:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "10:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "12:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "14:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "16:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "18:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "20:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "22:00:00", "Desert");
            AddBossEvent("Death-Branded Shatterer", "00:00:00", "Desert");

            AddBossEvent("Junundu Rising", "00:30:00", "Desert");
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

            AddBossEvent("Path to Ascension", "00:30:00", "Desert");
            AddBossEvent("Path to Ascension", "02:30:00", "Desert");
            AddBossEvent("Path to Ascension", "04:30:00", "Desert");
            AddBossEvent("Path to Ascension", "06:30:00", "Desert");
            AddBossEvent("Path to Ascension", "08:30:00", "Desert");
            AddBossEvent("Path to Ascension", "10:30:00", "Desert");
            AddBossEvent("Path to Ascension", "12:30:00", "Desert");
            AddBossEvent("Path to Ascension", "14:30:00", "Desert");
            AddBossEvent("Path to Ascension", "16:30:00", "Desert");
            AddBossEvent("Path to Ascension", "18:30:00", "Desert");
            AddBossEvent("Path to Ascension", "20:30:00", "Desert");
            AddBossEvent("Path to Ascension", "22:30:00", "Desert");

            AddBossEvent("Doppelganger", "00:55:00", "Desert");
            AddBossEvent("Doppelganger", "02:55:00", "Desert");
            AddBossEvent("Doppelganger", "04:55:00", "Desert");
            AddBossEvent("Doppelganger", "06:55:00", "Desert");
            AddBossEvent("Doppelganger", "08:55:00", "Desert");
            AddBossEvent("Doppelganger", "10:55:00", "Desert");
            AddBossEvent("Doppelganger", "12:55:00", "Desert");
            AddBossEvent("Doppelganger", "14:55:00", "Desert");
            AddBossEvent("Doppelganger", "16:55:00", "Desert");
            AddBossEvent("Doppelganger", "18:55:00", "Desert");
            AddBossEvent("Doppelganger", "20:55:00", "Desert");
            AddBossEvent("Doppelganger", "22:55:00", "Desert");

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
            AddBossEvent("Forged with Fire", "24:00:00", "Desert");

            AddBossEvent("Pinata", "01:20:00", "Desert");
            AddBossEvent("Pinata", "03:20:00", "Desert");
            AddBossEvent("Pinata", "05:20:00", "Desert");
            AddBossEvent("Pinata", "07:20:00", "Desert");
            AddBossEvent("Pinata", "09:20:00", "Desert");
            AddBossEvent("Pinata", "11:20:00", "Desert");
            AddBossEvent("Pinata", "13:20:00", "Desert");
            AddBossEvent("Pinata", "15:20:00", "Desert");
            AddBossEvent("Pinata", "17:20:00", "Desert");
            AddBossEvent("Pinata", "19:20:00", "Desert");
            AddBossEvent("Pinata", "21:20:00", "Desert");
            AddBossEvent("Pinata", "23:20:00", "Desert");
            
        }

        public static void AddBossEvent(string bossName, string timing, string category)
        {
            Events.Add(new BossEvent(bossName, timing, category));
        }

        public static BossEvent GetNextBossEvent()
        {
            // Wenn die Liste leer ist, null zurückgeben
            if (Events.Count == 0)
                return null;

            // Index des nächsten Boss-Events in der Liste
            int nextIndex = DateTime.Now.Hour % Events.Count;

            return Events[nextIndex];
        }
    }

    public class BossEvent
    {
        public string BossName { get; }
        public TimeSpan Timing { get; }
        public string Category { get; }

        public BossEvent(string bossName, string timing, string category)
        {
            BossName = bossName;
            Timing = TimeSpan.Parse(timing);
            Category = category;
        }
    }
}