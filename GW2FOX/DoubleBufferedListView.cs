using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace GW2FOX
{
    public class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView()
        {
            // Doppelpufferung aktivieren
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
