using static GW2FOX.BossTimings;

namespace GW2FOX
{
    public partial class Overlay : Form
    {
 
        private static readonly Color DefaultFontColor = Color.White;
        private static readonly Color PastBossFontColor = Color.OrangeRed;
        private static readonly Color MyAlmostBlackColor = Color.FromArgb(255, 1, 1, 1);
        private ListView overlayListView;



        private Panel listViewPanel;
        public Overlay(ListView listViewItems)
        {
            InitializeComponent();

            overlayListView = listViewItems;
            overlayListView.DrawItem += OverlayListView_SetColor;

            listViewPanel = new Panel();
            listViewPanel.Size = new Size(listViewPanel.Width, 21 * overlayListView.Items.Count);

            // Konfiguriere das Overlay-Formular
            BackColor = MyAlmostBlackColor;
            TransparencyKey = MyAlmostBlackColor;
            TopMost = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Opacity = 1;

            Width = 240;
            Height = 310;
            AutoScroll = true;

            listViewPanel.BackColor = Color.Transparent;

            // Berechne die Größe des listViewPanel
            int panelWidth = Width - 10;
            int panelHeight = (int)(Height * 10);
            listViewPanel.Size = new Size(panelWidth, panelHeight);

            listViewPanel.Location = new Point(0, 0);

            // Erstelle die ListView
           
            overlayListView.BackColor = BackColor;

            // Setze die View auf Details, um die horizontale Scrollleiste zu deaktivieren
            overlayListView.View = View.Details;
            
            // Entferne die Spaltenüberschriften, um die horizontale Scrollleiste zu verbergen
            overlayListView.HeaderStyle = ColumnHeaderStyle.None;

            overlayListView.OwnerDraw = true;
            overlayListView.Location = new Point(0, 0);
            overlayListView.Width = listViewPanel.Width;
            overlayListView.Visible = true;
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


        private void OverlayListView_SetColor(object sender, DrawListViewItemEventArgs e)
        {
            listViewPanel.Size = new Size(listViewPanel.Width, 21 * overlayListView.Items.Count);
            //this was not needed we have already assigned color in BossTimerService.cs line 312 and 318

        }

        public void CloseOverlay()
        {
            Dispose();
        }

        private void Overlay_Load(object sender, EventArgs e)
        {

        }
    }
}