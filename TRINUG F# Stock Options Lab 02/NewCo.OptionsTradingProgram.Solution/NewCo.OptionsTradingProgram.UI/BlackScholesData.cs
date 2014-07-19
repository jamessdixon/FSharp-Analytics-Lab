using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCo.OptionsTradingProgram.UI
{
    public class BlackScholesData
    {
        public Double StrikePrice { get; set; }
        public Double Call { get; set; }
        public Double Put { get; set; }
        public Double MonteCarlo { get; set; }

    }
}
