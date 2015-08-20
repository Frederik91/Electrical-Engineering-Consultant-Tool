using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCostEstimator.Power
{
   public interface IPowerCalculations
    {
        double Power(double current, double voltage, double phases);
        double Current(double voltage, double power, double phases);
        double Voltage(double current, double power, double phases);
        double TransformerCurrent(double Sk, double voltage);
    }
}
