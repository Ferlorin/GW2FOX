using System;
using System.Windows.Forms;

namespace GW2FOX
{
    internal static class Program
    {
        private static Form mainForm = new Main(); // Initialize with an instance of Main

        [STAThread]
        static void Main()
        {
            // Initialisiere WPF-Application, falls noch nicht vorhanden
            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application();
            }

            // Starten der WinForms-Anwendung
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm); // Use the initialized mainForm
        }
    }
}
