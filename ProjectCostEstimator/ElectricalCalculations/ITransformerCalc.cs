using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCostEstimator.ElectricalCalculations
{
    public interface ITransformerCalc
    {
        double Sk(double S, double Ek);
        double Z(double Ek, double Voltage, double S);
        double Ik(double Sk, double Voltage);
    }
}
