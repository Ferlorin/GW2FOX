using TimeSpan = System.TimeSpan;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    public static class BossTimings
    {
        public static BossConfigInfos? LoadedConfigInfos { get; set; }
        public static BossConfig? LoadedConfig { get; set; }

        public static Dictionary<string, List<BossEvent>> BossEvents { get; } = new();
        public static List<string> BossList23 { get; set; } = new();
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

        public static void ApplyBossGroupFromConfig(string groupName)
        {
            var config = LoadBossConfigFromFile("BossTimings.json");

            string groupLine = groupName.ToLower() switch
            {
                "Meta" => config.Meta,
                "Mixed" => config.Mixed,
                "Morld" => config.World,
                "Fido" => config.Fido,
                _ => ""
            };

            var bossNames = groupLine
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(b => b.Trim())
                .ToList();

            // Filter Bosses aus der Konfiguration
            var bosses = config.Bosses
                .Where(b => bossNames.Contains(b.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            BossEventsList.Clear();
            BossList23 = bosses.Select(b => b.Name).ToList();

            foreach (var boss in bosses)
            {
                AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category, boss.Waypoint ?? "");
            }

            GenerateBossEventGroups();  // neu erzeugen
            BossTimer.UpdateBossList(); // ListView aktualisieren
            UpdateBossOverlayList();    // Overlay aktualisieren
        }


        public static BossConfig LoadBossConfigFromFile(string path)
        {
            if (!File.Exists(path))
                return new BossConfig();

            var json = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<BossConfig>(json);
            return config ?? new BossConfig();
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
               // Console.WriteLine($"[AddBossEvent] Fehler bei {bossName}: {ex.Message}");
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
                   // Console.WriteLine($"- {boss.BossName} @ {boss.NextRunTime} | Show: {boss.TimeToShow}");
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
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
        public static void LoadBossConfig()
        {
            LoadBossConfig("BossTimings.json");
        }

        // Speichern der Bosse
        private static void SaveBossConfig(BossConfig config)
        {
            string path = "BossTimings.json";
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private static void UpdateChosenBosses(IEnumerable<string> selectedBossNames)
        {
            var config = LoadBossConfigAndReturn(); // <- jetzt korrekt

            // Alte ChosenBosses entfernen
            config.Bosses.RemoveAll(b => b.Category == "CustomSelection");

            // Neue hinzufügen
            foreach (var bossName in selectedBossNames.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                config.Bosses.Add(new Boss
                {
                    Name = bossName,
                    Timings = new List<string> { "00:00:00" },
                    Category = "ChosenBosses",
                    Waypoint = ""
                });
            }

            SaveBossConfig(config);
        }


        public static void LoadChosenBossesToUI(Dictionary<string, CheckBox> bossCheckBoxMap)
        {
            var config = LoadBossConfigAndReturn();

            var chosenBossNames = config.Bosses
                .Where(b => b.Category == "CustomSelection")
                .Select(b => b.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in bossCheckBoxMap)
            {
                entry.Value.Checked = chosenBossNames.Contains(entry.Key);
                entry.Value.ForeColor = entry.Value.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
            }
        }

        private static BossConfig LoadBossConfigAndReturn()
        {
            string path = "BossTimings.json";

            if (!File.Exists(path))
                return new BossConfig();

            try
            {
                var json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<BossConfig>(json);
                return config ?? new BossConfig();
            }
            catch
            {
                return new BossConfig();
            }
        }

        public static void LoadBossConfigInfos(string filePath)
        {;

            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);
                var jObject = JObject.Parse(json);

                // Direktes Mapping auf BossConfigInfos
                var configInfos = jObject.ToObject<BossConfigInfos>();

                if (configInfos == null)
                {
                    return;
                }

                LoadedConfigInfos = configInfos;

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[Error] Fehler beim Laden der Konfig-Infos: {ex.Message}");
            }
        }

        public static void LoadBossConfig(string filePath)
        {
            Init();

            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);

                var bossConfig = JsonConvert.DeserializeObject<BossConfig>(json);

                if (bossConfig == null)
                {
                    return;
                }

                LoadedConfig = bossConfig;

                if (bossConfig.Bosses == null)
                {
                    return;
                }

                if (bossConfig.Bosses.Count == 0)
                {
                    return;
                }


                var newBossList = new List<string>();

                foreach (var boss in bossConfig.Bosses)
                {
                    if (boss?.Name == null || boss.Timings == null)
                    {
                        continue;
                    }


                    newBossList.Add(boss.Name);
                    AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "");
                }

                BossList23 = newBossList;

                GenerateBossEventGroups();
                UpdateBossOverlayList();

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[Error] Fehler beim Laden der Boss-JSON: {ex.Message}");
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
