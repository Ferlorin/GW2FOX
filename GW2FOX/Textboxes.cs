using static GW2FOX.BossTimerService;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    public partial class Textboxes : BaseForm
    {
        public static System.Windows.Controls.ListView? CustomBossList { get; private set; }
        public static Dictionary<string, CheckBox> bossCheckBoxMap;
        public static readonly char[] Separator = { ',' };
        public static List<string> BossList23 { get; set; } = new();


        public Textboxes()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Guild, Welcome, Symbols);
        }

        protected void LoadConfigText(
   System.Windows.Forms.TextBox runinfoBox,
   System.Windows.Forms.TextBox guildBox,
   System.Windows.Forms.TextBox welcomeBox,
   System.Windows.Forms.TextBox symbolsBox)
        {

            var config = BossTimings.LoadedConfigInfos;

            if (config == null)
            {
                return;
            }

            runinfoBox.Text = (config.Runinfo ?? "").Replace("\n", Environment.NewLine);
            guildBox.Text = (config.Guild ?? "").Replace("\n", Environment.NewLine);
            welcomeBox.Text = (config.Welcome ?? "").Replace("\n", Environment.NewLine);
            symbolsBox.Text = (config.Symbols ?? "").Replace("\n", Environment.NewLine);
            Console.WriteLine("LoadedConfigInfos: " + (config != null ? "OK" : "NULL"));

        }

        public void button67_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(Quantity.Text, out int numberOfBosses) && numberOfBosses > 0)
                {
                    List<string> bossNamesFromConfig = BossList23;

                    var bossEventGroups = BossEventGroups
                        .Where(bossEventGroup => bossNamesFromConfig.Contains(bossEventGroup.BossName))
                        .ToList();

                    // Alle Runs sammeln
                    var allBosses = bossEventGroups
                        .SelectMany(bossEventGroup => bossEventGroup.GetNextRuns())
                        .Where(run => run.TimeToShow > GlobalVariables.CURRENT_DATE_TIME) // Nur zukünftige Runs!
                        .OrderBy(run => run.TimeToShow)
                        .ToList();

                    var bossInfo = allBosses
                        .Take(numberOfBosses)
                        .Select(bossEvent => $"{bossEvent.BossName} - {bossEvent.Waypoint}")
                        .ToList();

                    string bossNamesString = "Upcoming Meta:" + Environment.NewLine + string.Join(Environment.NewLine, bossInfo);

                    Clipboard.SetText(bossNamesString);
                    SearchResults.Text = bossNamesString;
                }
                else
                {
                    MessageBox.Show("A Number please!.", "Do you know the meaning of a NUMBER, try 10!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button66_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SearchResults.Text);
            BringGw2ToFront();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Guild.Text, "Guild");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Welcome.Text, "Welcome");
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
            BringGw2ToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Runinfo.Text, "Runinfo");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Symbols.Text, "Symbols");
        }
    }
}
