using GW2FOX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class BossConfig
{
    public List<Boss> Bosses { get; set; } = new();
    public string Meta { get; set; } = "";
    public string Mixed { get; set; } = "";
    public string World { get; set; } = "";
    public string Fido { get; set; } = "";
    public string Runinfo { get; set; } = "";
    public string Squadinfo { get; set; } = "";
    public string Guild { get; set; } = "";
    public string Welcome { get; set; } = "";
    public string Symbols { get; set; } = "";
}

public class Boss
{
    public string Name { get; set; }
    public List<string> Timings { get; set; }
    public string Category { get; set; }
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
