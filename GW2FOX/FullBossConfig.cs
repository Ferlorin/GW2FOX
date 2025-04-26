using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2FOX
{
    public class FullBossConfig
    {
        public string Runinfo { get; set; }
        public string Squadinfo { get; set; }
        public string Guild { get; set; }
        public string Welcome { get; set; }
        public string Symbols { get; set; }

        public string World { get; set; }
        public string Fido { get; set; }
        public string Meta { get; set; }
        public string Mixed { get; set; }
        public List<string> ChoosenOnes { get; set; }
        public List<Boss> Bosses { get; set; }
    }

}
