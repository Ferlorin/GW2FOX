using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace GW2FOX
{
    // -------------------- BossConfig & Hilfsklassen --------------------
    public class BossConfig
    {
        [JsonProperty("Bosses")]
        public List<Boss> Bosses { get; set; } = new();

        [JsonProperty("ChoosenOnes")]
        public List<string> ChoosenOnes { get; set; } = new();

        [JsonProperty("Meta")]
        public string Meta { get; set; } = "";

        [JsonProperty("Mixed")]
        public string Mixed { get; set; } = "";

        [JsonProperty("World")]
        public string World { get; set; } = "";

        [JsonProperty("Fido")]
        public string Fido { get; set; } = "";

        [JsonProperty("CustomSelection")]
        public string CustomSelection { get; set; } = "";

        public bool ChestOpened { get; set; } = false;
        public List<Boss> DynamicBosses { get; set; } = new();
    }

    public class BossConfigInfos
    {
        [JsonProperty("Runinfo")]
        public string Runinfo { get; set; } = "";

        [JsonProperty("Squadinfo")]
        public string Squadinfo { get; set; } = "";

        [JsonProperty("Guild")]
        public string Guild { get; set; } = "";

        [JsonProperty("Welcome")]
        public string Welcome { get; set; } = "";

        [JsonProperty("Symbols")]
        public string Symbols { get; set; } = "";
    }

    public class Boss
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Timings")]
        public List<string> Timings { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Level")]
        public string Level { get; set; } = "";

        [JsonProperty("Waypoint")]
        public string? Waypoint { get; set; } = "";

        [JsonProperty("chestOpened")]
        public bool ChestOpened { get; set; } = false;

        [JsonProperty("LootItemId")]
        public List<int> LootItemId { get; set; } = new();
    }

    // -------------------- BossEvent --------------------
    public class BossEvent
    {
        public string BossName { get; }
        public string Waypoint { get; }
        public TimeSpan Timing { get; }
        public string Category { get; }
        public string Level { get; } = string.Empty;

        public BossEvent(string bossName, TimeSpan timing, string category, string waypoint = "", string level = "")
        {
            BossName = bossName;
            Timing = GlobalVariables.IsDaylightSavingTimeActive()
                ? timing.Add(TimeSpan.FromHours(1))
                : timing;
            Category = category;
            Waypoint = waypoint;
            Level = level;
        }

        public BossEvent(string bossName, string timing, string category, string waypoint = "", string level = "")
            : this(bossName, TimeSpan.Parse(timing), category, waypoint, level) { }

        public System.Windows.Media.Brush CategoryBrush =>
            Category switch
            {
                "Maguuma" => System.Windows.Media.Brushes.LimeGreen,
                "Desert" => System.Windows.Media.Brushes.DeepPink,
                "WBs" => System.Windows.Media.Brushes.WhiteSmoke,
                "Ice" => System.Windows.Media.Brushes.DeepSkyBlue,
                "Cantha" => System.Windows.Media.Brushes.Blue,
                "SotO" => System.Windows.Media.Brushes.Yellow,
                "LWS2" => System.Windows.Media.Brushes.LightYellow,
                "LWS3" => System.Windows.Media.Brushes.ForestGreen,
                "Treasures" => System.Windows.Media.Brushes.Red,
                _ => System.Windows.Media.Brushes.White
            };
    }

    // -------------------- BossEventRun --------------------
    public class BossEventRun : BossEvent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime NextRunTime { get; set; }

        public BossEventRun(string bossName, TimeSpan timing, string category, DateTime nextRunTime, string waypoint = "", string level = "")
            : base(bossName, timing, category, waypoint, level)
        {
            NextRunTime = nextRunTime;
        }

        public DateTime TimeToShow => NextRunTime;

        public bool IsPreviousBoss => NextRunTime < GlobalVariables.CURRENT_DATE_TIME;

        public TimeSpan TimeRemaining =>
            IsPreviousBoss
                ? GlobalVariables.CURRENT_DATE_TIME.AddMinutes(15) - TimeToShow
                : TimeToShow - GlobalVariables.CURRENT_DATE_TIME;

        public string TimeRemainingFormatted =>
            $"{(int)TimeRemaining.TotalHours:D2}:{TimeRemaining.Minutes:D2}:{TimeRemaining.Seconds:D2}";

        public bool IsPastEvent
        {
            get => _isPastEvent;
            set
            {
                if (_isPastEvent != value)
                {
                    _isPastEvent = value;
                    OnPropertyChanged(nameof(IsPastEvent));
                }
            }
        }
        private bool _isPastEvent;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void TriggerTimeRemainingChanged() =>
            OnPropertyChanged(nameof(TimeRemainingFormatted));
    }

    // -------------------- BossEventGroup --------------------
    public class BossEventGroup
    {
        public string BossName { get; }

        private readonly List<BossEvent> _timings;
        private const int DaysExtraToCalculate = 1;

        public BossEventGroup(string bossName, IEnumerable<BossEvent> bossEvents)
        {
            BossName = bossName;
            _timings = bossEvents
                .Where(e => e.BossName.Equals(bossName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.Timing)
                .ToList();
        }

        public List<BossEvent> Events => _timings;

        public IEnumerable<BossEventRun> GetNextRuns()
        {
            List<BossEventRun> result = new();

            for (int i = -1; i <= DaysExtraToCalculate; i++)
            {
                result.AddRange(_timings.Select(e => new BossEventRun(
                    e.BossName,
                    e.Timing,
                    e.Category,
                    GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                    e.Waypoint,
                    e.Level
                )));
            }

            DateTime now = GlobalVariables.CURRENT_DATE_TIME;

            var filtered = result
                .Where(run =>
                        run.TimeToShow >= now - TimeSpan.FromMinutes(15) &&
                        run.TimeToShow <= now + TimeSpan.FromHours(8))
                .OrderBy(run => run.TimeToShow)
                .ToList();

            return filtered;
        }

        public IEnumerable<BossEventRun> GetAllRuns()
        {
            List<BossEventRun> result = new();

            for (int i = -1; i <= DaysExtraToCalculate; i++)
            {
                result.AddRange(_timings.Select(e => new BossEventRun(
                    e.BossName,
                    e.Timing,
                    e.Category,
                    GlobalVariables.CURRENT_DATE_TIME.Date.AddDays(i) + e.Timing,
                    e.Waypoint,
                    e.Level
                )));
            }

            return result.OrderBy(run => run.TimeToShow);
        }
    }

    // -------------------- BossTimings (statisch) --------------------
  
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

        public static void RegisterListView(System.Windows.Controls.ListView listView)
        {
            BossListView = listView;
        }

        public static void GenerateBossEventGroups()
        {
            BossEventGroups = BossEventsList
                .GroupBy(be => be.BossName)
                .Select(g => new BossEventGroup(g.Key, g))
                .ToList();
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
            catch (Exception)
            {
                // Fehlerbehandlung optional
            }
        }

        public static void ApplyBossGroupFromConfig(string groupName, bool updateUI = true)
        {
            const string configPath = "BossTimings.json";
            if (!File.Exists(configPath))
                return;

            var json = JObject.Parse(File.ReadAllText(configPath));
            var config = JsonConvert.DeserializeObject<BossConfig>(json.ToString()) ?? new BossConfig();

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

            json["ChoosenOnes"] = JArray.FromObject(bossNames);
            File.WriteAllText(configPath, json.ToString(Formatting.Indented));

            BossList23.Clear();
            BossEventsList.Clear();
            BossEventGroups.Clear();

            Worldbosses.CheckBossCheckboxes(bossNames, Worldbosses.bossCheckBoxMap);

            var matched = config.Bosses
                .Where(b => bossNames.Contains(b.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            foreach (var boss in matched)
            {
                AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category, boss.Waypoint ?? "");
            }

            GenerateBossEventGroups();

            if (updateUI)
            {
                BossTimer.UpdateBossList();
                UpdateBossOverlayList();
            }
        }

        public static async Task UpdateBossOverlayList()
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

                var items = BossTimerService.GetBossOverlayItems(combinedBosses, now);

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
                if (BossTimerService._overlayWindow != null)
                    await BossTimerService._overlayWindow.UpdateBossOverlayListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren der BossOverlay-Liste: {ex.Message}");
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
            catch (Exception)
            {
                // Fehlerbehandlung optional
            }
        }

        public static DateTime ConvertToUtcFromConfigTime(string configTime)
        {
            return DateTime.SpecifyKind(
                DateTime.ParseExact(configTime, "HH:mm:ss", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );
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

        public async static void SetChestState(string bossName, bool opened)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BossTimings.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var jObject = JObject.Parse(json);
            var bosses = jObject["Bosses"] as JArray;

            if (bosses == null) return;

            var linkedBosses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "LLA Timberline",
                "LLA Iron Marches",
                "LLA Gendarran"
            };

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
                var boss = bosses.FirstOrDefault(b =>
                    string.Equals((string?)b["Name"], bossName, StringComparison.OrdinalIgnoreCase));

                if (boss != null)
                {
                    boss["chestOpened"] = opened;
                }
            }

            File.WriteAllText(path, jObject.ToString(Formatting.Indented));
            await OverlayWindow.GetInstance()
                                 .UpdateBossOverlayListAsync();
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
            var bosses = jObject["Bosses"] as JArray;

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
