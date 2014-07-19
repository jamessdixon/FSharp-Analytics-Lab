using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCo.OptionsTradingProgram.UI
{
    public class GreekData
    {
        public Double StrikePrice { get; set; }
        public Double DeltaCall { get; set; }
        public Double DeltaPut { get; set; }
        public Double Gamma { get; set; }
        public Double Vega { get; set; }
        public Double ThetaCall { get; set; }
        public Double ThetaPut { get; set; }
        public Double RhoCall { get; set; }
        public Double RhoPut { get; set; }

    }
}
