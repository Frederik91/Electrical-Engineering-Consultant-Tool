using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.Model
{
    public class GridAndTransformerData
    {
        public double GridVoltage { get; set; }
        public double GridIk { get; set; }
        public double TransformerPowerRating { get; set; }
        public double TransformerVoltageLow { get; set; }
        public double Ek { get; set; }
        public double TransformerR0 { get; set; }
        public double TransformerX0 { get; set; }
        public double TransformerFullLoadLoss { get; set; }
    }
}
