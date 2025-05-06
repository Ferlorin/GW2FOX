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

        public static void ApplyBossGroupFromConfig(string groupName, bool updateUI = true)
        {
            const string configPath = "BossTimings.json";
            if (!File.Exists(configPath))
                return;

            // 1. JSON Datei laden
            var json = JObject.Parse(File.ReadAllText(configPath));
            var config = JsonConvert.DeserializeObject<BossConfig>(json.ToString()) ?? new BossConfig();

            // 2. Bossnamen der gewählten Gruppe ermitteln
            string groupLine = groupName.ToLower() switch
            {
                "meta" => config.Meta,
                "mixed" => config.Mixed,
                "world" => config.World,
                "fido" => config.Fido,
                "choosenones" => string.Join(",", config.Bosses
                                      .Where(b => string.Equals(b.Category, "ChoosenOnes", StringComparison.OrdinalIgnoreCase))
                                      .Select(b => b.Name)),
                _ => ""
            };

            var bossNames = groupLine
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(b => b.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // 3. In JSON unter "ChoosenOnes" schreiben und speichern
            json["ChoosenOnes"] = JArray.FromObject(bossNames);
            File.WriteAllText(configPath, json.ToString(Formatting.Indented));

            BossList23.Clear();
            BossEventsList.Clear();
            BossEventGroups.Clear();

            // 6. Checkboxen anhand ChoosenOnes setzen
            Worldbosses.CheckBossCheckboxes(bossNames, Worldbosses.bossCheckBoxMap);

            // 7. BossEvents hinzufügen – nur aus ChoosenOnes
            var matched = config.Bosses
                .Where(b => bossNames.Contains(b.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            foreach (var boss in matched)
            {
                AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category, boss.Waypoint ?? "");
            }

            // 8. Gruppen neu generieren und UI aktualisieren
            GenerateBossEventGroups();

            if (updateUI)
            {
                BossTimer.UpdateBossList();
                UpdateBossOverlayList();
            }
        }



        public static void CheckBossCheckboxes(IEnumerable<string> bossNames, Dictionary<string, CheckBox> checkBoxMap)
        {
            foreach (var bossName in bossNames)
            {
                if (checkBoxMap.TryGetValue(bossName, out var checkBox))
                {
                    checkBox.Checked = true;
                    checkBox.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        public static BossConfig LoadBossConfigFromFile(string path)
        {
            if (!File.Exists(path))
                return new BossConfig();

            var json = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<BossConfig>(json);
            return config ?? new BossConfig();
        }

        public static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "", string level = "")
        {
            try
            {

                foreach (var timing in timings)
                {
                    var utcTime = ConvertToUtcFromConfigTime(timing);
                    BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint, level));
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
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
            }
        }

        public static List<BossEventRun> GetCombinedBossEvents()
        {
            return StaticBossEvents.Concat(DynamicBossEvents).ToList();
        }

        public static void GenerateBossEventGroups()
        {
            BossEventGroups = BossEventsList
                .GroupBy(be => be.BossName)
                .Select(g => new BossEventGroup(g.Key, g))
                .ToList();
        }


        public static void LoadChosenBossesToUI(Dictionary<string, CheckBox> bossCheckBoxMap)
        {
            var config = LoadBossConfigAndReturn();

            var chosenBossNames = config.Bosses
                .Where(b => b.Category == "ChoosenOnes")
                .Select(b => b.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in bossCheckBoxMap)
            {
                entry.Value.Checked = chosenBossNames.Contains(entry.Key);
                entry.Value.ForeColor = entry.Value.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
            }

            BossList23 = chosenBossNames.ToList();
        }


        public static BossConfig LoadBossConfigAndReturn()
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
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);
                var jObject = JObject.Parse(json);
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
                    AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "ChoosenOnes", boss.Waypoint ?? "", boss.Level ?? "");
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

        public static void ResetAllChestStates()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BossTimings.json");
            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var jObject = JObject.Parse(json);
            var bosses = jObject["Bosses"] as JArray;

            if (bosses == null)
                return;

            foreach (var boss in bosses)
            {
                boss["chestOpened"] = false;
            }

            File.WriteAllText(path, jObject.ToString(Formatting.Indented));
        }

        public static void SetChestState(string bossName, bool opened)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BossTimings.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var jObject = JObject.Parse(json);
            var bosses = jObject["Bosses"] as JArray;

            if (bosses == null) return;

            // Die verknüpften Bossnamen
            var linkedBosses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "LLA Timberline",
        "LLA Iron Marches",
        "LLA Gendarran"
    };

            // Wenn der Boss einer der verknüpften ist: Alle gleichzeitig setzen
            if (linkedBosses.Contains(bossName))
            {
                foreach (var boss in bosses)
                {
                    var name = (string?)boss["Name"];
                    if (name != null && linkedBosses.Contains(name))
                    {
                        boss["chestOpened"] = opened;
                    }
                }
            }
            else
            {
                // Nur den angeklickten Boss ändern
                var boss = bosses.FirstOrDefault(b =>
                    string.Equals((string?)b["Name"], bossName, StringComparison.OrdinalIgnoreCase));

                if (boss != null)
                {
                    boss["chestOpened"] = opened;
                }
            }

            File.WriteAllText(path, jObject.ToString(Formatting.Indented));
        }



        public static bool IsChestOpened(string bossName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BossTimings.json");
            if (!File.Exists(path))
            {
                return false;
            }

            var json = File.ReadAllText(path);
            var jObject = JObject.Parse(json);
            var bosses = jObject["Bosses"] as JArray; // GROSS geschrieben!

            if (bosses == null)
            {
                Console.WriteLine("[ERROR] Kein 'Bosses'-Array gefunden.");
                return false;
            }

            var boss = bosses.FirstOrDefault(b =>
                string.Equals((string?)b["Name"], bossName, StringComparison.OrdinalIgnoreCase));

            if (boss == null)
            {
                return false;
            }

            var state = boss["chestOpened"]?.ToObject<bool>() == true;
            return state;
        }

      


    }

}
