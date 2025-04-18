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
    public partial class Tutorial : BaseForm
    {
        public Tutorial()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Main());
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Theshatterer2());
            this.Hide();
        }
    }
}
