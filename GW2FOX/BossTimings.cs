using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static Dictionary<string, List<BossEvent>> BossEvents { get; } = new();
        public static List<string> BossList23 { get; private set; } = new();
        public static Dictionary<DateTime, List<string>> DoneBosses { get; private set; } = new();
        public static readonly List<BossEvent> BossEventsList = new();
       
        internal static System.Windows.Controls.ListView? BossListView { get; set; }
        public static List<BossEventRun> StaticBossEvents { get; set; } = new();
        public static List<BossEventRun> DynamicBossEvents { get; set; } = new();

        public static List<BossEventGroup> BossEventGroups { get; private set; } = new();

        private static void Init()
        {
            BossEventsList.Clear();
            BossEventGroups.Clear();
        }

        public static void RegisterListView(System.Windows.Controls.ListView listView)
        {
            BossListView = listView;
        }

        public static void LoadBossConfig(string filePath)
        {
            Console.WriteLine("[Config] LoadBossConfig (JSON) wurde aufgerufen.");
            Init();

            try
            {
                var json = File.ReadAllText(filePath);
                var bossConfig = JsonConvert.DeserializeObject<BossConfig>(json);

                if (bossConfig?.Bosses == null || bossConfig.Bosses.Count == 0)
                {
                    Console.WriteLine("[Config] Keine Bosse gefunden oder ungültige JSON.");
                    return;
                }

                var newBossList = new List<string>();

                foreach (var boss in bossConfig.Bosses)
                {
                    newBossList.Add(boss.Name);
                    AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "");
                }

                BossList23 = newBossList;
                GenerateBossEventGroups();
                UpdateBossOverlayList();

                Console.WriteLine($"[Config] {BossEventsList.Count} BossEvents erfolgreich geladen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Fehler beim Laden der Boss-JSON: {ex.Message}");
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

        private static void GenerateBossEventGroups()
        {
            BossEventGroups.Clear();

            var grouped = BossEventsList
                .GroupBy(e => e.BossName)
                .Select(g => new BossEventGroup(g.Key, g.ToList()));

            BossEventGroups.AddRange(grouped);
        }
    }
}
