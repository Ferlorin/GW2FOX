namespace GW2FOX
{
    internal static class Program
    {
        private static Main mainForm;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            mainForm = new Main();
            Application.Run(mainForm);
        }

    }
}
