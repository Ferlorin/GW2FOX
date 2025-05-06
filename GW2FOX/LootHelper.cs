using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

public class LootHelper
{
    private static readonly HttpClient client = new HttpClient();


    public class LootResult
    {
        public string Name { get; set; }
        public string ChatLink { get; set; }
        public string FormattedPrice { get; set; }
        public string BossName { get; set; }
    }

    // Dynamisch geladene Zuordnung aus JSON
    private List<(int Id, string BossName)> items = new();

    public Dictionary<int, string> ItemIdToBossName => items.ToDictionary(x => x.Id, x => x.BossName);

    public LootHelper(string bossConfigPath = "BossTimings.json")
    {
        LoadItemIdsFromBossConfig(bossConfigPath);
    }


    private void LoadItemIdsFromBossConfig(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"⚠️ BossConfig-Datei nicht gefunden: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        Console.WriteLine("[INFO] BossConfig-Datei erfolgreich geladen.");

        var config = JsonConvert.DeserializeObject<BossConfig>(json);
        if (config == null || config.Bosses == null || !config.Bosses.Any())
        {
            Console.WriteLine("[WARN] Keine Bosse in der BossConfig-Datei gefunden oder Fehler beim Deserialisieren.");
            return;
        }

        Console.WriteLine("[INFO] Lade Boss-Konfigurationsdaten...");

        foreach (var boss in config.Bosses)
        {
            Console.WriteLine($"[DEBUG] Prüfe Boss: {boss.Name}");

            if (boss.LootItemId != null && boss.LootItemId.Any())
            {
                foreach (var id in boss.LootItemId)
                {
                    items.Add((id, boss.Name));
                    Console.WriteLine($"   [INFO] Hinzugefügt: ItemId {id} für Boss {boss.Name}");
                }
            }
            else
            {
                Console.WriteLine($"   [WARN] Keine gültigen LootItemIds für Boss {boss.Name}");
            }
        }
    }

    public async Task<Dictionary<int, LootResult>> LoadLootDataAsync()
    {
        Dictionary<int, LootResult> results = new();

        foreach (var item in items)
        {
            try
            {
                Console.WriteLine($"==> Bearbeite ItemId: {item.Id} ({item.BossName})");

                // Optimierung: Zuerst Item-Details abfragen
                var itemDetails = await GetItemDetailsAsync(item.Id);
                if (itemDetails == null)
                {
                    Console.WriteLine($"   Fehler beim Abrufen von Item-Daten für ID {item.Id}");
                    continue;
                }

                // Preis-Daten abfragen
                var priceDetails = await GetItemPriceAsync(item.Id);
                if (priceDetails == null)
                {
                    Console.WriteLine($"   Kein Preis verfügbar für ID {item.Id}");
                    continue;
                }

                // Verarbeitung der erhaltenen Daten
                var lootResult = CreateLootResult(itemDetails, priceDetails, item.BossName);
                results[item.Id] = lootResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei Item {item.Id} ({item.BossName}): {ex.Message}");
            }
        }

        // Ausgabe der Zusammenfassung
        Console.WriteLine("\n=== Zusammenfassung der geladenen Loot-Daten ===");
        foreach (var result in results)
        {
            Console.WriteLine($"ID: {result.Key}, Name: {result.Value.Name}, Preis: {result.Value.FormattedPrice}");
        }

        return results;
    }

    private async Task<JObject> GetItemDetailsAsync(int itemId)
    {
        try
        {
            using var response = await client.GetAsync($"https://api.guildwars2.com/v2/items/{itemId}");
            if (!response.IsSuccessStatusCode) return null;
            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abrufen der Item-Details für ID {itemId}: {ex.Message}");
            return null;
        }
    }

    private async Task<JObject> GetItemPriceAsync(int itemId)
    {
        try
        {
            using var response = await client.GetAsync($"https://api.guildwars2.com/v2/commerce/prices/{itemId}");
            if (!response.IsSuccessStatusCode) return null;
            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abrufen des Preises für ItemId {itemId}: {ex.Message}");
            return null;
        }
    }

    private LootResult CreateLootResult(JObject itemDetails, JObject priceDetails, string bossName)
    {
        string name = itemDetails["name"]?.ToString();
        string chatLink = itemDetails["chat_link"]?.ToString();
        int priceCopper = (int)priceDetails["sells"]["unit_price"];

        int gold = priceCopper / 10000;
        int silver = (priceCopper % 10000) / 100;
        int copper = priceCopper % 100;
        string formattedPrice = $"{gold}g {silver}s {copper}c";

        return new LootResult
        {
            Name = name,
            ChatLink = chatLink,
            FormattedPrice = formattedPrice,
            BossName = bossName
        };
    }
    public async Task<Dictionary<string, List<LootResult>>> LoadLootGroupedByBossAsync()
    {
        var flatResults = await LoadLootDataAsync();
        return flatResults.Values
                          .GroupBy(r => r.BossName)
                          .ToDictionary(g => g.Key, g => g.ToList());
    }

}
