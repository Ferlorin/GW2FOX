using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    public partial class Behemoth : BaseForm
    {
        public Behemoth()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
            _ = LoadItemPriceInformation();
        }

        private async Task LoadItemPriceInformation()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.guildwars2.com/v2/items/19360";
                    string jsonResult = await client.GetStringAsync(apiUrl);

                    JObject resultObject = JObject.Parse(jsonResult);

                    string itemName = (string)resultObject["name"];
                    string chatLink = (string)resultObject["chat_link"];
                    int itemPriceCopper = await GetItemPriceCopper();

                    int gold = itemPriceCopper / 10000;
                    int silver = (itemPriceCopper % 10000) / 100;
                    int copper = itemPriceCopper % 100;

                    // Update the existing "Itempriceexeofzhaitan" TextBox text
                    Mawitemname.Text = $"{itemName}";

                    Mawitem.Text = $"{chatLink}, Price: {gold} Gold, {silver} Silver, {copper} Copper";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Oh NO something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<int> GetItemPriceCopper()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string jsonResult = await client.GetStringAsync("https://api.guildwars2.com/v2/commerce/prices/19360");
                    JObject resultObject = JObject.Parse(jsonResult);
                    return (int)resultObject["sells"]["unit_price"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        private void Behepres_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Behepres.Text);
            BringGw2ToFront();
        }

        private void Beheinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinfo.Text);
            BringGw2ToFront();
        }

        private void Beheinstance_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinstance.Text);
            BringGw2ToFront();
        }

        private void Behemapinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Attentionbehe.Text);

            BringGw2ToFront();
        }


        private void Runinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
            BringGw2ToFront();
        }

        private void Squadinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Squadinfo.Text);
            BringGw2ToFront();
        }

        private void Guild_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void Welcome_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string homepageUrl = "https://wiki.guildwars2.com/wiki/Defeat_the_shadow_behemoth";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = homepageUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"GREAT - you deleted the INTERNET!: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Mawitem.Text);
            BringGw2ToFront();
        }
    }
}