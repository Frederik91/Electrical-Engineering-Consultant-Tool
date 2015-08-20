using ProjectCostEstimator.Power;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCostEstimator.ViewModel
{
   public class Logic
    {
        public IPowerCalculations PowerCalculations { get; set; }
        public Logic(IPowerCalculations pwc)
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
