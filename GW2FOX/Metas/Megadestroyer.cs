using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace GW2FOX
{
    public partial class Megadestroyer : BaseForm
    {
        public Megadestroyer()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
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

        private void Megainfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Megainfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Megainstance_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Megainstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Attentionmega_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Attentionmega.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Megapre1_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Megapre1.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Megapre2_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Megapres2.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Megapre3_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Megapres3.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string homepageUrl = "https://wiki.guildwars2.com/wiki/Maws_of_Torment";
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