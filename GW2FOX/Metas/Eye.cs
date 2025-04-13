using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Eye : BaseForm
    {
        private TextBox Itempriceexeofzhaitan; // Keep this declaration

        public Eye()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
            // No need to re-create the TextBox here
            _ = LoadItemPriceInformation();
        }

        // Variable zur Speicherung des Ursprungs der Seite
        private string originPage;

        // Konstruktor, der den Ursprung der Seite als Parameter akzeptiert
        public Eye(string origin) : this()
        {
            InitializeItemPriceTextBox();

            // Setze den Ursprung der Seite
            originPage = origin;
        }

        private void InitializeItemPriceTextBox()
        {
            // Remove this line since Itempriceexeofzhaitan is already declared in the designer.
            // Itempriceexeofzhaitan = new TextBox(); 
            Itempriceexeofzhaitan.Text = "Item-Preis: Wird geladen...";
            Itempriceexeofzhaitan.AutoSize = true;
            Itempriceexeofzhaitan.ReadOnly = true;
            Itempriceexeofzhaitan.Location = new Point(/* Specify the X and Y coordinates */);
            Controls.Add(Itempriceexeofzhaitan);
        }



        private async Task LoadItemPriceInformation()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.guildwars2.com/v2/items/39475";
                    string jsonResult = await client.GetStringAsync(apiUrl);

                    JObject resultObject = JObject.Parse(jsonResult);

                    string itemName = (string)resultObject["name"];
                    string chatLink = (string)resultObject["chat_link"];
                    int itemPriceCopper = await GetItemPriceCopper();

                    int gold = itemPriceCopper / 10000;
                    int silver = (itemPriceCopper % 10000) / 100;
                    int copper = itemPriceCopper % 100;

                    // Update the existing "Eyeitemname" TextBox text
                    Eyeitemname.Text = $"{itemName}";

                    // Update the existing "Itempriceexeofzhaitan" TextBox text
                    Itempriceexeofzhaitan.Text = $"{chatLink}, Price: {gold} Gold";
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
                    string jsonResult = await client.GetStringAsync("https://api.guildwars2.com/v2/commerce/prices/39475");
                    JObject resultObject = JObject.Parse(jsonResult);
                    return (int)resultObject["sells"]["unit_price"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Abrufen des Item-Preises: {ex.Message}");
            }
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

        private void Eyeinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Eyeinfo.Text);
            BringGw2ToFront();
        }

        private void EyeinstanceClick(object sender, EventArgs e)
        {
            Clipboard.SetText(Eyeinstance.Text);
            BringGw2ToFront();
        }

        private void Attentioneye_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Attentioneye.Text);
            BringGw2ToFront();
        }

        private void Eyeinfos_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Eyeinfos.Text);
            BringGw2ToFront();
        }

        private void Eyeinfos2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Eyeinfos2.Text);
            BringGw2ToFront();
        }

        private void Preis_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Itempriceexeofzhaitan.Text);
            BringGw2ToFront();
        }
    }
}
