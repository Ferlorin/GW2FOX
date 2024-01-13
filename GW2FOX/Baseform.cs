namespace GW2FOX
{
    public class BaseForm : Form
    {
        private Form previousPage;
        protected Overlay overlay;
        protected ListView customBossList;
        protected BossTimer bossTimer;

        public static ListView? CustomBossList { get; private set; }

        public BaseForm()
        {
            InitializeCustomBossList(); // Initialize customBossList before using it
            overlay = new Overlay(customBossList);
            bossTimer = new BossTimer(customBossList);
        }
        protected void InitializeBossTimerAndOverlay()
        {
            // Hier solltest du entscheiden, ob overlay erneut erstellt werden soll
            // overlay = new Overlay(customBossList);
            bossTimer = new BossTimer(customBossList);
            overlay.WindowState = FormWindowState.Normal;
        }

        protected void InitializeCustomBossList()
        {
            customBossList = new ListView();
            customBossList.View = View.Details;
            customBossList.Columns.Add("Boss Name", 145);
            customBossList.Columns.Add("Time", 78);
            customBossList.Location = new Point(0, 0);
            customBossList.ForeColor = Color.White;
            customBossList.Font = new Font("Segoe UI", 10, FontStyle.Bold);

        }

        // Füge dies zu deiner BaseForm-Klasse hinzu
        public void UpdateCustomBossList(ListView updatedList)
        {
            CustomBossList = updatedList;
        }

        protected void Timer_Click(object sender, EventArgs e)
        {
            if (CustomBossList == null || CustomBossList.IsDisposed)
            {
                InitializeCustomBossList();
            }

            if (overlay == null || overlay.IsDisposed)
            {
                InitializeBossTimerAndOverlay();
            }

            bossTimer.Start();
            overlay.Show();
        }

        protected void ShowAndHideForm(Form newForm)
        {
            previousPage = this;

            newForm.Show();
            this.Hide();
        }



        // Method to navigate back to the previous page
        protected void NavigateBack()

        {
            if (previousPage != null)
            {
                previousPage.Show();
                this.Hide();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            SuspendLayout();
            // 
            // BaseForm
            // 
            BackgroundImage = Properties.Resources.Background;
            ClientSize = new Size(1904, 1041);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BaseForm";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        protected void AdjustWindowSize()
        {
            // Dein bestehender Code zur Anpassung der Fenstergröße
            Screen currentScreen = Screen.FromControl(this);
            Rectangle workingArea = currentScreen.WorkingArea;

            if (this.Width > workingArea.Width || this.Height > workingArea.Height)
            {
                this.Size = new Size(
                    Math.Min(this.Width, workingArea.Width),
                    Math.Min(this.Height, workingArea.Height)
                );

                this.Location = new Point(
                    workingArea.Left + (workingArea.Width - this.Width) / 2,
                    workingArea.Top + (workingArea.Height - this.Height) / 2
                );
            }
            if (this.Height > workingArea.Height)
            {
                this.Height = workingArea.Height;
            }


            if (this.Bottom > workingArea.Bottom)
            {
                this.Location = new Point(
                    this.Left,
                    Math.Max(workingArea.Top, workingArea.Bottom - this.Height)
                );
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);


            Application.Exit();
        }
    }
}
