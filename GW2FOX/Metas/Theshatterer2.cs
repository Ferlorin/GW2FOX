using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Theshatterer2 : Form
    {
        private TextBox Shattiitem;

        public Theshatterer2()
        {
            InitializeComponent();
            _ = LoadItemPriceInformation();
        }



        private async Task LoadItemPriceInformation()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.guildwars2.com/v2/items/39467";
                    string jsonResult = await client.GetStringAsync(apiUrl);

                    JObject resultObject = JObject.Parse(jsonResult);

                    string itemName = (string)resultObject["name"];
                    string chatLink = (string)resultObject["chat_link"];
                    int itemPriceCopper = await GetItemPriceCopper();

                    int gold = itemPriceCopper / 10000;
                    int silver = (itemPriceCopper % 10000) / 100;
                    int copper = itemPriceCopper % 100;

                    // Update the existing "Itempriceexeofzhaitan" TextBox text
                    Shattiitem.Text = $"{chatLink}, Price: {gold} Gold, {silver} Silver, {copper} Copper";

                    Shattiitemname.Text = $"{itemName}";
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
                    string jsonResult = await client.GetStringAsync("https://api.guildwars2.com/v2/commerce/prices/39467");
                    JObject resultObject = JObject.Parse(jsonResult);
                    return (int)resultObject["sells"]["unit_price"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Abrufen des Item-Preises: {ex.Message}");
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }


        private void Runinfo_Click(object sender, EventArgs e)
        {
            //
        }

        private void Squadinfo_Click(object sender, EventArgs e)
        {
            //
        }

        private void Guild_Click(object sender, EventArgs e)
        {
        }

        private void Welcome_Click(object sender, EventArgs e)
        {
            //
        }

        private void Shattererinfo_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Shattererinstance_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Attentionshatterer_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Squadmessage2_Click(object sender, EventArgs e)
        {
            // 
        }


        private void Pres_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Petsback_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Crystalls_Click(object sender, EventArgs e)
        {
            //
        }

        private void Nochance_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Burnsblunder_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Nofly_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Nofly2_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Nofly3_Click(object sender, EventArgs e)
        {
            // 
        }

        private void Preis_Click(object sender, EventArgs e)
        {
            // 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string homepageUrl = "https://wiki.guildwars2.com/wiki/Slay_the_Shatterer";
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
    }
}
