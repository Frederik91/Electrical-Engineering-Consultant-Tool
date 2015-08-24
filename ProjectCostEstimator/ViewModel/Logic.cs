
using EECT.ElectricalCalculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
   public class Logic
    {
        public IPowerCalc PowerCalculations { get; set; }
        public Logic(IPowerCalc pwc)
        {
            PowerCalculations = pwc;
        }
        public int CalculationAdd(int number1, int number2)
        {
            return number1 + number2;
        }
        public double Calculate(double i, double u, double p)
        {
            return PowerCalculations.Current(i,u,p);
        }
    }

    public class lol
    {
        public lol()
        {

        }
    }
}
