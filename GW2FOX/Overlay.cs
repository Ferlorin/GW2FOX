using System;
using System.Drawing;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class Overlay : Form
    {
        private Point mouse_offset;
        private ListView listViewItems;

        public Overlay(ListView listViewItems)
        {
            InitializeComponent();

            // Configure the Overlay Form
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Opacity = 0.7;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.Width = 255;
            this.Height = 327;

            Panel listViewPanel = new Panel();
            listViewPanel.BackColor = Color.Transparent; // Mache das Panel transparent
            FormBorderStyle = FormBorderStyle.Fixed3D;
            // Calculate the size of listViewPanel to be 2% smaller than the overlay
            int panelWidth = (int)(this.Width);
            int panelHeight = (int)(this.Height);
            listViewPanel.Size = new Size(panelWidth, panelHeight);

            listViewPanel.Location = new Point(0, 0);

            // Create the ListView
            ListView overlayListView = listViewItems;
            overlayListView.ForeColor = Color.Black;
            overlayListView.Font = new Font("Segoe UI", 10, FontStyle.Bold | FontStyle.Regular);
            overlayListView.BackColor = this.BackColor;
            overlayListView.View = View.Details;
            overlayListView.Location = new Point(0, 0);
            overlayListView.Width = listViewPanel.Width;
            overlayListView.Height = listViewPanel.Height;
            overlayListView.Scrollable = false;
            overlayListView.Enabled = true;
            overlayListView.ItemSelectionChanged += (sender, e) =>
            {
                overlayListView.SelectedIndices.Clear();
            };

            // Add the ListView to the ListView Panel
            listViewPanel.Controls.Add(overlayListView);
            this.Controls.Add(listViewPanel);
            this.SizeGripStyle = SizeGripStyle.Show;
            // Set up event handlers for moving the form
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }
    }
}
