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
            Load += Worldbosses_Load_1;
        }

        private void Worldbosses_Load_1(object? sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point(screen.Width - this.Width, 0);
            InitializeBossCheckBoxMap();
            LoadBossBoxesFromJson();
            UpdateBoxes();
        }

        protected override void AfterControlsLoaded()
        {
            base.AfterControlsLoaded();
        }

        const int SW_RESTORE = 9;

        private void TheOilFloes_Click(object sender, EventArgs e)
        {
            Oil.Checked = !Oil.Checked;
        }

        private void Behe_Click(object sender, EventArgs e)
        {
            Behemoth.Checked = !Behemoth.Checked;
        }

        private void Fireelemental_Click(object sender, EventArgs e)
        {
            Fire_Elemental.Checked = !Fire_Elemental.Checked;
        }

        private void Junglewurm_Click(object sender, EventArgs e)
        {
            JungleWurm.Checked = !JungleWurm.Checked;
        }


        private void Ulgoth_Click(object sender, EventArgs e)
        {
            Ulgoth.Checked = !Ulgoth.Checked;
        }

        private void Thaida_Click(object sender, EventArgs e)
        {
            Thaida.Checked = !Thaida.Checked;
        }

        private void Fireshaman_Click(object sender, EventArgs e)
        {
            FireShaman.Checked = !FireShaman.Checked;
        }

        private void Megadestroyer_Click(object sender, EventArgs e)
        {
            Megadestroyer.Checked = !Megadestroyer.Checked;
        }

        private void Karkaqueen_Click(object sender, EventArgs e)
        {
            Karka.Checked = !Karka.Checked;
        }

        private void Dragonstorm_Click(object sender, EventArgs e)
        {
            Dragonstorm.Checked = !Dragonstorm.Checked;
        }

        private void Drakkar_Click(object sender, EventArgs e)
        {
            Drakkar.Checked = !Drakkar.Checked;
        }

        private void Effigy_Click(object sender, EventArgs e)
        {
            Effigy.Checked = !Effigy.Checked;
        }

        private void DoomloreShrine_Click(object sender, EventArgs e)
        {
            Doomlore.Checked = !Doomlore.Checked;
        }

        private void StormsOfWinter_Click(object sender, EventArgs e)
        {
            SormsOfWinter.Checked = !SormsOfWinter.Checked;
        }

        private void DefendJorasKeep_Click(object sender, EventArgs e)
        {
            JorasKeep.Checked = !JorasKeep.Checked;
        }

        private void Sandstorm_Click(object sender, EventArgs e)
        {
            Sandstorm.Checked = !Sandstorm.Checked;
        }

        private void SaidrasHaven_Click(object sender, EventArgs e)
        {
            SaidrasHeaven.Checked = !SaidrasHeaven.Checked;
        }

        private void NewLoamhurst_Click(object sender, EventArgs e)
        {
            Loamhurst.Checked = !Loamhurst.Checked;
        }

        private void NoransHomestead_Click(object sender, EventArgs e)
        {
            Homestead.Checked = !Homestead.Checked;
        }

        private void AetherbladeAssault_Click(object sender, EventArgs e)
        {
            Atherblade.Checked = !Atherblade.Checked;
        }

        private void KainengBlackout_Click(object sender, EventArgs e)
        {
            Blackout.Checked = !Blackout.Checked;
        }

        private void GangWar_Click(object sender, EventArgs e)
        {
            GangWar.Checked = !GangWar.Checked;
        }

        private void Aspenwood_Click(object sender, EventArgs e)
        {
            Aspenwood.Checked = !Aspenwood.Checked;
        }

        private void JadeSea_Click(object sender, EventArgs e)
        {
            JadeSea.Checked = !JadeSea.Checked;
        }

        private void WizardsTower_Click(object sender, EventArgs e)
        {
            WizzardsTower.Checked = !WizzardsTower.Checked;
        }

        private void FlyByNight_Click(object sender, EventArgs e)
        {
            Flybynigtht.Checked = !Flybynigtht.Checked;
        }

        private void DefenseOfAmnytas_Click(object sender, EventArgs e)
        {
            Amnytas.Checked = !Amnytas.Checked;
        }

        private void Convergences_Click(object sender, EventArgs e)
        {
            Convergence.Checked = !Convergence.Checked;
        }


        private void Tequatl_Click(object sender, EventArgs e)
        {
            Tequatl.Checked = !Tequatl.Checked;
        }

        private void TheShatterer_Click(object sender, EventArgs e)
        {
            Shatterer.Checked = !Shatterer.Checked;
        }

        private void Inquestgolem_Click(object sender, EventArgs e)
        {
            MarkTwo.Checked = !MarkTwo.Checked;
        }

        private void Clawjormag_Click(object sender, EventArgs e)
        {
            Claw.Checked = !Claw.Checked;
        }

        private void Maw_Click(object sender, EventArgs e)
        {
            Maw.Checked = !Maw.Checked;
        }

        private void Concert_Click(object sender, EventArgs e)
        {
            Metalconcert.Checked = !Metalconcert.Checked;
        }


        private void button68_Click(object sender, EventArgs e)
        {
            LLA.Checked = !LLA.Checked;
        }


        private void MawsOfTorment_Click(object sender, EventArgs e)
        {
            MawsOfTorment.Checked = !MawsOfTorment.Checked;
        }

        private void Chak_Click(object sender, EventArgs e)
        {
            Chak.Checked = !Chak.Checked;
        }

        private void Tarir_Click(object sender, EventArgs e)
        {
            Tarir.Checked = !Tarir.Checked;
        }

        private void Massen_Click(object sender, EventArgs e)
        {
            Mascen.Checked = !Mascen.Checked;
        }

        private void DBS_Click(object sender, EventArgs e)
        {
            DBS.Checked = !DBS.Checked;
        }

        private void Junundo_Click(object sender, EventArgs e)
        {
            Junundu.Checked = !Junundu.Checked;
        }

        private void PTA_Click(object sender, EventArgs e)
        {
            PTA.Checked = !PTA.Checked;
        }

        private void Doppel_Click(object sender, EventArgs e)
        {
            Doppelganger.Checked = !Doppelganger.Checked;
        }

        private void Forged_Click(object sender, EventArgs e)
        {
            Doggies.Checked = !Doggies.Checked;
        }

        private void Pinata_Click(object sender, EventArgs e)
        {
            Pinata.Checked = !Pinata.Checked;
        }



        private void OozePits_Click(object sender, EventArgs e)
        {
            OozePits.Checked = !OozePits.Checked;
        }

        private void SerpentsIre_Click(object sender, EventArgs e)
        {
            SerpentsIre.Checked = !SerpentsIre.Checked;
        }

        private void DragonsStand_Click(object sender, EventArgs e)
        {
            DS.Checked = !DS.Checked;
        }

        private void Palawadan_Click(object sender, EventArgs e)
        {
            Palawadan.Checked = !Palawadan.Checked;
        }

        private void ThunderheadKeep_Click(object sender, EventArgs e)
        {
            ThunderheadKeep.Checked = !ThunderheadKeep.Checked;
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
            string bossName = "Nightbosses";



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
        public string Level { get; set; } = "";


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


        public static void AddBossEvent(string bossName, string[] timings, string category, string waypoint = "", string level = "")
        {
            foreach (var timing in timings)
            {
                var utcTime = ConvertToUtcFromConfigTime(timing);
                BossEventsList.Add(new BossEvent(bossName, utcTime.TimeOfDay, category, waypoint, level));
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
                UpdateBossUiBosses();
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
                UpdateBossUiBosses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der World-Gruppe: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ClearAll_Click(object sender, EventArgs e)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BossTimings.json");

            try
            {
                JObject json;
                if (File.Exists(configPath))
                {
                    json = JObject.Parse(await File.ReadAllTextAsync(configPath));
                }
                else
                {
                    json = new JObject();
                }

                json["ChoosenOnes"] = new JArray();

                // Sicheres Schreiben mit Retry (für Edge-Fälle, z. B. Datei noch in Benutzung)
                const int maxRetries = 5;
                int retries = 0;
                bool writeSuccess = false;

                while (!writeSuccess && retries < maxRetries)
                {
                    try
                    {
                        await File.WriteAllTextAsync(configPath, json.ToString(Formatting.Indented));
                        writeSuccess = true;
                    }
                    catch (IOException)
                    {
                        retries++;
                        await Task.Delay(100); // kurz warten und nochmal versuchen
                    }
                }

                if (!writeSuccess)
                {
                    MessageBox.Show("Konnte die Bossliste nicht speichern. Versuche es erneut.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Daten im Speicher leeren
                BossTimings.BossList23.Clear();
                BossTimings.BossEventsList.Clear();
                BossTimings.BossEventGroups.Clear();
                DynamicEventManager.Events.Clear();

                // Overlay manuell leeren
                OverlayWindow.GetInstance().OverlayItems.Clear();

                // Danach UI neu laden
                await UpdateBossUiBosses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Leeren der Bossliste: " + ex.Message);
            }
        }

        private async void ShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                var config = BossTimings.LoadBossConfigAndReturn();

                // 1. Alle Boss-Namen aus der Konfiguration holen
                var allBossNames = config.Bosses
                    .Where(b => !string.IsNullOrWhiteSpace(b.Name))
                    .Select(b => b.Name)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // 2. Checkboxen aktivieren
                CheckAllBossCheckboxesAll();

                // 3. "ChoosenOnes" in der JSON setzen
                string configPath = "BossTimings.json";
                if (File.Exists(configPath))
                {
                    var json = JObject.Parse(File.ReadAllText(configPath));
                    json["ChoosenOnes"] = new JArray(allBossNames);
                    File.WriteAllText(configPath, json.ToString(Formatting.Indented));
                }

                // 4. Update durchführen
                await UpdateBossUiBosses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Anzeigen aller Bosse: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                UpdateBossUiBosses();
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
        { "Captain Rotbeard", RotbeardBox },
        { "Dredge Commissar", CommissarBox },
        { "Branded Generals", GeneralsBox },
        { "Gates of Arah", ArahBox },
        { "Rhendak", RhendakBox },
        { "Priestess of Lyssa", LyssaBox },
        { "The Eye of Zhaitan", EyeBox },
        { "Statue of Dwanya", DwanyaBox },
        { "Ogrewars", OgreBox },
        };
        }

        public void button65_Click(object sender, EventArgs e)
        {
            RotbeardBox.Checked = !RotbeardBox.Checked;
            DynamicEventManager.TriggerIt("Captain Rotbeard");
            UpdateBossUiBosses();
        }

        public void button63_Click(object sender, EventArgs e)
        {
            CommissarBox.Checked = !CommissarBox.Checked;
            DynamicEventManager.TriggerIt("Dredge Commissar");
            UpdateBossUiBosses();
        }

        public void button42_Click(object sender, EventArgs e)
        {
            GeneralsBox.Checked = !GeneralsBox.Checked;
            DynamicEventManager.TriggerIt("Branded Generals");
            UpdateBossUiBosses();
        }

        public void button31_Click(object sender, EventArgs e)
        {
            ArahBox.Checked = !ArahBox.Checked;
            DynamicEventManager.TriggerIt("Gates of Arah");
            UpdateBossUiBosses();
        }

        public void Rhendak_Click(object sender, EventArgs e)
        {
            RhendakBox.Checked = !RhendakBox.Checked;
            DynamicEventManager.TriggerIt("Rhendak");
            UpdateBossUiBosses();
        }

        public void Lyssa_Click(object sender, EventArgs e)
        {
            LyssaBox.Checked = !LyssaBox.Checked;
            DynamicEventManager.TriggerIt("Priestess of Lyssa");
            UpdateBossUiBosses();
        }

        public void Eye_Click(object sender, EventArgs e)
        {
            EyeBox.Checked = !EyeBox.Checked;
            DynamicEventManager.TriggerIt("The Eye of Zhaitan");
            UpdateBossUiBosses();
        }

        public void Dwayna_Click(object sender, EventArgs e)
        {
            DwanyaBox.Checked = !DwanyaBox.Checked;
            DynamicEventManager.TriggerIt("Statue of Dwanya");
            UpdateBossUiBosses();
        }

        public void Ogrewars_Click(object sender, EventArgs e)
        {
            OgreBox.Checked = !OgreBox.Checked;
            DynamicEventManager.TriggerIt("Ogrewars");
            UpdateBossUiBosses();
        }

        private void SaveBossSelection(string boxText)
        {
            string configPath = "BossTimings.json";

            if (string.IsNullOrEmpty(boxText))
            {
                MessageBox.Show("Give a Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var json = File.Exists(configPath) ? File.ReadAllText(configPath) : "{}";
                var jObj = JObject.Parse(json);

                // Auswahl speichern
                var selectedBossNames = bossCheckBoxMap
                    .Where(entry => entry.Value.Checked)
                    .Select(entry => entry.Key)
                    .ToList();

                jObj[boxText] = new JArray(selectedBossNames);

                // Textbox-Namen ebenfalls speichern
                jObj["BossBox1"] = BossBox1.Text;
                jObj["BossBox2"] = BossBox2.Text;

                File.WriteAllText(configPath, jObj.ToString(Formatting.Indented));
                MessageBox.Show($"Saved under '{boxText}'!", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBossSelection(string boxText)
        {
            string configPath = "BossTimings.json";

            if (string.IsNullOrEmpty(boxText))
            {
                MessageBox.Show("First a Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (!File.Exists(configPath))
                {
                    MessageBox.Show("Json is Missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);

                var selectedBossNames = jObj[boxText] is JArray array
                    ? array.Select(x => x.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entry in bossCheckBoxMap)
                {
                    var checkBox = entry.Value;
                    checkBox.Checked = selectedBossNames.Contains(entry.Key);
                    checkBox.ForeColor = checkBox.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBossBoxesFromJson()
        {
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            try
            {
                var json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);

                BossBox1.Text = jObj["BossBox1"]?.ToString() ?? "Empty";
                BossBox2.Text = jObj["BossBox2"]?.ToString() ?? "Empty";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der BossBoxen: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button-Handler
        private void button4_Click(object sender, EventArgs e) => SaveBossSelection(BossBox1.Text.Trim());
        private void button1_Click_1(object sender, EventArgs e) => SaveBossSelection(BossBox2.Text.Trim());

        private void button67_Click(object sender, EventArgs e) => LoadBossSelection(BossBox1.Text.Trim());
        private void button29_Click(object sender, EventArgs e) => LoadBossSelection(BossBox2.Text.Trim());

        public async Task UpdateBoxes()
        {
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            try
            {
                var json = await File.ReadAllTextAsync(configPath);
                var jObj = JObject.Parse(json);

                var selectedBossNames = jObj["ChoosenOnes"] is JArray array
                    ? array.Select(x => x.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entry in bossCheckBoxMap)
                {
                    var checkBox = entry.Value;
                    if (checkBox != null)
                    {
                        checkBox.Checked = selectedBossNames.Contains(entry.Key);
                        checkBox.ForeColor = checkBox.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                // Optional: Logge den Fehler oder zeige eine Nachricht
                Console.WriteLine("Fehler beim Lesen der Boss-Konfiguration: " + ex.Message);
            }
        }


        public async Task UpdateBossUiBosses()
        {
            string configPath = "BossTimings.json";
            if (!File.Exists(configPath)) return;

            try
            {
                var json = File.ReadAllText(configPath);
                var jObj = JObject.Parse(json);

                var selectedBossNames = jObj["ChoosenOnes"] is JArray array
                    ? array.Select(x => x.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entry in bossCheckBoxMap)
                {
                    var checkBox = entry.Value;
                    if (checkBox != null)
                    {
                        checkBox.Checked = selectedBossNames.Contains(entry.Key);
                        checkBox.ForeColor = checkBox.Checked ? System.Drawing.Color.White : System.Drawing.Color.Gray;
                    }
                }

                BossList23 = selectedBossNames.ToList();

                BossEventsList.Clear();
                BossEventGroups.Clear();

                var config = BossTimings.LoadBossConfigAndReturn();

                foreach (var bossName in BossList23)
                {
                    var boss = config.Bosses.FirstOrDefault(b => b.Name.Equals(bossName, StringComparison.OrdinalIgnoreCase));
                    if (boss != null)
                    {
                        AddBossEvent(boss.Name, boss.Timings.ToArray(), boss.Category ?? "WBs", boss.Waypoint ?? "", boss.Level ?? "");
                    }
                    else
                    {
                        Console.WriteLine($"[WARN] Boss nicht gefunden in config.Bosses: {bossName}");
                    }
                }

                GenerateBossEventGroups();
                await OverlayWindow.GetInstance()
                                 .UpdateBossOverlayListAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden von ChoosenOnes: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RestartApplication()
        {
            Application.Restart();
        }

    }
}

