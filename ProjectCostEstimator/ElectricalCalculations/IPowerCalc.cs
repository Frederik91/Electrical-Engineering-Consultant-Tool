using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ElectricalCalculations
{
   public interface IPowerCalc
    {
        double Power(double current, double voltage, double phases);
        double Current(double voltage, double power, double phases);
        double Voltage(double current, double power, double phases);
        double Zk3p(double voltage, double Ik, double Ek);
        double Xk3p(double voltage, double S, double Ex);
        double Rk3p(double voltage, double S, double Er);
        double CosPhi(double Er, double Ek);
        double Ex(double Ek);
        Complex Z3p(double R, double X);
        Complex Z2p(double R, double X);
        Complex Z3Phase1polePE(double R, double X, double R0pen, double X0pen);
        Complex Z1Phase1PolePE(double RphasePEN, double XphasePEN);
    }
}
