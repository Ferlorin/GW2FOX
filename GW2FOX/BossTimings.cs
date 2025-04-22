using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static BossConfig? LoadedConfig { get; set; }

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



        private static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "")
        {
            try
            {
                Console.WriteLine($"[AddBossEvent] {bossName}: {timings.Length} Zeiten, Kategorie: {category}, WP: {waypoint}");

                foreach (var timing in timings)
                {
                    var utcTime = ConvertToUtcFromConfigTime(timing);
                    BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddBossEvent] Fehler bei {bossName}: {ex.Message}");
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
                 //   var window = OverlayWindow.GetInstance();
                   // window.OverlayItems.Clear();

                    foreach (var item in items.OrderBy(i => i.TimeToShow))
                    {
                        item.UpdateCountdown();
                   //     window.OverlayItems.Add(item);
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

        public static void LoadBossConfig(string filePath)
        {
            Console.WriteLine($"[Config] Lade Boss-Konfiguration aus: {filePath}");
            Init();

            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[Config] Datei nicht gefunden: {filePath}");
                    return;
                }

                var json = File.ReadAllText(filePath);
                Console.WriteLine($"[Config] JSON geladen. Länge: {json.Length} Zeichen");

                var bossConfig = JsonConvert.DeserializeObject<BossConfig>(json);

                if (bossConfig == null)
                {
                    Console.WriteLine("[Config] ❌ BossConfig ist null!");
                    return;
                }

                LoadedConfig = bossConfig;

                if (bossConfig.Bosses == null)
                {
                    Console.WriteLine("[Config] ❌ BossConfig.Bosses ist null!");
                    return;
                }

                if (bossConfig.Bosses.Count == 0)
                {
                    Console.WriteLine("[Config] ⚠️ BossConfig.Bosses ist leer.");
                    return;
                }

                Console.WriteLine($"[Config] Bosses.Count: {bossConfig.Bosses.Count}");

                var newBossList = new List<string>();

                foreach (var boss in bossConfig.Bosses)
                {
                    if (boss?.Name == null || boss.Timings == null)
                    {
                        Console.WriteLine("[Config] ⚠️ Ungültiger Boss-Eintrag übersprungen.");
                        continue;
                    }

                    Console.WriteLine($"[Config] Lade Boss: {boss.Name}, Kategorie: {boss.Category}, Zeiten: {string.Join(", ", boss.Timings)}");

                    newBossList.Add(boss.Name);
                    AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "");
                }

                BossList23 = newBossList;

                Console.WriteLine($"[Config] BossList23 enthält {BossList23.Count} Einträge.");
                GenerateBossEventGroups();
                UpdateBossOverlayList();

                Console.WriteLine($"[Config] ✅ {BossEventsList.Count} BossEvents erfolgreich geladen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Fehler beim Laden der Boss-JSON: {ex.Message}");
            }
        }
        public static List<Boss> GetBossesByCategory(string categoryName)
        {
            if (LoadedConfig == null || LoadedConfig.Bosses == null)
                return new();

            var categoryList = categoryName switch
            {
                "Meta" => LoadedConfig.Meta,
                "Mixed" => LoadedConfig.Mixed,
                "World" => LoadedConfig.World,
                "Fido" => LoadedConfig.Fido,
                _ => ""
            };

            var names = categoryList
                .Split(',')
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return LoadedConfig.Bosses
                .Where(b => names.Contains(b.Name))
                .ToList();
        }



    }
}
