using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GW2FOX
{
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
            return;
        }

        string json = File.ReadAllText(path);

        var config = JsonConvert.DeserializeObject<BossConfig>(json);
        if (config == null || config.Bosses == null || !config.Bosses.Any())
        {
            return;
        }

        foreach (var boss in config.Bosses)
        {

            if (boss.LootItemId != null && boss.LootItemId.Any())
            {
                foreach (var id in boss.LootItemId)
                {
                    items.Add((id, boss.Name));
                }
            }
            else
            {
               // Console.WriteLine($"   [WARN] Keine gültigen LootItemIds für Boss {boss.Name}");
            }
        }
    }

    public async Task<List<LootResult>> LoadLootDataAsync()
    {
        var result = new List<LootResult>();

        // Cache, damit API nur einmal je ID abgefragt wird
        Dictionary<int, JObject> itemDetailsCache = new();
        Dictionary<int, JObject> priceDetailsCache = new();

        foreach (var item in items)
        {
            try
            {
                if (!itemDetailsCache.TryGetValue(item.Id, out var itemDetails))
                {
                    itemDetails = await GetItemDetailsAsync(item.Id);
                    itemDetailsCache[item.Id] = itemDetails;
                }

                if (!priceDetailsCache.TryGetValue(item.Id, out var priceDetails))
                {
                    priceDetails = await GetItemPriceAsync(item.Id);
                    priceDetailsCache[item.Id] = priceDetails;
                }

                if (itemDetails == null || priceDetails == null)
                    continue;

                var lootResult = CreateLootResult(itemDetails, priceDetails, item.BossName);
                result.Add(lootResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing item {item.Id} (Boss: {item.BossName}): {ex.Message}");
            }
        }

        return result;
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
        string formattedPrice = $"{gold}g";

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
        return flatResults
                .GroupBy(r => r.BossName)
                .ToDictionary(g => g.Key, g => g.ToList());
    }
    }

}
