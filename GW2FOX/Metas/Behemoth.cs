using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Behemoth : BaseForm
    {
        public Behemoth()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
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
            Dispose();
        }

    }
}