using System;
using System.Windows.Threading;

namespace GW2FOX
{
    internal static class Program
    {
        private static System.Windows.Forms.Form mainForm;

        [STAThread]
        static void Main()
        {

            mainForm = new Main();

            // WinForms Anwendung starten
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}
