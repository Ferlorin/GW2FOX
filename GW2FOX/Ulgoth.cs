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
            LoadConfigText();
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

        private void LoadConfigText()
        {
            try
            {
                // Pfad zur Konfigurationsdatei
                string configFilePath = "config.txt";

                // Überprüfen, ob die Datei existiert
                if (File.Exists(configFilePath))
                {
                    // Den gesamten Text aus der Datei lesen
                    string configText = File.ReadAllText(configFilePath);

                    // Laden von Runinfo
                    LoadTextFromConfig("Runinfo:", Runinfo, configText);

                    // Laden von Squadinfo
                    LoadTextFromConfig("Squadinfo:", Squadinfo, configText);

                    // Laden von Guild
                    LoadTextFromConfig("Guild:", Guild, configText);

                    // Laden von Welcome
                    LoadTextFromConfig("Welcome:", Welcome, configText);

                    // Laden von Symbols
                    LoadTextFromConfig("Symbols:", Symbols, configText);
                }
                else
                {
                    // Die Konfigurationsdatei existiert nicht
                    MessageBox.Show("Die Konfigurationsdatei 'config.txt' wurde nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Fehler beim Laden der Konfigurationsdatei
                MessageBox.Show($"Fehler beim Laden der Konfigurationsdatei: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTextFromConfig(string sectionHeader, TextBox textBox, string configText)
        {
            // Suchmuster für den Abschnitt und den eingeschlossenen Text in Anführungszeichen
            string pattern = $@"{sectionHeader}\s*""([^""]*)""";

            // Mit einem regulären Ausdruck nach dem Muster suchen
            var match = System.Text.RegularExpressions.Regex.Match(configText, pattern);

            // Überprüfen, ob ein Treffer gefunden wurde
            if (match.Success)
            {
                // Den extrahierten Text in das Textfeld einfügen
                textBox.Text = match.Groups[1].Value;
            }
            else
            {
                // Muster wurde nicht gefunden
                MessageBox.Show($"Das Muster '{sectionHeader}' wurde in der Konfigurationsdatei nicht gefunden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BringGw2ToFront()
        {
            try
            {
                // Specify the process name without the file extension
                string processName = "Gw2-64";

                // Get the processes by name
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    // Bring the first instance to the foreground
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

        // Constants for window handling
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
    }
}
