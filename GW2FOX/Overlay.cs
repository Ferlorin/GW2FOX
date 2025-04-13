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

            ListView overlayListView = CustomBossList;
            overlayListView.ForeColor = Color.Black;

            // Konfiguriere das Overlay-Formular
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            TopMost = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Opacity = 100;
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
            overlayListView.Font = new Font("Segoe UI", 12, FontStyle.Bold);
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
            e.DrawBackground();

            string text = e.Item.Text;
            Font font = e.Item.Font;
            Point textLocation = new Point(e.Bounds.Left + 4, e.Bounds.Top + 4);

            // Erzeuge dickeren schwarzen Rand durch mehrfach versetztes Zeichnen
            for (int dx = -4; dx <= 4; dx++)
            {
                for (int dy = -4; dy <= 4; dy++)
                {
                    // Nur äußere Punkte zeichnen, um den Rand dicker zu machen
                    if (Math.Abs(dx) + Math.Abs(dy) > 1)
                    {
                        Point offsetLocation = new Point(textLocation.X + dx, textLocation.Y + dy);
                        TextRenderer.DrawText(e.Graphics, text, font, offsetLocation, Color.Black, TextFormatFlags.Default);
                    }
                }
            }

            // Haupttext (Vordergrund)
            TextRenderer.DrawText(e.Graphics, text, font, textLocation, Color.White, TextFormatFlags.Default);
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
