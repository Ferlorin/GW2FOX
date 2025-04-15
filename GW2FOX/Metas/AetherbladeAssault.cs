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
    public partial class AetherbladeAssault : BaseForm
    {
        public AetherbladeAssault()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }

        private void Behepres_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
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

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Squadinfo.Text);
            BringGw2ToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }
    }
}
