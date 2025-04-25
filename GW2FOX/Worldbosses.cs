using static GW2FOX.BossTimerService;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2FOX
{
    public partial class Worldbosses : BaseForm
    {
        public static System.Windows.Controls.ListView? CustomBossList { get; private set; }
        public static Dictionary<string, CheckBox> bossCheckBoxMap;
        public static readonly char[] Separator = { ',' };
        public static List<string> BossList23 { get; set; } = new();

        public Worldbosses()
        {
            InitializeComponent();
            InitializeBossCheckBoxMap();
            bossCheckBoxMap = new Dictionary<string, CheckBox>(); 
            UpdateBossUiBosses();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);

            Load += Worldbosses_Load_1;         
        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {

            InitializeBossCheckBoxMap();
            UpdateBossUiBosses();
        }


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

        private void TheOilFloes_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new TheOilFloes());
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

        private void Effigy_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Effigy());
        }

        private void DoomloreShrine_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new DoomloreShrine());
        }

        private void StormsOfWinter_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new StormsOfWinter());
        }

        private void DefendJorasKeep_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new DefendJorasKeep());
        }

        private void Sandstorm_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Sandstorm());
        }

        private void SaidrasHaven_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new SaidrasHaven());
        }

        private void NewLoamhurst_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new NewLoamhurst());
        }

        private void NoransHomestead_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new NoransHomestead());
        }

        private void AetherbladeAssault_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new AetherbladeAssault());
        }

        private void KainengBlackout_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new KainengBlackout());
        }

        private void GangWar_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new GangWar());
        }

        private void Aspenwood_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Aspenwood());
        }

        private void JadeSea_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new JadeSea());
        }

        private void WizardsTower_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new WizardsTower());
        }

        private void FlyByNight_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new FlyByNight());
        }

        private void DefenseOfAmnytas_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new DefenseOfAmnytas());
        }

        private void Convergences_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new Convergences());
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

        private void button66_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SearchResults.Text);
            BringGw2ToFront();
        }

        private void button68_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new LLA());
        }

        private void button65_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new CaptainRotbeard());
        }

        private void button63_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new DredgeComissar());
        }

        private void button42_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new BrandedGenerals());
        }

        private void button31_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new GatesOfArah());
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Runinfo.Text);
            BringGw2ToFront();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Squadinfo.Text);
            BringGw2ToFront();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Guild.Text);
            BringGw2ToFront();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Welcome.Text);
            BringGw2ToFront();
        }

        private void EyeButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("The Eye of Zhaitan");
            UpdateBossUiBosses();
        }

        private void DwanyButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Statue of Dwanya");
            UpdateBossUiBosses();
        }

        private void LyssaButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Priestess of Lyssa");
            UpdateBossUiBosses();
        }

        private void OgresButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Ogrewars");
            UpdateBossUiBosses();
        }

        private void RhendakButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Rhendak");
            UpdateBossUiBosses();
        }

        private void CommissarButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Dredge Commissar");
            UpdateBossUiBosses();
        }

        private void GeneralsButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Branded Generals");
            UpdateBossUiBosses();
        }

        private void ArahButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.TriggerIt("Gates of Arah");
            UpdateBossUiBosses();
        }

        private void MawsOfTorment_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new MawsOfTorement());
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


        private void OozePits_Click(object sender, EventArgs e)
        {
            ShowAndHideForm(new TheOozePits());
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
            string bossName = "The frozen Maw";



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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
        }

        private void Ulgoth_CheckedChanged(object sender, EventArgs e)

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
            UpdateBossUiBosses();
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
                UpdateBossUiBosses();
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
            UpdateBossUiBosses();
        }

        private void MarkTwo_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Inquest Golem M2";



            if (MarkTwo.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
        }

        private void DBS_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "DB Shatterer";



            if (DBS.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
        }

        private void WizzardsTower_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "Wizard's Tower";



            if (WizzardsTower.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
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
            UpdateBossUiBosses();
        }

        private void LLA_CheckedChanged(object sender, EventArgs e)
        {
            string[] bossNames = { "LLA Timberline", "LLA Iron Marches", "LLA Gendarran" };

            if (LLA.Checked)
            {
                foreach (string bossName in bossNames)
                {
                    SaveBossNameToConfig(bossName);
                }
            }
            else
            {
                foreach (string bossName in bossNames)
                {
                    RemoveBossNameFromConfig(bossName);
                }
            }
            UpdateBossUiBosses();
        }

        private void InitializeBossCheckBoxMap()
        {
            bossCheckBoxMap = new Dictionary<string, CheckBox>
        {
        { "LLA Timberline", LLA },
        { "LLA Iron Marches", LLA },
        { "LLA Gendarran", LLA },
        { "The frozen Maw", Maw },
        { "Shadow Behemoth", Behemoth },
        { "Fire Elemental", Fire_Elemental },
        { "Great Jungle Wurm", JungleWurm },
        { "Ulgoth the Modniir", Ulgoth },
        { "Taidha Covington", Thaida },
        { "Megadestroyer", Megadestroyer },
        { "Inquest Golem M2", MarkTwo },
        { "Tequatl the Sunless", Tequatl },
        { "The Shatterer", Shatterer },
        { "Karka Queen", Karka },
        { "Claw of Jormag", Claw },
        { "Chak Gerent", Chak },
        { "Battle in Tarir", Tarir },
        { "Nightbosses", Mascen },
        { "Dragon's Stand", DS },
        { "DB Shatterer", DBS },
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
        { "Wizard's Tower", WizzardsTower },
        { "Fly by Night", Flybynigtht },
        { "Defense of Amnytas", Amnytas },
        { "Convergences", Convergence },
        { "FireShaman", FireShaman },
        };
        }

        public static CheckBox FindCheckBoxByName(string bossName)
        {
            if (bossCheckBoxMap != null && bossCheckBoxMap.TryGetValue(bossName, out var checkBox))
            {
                return checkBox;
            }
            return null;
        }

        private static void SaveChoosenOnesToConfig()
        {
            string configPath = "BossTimings.json";
            JObject json;

            // Bestehende JSON laden oder neu erzeugen
            if (File.Exists(configPath))
            {
                json = JObject.Parse(File.ReadAllText(configPath));
            }
            else
            {
                json = new JObject();
            }

            // Alle aktivierten Bosse sammeln
            var selectedBosses = bossCheckBoxMap
                .Where(kvp => kvp.Value.Checked)
                .Select(kvp => kvp.Key)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // In die JSON schreiben (überschreibt ggf. bestehende Liste)
            json["ChoosenOnes"] = JArray.FromObject(selectedBosses);

            // Datei speichern
            File.WriteAllText(configPath, json.ToString(Formatting.Indented));
        }



        public string Name { get; set; }
        public List<string> Timings { get; set; }
        public string Category { get; set; }
        public string Waypoint { get; set; }


        private static void SaveBossNameToConfig(string bossName)
        {
            string configPath = "BossTimings.json";
            JObject json = File.Exists(configPath)
                ? JObject.Parse(File.ReadAllText(configPath))
                : new JObject();

            JArray choosen = (JArray?)json["ChoosenOnes"] ?? new JArray();
            var bossNames = choosen.Select(t => t.ToString()).ToList();

            if (!bossNames.Contains(bossName, StringComparer.OrdinalIgnoreCase))
            {
                bossNames.Add(bossName);
                json["ChoosenOnes"] = JArray.FromObject(bossNames.Distinct(StringComparer.OrdinalIgnoreCase));

                // ✅ Datei speichern nicht vergessen!
                File.WriteAllText(configPath, json.ToString(Formatting.Indented));
            }
        }


        private static void RemoveBossNameFromConfig(string bossName)
        {
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            var json = JObject.Parse(File.ReadAllText(configPath));
            var choosen = (JArray?)json["ChoosenOnes"] ?? new JArray();

            var updated = choosen
                .Select(t => t.ToString())
                .Where(n => !n.Equals(bossName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            json["ChoosenOnes"] = JArray.FromObject(updated);
            File.WriteAllText(configPath, json.ToString(Formatting.Indented));
        }



        public static void CheckAllBossCheckboxesAll()
        {
            foreach (var checkBox in bossCheckBoxMap.Values)
            {
                checkBox.Checked = true;
                checkBox.ForeColor = System.Drawing.Color.White;
                checkBox.Invalidate();
            }

            // Speichere alle aktuell aktivierten Checkbox-Namen in "ChoosenOnes"
            SaveChoosenOnesToConfig();
        }


        public static void CheckBossCheckboxes(IEnumerable<string> bossNames)
        {
            CheckBossCheckboxes(bossNames, bossCheckBoxMap);
        }

        public static void CheckBossCheckboxes(IEnumerable<string> bossNames, Dictionary<string, CheckBox> checkBoxMap)
        {
            foreach (var bossName in bossNames)
            {
                if (checkBoxMap.TryGetValue(bossName, out var checkBox))
                {
                    checkBox.Checked = true;
                    checkBox.ForeColor = System.Drawing.Color.White;
                }
            }
        }


        public static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "")
        {
            foreach (var timing in timings)
            {
                var utcTime = ConvertToUtcFromConfigTime(timing);
                BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint));
            }
        }

        public static DateTime ConvertToUtcFromConfigTime(string configTime)
        {
            return DateTime.SpecifyKind(
                DateTime.ParseExact(configTime, "HH:mm:ss", CultureInfo.InvariantCulture),
                DateTimeKind.Utc
            );
        }


        private void FidosSpecial_Click(object sender, EventArgs e)
        {
            try
            {
                BossTimings.ApplyBossGroupFromConfig("Fido");
                SaveChoosenOnesToConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Fido-Gruppe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void World_Click(object sender, EventArgs e)
        {
            try
            {
                BossTimings.ApplyBossGroupFromConfig("World");
                SaveChoosenOnesToConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der World-Gruppe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Checkboxen deaktivieren
                foreach (var checkBox in Worldbosses.bossCheckBoxMap.Values)
                {
                    checkBox.Checked = false;
                    checkBox.ForeColor = System.Drawing.Color.Gray;
                    checkBox.Invalidate();
                }

                // 2. "ChoosenOnes" in der JSON leeren
                string configPath = "BossTimings.json";
                if (File.Exists(configPath))
                {
                    var json = JObject.Parse(File.ReadAllText(configPath));
                    json["ChoosenOnes"] = new JArray(); // Liste leeren
                    File.WriteAllText(configPath, json.ToString(Formatting.Indented));
                }

                // 2.5. dynamic_events.json löschen
                string dynamicEventsPath = "dynamic_events.json";
                if (File.Exists(dynamicEventsPath))
                {
                    File.Delete(dynamicEventsPath);
                }

                // 2.6. Runtime-DynamicEvents löschen
                DynamicEventManager.Events.Clear();
                DynamicEventManager.SavePersistedEvents();


                // 3. Runtime-Listen leeren
                BossTimings.BossList23.Clear();
                BossTimings.BossEventsList.Clear();
                BossTimings.BossEventGroups.Clear();

                // 4. Overlay und Timer aktualisieren
                BossTimer.UpdateBossList();
                BossTimings.UpdateBossOverlayList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Deaktivieren aller Bosse: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                var config = BossTimings.LoadBossConfigAndReturn();

                // 1. Alle Namen aus JSON
                var allBossNames = config.Bosses
                    .Where(b => !string.IsNullOrWhiteSpace(b.Name))
                    .Select(b => b.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // 2. Checkboxen aktivieren
                BossTimings.CheckBossCheckboxes(allBossNames.ToArray(), Worldbosses.bossCheckBoxMap);

                // 3. BossList23 setzen
                BossList23.Clear();

                // 4. Events neu aufbauen
                BossTimings.BossEventsList.Clear();
                BossTimings.BossEventGroups.Clear();
                foreach (var boss in config.Bosses)
                {
                    if (boss.Name != null && boss.Timings != null)
                    {
                        BossTimings.AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "");
                    }
                }

                BossTimings.GenerateBossEventGroups();

                // 5. UI & Overlay aktualisieren
                BossTimer.UpdateBossList();
                BossTimings.UpdateBossOverlayList();
                SaveChoosenOnesToConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Anzeigen aller Bosse: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button67_Click(object sender, EventArgs e)
        {
            try
            {
                // Anzahl der Bosse aus dem Textfeld entnehmen (ScheduleTextBox)
                if (int.TryParse(Quantity.Text, out int numberOfBosses) && numberOfBosses > 0)
                {
                    // Hier die Liste der kommenden Bosse laden (bereits vorhandene Methode)
                    List<string> bossNamesFromConfig = BossList23;

                    var bossEventGroups = BossEventGroups
                        .Where(bossEventGroup => bossNamesFromConfig.Contains(bossEventGroup.BossName))
                        .ToList();

                    var allBosses = bossEventGroups
                        .SelectMany(bossEventGroup => bossEventGroup.GetNextRuns())
                        .ToList();

                    // Sortierung nach der Zeit des nächsten Bosskampfs
                    allBosses.Sort((bossEvent1, bossEvent2) =>
                    {
                        return bossEvent1.NextRunTime.CompareTo(bossEvent2.NextRunTime);
                    });

                    // Bossnamen extrahieren (bis zur gewünschten Anzahl)
                    var bossNames = allBosses.Take(numberOfBosses).Select(bossEvent => bossEvent.BossName).ToList();

                    // Bossnamen durch Kommas getrennt
                    string bossNamesString = string.Join("," + Environment.NewLine, bossNames);

                    // Die Bossnamen in die Zwischenablage kopieren
                    Clipboard.SetText(bossNamesString);

                    // Bossnamen in ResultTextBox anzeigen
                    SearchResults.Text = bossNamesString;
                }
                else
                {
                    MessageBox.Show("A Number please!.", "Do you know the meaning of a NUMBER, try 10!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string bossName = "FireShaman";



            if (FireShaman.Checked)
            {
                SaveBossNameToConfig(bossName);
            }
            else
            {
                RemoveBossNameFromConfig(bossName);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }



        private void Mixed_Click_1(object sender, EventArgs e)
        {
            try
            {
                BossTimings.ApplyBossGroupFromConfig("Mixed");
                SaveChoosenOnesToConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Mixed-Gruppe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Meta_Click_1(object sender, EventArgs e)
        {
            try
            {
                BossTimings.ApplyBossGroupFromConfig("Meta");
                SaveChoosenOnesToConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Meta-Gruppe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBossUiBosses()
        {
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            try
            {
                var json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);

                // 1. Lade ChoosenOnes
                var selectedBossNames = jObj["ChoosenOnes"] is JArray array
                    ? array.Select(x => x.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // 2. Checkboxen setzen
                foreach (var entry in bossCheckBoxMap)
                {
                    var checkBox = entry.Value;
                    if (checkBox != null)
                    {
                        checkBox.Checked = selectedBossNames.Contains(entry.Key);
                        checkBox.ForeColor = checkBox.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
                    }
                }

                // 3. BossList23 aktualisieren
                BossList23 = selectedBossNames.ToList();

                // 4. Events und Gruppen neu aufbauen
                BossEventsList.Clear();
                BossEventGroups.Clear();

                var config = BossTimings.LoadBossConfigAndReturn();

                foreach (var bossName in BossList23)
                {
                    var boss = config.Bosses.FirstOrDefault(b => b.Name.Equals(bossName, StringComparison.OrdinalIgnoreCase));
                    if (boss != null)
                    {
                        AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "");
                    }
                    else
                    {
                        //Console.WriteLine($"⚠ Boss '{bossName}' nicht in Bosses gefunden.");
                    }
                }

                GenerateBossEventGroups();
                
                UpdateBossOverlayList();    // wichtig für WPF-Overlay
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden von ChoosenOnes: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

