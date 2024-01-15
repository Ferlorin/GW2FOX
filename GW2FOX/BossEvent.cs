namespace GW2FOX
{
    public class BossEvent
    {
        public string BossName { get; }
        public TimeSpan Timing { get; }
        public string Category { get; }
        public TimeSpan Duration { get; set; }

        public BossEvent(string bossName, string timing, string category)
        {
            BossName = bossName;
            Timing = TimeSpan.Parse(timing);
            Category = category;
        }
    }
}