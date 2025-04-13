using System;
using System.Drawing;
using System.Reflection;
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

            // Aktiviert Double Buffering für das Overlay-Form
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            if (Owner is BaseForm baseForm)
            {
                baseForm.UpdateCustomBossList(listViewItems);
            }

            CustomBossList = listViewItems;

            // Aktiviert Double Buffering auf der ListView über Reflection
            typeof(ListView).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, CustomBossList, new object[] { true });

            // Konfiguriere das Overlay-Fenster
            BackColor = Color.FromArgb(5, 5, 5);
            TransparencyKey = Color.FromArgb(5, 5, 5);
            TopMost = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Opacity = 1; // 1 = 100%
            Width = 235;
            Height = 310;
            AutoScroll = true;

            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;

            // Panel für ListView
            Panel listViewPanel = new Panel
            {
                BackColor = Color.Transparent,
                Size = new Size(Width, Height * 10),
                Location = new Point(0, 0)
            };
            FormBorderStyle = FormBorderStyle.Fixed3D;

            // Konfiguriere die übergebene ListView
            ListView overlayListView = CustomBossList;
            overlayListView.OwnerDraw = true;
            overlayListView.DrawItem += OverlayListView_DrawItem;
            overlayListView.ForeColor = Color.Black;
            overlayListView.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            overlayListView.BackColor = BackColor;
            overlayListView.View = View.Details;
            overlayListView.Scrollable = true;
            overlayListView.Location = new Point(0, 0);
            overlayListView.Width = listViewPanel.Width;
            overlayListView.Height = listViewPanel.Height - SystemInformation.HorizontalScrollBarHeight;
            overlayListView.Enabled = true;
            overlayListView.ItemSelectionChanged += (sender, e) =>
            {
                overlayListView.SelectedIndices.Clear();
            };

            listViewPanel.Controls.Add(overlayListView);
            Controls.Add(listViewPanel);
        }

        private void OverlayListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();

            string text = e.Item.Text;
            Font font = e.Item.Font;
            Point textLocation = new Point(e.Bounds.Left + 1, e.Bounds.Top + 1);

            // Erzeuge dickeren schwarzen Rand durch mehrfach versetztes Zeichnen
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
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
