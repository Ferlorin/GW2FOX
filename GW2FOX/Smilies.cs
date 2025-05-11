using static GW2FOX.BossTimerService;
using static GW2FOX.BossTimings;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace GW2FOX
{
    public partial class Smilies : BaseForm
    {
        private OverlayWindow _parent;
        public Smilies(OverlayWindow parent)
        {
            _parent = parent;
            InitializeComponent();
            LoadConfigText(Symbols);

        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        protected void LoadConfigText(
   System.Windows.Forms.TextBox symbolsBox)
        {

            var config = BossTimings.LoadedConfigInfos;

            if (config == null)
            {
                return;
            }

            symbolsBox.Text = (config.Symbols ?? "").Replace("\n", Environment.NewLine);

        }
    }
}
