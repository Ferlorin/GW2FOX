﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Junglewurm : BaseForm
    {
        public Junglewurm()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        

        

        


        

        private void Backtomain_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }

        private void Runinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Runinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Squadinfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Squadinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Guild_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Guild.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void WelcomeClick(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Welcome.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Junglewurminfo_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Junglewurminfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Junglewurminstance_Click(object sender, EventArgs e)
        {
            {
                // Copy the text from Leyline60 TextBox to the clipboard
                Clipboard.SetText(Junglewurminstance.Text);

                // Bring the Gw2-64.exe window to the foreground
                BringGw2ToFront();
            }
        }

        private void Attentionjunglewurm_Click(object sender, EventArgs e)
        {
            {
                // Copy the text from Leyline60 TextBox to the clipboard
                Clipboard.SetText(Attentionjunglewurm.Text);

                // Bring the Gw2-64.exe window to the foreground
                BringGw2ToFront();
            }
        }

        private void Junglewurminfos_Click(object sender, EventArgs e)
        {
            {
                // Copy the text from Leyline60 TextBox to the clipboard
                Clipboard.SetText(Junglewurminfos.Text);

                // Bring the Gw2-64.exe window to the foreground
                BringGw2ToFront();
            }
        }

        
    }
}