﻿namespace GW2FOX
{
    public class BaseForm : Form
    {
        private Form previousPage;


        public static ListView? CustomBossList { get; private set; }

        public BaseForm()
        {

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
