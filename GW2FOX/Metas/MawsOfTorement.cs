using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class MawsOfTorement : BaseForm
    {
        public MawsOfTorement()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        private void Runinfo_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
            BringGw2ToFront();
        }

        private void Squadinfo_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Squadinfo.Text);
            BringGw2ToFront();
        }

        private void Guild_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void WelcomeClick_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }

        private void Beheinfo_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinfo.Text);
            BringGw2ToFront();
        }

        private void Beheinstance_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinstance.Text);
            BringGw2ToFront();
        }

        private void Behemapinfo_Click_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Attentionbehe.Text);
            BringGw2ToFront();
        }

        private void WesternMawClick_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(WesternMaw.Text);
            BringGw2ToFront();
        }

        private void NorthernMawClick_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(NorthernMaw.Text);
            BringGw2ToFront();
        }

        private void EasternMawClick_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(EasternMaw.Text);
            BringGw2ToFront();
        }

        private void Back_Click_1(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }
    }
}
