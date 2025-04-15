using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace GW2FOX
{
    public partial class Ulgoth : BaseForm
    {
        private const string ItemApiUrl = "https://api.guildwars2.com/v2/items/39473";

        public Ulgoth()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
            InitializeItemPriceTextBox();
            _ = LoadItemPriceInformation();
        }

        private void InitializeItemPriceTextBox()
        {
            Itemprice.Text = "Item-Preis: Wird geladen...";
            Itemprice.AutoSize = true;
            Itemprice.ReadOnly = true;
        }

        private async Task LoadItemPriceInformation()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a request to the API and get the response as JSON
                    string jsonResult = await client.GetStringAsync(ItemApiUrl);

                    // Convert the JSON to a JObject
                    JObject resultObject = JObject.Parse(jsonResult);

                    // Extract the item name and chat link
                    string itemName = (string)resultObject["name"];
                    string chatLink = (string)resultObject["chat_link"];

                    // Get the item price from a separate API call
                    int itemPriceCopper = await GetItemPriceCopper();

                    // Convert the item price to gold
                    int gold = itemPriceCopper / 10000;

                    // Convert the item price to silver
                    int silver = (itemPriceCopper % 10000) / 100;

                    // Convert the item price to copper
                    int copper = itemPriceCopper % 100;

                    // Display the formatted result in the existing TextBox and Pricename TextBox
                    Itemprice.Text = $"{chatLink}, Price: {gold} Gold, {silver} Silver, {copper} Copper";
                    Pricename.Text = itemName;
                }
            }
            catch (Exception ex)
            {
                // Handle possible exceptions (e.g., if the API is not reachable)
                MessageBox.Show($"Oh NO something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        private async Task<int> GetItemPriceCopper()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a request to the API and get the response as JSON
                    string jsonResult = await client.GetStringAsync($"https://api.guildwars2.com/v2/commerce/prices/39473");

                    // Convert the JSON to a JObject
                    JObject resultObject = JObject.Parse(jsonResult);

                    // Extract the item price in copper
                    return (int)resultObject["sells"]["unit_price"];
                }
            }
            catch (Exception ex)
            {
                // Handle possible exceptions (e.g., if the API is not reachable)
                throw new Exception($"Fehler beim Abrufen des Item-Preises: {ex.Message}");
            }
        }












        private void Runinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Runinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Squadinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Squadinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Guild_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Guild.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Welcome_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Welcome.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Ulgothinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Ulgothinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Ulgothinstance_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Ulgothinstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Coordinate_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Coordinate.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Attentionulgoth_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(AttentionUlgoth.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Lootulgoth_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Lootulgoth.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Preis_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Itemprice.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Escord_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Escord.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string homepageUrl = "https://wiki.guildwars2.com/wiki/Defeat_Ulgoth_the_Modniir_and_his_minions";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = homepageUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim ?ffnen der Homepage: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
