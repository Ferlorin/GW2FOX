using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Lyssa : BaseForm
    {
        private TextBox Lyssacost; // Keep this declaration

        public Lyssa()
        {
            InitializeComponent();
            LoadConfigText();
            // No need to re-create the TextBox here
            _ = LoadItemPriceInformation();
        }

        // Variable zur Speicherung des Ursprungs der Seite
        private string originPage;

        // Konstruktor, der den Ursprung der Seite als Parameter akzeptiert
        public Lyssa(string origin)
        {
            InitializeComponent();
            LoadConfigText();
            InitializeItemPriceTextBox();
            _ = LoadItemPriceInformation();

            // Setze den Ursprung der Seite
            originPage = origin;
        }


        private void InitializeItemPriceTextBox()
        {
            // Remove this line since Itempriceexeofzhaitan is already declared in the designer.
            // Itempriceexeofzhaitan = new TextBox(); 
            Lyssacost.Text = "Item-Preis: Wird geladen...";
            Lyssacost.AutoSize = true;
            Lyssacost.ReadOnly = true;
            Lyssacost.Location = new Point(/* Specify the X and Y coordinates */);
            this.Controls.Add(Lyssacost);
        }



        private async Task LoadItemPriceInformation()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://api.guildwars2.com/v2/items/46468";
                    string jsonResult = await client.GetStringAsync(apiUrl);

                    JObject resultObject = JObject.Parse(jsonResult);

                    string itemName = (string)resultObject["name"];
                    string chatLink = (string)resultObject["chat_link"];
                    int itemPriceCopper = await GetItemPriceCopper();

                    int gold = itemPriceCopper / 10000;
                    int silver = (itemPriceCopper % 10000) / 100;
                    int copper = itemPriceCopper % 100;

                    // Update the existing "Itempriceexeofzhaitan" TextBox text
                    Lyssaitemname.Text = $"{itemName}";

                    Lyssacost.Text = $"{chatLink}, Price: {gold} Gold, {silver} Silver, {copper} Copper";
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
                    string jsonResult = await client.GetStringAsync("https://api.guildwars2.com/v2/commerce/prices/46468");
                    JObject resultObject = JObject.Parse(jsonResult);
                    return (int)resultObject["sells"]["unit_price"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Abrufen des Item-Preises: {ex.Message}");
            }
        }

        private void LoadConfigText()
        {
            try
            {
                string configFilePath = "config.txt";

                if (File.Exists(configFilePath))
                {
                    string configText = File.ReadAllText(configFilePath);

                    LoadTextFromConfig("Runinfo:", Runinfo, configText);
                    LoadTextFromConfig("Squadinfo:", Squadinfo, configText);
                    LoadTextFromConfig("Guild:", Guild, configText);
                    LoadTextFromConfig("Welcome:", Welcome, configText);
                    LoadTextFromConfig("Symbols:", Symbols, configText);
                }
                else
                {
                    MessageBox.Show("Die Konfigurationsdatei 'config.txt' wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTextFromConfig(string sectionHeader, TextBox textBox, string configText)
        {
            string pattern = $@"{sectionHeader}\s*""([^""]*)""";
            var match = System.Text.RegularExpressions.Regex.Match(configText, pattern);

            if (match.Success)
            {
                textBox.Text = match.Groups[1].Value;
            }
            else
            {
                MessageBox.Show($"Das Muster '{sectionHeader}' wurde in der Konfigurationsdatei nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BringGw2ToFront()
        {
            try
            {
                string processName = "Gw2-64";
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                    ShowWindow(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                }
                else
                {
                    MessageBox.Show("Gw2-64.exe is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error bringing Gw2-64.exe to the foreground: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void Back_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Worldbosses());
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

        private void Lyssainfo2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Lyssainfo2.Text);
            BringGw2ToFront();
        }

        private void Lyssainfo1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Lyssainfo2.Text);
            BringGw2ToFront();
        }

        private void AttentionlyssaClick(object sender, EventArgs e)
        {
            Clipboard.SetText(Attentionlyssa.Text);
            BringGw2ToFront();
        }

        private void Preis_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Lyssacost.Text);
            BringGw2ToFront();
        }

        private void Lyssainfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Lyssainfo.Text);
            BringGw2ToFront();
        }

        private void Lyssainstance_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Lyssainstance.Text);
            BringGw2ToFront();
        }
    }
}
