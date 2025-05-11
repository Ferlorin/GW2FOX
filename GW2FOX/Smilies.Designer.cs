namespace GW2FOX
{
    partial class Smilies
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Symbols = new TextBox();
            SuspendLayout();
            // 
            // Symbols
            // 
            Symbols.Cursor = Cursors.IBeam;
            Symbols.Font = new Font("Segoe UI", 11F);
            Symbols.Location = new Point(12, 12);
            Symbols.Multiline = true;
            Symbols.Name = "Symbols";
            Symbols.ScrollBars = ScrollBars.Vertical;
            Symbols.Size = new Size(213, 187);
            Symbols.TabIndex = 239;
            // 
            // Smilies
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Magenta;
            ClientSize = new Size(237, 208);
            Controls.Add(Symbols);
            Name = "Smilies";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Smilies";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Symbols;
    }
}