using static GW2FOX.BossTimerService;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace GW2FOX
{
    public partial class Worldbosses : BaseForm
    {
        public static System.Windows.Controls.ListView? CustomBossList { get; private set; }
        public static Dictionary<string, CheckBox> bossCheckBoxMap;


        public Worldbosses()
        {
            InitializeComponent();
            
            bossCheckBoxMap = new Dictionary<string, CheckBox>();
            InitializeBossCheckBoxMap();
            UpdateBossUiBosses();
            LoadConfigText(Runinfo, Squadinfo, Guild, Welcome, Symbols);

            Load += Worldbosses_Load_1;
        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {
            SetBossListFromConfig_Bosses();
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
            BossTimerService.Update();
        }

        private void DwanyButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Statue of Dwanya");
            BossTimerService.Update();
        }

        private void LyssaButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Priestess of Lyssa");
            BossTimerService.Update();
        }

        private void OgresButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Ogrewars");
            BossTimerService.Update();
        }

        private void RhendakButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Rhendak");
            BossTimerService.Update();
        }

        private void CommissarButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Dredge Commissar");
            BossTimerService.Update();
        }

        private void GeneralsButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Branded Generals");
            BossTimerService.Update();
        }

        private void ArahButton_Click(object sender, EventArgs e)
        {
            DynamicEventManager.Trigger("Gates of Arah");
            BossTimerService.Update();
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
            string[] bossNames = ["Battle in Tarir", "Octovine"];



            if (Tarir.Checked)
            {
                SaveBossNameToConfig(bossNames);
            }
            else
            {
                RemoveBossNameFromConfig(bossNames);
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

        private static void SaveBossNameToConfig(string[] bossNames)
        {
            foreach (var bossName in bossNames)
            {
                SaveBossNameToConfig(bossName);
            }
        }

        private static void SaveBossNameToConfig(string bossName)
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string[] ReadConfigFile()
        {
            try
            {
                if (File.Exists(GlobalVariables.FILE_PATH))
                {
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
                        Console.WriteLine($"Error creating config file: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from config file: {ex.Message}");
                throw;
            }
        }

        private static void RemoveBossNameFromConfig(string[] bossNames)
        {
            foreach (var bossName in bossNames)
            {
                RemoveBossNameFromConfig(bossName);
            }
        }

        public static void RemoveBossNameFromConfig(string bossName)
        {
            try
            {
                string[] lines = ReadConfigFile();

                if (lines == null)
                {
                    return;
                }

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
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    List<string> bossNames = bossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())
                        .ToList();

                    bossNames.Remove(bossName);

                    lines[bossIndex] = $"Bosses: \"{string.Join(", ", bossNames)}\"";
                    WriteConfigFile(lines);

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing boss {bossName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Meta_Click_1(object sender, EventArgs e)
        {
            try
            {
                ClearAll_Click(null, EventArgs.Empty);
                string[] lines = ReadConfigFile();

                int metaIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Meta:"))
                    {
                        metaIndex = i;
                        break;
                    }
                }

                if (metaIndex != -1 && metaIndex < lines.Length)
                {
                    string metaBossLine = lines[metaIndex].Replace("Meta:", "").Trim();

                    string[] metaBosses = metaBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();
                    CheckBossCheckboxes(metaBosses);
                    UpdateBossesSection(metaBosses, lines);

                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_META, "Meta");
                    Meta_Click_1(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Meta bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBossesSection(string[] metaBosses, string[] lines)
        {
            int bossesIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    bossesIndex = i;
                    break;
                }
            }

            if (bossesIndex != -1 && bossesIndex < lines.Length)
            {
                lines[bossesIndex] = $"Bosses: \"{string.Join(", ", metaBosses)}\"";
            }

        }



        public static void CheckAllBossCheckboxes()
        {
            try
            {
                foreach (CheckBox checkBox in bossCheckBoxMap.Values)
                {
                    checkBox.Checked = true;
                    checkBox.Invalidate();
                }

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
                    lines[bossIndex] = $"Bosses: \"{string.Join(", ", BossEventGroups.Select(group => group.BossName).ToList())}\"";
                    WriteConfigFile(lines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking all bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            int bossesIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    bossesIndex = i;
                    break;
                }
            }

            if (bossesIndex != -1 && bossesIndex < lines.Length)
            {
                lines[bossesIndex] = $"Bosses: \"{string.Join(", ", metaBosses)}\"";
            }
        }



        private void Mixed_Click_1(object sender, EventArgs e)
        {
            try
            {
                ClearAll_Click(null, EventArgs.Empty);


                string[] lines = ReadConfigFile();

                int mixedIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Mixed:"))
                    {
                        mixedIndex = i;
                        break;
                    }
                }

                if (mixedIndex != -1 && mixedIndex < lines.Length)
                {
                    string mixedBossLine = lines[mixedIndex].Replace("Mixed:", "").Trim();

                    string[] mixedBosses = mixedBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    CheckBossCheckboxes(mixedBosses);
                    UpdateBossesSection2(mixedBosses, lines);
                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_MIXED, "Mixed");
                    Mixed_Click_1(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Mixed bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateBossesSection2(string[] mixedBosses, string[] lines)
        {
            int mixedIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Bosses:"))
                {
                    mixedIndex = i;
                    break;
                }
            }

            if (mixedIndex != -1 && mixedIndex < lines.Length)
            {
                lines[mixedIndex] = $"Bosses: \"{string.Join(", ", mixedBosses)}\"";
            }
        }


        public static void SetBossListFromConfig_Bosses()
        {
            try
            {
                string[] lines = ReadConfigFile();

                if (lines == null)
                {
                    return;
                }

                int bossIndex = Array.FindIndex(lines, line => line.StartsWith("Bosses:"));

                if (bossIndex != -1 && bossIndex < lines.Length)
                {
                    string bossLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    List<string> bossNames = bossLine
                        .Trim('"')  // Entferne äußere Anführungszeichen
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(name => name.Trim())
                        .ToList();  // Konvertiere in eine Liste

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



        public static string getConfigLineForItem(string configItem)
        {
            string[] lines = ReadConfigFile();

            // Index of the line with the Meta bosses
            int versionIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(configItem + ":"))
                {
                    versionIndex = i;
                    break;
                }
            }
            if (versionIndex != -1 && versionIndex < lines.Length)
            {
                // Extract the bosses from the Meta line
                return lines[versionIndex].Replace(configItem + ":", "").Trim();


            }
            else
            {
                SaveTextToFile("1", configItem);
                return getConfigLineForItem(configItem);
            }
        }

        private void FidosSpecial_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll_Click(null, EventArgs.Empty);


                string[] lines = ReadConfigFile();

                int worldIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Fido:"))
                    {
                        worldIndex = i;
                        break;
                    }
                }

                if (worldIndex != -1 && worldIndex < lines.Length)
                {
                    string worldBossLine = lines[worldIndex].Replace("Fido:", "").Trim();

                    string[] worldBosses = worldBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    CheckBossCheckboxes(worldBosses);
                    UpdateBossesSection1(worldBosses, lines);
                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_WORLD, "Fido");
                    World_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading World bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void World_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll_Click(null, EventArgs.Empty);


                string[] lines = ReadConfigFile();

                int worldIndex = -1;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("World:"))
                    {
                        worldIndex = i;
                        break;
                    }
                }

                if (worldIndex != -1 && worldIndex < lines.Length)
                {
                    string worldBossLine = lines[worldIndex].Replace("World:", "").Trim();

                    string[] worldBosses = worldBossLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    CheckBossCheckboxes(worldBosses);
                    UpdateBossesSection1(worldBosses, lines);
                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_WORLD, "World");
                    World_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading World bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (CheckBox checkBox in bossCheckBoxMap.Values)
                {
                    checkBox.Checked = false;
                    checkBox.Invalidate();
                }

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
                    lines[bossIndex] = "Bosses: ";
                    WriteConfigFile(lines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unchecking all bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                CheckAllBossCheckboxes();
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
                    string bossesLine = lines[bossIndex].Replace("Bosses:", "").Trim();

                    string[] worldBosses = bossesLine
                        .Trim('"')
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(boss => boss.Trim())
                        .ToArray();

                    CheckBossCheckboxes(worldBosses);
                    UpdateBossesSection1(worldBosses, lines);
                }
                else
                {
                    SaveTextToFile(GlobalVariables.DEFAULT_BOSSES, "Bosses");
                    World_Click(sender, e);
                    MessageBox.Show($"World section not found in config.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading World bosses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button69_Click(object sender, EventArgs e)
        {
            if (_overlayWindow == null)
            {
                _overlayWindow = new OverlayWindow();
                _overlayWindow.Closed += (s, args) => _overlayWindow = null; // Clean up on close
            }

            if (_overlayWindow.IsVisible)
            {
                _overlayWindow.Hide();
            }
            else
            {
                _overlayWindow.Show();
            }
        }

        public static void WriteConfigFile(string[] lines)
        {
            try
            {
                string[] existingLines = ReadConfigFile();

                File.WriteAllLines(GlobalVariables.FILE_PATH, lines);

                UpdateBossUiBosses();



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to config file: {ex.Message}");
                throw new Exception($"Error writing to config file: {ex.Message}");
            }
        }

        public static void UpdateBossUiBosses()
        {
            BossTimings.SetBossListFromConfig_Bosses();
            if (CustomBossList != null)
            {
                BossTimer.UpdateBossList();
            }
        }

    }
}

