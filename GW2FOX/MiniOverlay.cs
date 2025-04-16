using System;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class MiniOverlay : BaseForm
    {
        private Worldbosses worldbossesForm;

        public MiniOverlay()
        {
            InitializeComponent();
            this.TopMost = true;
            this.Load += MiniOverlay_Load;
        }

        private void MiniOverlay_Load(object sender, EventArgs e)
        {
            // Position: Oben mittig auf dem Bildschirm
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (screen.Width - this.Width) / 2,
                0 // Oben am Bildschirm
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verhindert mehrfaches Öffnen von Worldbosses
            if (worldbossesForm == null || worldbossesForm.IsDisposed)
            {
                worldbossesForm = new Worldbosses();
                worldbossesForm.Show();
            }
            else
            {
                worldbossesForm.BringToFront();
                worldbossesForm.Focus();
            }
        }
    }
}
