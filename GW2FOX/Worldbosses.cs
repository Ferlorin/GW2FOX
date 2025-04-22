using static GW2FOX.BossTimerService;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

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
            
            bossCheckBoxMap = new Dictionary<string, CheckBox>();
            InitializeBossCheckBoxMap();
            BossTimerService.UpdateBossUiBosses();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);

            Load += Worldbosses_Load_1;
        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {
            LoadBossGroup("World");
        }

        const int SW_RESTORE = 9;

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
            DynamicEventManager.Trigger("The Eye of Zhaitan");
;
            UpdateBossUiBosses();
        }

        private void DwanyButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Statue of Dwanya");
            UpdateBossUiBosses();
        }

        private void LyssaButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Priestess of Lyssa");
            UpdateBossUiBosses();
        }

        private void OgresButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Ogrewars");
            UpdateBossUiBosses();
        }

        private void RhendakButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Rhendak");
            UpdateBossUiBosses();
        }

        private void CommissarButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Dredge Commissar");
            UpdateBossUiBosses();
        }

        private void GeneralsButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Branded Generals");
            UpdateBossUiBosses();
        }

        private void ArahButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Gates of Arah");
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
            string bossName = "Inquest Golem M2";



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
            string bossName = "DB Shatterer";



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
            string bossName = "Wizard's Tower";



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
        public class BossConfig
        {
            public List<Boss> Bosses { get; set; } = new();
            public string Meta { get; set; } = "";
            public string Mixed { get; set; } = "";
            public string World { get; set; } = "";
            public string Fido { get; set; } = "";
            public string Runinfo { get; set; } = "";
            public string Squadinfo { get; set; } = "";
            public string Guild { get; set; } = "";
            public string Welcome { get; set; } = "";
            public string Symbols { get; set; } = "";
        }

        public class Boss
        {
            public string Name { get; set; }
            public List<string> Timings { get; set; }
            public string Category { get; set; }
            public string Waypoint { get; set; }
        }

        private static BossConfig LoadBossConfig()
        {
            string path = "bosses_config.json";

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<BossConfig>(json);
                if (config != null)
                    return config;
            }

            // Default-Fallback
            return new BossConfig();
        }

        private static void SaveBossConfig(BossConfig config)
        {
            File.WriteAllText("bosses_config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        private static void SaveBossNameToConfig(string bossName)
        {
            var config = LoadBossConfig();

            if (!config.Bosses.Any(b => b.Name.Equals(bossName, StringComparison.OrdinalIgnoreCase)))
            {
                config.Bosses.Add(new Boss
                {
                    Name = bossName,
                    Timings = new List<string> { "00:00:00" },
                    Category = "WBs",
                    Waypoint = ""
                });

                SaveBossConfig(config);
            }
        }

        private static void RemoveBossNameFromConfig(string bossName)
        {
            var config = LoadBossConfig();
            var boss = config.Bosses.FirstOrDefault(b => b.Name.Equals(bossName, StringComparison.OrdinalIgnoreCase));

            if (boss != null)
            {
                config.Bosses.Remove(boss);
                SaveBossConfig(config);
            }
        }

        private void ClearAll_Click(object? sender, EventArgs e)
        {
            foreach (var checkBox in bossCheckBoxMap.Values)
            {
                checkBox.Checked = false;
                checkBox.Invalidate();
            }

            var config = LoadBossConfig();
            config.Bosses.Clear();
            SaveBossConfig(config);
        }


        private void LoadBossGroup(string groupName)
        {
            try
            {
                ClearAll_Click(null, EventArgs.Empty);

                var config = LoadBossConfig();
                var groupLine = GetGroupLine(config, groupName);

                var bossNames = groupLine
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(b => b.Trim())
                    .ToArray();

                CheckBossCheckboxes(bossNames);

                config.Bosses = config.Bosses
                    .Where(b => bossNames.Contains(b.Name, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                SaveBossConfig(config);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden von {groupName}: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetGroupLine(BossConfig config, string key) => key.ToLower() switch
        {
            "meta" => config.Meta,
            "mixed" => config.Mixed,
            "world" => config.World,
            "fido" => config.Fido,
            _ => ""
        };

        public static void CheckAllBossCheckboxes()
        {
            foreach (var checkBox in bossCheckBoxMap.Values)
            {
                checkBox.Checked = true;
                checkBox.Invalidate();
            }

            var config = LoadBossConfig();
            config.Bosses = BossEventGroups.Select(g => new Boss
            {
                Name = g.BossName,
                Timings = new List<string>(),
                Category = "WBs",
                Waypoint = ""
            }).ToList();

            SaveBossConfig(config);
        }

        private void CheckBossCheckboxes(string[] bossNames)
        {
            foreach (var bossName in bossNames)
            {
                var checkBox = FindCheckBoxByBossName(bossName);
                if (checkBox != null)
                {
                    checkBox.Checked = true;
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
    }
}

