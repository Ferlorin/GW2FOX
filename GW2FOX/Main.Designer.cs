﻿namespace GW2FOX
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
            button4 = new Button();
            button3 = new Button();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button4
            // 
            button4.Location = new Point(12, 12);
            button4.Name = "button4";
            button4.Size = new Size(93, 23);
            button4.TabIndex = 5;
            button4.Text = "Homepage";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Fox_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 41);
            button3.Name = "button3";
            button3.Size = new Size(93, 23);
            button3.TabIndex = 8;
            button3.Text = "Repair Client";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Repair_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 70);
            button1.Name = "button1";
            button1.Size = new Size(93, 23);
            button1.TabIndex = 11;
            button1.Text = "Leading Tool";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Leading_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 146);
            button2.Name = "button2";
            button2.Size = new Size(93, 23);
            button2.TabIndex = 12;
            button2.Text = "Close";
            button2.UseVisualStyleBackColor = true;
            button2.Click += CloseAll_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(111, 70);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(47, 23);
            textBox1.TabIndex = 13;
            textBox1.Text = "ALT + T";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(167, 192);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button3);
            Controls.Add(button4);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GW2FOX";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button4;
        private Button button3;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
    }
}
