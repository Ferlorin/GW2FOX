using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GW2FOX
{
    public partial class Overlay : Form
    {
        public static ListView CustomBossList { get; private set; }

        private Point mouse_offset;

        public Overlay(ListView listViewItems)
        {
            InitializeComponent();

            CustomBossList = listViewItems; // Setze die statische Eigenschaft

            // Konfiguriere das Overlay-Formular
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
            listViewPanel.BackColor = Color.Transparent;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            // Berechne die Größe des listViewPanel
            int panelWidth = (int)(this.Width * 0.98);
            int panelHeight = (int)(this.Height * 0.98);
            listViewPanel.Size = new Size(panelWidth, panelHeight);

            listViewPanel.Location = new Point(0, 0);

            // Erstelle die ListView
            ListView overlayListView = CustomBossList; // Verwende CustomBossList statt listViewItems
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

            // Füge die ListView zum ListView Panel hinzu
            listViewPanel.Controls.Add(overlayListView);
            this.Controls.Add(listViewPanel);
            this.SizeGripStyle = SizeGripStyle.Show;
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

        public void CloseOverlay()
        {
            this.Close();
        }
    }
}
