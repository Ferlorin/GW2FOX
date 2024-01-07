using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Worldbosses : BaseForm
    {
        private string filePath = "config.txt";

        public Worldbosses()
        {
            InitializeComponent();
            Load += Worldbosses_Load_1;
            SetCheckBoxesFromConfig();

        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {
            // Initialisierungscode
            WindowState = FormWindowState.Maximized;
        }

        // Hinzufügen einer gemeinsamen Methode zum Instanziieren und Anzeigen von Formen

        private void SaveTextToFile(string textToSave, string sectionHeader)
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(filePath);

                // Index der Zeile mit dem angegebenen Header finden
                int headerIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(sectionHeader + ":"))
                    {
                        headerIndex = i;
                        break;
                    }
                }

                // Wenn der Header gefunden wird, den Text aktualisieren
                if (headerIndex != -1)
                {
                    lines[headerIndex] = $"{sectionHeader}: \"{textToSave}\"";
                }
                else
                {
                    // Wenn der Header nicht gefunden wird, eine neue Zeile hinzufügen
                    lines = lines.Concat(new[] { $"{sectionHeader}: \"{textToSave}\"" }).ToArray();
                }

                // Aktualisierten Inhalt zurück in die Datei schreiben
                File.WriteAllLines(filePath, lines);

                MessageBox.Show($"{sectionHeader} saved.", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {sectionHeader}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BringGw2ToFront()
        {
            try
            {
                // Specify the process name without the file extension
                string processName = "Gw2-64";

                // Get the processes by name
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    // Bring the first instance to the foreground
                    IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                    ShowWindow(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                }
                else
                {
                    MessageBox.Show("Gw2-64.exe is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error bringing Gw2-64.exe to the foreground: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Constants for window handling
        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void Saverun_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Runinfo.Text, "Runinfo");
        }

        private void Info_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Squadinfo.Text, "Squadinfo");
        }

        private void Guild_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Guild.Text, "Guild");
        }

        private void Welcome_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Welcome.Text, "Welcome");
        }

        private void Symbols_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Symbols.Text, "Symbols");
        }

        private void Behe_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Behemoth());
        }

        private void Fireelemental_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Fireelemental());
        }

        private void Junglewurm_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Junglewurm());
        }

        private void Rhendak_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Rhendak());
        }

        private void Ulgoth_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Ulgoth());
        }

        private void Thaida_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Thaida());
        }

        private void Fireshaman_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Fireshaman());
        }

        private void Megadestroyer_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Megadestroyer());
        }

        private void Karkaqueen_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Karkaqueen());
        }

        private void Dragonstorm_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Dragonstorm());
        }

        private void Drakkar_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Drakkar());
        }

        private void Eye_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Eye());
        }

        private void Dwayna_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Dwayna());
        }

        private void Ogrewars_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Ogrewars());
        }

        private void Tequatl_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Tequatl());
        }

        private void TheShatterer_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Theshatterer());
        }

        private void Inquestgolem_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Inquestgolem());
        }

        private void Clawjormag_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Clawofjormag());
        }

        private void Lyssa_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Lyssa());
        }

        private void Maw_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Maw());
        }

        private void Concert_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Metalconcert());
        }

        private void Runinfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Squadinfos_Click(object sender, EventArgs e)
        {
            // Copy the text from Leyline60 TextBox to the clipboard
            Clipboard.SetText(Squadinfo.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Guildcopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Welcomecopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);

            // Bring the Gw2-64.exe window to the foreground
            BringGw2ToFront();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Main());
        }

        private void Chak_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Chak());
        }

        private void Tarir_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Tarir());
        }

        private void Massen_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Spellmaster());
        }

        private void DBS_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Dbs());
        }

        private void Junundo_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Juundu());
        }

        private void PTA_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Pta());
        }

        private void Doppel_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Doppelganger());
        }

        private void Forged_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Forgedwithfire());
        }

        private void Pinata_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Pinatas());
        }

        private void Savesquadmessage_Click(object sender, EventArgs e)
        {
            SaveTextToFile(Squadinfo.Text, "Squadinfo");
        }

        private void SerpentsIre_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new SerpentsIre());
        }

        private void DragonsStand_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new DragonStand());
        }

        private void Palawadan_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Palawadan());
        }

        private void ThunderheadKeep_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new ThunderheadPeaks());
        }

        private void RemoveBossNameFromConfig(string bossName)
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(filePath);

                // Index der Zeile mit dem Bossnamen finden
                int bossIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Bosses:"))
                    {
                        bossIndex = i; // Die aktuelle Zeile enthält den Namen
                        break;
                    }
                }

                // Wenn der Bossname gefunden wird, die Bosses-Liste aktualisieren und in die Datei schreiben
                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    // Entferne die äußeren Anführungszeichen und teile die Bosse
                    List<string> bossNames = bossLine
                        .Trim('"')  // Entferne äußere Anführungszeichen
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())
                        .ToList();  // Konvertiere in eine Liste

                    // Entferne den zu löschenden Boss
                    bossNames.Remove(bossName);

                    // Erstelle eine neue Zeile für die Bosse
                    string newBossLine = $"Bosses: \"{string.Join(", ", bossNames)}\"";

                    // Aktualisierten Inhalt zurück in die Datei schreiben
                    lines[bossIndex] = newBossLine;
                    File.WriteAllLines(filePath, lines);

                    // Setze das Häkchen im CheckBox-Control
                    CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                    if (bossCheckBox != null)
                    {
                        bossCheckBox.Checked = false;
                        bossCheckBox.Invalidate(); // Füge diese Zeile hinzu
                    }


                }
                else
                {
                    // Wenn der Abschnitt "Bosses:" nicht gefunden wird, gibt es nichts zu entfernen
                    MessageBox.Show($"Bosses section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private bool userTriggeredSave = false;

        private void SaveBossNameToConfig(string bossName)
        {
            try
            {
                // Nur fortfahren, wenn die Aktion vom Benutzer ausgelöst wurde
                if (!userTriggeredSave)
                {
                    return;
                }

                // Setze den Trigger zurück, um sicherzustellen, dass zukünftige Aufrufe ignoriert werden
                userTriggeredSave = false;

                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(filePath);

                // Index der Zeile mit dem Bossnamen finden
                int bossIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Bosses:"))
                    {
                        bossIndex = i; // Die aktuelle Zeile enthält den Namen
                        break;
                    }
                }

                // Wenn der Bossname gefunden wird, ihn hinzufügen
                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    // Entferne die äußeren Anführungszeichen und teile die Bosse
                    string[] bossNames = bossLine
                        .Trim('"')  // Entferne äußere Anführungszeichen
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())  // Entferne führende und abschließende Leerzeichen
                        .ToArray();

                    // Überprüfen, ob der Name bereits vorhanden ist
                    if (!bossNames.Contains(bossName))
                    {
                        // Füge den Bossnamen hinzu
                        lines[bossIndex] = $"Bosses: \"{string.Join(", ", bossNames.Concat(new[] { bossName }).ToArray())}\"".Trim();

                        // Aktualisierten Inhalt zurück in die Datei schreiben
                        File.WriteAllLines(filePath, lines);

                        // Setze das Häkchen im CheckBox-Control
                        CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                        if (bossCheckBox != null)
                        {
                            bossCheckBox.Checked = true;
                        }

                        // Keine Meldung anzeigen
                    }
                }
                else
                {
                    // Wenn der Abschnitt "Bosses:" nicht gefunden wird, gibt es nichts zu hinzufügen
                    MessageBox.Show($"Bosses section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void SetCheckBoxesFromConfig()
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = File.ReadAllLines(filePath);

                // Index der Zeile mit dem Bossnamen finden
                int bossIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Bosses:"))
                    {
                        bossIndex = i; // Die aktuelle Zeile enthält den Namen
                        break;
                    }
                }

                // Wenn der Bossname gefunden wird, setze die CheckBox-Stati
                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    // Extrahiere die Bosse aus der Zeile zwischen den Anführungszeichen
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    // Entferne die äußeren Anführungszeichen und teile die Bosse
                    string[] bossNames = bossLine
                        .Trim('"')  // Entferne äußere Anführungszeichen
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())  // Entferne führende und abschließende Leerzeichen
                        .ToArray();

                    // Für jeden Bossnamen prüfen und CheckBox-Stati setzen
                    foreach (string bossName in bossNames)
                    {
                        CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                        if (bossCheckBox != null)
                        {
                            bossCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hier kann eine Fehlermeldung protokolliert oder geloggt werden, wenn gewünscht
            }
        }


        private void Maw_CheckedChanged(object sender, EventArgs e)
        {
            // Der Name des Bosses
            string bossName = "The Frozen Maw";

            // Setze den Trigger, um anzuzeigen, dass die Aktion vom Benutzer ausgelöst wurde
            userTriggeredSave = true;

            if (Maw.Checked)
            {
                // Wenn das Kontrollkästchen ausgewählt ist
                SaveBossNameToConfig(bossName);
            }
            else
            {
                // Wenn das Kontrollkästchen nicht ausgewählt ist
                RemoveBossNameFromConfig(bossName);
            }

            userTriggeredSave = false;
        }


        private void Behemoth_CheckedChanged(object sender, EventArgs e)
        {
            // Der Name des Bosses
            string bossName = "Shadow Behemoth";

            userTriggeredSave = true;

            if (Behemoth.Checked)
            {
                // Wenn das Kontrollkästchen ausgewählt ist
                SaveBossNameToConfig(bossName);
            }
            else
            {
                // Wenn das Kontrollkästchen nicht ausgewählt ist
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Fire_Elemental_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Fire Elemental";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void JungleWurm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Great Jungle Wurm";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Ulgoth_CheckedChanged(object sender, EventArgs e)
        {
            {
                string bossName = "Ulgoth the Modniir";

                userTriggeredSave = true;

                if (Fire_Elemental.Checked)
                {
                    SaveBossNameToConfig(bossName);
                }
                else
                {
                    RemoveBossNameFromConfig(bossName);
                }
                userTriggeredSave = false;
            }
        }

        private void Thaida_CheckedChanged(object sender, EventArgs e)
        {
            {
                string bossName = "Taidha Covington";

                userTriggeredSave = true;

                if (Fire_Elemental.Checked)
                {
                    SaveBossNameToConfig(bossName);
                }
                else
                {
                    RemoveBossNameFromConfig(bossName);
                }
                userTriggeredSave = false;
            }
        }

        private void Megadestroyer_CheckedChanged(object sender, EventArgs e)

        {
            string bossName = "Megadestroyer";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void MarkTwo_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Inquest Golem Mark II";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Tequatl_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Tequatl the Sunless";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Shatterer_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "The Shatterer";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Karka_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Karka Queen";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Claw_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Claw of Jormag";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Chak_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Chak Gerent";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }



        private void Tarir_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Battle in Tarir";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Mascen_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Spellmaster Macsen";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void DS_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Dragon's Stand";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void DBS_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Death-Branded Shatterer";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Junundu_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Junundu Rising";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void PTA_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Path to Ascension";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Doppelganger_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Doppelganger";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Doggies_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Forged with Fire";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }



        private void Pinata_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Choya Piñata";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void SerpentsIre_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Serpents' Ire";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Palawadan_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Palawadan";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void ThunderheadKeep_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Thunderhead Keep";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void MawsOfTorment_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Maws of Torment";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Oil_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "The Oil Floes";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Drakkar_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Drakkar";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Metalconcert_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Metal Concert";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Dragonstorm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Dragonstorm";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void OozePits_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Ooze Pits";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Effigy_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Effigy";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Doomlore_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Doomlore Shrine";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void SormsOfWinter_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Storms of Winter";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void JorasKeep_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Defend Jora's Keep";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Sandstorm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Sandstorm";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void SaidrasHeaven_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Saidra's Haven";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Loamhurst_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "New Loamhurst";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Homestead_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Noran's Homestead";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Atherblade_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Aetherblade Assault";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }



        private void Blackout_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Kaineng Blackout";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void GangWar_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Gang War";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }



        private void Aspenwood_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Aspenwood";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void JadeSea_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Battle for Jade Sea";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void WizzardsTower_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Unlock'Wizard's Tower";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Flybynigtht_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Fly by Night";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }


        private void Amnytas_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Defense of Amnytas";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private void Convergence_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Convergences";

            userTriggeredSave = true;

            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            userTriggeredSave = false;
        }

        private CheckBox FindCheckBoxByBossName(string bossName)
        {

            switch (bossName)
            {
                case "The Frozen Maw":
                    return Maw;
                case "Shadow Behemoth":
                    return Behemoth;
                case "Fire Elemental":
                    return Fire_Elemental;
                case "Great Jungle Wurm":
                    return JungleWurm;
                case "Ulgoth the Modniir":
                    return Ulgoth;
                case "Taidha Covington":
                    return Thaida;
                case "Megadestroyer":
                    return Megadestroyer;
                case "Inquest Golem Mark II":
                    return MarkTwo;
                case "Tequatl the Sunless":
                    return Tequatl;
                case "The Shatterer":
                    return Shatterer;
                case "Karka Queen":
                    return Karka;
                case "Claw of Jormag":
                    return Claw;
                case "Chak Gerent":
                    return Chak;
                case "Battle in Tarir":
                    return Tarir;
                case "Spellmaster Macsen":
                    return Mascen;
                case "Dragon's Stand":
                    return DS;
                case "Death-Branded Shatterer":
                    return DBS;
                case "Junundu Rising":
                    return Junundu;
                case "Path to Ascension":
                    return PTA;
                case "Doppelganger":
                    return Doppelganger;
                case "Forged with Fire":
                    return Doggies;
                case "Choya Piñata":
                    return Pinata;
                case "Serpents' Ire":
                    return SerpentsIre;
                case "Palawadan":
                    return Palawadan;
                case "Thunderhead Keep":
                    return ThunderheadKeep;
                case "Maws of Torment":
                    return MawsOfTorment;
                case "The Oil Floes":
                    return Oil;
                case "Drakkar":
                    return Drakkar;
                case "Metal Concert":
                    return Metalconcert;
                case "Dragonstorm":
                    return Dragonstorm;
                case "Ooze Pits":
                    return OozePits;
                case "Effigy":
                    return Effigy;
                case "Doomlore Shrine":
                    return Doomlore;
                case "Storms of Winter":
                    return SormsOfWinter;
                case "Defend Jora's Keep":
                    return JorasKeep;
                case "Sandstorm":
                    return Sandstorm;
                case "Saidra's Haven":
                    return SaidrasHeaven;
                case "New Loamhurst":
                    return Loamhurst;
                case "Noran's Homestead":
                    return Homestead;
                case "Aetherblade Assault":
                    return Atherblade;
                case "Kaineng Blackout":
                    return Blackout;
                case "Gang War":
                    return GangWar;
                case "Aspenwood":
                    return Aspenwood;
                case "Battle for Jade Sea":
                    return JadeSea;
                case "Unlock'Wizard's Tower":
                    return WizzardsTower;
                case "Fly by Night":
                    return Flybynigtht;
                case "Defense of Amnytas":
                    return Amnytas;
                case "Convergences":
                    return Convergence;

                default:
                    return null;
            }
        }


    }
}

