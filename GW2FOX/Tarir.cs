using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Tarir : BaseForm
    {
        public Tarir()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        

        

        


        

        private void Beheinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Mawinfo TextBox to the clipboard
            Clipboard.SetText(Beheinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Beheinstance_Click(object sender, EventArgs e)
        {
            // Copy the text from Mawinfo TextBox to the clipboard
            Clipboard.SetText(Beheinstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Backtomain_Click(object sender, EventArgs e)
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
            Clipboard.SetText(Welcome.Text);

            BringGw2ToFront();
        }

        

        private void North_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Noth.Text);

            BringGw2ToFront();
        }

        private void East_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(East.Text);

            BringGw2ToFront();
        }

        private void West_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(West.Text);

            BringGw2ToFront();
        }

        private void Bombs_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Bombs.Text);

            BringGw2ToFront();
        }

        private void Spray_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Spray.Text);

            BringGw2ToFront();
        }

        private void Gliding_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Gliding.Text);

            BringGw2ToFront();
        }

        private void Shroom_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Shroom.Text);

            BringGw2ToFront();
        }

        private void Southside_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Southside.Text);

            BringGw2ToFront();
        }
    }
}