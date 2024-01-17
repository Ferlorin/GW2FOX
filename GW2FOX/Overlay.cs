namespace GW2FOX
{
    public partial class Overlay : Form
    {
        public static ListView CustomBossList { get; private set; }

        private Point mouse_offset;


        public Overlay(ListView listViewItems)
        {
            InitializeComponent();
            if (Owner is BaseForm baseForm)
            {
                baseForm.UpdateCustomBossList(listViewItems);
            };
            CustomBossList = listViewItems;

            ListView overlayListView = CustomBossList; // Behalte nur diese Zeile
            overlayListView.ForeColor = Color.Black;

            // Konfiguriere das Overlay-Formular
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            TopMost = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Opacity = 0.7;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            Width = 235;
            Height = 310;
            AutoScroll = true;


            Panel listViewPanel = new Panel();
            listViewPanel.BackColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            // Berechne die Größe des listViewPanel
            int panelWidth = (int)(Width);
            int panelHeight = (int)(Height * 10);
            listViewPanel.Size = new Size(panelWidth, panelHeight);

            listViewPanel.Location = new Point(0, 0);

            // Erstelle die ListView

            overlayListView.ForeColor = Color.Black;
            overlayListView.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            overlayListView.BackColor = BackColor;
            overlayListView.View = View.Details;
            overlayListView.Location = new Point(0, 0);
            overlayListView.Width = listViewPanel.Width;

            // Enable vertical scrollbar
            overlayListView.Scrollable = true;

            // Set the height considering the horizontal scrollbar
            overlayListView.Height = listViewPanel.Height - SystemInformation.HorizontalScrollBarHeight;

            overlayListView.Enabled = true;
            overlayListView.ItemSelectionChanged += (sender, e) =>
            {
                overlayListView.SelectedIndices.Clear();
            };

            // Füge die ListView zum ListView Panel hinzu
            listViewPanel.Controls.Add(overlayListView);
            Controls.Add(listViewPanel);

        }

        private void OverlayListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Zeichne den Hintergrund
            e.DrawBackground();

            // Definiere den Text und die Schriftart
            string text = e.Item.Text;
            Font font = e.Item.Font;

            // Definiere die Position des Textes mit schwarzer Umrandung
            Point textLocation = new Point(e.Bounds.Left + 2, e.Bounds.Top + 2);

            // Zeichne den Text mit dickerem schwarzen Rand
            TextRenderer.DrawText(e.Graphics, text, font, textLocation, Color.Black, Color.Transparent, TextFormatFlags.Default);

            // Zeichne den Text ohne Umrandung (darüber, um die Umrandung zu überlagern)
            TextRenderer.DrawText(e.Graphics, text, font, textLocation, Color.White, Color.Transparent, TextFormatFlags.Default);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }

        public void CloseOverlay()
        {
            Dispose();
        }

    }
}
