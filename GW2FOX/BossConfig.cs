using GW2FOX;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

public class BossConfig
{
    [JsonProperty("Bosses")]
    public List<Boss> Bosses { get; set; } = new();

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

    public List<Boss> DynamicBosses { get; set; } = new List<Boss>();
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

    [JsonProperty("Waypoint")]
    public string? Waypoint { get; set; } = "";
}


public static class BossTimings
{
    public static void LoadBossConfig(string filePath)
    {
        try
        {

            var json = File.ReadAllText(filePath);
            var bossConfig = JsonConvert.DeserializeObject<BossConfig>(json);

            Console.WriteLine("[DEBUG] JSON-Inhalt nach Deserialisierung:");
            Console.WriteLine(JsonConvert.SerializeObject(bossConfig, Formatting.Indented));

            foreach (var boss in bossConfig.Bosses)
            {
                AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category, boss.Waypoint);
            }

            Console.WriteLine("[Config] Bosses loaded from JSON.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading boss config: {ex.Message}");
        }
    }
   
    public static BossConfigInfos LoadedConfigInfos { get; set; } = new();


    public static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "")
    {
        foreach (var timing in timings)
        {
            var utcTime = ConvertToUtcFromConfigTime(timing);
            BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint));
        }
    }


    public static List<BossEvent> BossEventsList { get; set; } = new();

    public static DateTime ConvertToUtcFromConfigTime(string configTime)
    {
        return DateTime.SpecifyKind(
            DateTime.ParseExact(configTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
            DateTimeKind.Utc
        );
    }
}
