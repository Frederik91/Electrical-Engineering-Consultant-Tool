using EECT.ElectricalCalculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ElectricalCalculations
{
    public class TransformerCalc : ITransformerCalc
    {
        public double Ik(double Sk, double Voltage)
        {
            return Sk / (Math.Sqrt(3) * Voltage);
        }

        public double Sk(double S, double Ek)
        {
            return S / Ek;
        }
        
        public double Z(double Ek, double Voltage, double S)
        {
            return (Math.Pow(Voltage, 2) / (S/ Ek));
        }
    }
}
