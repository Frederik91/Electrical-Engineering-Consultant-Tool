using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using EECT.Model;

namespace EECT.ElectricalCalculations
{
    public class PowerCalc : IPowerCalc
    {
        public Complex CurrentCosPhiToImpedance2p(double Ik, double CosPhi, double Voltage, double Tolerance)
        {
            var Zmagnetude = (Tolerance * Voltage) / Ik;
            var R = Zmagnetude * CosPhi;
            var X = Zmagnetude * Math.Sin(Math.Cosh(CosPhi));

            return new Complex(R, X);
        }

        public Complex CurrentCosPhiToImpedance3p(double Ik, double CosPhi, double Voltage, double Tolerance)
        {
            var Zmagnetude = (Tolerance * Voltage) / (Math.Sqrt(3) * Ik);
            var R = Zmagnetude * CosPhi;
            var X = Zmagnetude * Math.Sin(Math.Cosh(CosPhi));

            return new Complex(R, X);
        }

        public Complex TemperatureCorrectedImpedance(double MaxTemp, Complex Z, double aAL, double aCU, CableProperties cable)
        {
            double tempCoeff = 0;

            switch (cable.CableData.Material)
            {
                case ("AL"):
                    tempCoeff = aAL;
                    break;
                case ("CU"):
                    tempCoeff = aCU;
                    break;
            }

            var correctionFactor = (1 + tempCoeff * (MaxTemp - 20));

            return new Complex(Z.Real * correctionFactor, Z.Imaginary);
        }

        public Complex GridImpedance(GridAndTransformerData GTD, double Tolerance)
        {
            var GridImpedance = ((Tolerance * GTD.GridVoltage) / (Math.Sqrt(3) * GTD.GridIk)) * (Math.Pow(GTD.TransformerVoltageLow / GTD.GridVoltage, 2));
            var Zq = new Complex(0.1 * 0.995 * GridImpedance, 0.995 * GridImpedance);
            return Zq;
        }

        public Complex TransformerImpedance(GridAndTransformerData GTD, double Cmax, double GridImpedance)
        {
            
            
            var Ztmagnitude = GTD.Ek * (Math.Pow(GTD.TransformerVoltageLow, 2) / GTD.TransformerPowerRating);
            var Rt = GTD.TransformerFullLoadLoss * (Math.Pow(GTD.TransformerVoltageLow, 2) / Math.Pow(GTD.TransformerPowerRating, 2));
            var Xt = Math.Sqrt(Math.Pow(Ztmagnitude, 2) - Math.Pow(Rt, 2));

            var _xt = Xt * (GTD.TransformerPowerRating / Math.Pow(GTD.TransformerVoltageLow, 2));

            var Kt = 0.95 * (Cmax / (1 + _xt));

            var Zt = new Complex(Kt * Rt, Kt * Xt);

            return Zt;
        }


        public double IkMultipleImpedances(double Voltage, params Complex[] Impedance)
        {
            Complex TotalImpedance = new Complex();

            foreach (var Z in Impedance)
            {
                TotalImpedance = TotalImpedance + Z;
            }

            return Voltage / (Math.Sqrt(3) * TotalImpedance.Magnitude);
        }

        public double Sk(double Voltage, Complex Z)
        {
            return Math.Pow(Voltage, 2) / Z.Magnitude;

        }

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

        public double Ik3p(double voltage, Complex Z, double Tolerance)
        {
            return (Tolerance * voltage) / (Math.Sqrt(3) * Z.Magnitude);
        }

        public double Ik2p(double voltage, Complex Z, double Tolerance)
        {
            return (Tolerance * voltage) / (2*Z.Magnitude);
        }


        public double Ik3pPeak(Complex Ztotal, double Ik3p)
        {
            var RX = Ztotal.Real / Ztotal.Imaginary;
            var k = 1.02 + 0.98 * Math.Pow(Math.E, -3 * RX);

            return k * Math.Sqrt(2) * Ik3p;
        }

        public double Zk3p(double voltage, double Ik, double Ek)
        {
            return voltage / (Ik * Math.Sqrt(3));
        }

        public double Xk3p(double voltage, double S, double Ex)
        {
            return Math.Pow(voltage, 2) / (S / Ex);
        }

        public double Rk3p(double voltage, double S, double Er)
        {
            return (voltage * voltage) / (S / Er);
        }

        public double CosPhi(double Er, double Ek)
        {
            return Er / Ek;
        }

        public double Ex(double Ek)
        {
            return Math.Sqrt(Math.Pow(Ek, 2) - Math.Pow(0.01, 2));
        }

        public Complex Z3p(double R3p, double X3p)
        {
            return new Complex(R3p, X3p);
        }

        public Complex Z2p(double R2p, double X2p)
        {
            return new Complex(R2p * 2, X2p * 2);
        }

        public Complex Z3Phase1polePE(double R, double X, double R0pen, double X0pen)
        {
            var CablePart = new Complex(R * 2, X * 2);
            var PENPart = new Complex(R0pen, X0pen);

            return CablePart + PENPart;
        }

        public Complex Z1Phase1PolePE(double RphasePEN, double XphasePEN)
        {
            return new Complex(3 * RphasePEN, 3 * XphasePEN);
        }

        public Complex SumImpedances(params Complex[] Z)
        {
            Complex sum = 0;

            foreach (var Impedance in Z)
            {
                sum = sum + Impedance;
            }

            return sum;
        }

        public double SumSk(params double[] Sk)
        {
            double sum = 0;

            foreach (var S in Sk)
            {
                sum = sum + 1 / S;
            }

            return 1/sum;
        }
    }
}
