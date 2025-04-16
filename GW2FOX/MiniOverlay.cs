using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class MiniOverlay : BaseForm
    {
        private Worldbosses _worldbossesForm;

        public MiniOverlay(Worldbosses worldbosses)
        {
            InitializeComponent();
            this.TopMost = true;
            this.Load += MiniOverlay_Load;
            _worldbossesForm = worldbosses;
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
            // Nur einblenden, wenn es ausgeblendet ist
            if (_worldbossesForm != null && !_worldbossesForm.IsDisposed)
            {
                if (_worldbossesForm.Visible)
                {
                    _worldbossesForm.Hide(); // Ausblenden
                }
                else
                {
                    _worldbossesForm.Show(); // Einblenden
                    _worldbossesForm.BringToFront(); // In den Vordergrund holen
                    _worldbossesForm.Focus(); // Fokus setzen
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, "data2", "Blish HUD.exe");

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string filePath = Path.Combine(exeDirectory, "data", "GW2TacO.exe");

            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

