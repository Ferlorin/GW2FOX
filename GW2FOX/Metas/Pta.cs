﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Pta : BaseForm
    {
        public Pta()
        {
            InitializeComponent();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Dispose();
        }

        private void Beheinstance_Click(object sender, EventArgs e)
        {
            // Copy the text from Mawinfo TextBox to the clipboard
            Clipboard.SetText(Beheinstance.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
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

        private void Welcome_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Welcome.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }



        private void Beheinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Beheinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Mapinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Mapinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }
    }
}