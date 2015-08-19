using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCostEstimator.Model
{
    class PowerCalculations
    {
        public double Power(double current, double voltage, double phases)
        {
            return Math.Sqrt(phases) * current * voltage;
        }

        public double Current(double voltage, double power, double phases)
        {
            return power / (Math.Sqrt(phases) * voltage);
        }
        
        public double Voltage(double current, double power, double phases)
        {
            return power / (Math.Sqrt(phases) * current);
        }
    }
}
