namespace GW2FOX
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            pictureBox1 = new PictureBox();
            button4 = new Button();
            button3 = new Button();
            button5 = new Button();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Header;
            pictureBox1.Location = new Point(42, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(700, 175);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button4
            // 
            button4.Location = new Point(284, 222);
            button4.Name = "button4";
            button4.Size = new Size(217, 23);
            button4.TabIndex = 5;
            button4.Text = "Homepage";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Fox_Click;
            // 
            // button3
            // 
            button3.Location = new Point(284, 251);
            button3.Name = "button3";
            button3.Size = new Size(217, 23);
            button3.TabIndex = 8;
            button3.Text = "Repair Client";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Repair_Click;
            // 
            // button5
            // 
            button5.Location = new Point(284, 309);
            button5.Name = "button5";
            button5.Size = new Size(217, 23);
            button5.TabIndex = 10;
            button5.Text = "Overlay Timer";
            button5.UseVisualStyleBackColor = true;
            button5.Click += Timer_Click;
            // 
            // button1
            // 
            button1.Location = new Point(284, 280);
            button1.Name = "button1";
            button1.Size = new Size(217, 23);
            button1.TabIndex = 11;
            button1.Text = "Leading Tool";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Leading_Click;
            // 
            // button2
            // 
            button2.Location = new Point(284, 356);
            button2.Name = "button2";
            button2.Size = new Size(217, 23);
            button2.TabIndex = 12;
            button2.Text = "Close";
            button2.UseVisualStyleBackColor = true;
            button2.Click += CloseAll_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(507, 310);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(47, 23);
            textBox1.TabIndex = 13;
            textBox1.Text = "ALT + T";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background;
            ClientSize = new Size(784, 391);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button5);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GW2FOX";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button4;
        private Button button3;
        private Button button5;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
    }
}
