using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GW2FOX
{
    public partial class Worldbosses : BaseForm
    {
        public static ListView? CustomBossList { get; private set; }
        public static Dictionary<string, CheckBox> bossCheckBoxMap;

        public Worldbosses()
        {
            InitializeComponent();
            bossCheckBoxMap = new Dictionary<string, CheckBox>();
            InitializeBossCheckBoxMap();
            InitializeBossTimerAndOverlay();
            UpdateBossUIBosses();

            Load += Worldbosses_Load_1;
        }
        private new void InitializeBossTimerAndOverlay()
        {
            base.InitializeBossTimerAndOverlay();
        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {

            WindowState = FormWindowState.Maximized;
            SetBossListFromConfig_Bosses();
        }



        

        // Constants for window handling
        const int SW_RESTORE = 9;

        

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


        private void Maw_CheckedChanged(object sender, EventArgs e)
        {
            // Der Name des Bosses
            string[] bossNames = { "LLLA - Timberline Falls", "LLLA - Iron Marches", "LLLA - Gendarran Fields", "The frozen Maw" };


            // Setze den Trigger, um anzuzeigen, dass die Aktion vom Benutzer ausgelöst wurde


            if (Maw.Checked)
            {
                // Wenn das Kontrollkästchen ausgewählt ist
                foreach (string bossName in bossNames)
                {
                    SaveBossNameToConfig(bossName);
                }
            }
            else
            {
                // Wenn das Kontrollkästchen nicht ausgewählt ist
                foreach (string bossName in bossNames)
                {
                    RemoveBossNameFromConfig(bossName);
                }
            }


        }


        private void Behemoth_CheckedChanged(object sender, EventArgs e)
        {
            // Der Name des Bosses
            string bossName = "Shadow Behemoth";



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

        }


        private void Fire_Elemental_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Fire Elemental";



            if (Fire_Elemental.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void JungleWurm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Great Jungle Wurm";



            if (JungleWurm.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Ulgoth_CheckedChanged(object sender, EventArgs e)
        {
            {
                string bossName = "Ulgoth the Modniir";



                if (Ulgoth.Checked)
                {
                    SaveBossNameToConfig(bossName);
                }
                else
                {
                    RemoveBossNameFromConfig(bossName);
                }

            }
        }

        private void Thaida_CheckedChanged(object sender, EventArgs e)
        {
            {
                string bossName = "Taidha Covington";



                if (Thaida.Checked)
                {
                    SaveBossNameToConfig(bossName);
                }
                else
                {
                    RemoveBossNameFromConfig(bossName);
                }

            }
        }

        private void Megadestroyer_CheckedChanged(object sender, EventArgs e)

        {
            string bossName = "Megadestroyer";



            if (Megadestroyer.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void MarkTwo_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Inquest Golem Mark II";



            if (MarkTwo.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Tequatl_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Tequatl the Sunless";



            if (Tequatl.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Shatterer_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "The Shatterer";



            if (Shatterer.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Karka_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Karka Queen";



            if (Karka.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Claw_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Claw of Jormag";



            if (Claw.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Chak_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Chak Gerent";



            if (Chak.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }



        private void Tarir_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Battle in Tarir";



            if (Tarir.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Mascen_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Spellmaster Macsen";



            if (Mascen.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void DS_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Dragon's Stand";



            if (DS.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void DBS_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Death-Branded Shatterer";



            if (DBS.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Junundu_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Junundu Rising";



            if (Junundu.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void PTA_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Path to Ascension";



            if (PTA.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Doppelganger_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Doppelganger";



            if (Doppelganger.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Doggies_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Forged with Fire";



            if (Doggies.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }



        private void Pinata_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Choya Piñata";



            if (Pinata.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void SerpentsIre_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Serpents' Ire";



            if (SerpentsIre.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Palawadan_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Palawadan";



            if (Palawadan.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void ThunderheadKeep_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Thunderhead Keep";



            if (ThunderheadKeep.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void MawsOfTorment_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Maws of Torment";



            if (MawsOfTorment.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Oil_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "The Oil Floes";



            if (Oil.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Drakkar_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Drakkar";



            if (Drakkar.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Metalconcert_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Metal Concert";



            if (Metalconcert.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Dragonstorm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Dragonstorm";



            if (Dragonstorm.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void OozePits_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Ooze Pits";



            if (OozePits.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Effigy_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Effigy";



            if (Effigy.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Doomlore_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Doomlore Shrine";



            if (Doomlore.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void SormsOfWinter_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Storms of Winter";



            if (SormsOfWinter.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void JorasKeep_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Defend Jora's Keep";



            if (JorasKeep.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Sandstorm_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Sandstorm";



            if (Sandstorm.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void SaidrasHeaven_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Saidra's Haven";



            if (SaidrasHeaven.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Loamhurst_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "New Loamhurst";



            if (Loamhurst.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Homestead_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Noran's Homestead";



            if (Homestead.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Atherblade_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Aetherblade Assault";



            if (Atherblade.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }



        private void Blackout_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Kaineng Blackout";



            if (Blackout.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void GangWar_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Gang War";



            if (GangWar.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }



        private void Aspenwood_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Aspenwood";



            if (Aspenwood.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void JadeSea_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Battle for Jade Sea";



            if (JadeSea.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void WizzardsTower_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Unlock'Wizard's Tower";



            if (WizzardsTower.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Flybynigtht_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Fly by Night";



            if (Flybynigtht.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }


        private void Amnytas_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Defense of Amnytas";



            if (Amnytas.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void Convergence_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Convergences";



            if (Convergence.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void InitializeBossCheckBoxMap()
        {
            bossCheckBoxMap = new Dictionary<string, CheckBox>
        {
        { "LLLA - Timberline Falls", Maw },
        { "LLLA - Iron Marches", Maw },
        { "LLLA - Gendarran Fields", Maw },
        { "The frozen Maw", Maw },
        { "Shadow Behemoth", Behemoth },
        { "Fire Elemental", Fire_Elemental },
        { "Great Jungle Wurm", JungleWurm },
        { "Ulgoth the Modniir", Ulgoth },
        { "Taidha Covington", Thaida },
        { "Megadestroyer", Megadestroyer },
        { "Inquest Golem Mark II", MarkTwo },
        { "Tequatl the Sunless", Tequatl },
        { "The Shatterer", Shatterer },
        { "Karka Queen", Karka },
        { "Claw of Jormag", Claw },
        { "Chak Gerent", Chak },
        { "Battle in Tarir", Tarir },
        { "Spellmaster Macsen", Mascen },
        { "Dragon's Stand", DS },
        { "Death-Branded Shatterer", DBS },
        { "Junundu Rising", Junundu },
        { "Path to Ascension", PTA },
        { "Doppelganger", Doppelganger },
        { "Forged with Fire", Doggies },
        { "Choya Piñata", Pinata },
        { "Serpents' Ire", SerpentsIre },
        { "Palawadan", Palawadan },
        { "Thunderhead Keep", ThunderheadKeep },
        { "Maws of Torment", MawsOfTorment },
        { "The Oil Floes", Oil },
        { "Drakkar", Drakkar },
        { "Metal Concert", Metalconcert },
        { "Dragonstorm", Dragonstorm },
        { "Ooze Pits", OozePits },
        { "Effigy", Effigy },
        { "Doomlore Shrine", Doomlore },
        { "Storms of Winter", SormsOfWinter },
        { "Defend Jora's Keep", JorasKeep },
        { "Sandstorm", Sandstorm },
        { "Saidra's Haven", SaidrasHeaven },
        { "New Loamhurst", Loamhurst },
        { "Noran's Homestead", Homestead },
        { "Aetherblade Assault", Atherblade },
        { "Kaineng Blackout", Blackout },
        { "Gang War", GangWar },
        { "Aspenwood", Aspenwood },
        { "Battle for Jade Sea", JadeSea },
        { "Unlock'Wizard's Tower", WizzardsTower },
        { "Fly by Night", Flybynigtht },
        { "Defense of Amnytas", Amnytas },
        { "Convergences", Convergence }
        };
        }

        public static List<string> GetSelectedBosses()
        {
            List<string> selectedBosses = new List<string>();

            foreach (var entry in bossCheckBoxMap)
            {
                if (entry.Value.Checked)
                {
                    selectedBosses.Add(entry.Key);
                }
            }

            return selectedBosses;
        }


        public static CheckBox FindCheckBoxByBossName(string bossName)
        {
            if (bossCheckBoxMap != null && bossCheckBoxMap.TryGetValue(bossName, out var checkBox))
            {
                return checkBox;
            }
            return null;
        }


        private void BossCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                string bossName = checkBox.Text;
                SaveBossNameToConfig(bossName);
            }
            else
            {
                string bossName = checkBox.Text;
                RemoveBossNameFromConfig(bossName);

            }

        }

        private void SaveBossNameToConfig(string bossName)
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = ReadConfigFile();

                if (lines == null)
                {
                    // Fehler beim Lesen der Datei, abbrechen
                    return;
                }

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

                    // Überprüfen, ob der Name bereits vorhanden ist (case-insensitive)
                    bool nameExists = bossNames.Any(name => name.Equals(bossName, StringComparison.OrdinalIgnoreCase));

                    if (!nameExists)
                    {
                        // Füge den Bossnamen hinzu
                        lines[bossIndex] = $"Bosses: \"{string.Join(", ", bossNames.Concat(new[] { bossName }).ToArray())}\"".Trim();

                        // Aktualisierten Inhalt zurück in die Datei schreiben
                        WriteConfigFile(lines);

                        // Setze das Häkchen im CheckBox-Control auf true
                        CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                        if (bossCheckBox != null) 
                        {
                            bossCheckBox.Checked = true;
                        }

                    }

                }
                else
                {
                    //This will create a new section
                    SaveTextToFile(GlobalVariables.DEFAULT_BOSSES, "Bosses");
                    SaveBossNameToConfig(bossName);
                    // // Wenn der Abschnitt "Bosses:" nicht gefunden wird, gibt es nichts zu hinzufügen
                    // MessageBox.Show($"Bosses section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static void WriteConfigFile(string[] lines)
        {
            try
            {

                // Holen Sie die ausgewählten Bosses
                List<string> selectedBosses = GetSelectedBosses();

                // Erstellen Sie die Zeile für die "Bosses:"-Sektion
                string bossesLine = $"Bosses: \"{string.Join(", ", selectedBosses)}\"";

                // Lesen Sie die bestehenden Zeilen aus der Datei
                string[] existingLines = ReadConfigFile();

                // Suchen Sie nach der Zeile mit der "Bosses:"-Sektion
                int bossesIndex = -1;
                for (int i = 0; i < existingLines.Length; i++)
                {
                    if (existingLines[i].StartsWith("Bosses:"))
                    {
                        bossesIndex = i;
                        break;
                    }
                }

                // Wenn die "Bosses:"-Sektion gefunden wurde, löschen Sie sie
                if (bossesIndex != -1)
                {
                    List<string> newLines = existingLines.ToList();

                    // Entfernen Sie die "Bosses:"-Sektion
                    newLines.RemoveAt(bossesIndex);

                    // Fügen Sie die aktualisierte "Bosses:"-Zeile hinzu
                    newLines.Insert(bossesIndex, bossesLine);

                    existingLines = newLines.ToArray();
                }
                else
                {
                    // Wenn die "Bosses:"-Sektion nicht gefunden wurde, fügen Sie sie einfach hinzu
                    List<string> newLines = existingLines.ToList();
                    newLines.Add(bossesLine);
                    existingLines = newLines.ToArray();
                }

                // Schreiben Sie die aktualisierten Zeilen zurück in die Datei
                File.WriteAllLines(GlobalVariables.FILE_PATH, existingLines);

                UpdateBossUIBosses();



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to config file: {ex.Message}");
                throw new Exception($"Error writing to config file: {ex.Message}");
            }
        }

        public static string[] ReadConfigFile()
        {
            try
            {

                // Check if the file exists
                if (File.Exists(GlobalVariables.FILE_PATH))
                {
                    // Read all lines from the file
                    return File.ReadAllLines(GlobalVariables.FILE_PATH);
                }
                else
                {
                    Console.WriteLine($"Config file does not exist. Will try to create it");
                    try
                    {
                        var fileStream = File.Create(GlobalVariables.FILE_PATH);
                        fileStream.Close();
                        SaveTextToFile(GlobalVariables.DEFAULT_BOSSES, "Bosses", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_META, "Meta", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_WORLD, "World", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_MIXED, "Mixed", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_GUILD, "Guild", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_RUN_INFO, "Runinfo", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_SQUAD_INFO, "Squadinfo", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_WELCOME, "Welcome", true);
                        SaveTextToFile(GlobalVariables.DEFAULT_SYMBOLS, "Symbols", true);
                        
                        return ReadConfigFile();
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception, but don't call ReadConfigFile recursively
                        Console.WriteLine($"Error creating config file: {ex.Message}");
                        throw; // Re-throw the exception to prevent infinite recursion
                    }
                    // Log or handle the case where the file is not found
                    Console.WriteLine($"Config file does not exist.");
                    throw new FileNotFoundException("Config file not found.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception, but don't call ReadConfigFile recursively
                Console.WriteLine($"Error reading from config file: {ex.Message}");
                throw; // Re-throw the exception to prevent infinite recursion
            }
        }

        private void RemoveBossNameFromConfig(string bossName)
        {
            try
            {
                // Vorhandenen Inhalt aus der Datei lesen
                string[] lines = ReadConfigFile();

                if (lines == null)
                {
                    // Fehler beim Lesen der Datei, abbrechen
                    return;
                }

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
                    WriteConfigFile(lines);



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
                    SaveTextToFile(GlobalVariables.DEFAULT_BOSSES, "Bosses");
                    SaveBossNameToConfig(bossName);
                    // Wenn der Abschnitt "Bosses:" nicht gefunden wird, gibt es nichts zu entfernen
                    // MessageBox.Show($"Bosses section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Meta_Click(object sender, EventArgs e)
        {
            try
            {
                // Uncheck all existing checkboxes
                UncheckAllBossCheckboxes();

                // Read the config file
                string[] lines = ReadConfigFile();

                // Index of the line with the Meta bosses
                int metaIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Meta:"))
                    {
                        metaIndex = i;
                        break;
                    }
                }

                // If Meta section is found, extract and check the checkboxes for Meta bosses
                if (metaIndex != -1 && metaIndex < lines.Length)
                {
                    // Extract the bosses from the Meta line
                    string metaBossLine = lines[metaIndex].Replace("Meta:", "").Trim();

                    // Remove the outer quotes and split the bosses
                    string[] metaBosses = metaBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    // Check the corresponding checkboxes for Meta bosses
                    CheckBossCheckboxes(metaBosses);

                    // Update the "Bosses:" section in the configuration file
                    UpdateBossesSection(metaBosses, lines);

                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_META, "Meta");
                    Meta_Click(sender, e);
                    // MessageBox.Show($"Meta section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Meta bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBossesSection(string[] metaBosses, string[] lines)
        {
            // Find the index of the "Bosses:" section
            int bossesIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    bossesIndex = i;
                    break;
                }
            }

            // If "Bosses:" section is found, update it with the Meta bosses
            if (bossesIndex != -1 && bossesIndex < lines.Length)
            {
                // Join the Meta bosses and update the "Bosses:" section
                lines[bossesIndex] = $"Bosses: \"{string.Join(", ", metaBosses)}\"";
            }

        }



        public static void UncheckAllBossCheckboxes()
        {
            try
            {
                // Uncheck all checkboxes in the UI
                foreach (CheckBox checkBox in bossCheckBoxMap.Values)
                {
                    checkBox.Checked = false;
                    checkBox.Invalidate();
                }

                // Remove all bosses from the "Bosses:" section in the config file
                string[] lines = ReadConfigFile();

                int bossIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Bosses:"))
                    {
                        bossIndex = i;
                        break;
                    }
                }

                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    // Remove the "Bosses:" section from the array
                    lines = lines.Take(bossIndex).Concat(lines.SkipWhile(line => line.StartsWith("Bosses:")).Skip(1)).ToArray();

                    // Save the modified configuration back to the file
                    WriteConfigFile(lines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unchecking all bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void CheckBossCheckboxes(string[] bossNames)
        {
            foreach (string bossName in bossNames)
            {
                CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                if (bossCheckBox != null)
                {
                    bossCheckBox.Checked = true;
                }
            }
        }



        private void UpdateBossesSection1(string[] metaBosses, string[] lines)
        {
            // Find the index of the "Bosses:" section
            int bossesIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    bossesIndex = i;
                    break;
                }
            }

            // If "Bosses:" section is found, update it with the Meta bosses
            if (bossesIndex != -1 && bossesIndex < lines.Length)
            {
                // Join the Meta bosses and update the "Bosses:" section
                lines[bossesIndex] = $"Bosses: \"{string.Join(", ", metaBosses)}\"";
            }
        }

        private void World_Click(object sender, EventArgs e)
        {
            try
            {
                // Uncheck all existing checkboxes
                UncheckAllBossCheckboxes();

                // Read the config file
                string[] lines = ReadConfigFile();

                // Index of the line with the World bosses
                int worldIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("World:"))
                    {
                        worldIndex = i;
                        break;
                    }
                }

                // If World section is found, extract and check the checkboxes for World bosses
                if (worldIndex != -1 && worldIndex < lines.Length)
                {
                    // Extract the bosses from the World line
                    string worldBossLine = lines[worldIndex].Replace("World:", "").Trim();

                    // Remove the outer quotes and split the bosses
                    string[] worldBosses = worldBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    // Check the corresponding checkboxes for World bosses
                    CheckBossCheckboxes(worldBosses);

                    // Update the "Bosses:" section in the configuration file
                    UpdateBossesSection1(worldBosses, lines);
                }
                else
                {
                    //This will create a new section
                    SaveTextToFile(GlobalVariables.DEFAULT_WORLD, "World");
                    World_Click(sender, e);   
                    // MessageBox.Show($"World section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading World bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearAll_Click(object sender, EventArgs e)
        {
            UncheckAllBossCheckboxes();
        }

        private void Mixed_Click(object sender, EventArgs e)
        {
            try
            {
                // Uncheck all existing checkboxes
                UncheckAllBossCheckboxes();

                // Read the config file
                string[] lines = ReadConfigFile();

                // Index of the line with the Mixed bosses
                int mixedIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Mixed:"))
                    {
                        mixedIndex = i;
                        break;
                    }
                }

                // If Mixed section is found, extract and check the checkboxes for Mixed bosses
                if (mixedIndex != -1 && mixedIndex < lines.Length)
                {
                    // Extract the bosses from the Mixed line
                    string mixedBossLine = lines[mixedIndex].Replace("Mixed:", "").Trim();

                    // Remove the outer quotes and split the bosses
                    string[] mixedBosses = mixedBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    // Check the corresponding checkboxes for Mixed bosses
                    CheckBossCheckboxes(mixedBosses);

                    // Update the "Bosses:" section in the configuration file
                    UpdateBossesSection2(mixedBosses, lines);
                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_MIXED, "Mixed");
                    Mixed_Click(sender, e);
                    // MessageBox.Show($"Mixed section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Mixed bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateBossesSection2(string[] mixedBosses, string[] lines)
        {
            // Find the index of the "Mixed:" section
            int mixedIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    mixedIndex = i;
                    break;
                }
            }

            // If "Mixed:" section is found, update it with the Mixed bosses
            if (mixedIndex != -1 && mixedIndex < lines.Length)
            {
                // Join the Mixed bosses and update the "Mixed:" section
                lines[mixedIndex] = $"Bosses: \"{string.Join(", ", mixedBosses)}\"";
            }
        }




        public static void UpdateBossUIBosses()
        {
            BossTimings.SetBossListFromConfig_Bosses();
            ListView bossList = CustomBossList;
            if (bossList != null)
            {
                BossTimer bossTimerInstance = new BossTimer(bossList);
                bossTimerInstance.UpdateBossList();
            }
        }




        public static void SetBossListFromConfig_Bosses()
        {
            try
            {
                // Lese die Bosse aus der Konfigurationsdatei
                string[] lines = ReadConfigFile();

                if (lines == null)
                {
                    // Fehler beim Lesen der Datei, abbrechen
                    return;
                }

                // Suche nach dem Abschnitt "Bosses:"
                int bossIndex = Array.FindIndex(lines, line => line.StartsWith("Bosses:"));

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

                    // Setze die Häkchen entsprechend der Bosse in der Liste
                    foreach (string bossName in bossNames)
                    {
                        CheckBox bossCheckBox = FindCheckBoxByBossName(bossName);
                        if (bossCheckBox != null)
                        {
                            bossCheckBox.Checked = true;
                            bossCheckBox.Invalidate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting boss checkboxes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private new void Timer_Click(object sender, EventArgs e)
        {
            base.Timer_Click(sender, e);
            // Additional logic specific to Timer_Click in Main class, if any
        }
    }


}



