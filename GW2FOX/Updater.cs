using System;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
// Please check bottom lines because the logic needs to change.
// You are never going to be able to update the application you are already running
public class Updater
{
    private const string RepositoryUrl = "https://api.github.com/repos/catnoid/gw2fox/releases/latest";
    private const string UpdateFolder = "Update";
    private static string UpdateFileName;
    

    public static void CheckForUpdates(string currentVersion)
    {
        string latestVersion = GetLatestVersionFromGitHub();

        if (latestVersion.CompareTo(currentVersion) > 0)
        {
            Console.WriteLine("Ein Update ist verfügbar.");

            if (DownloadUpdate())
            {
                Console.WriteLine("Update erfolgreich heruntergeladen.");
                InstallUpdate();
            }
            else
            {
                Console.WriteLine("Fehler beim Herunterladen des Updates.");
            }
        }
        else
        {
            Console.WriteLine("Die Anwendung ist auf dem neuesten Stand.");
        }
    }

    private static string GetLatestVersionFromGitHub()
    {
        using (WebClient client = new WebClient())
        {
            client.Headers.Add("User-Agent", "request");
            string json = client.DownloadString(RepositoryUrl);

            dynamic releaseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string latestVersion = releaseInfo.tag_name;

            return latestVersion;
        }
    }

    private static bool DownloadUpdate()
    {
        try
        {
            Directory.CreateDirectory(UpdateFolder);

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                string json = client.DownloadString(RepositoryUrl);
                dynamic releaseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                // Getting the download URL
                string downloadUrl = releaseInfo.assets[0].browser_download_url;

                // Getting the name of the zip file from the 'assets' field in the JSON file
                string fileNameFromJson = releaseInfo.assets[0].name;
                UpdateFileName = fileNameFromJson;

                // Use the file name from the JSON as the name of your downloaded file
                string updateFilePath = Path.Combine(UpdateFolder, fileNameFromJson);

                client.DownloadFile(downloadUrl, updateFilePath);

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading the update: {ex.Message}");
            return false;
        }
    }

    private static void InstallUpdate()
    {
        try
        {
            string updateFilePath = Path.Combine(UpdateFolder, UpdateFileName);

            // Hier implementierst du den Code zum Entpacken des Updates und zur Aktualisierung der Anwendung.
            // Je nach Update-Mechanismus kann dies variieren.

            // Beispiel (vereinfacht):
            ZipFile.ExtractToDirectory(updateFilePath, UpdateFolder);

            Console.WriteLine("Update successfully installed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error installing the update: {ex.Message}");
        }
    }
}


// Creating an updater application is specific to your application's requirements and execution environment. A generic approach for a .NET application can be as follows:
// Create a separate updater application: This is a standalone application responsible for updating your main application.
//     In the main application, check for updates: When your main application starts, or at regular intervals, check for updates from your repository.
//     Download the update: If an update is available, download it to a specific location and save the name of the update file.
//     Start the updater application: Once the update is downloaded, start the updater application by passing the path of the update file and the location of the application to be updated as arguments.
//     In the updater application, apply the update: The updater application should wait for the main application to completely exit (as it may still be closing), extract and install the update, and then restart the main application.
//     Handle failed updates: If the update process fails, the updater can roll back to the previous version of the application or notify the user about the issue.
//     Logging: Keep a detailed log of all the operations performed by the updater for debugging purposes.
//     This is a very broad approach to an updater application, emphasizing that an updater is a separate, standalone application. Therefore, you would have two projects; one for your main application, and another for your updater.
//     Keep in mind that writing an updater could be a challenging task based on what your application's requirements are. Always remember to account for situations where things can go wrong, such as lost internet connectivity, corrupted download files, interrupted installations, etc.
//     Here's an example of how the code would look like:
// In the main application, you have:
// if (IsUpdateAvailable())
// {
//     string updateFileName = DownloadUpdate();
//
//     Process updaterProcess = new Process();
//     updaterProcess.StartInfo.FileName = "path_to_updater_executable";
//     updaterProcess.StartInfo.Arguments = $"\"{UpdateFolder}\\{updateFileName}\" \"{Application.StartupPath}\"";
//     updaterProcess.Start();
//
//     Application.Exit();
// }
// And in the updater application:
// public static void Main(string[] args)
// {
//     string updateFile = args[0];
//     string applicationPath = args[1];
//
//     // Wait for the main application to fully close
//     Thread.Sleep(3000);
//
//     // Extract the update package to the main application directory
//     ZipFile.ExtractToDirectory(updateFile, applicationPath);
//
//     // Restart the main application
//     Process.Start(Path.Combine(applicationPath, "main_app_executable_name"));
//
//     Environment.Exit(0);
// }
// In this example, "main_app_executable_name" is the name of your main application's executable file. The paths to the updater executable and the names of the update files would be defined by your specific implementation.
//     Remember to add more logic to handle errors and edge cases according to your needs. This code is a simplified version and lacks handling for issues like failed updates, incomplete downloads, incorrect path information, etc., which should be addressed in a