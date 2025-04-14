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
    public partial class OilFloes : BaseForm
    {
        public OilFloes()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        private void Runinfos_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
            BringGw2ToFront();
        }

        private void Squadinfos_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Squadinfo.Text);
            BringGw2ToFront();
        }

        private void Guilds_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void Welcomes_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinfo.Text);
            BringGw2ToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinstance.Text);
            BringGw2ToFront();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Attentionbehe.Text);
            BringGw2ToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }
    }
}
