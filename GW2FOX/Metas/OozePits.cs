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
    public partial class OozePits : BaseForm
    {
        public OozePits()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
