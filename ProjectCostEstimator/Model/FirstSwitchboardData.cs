using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EECT.Model
{
    public class FirstSwitchboardData
    {
        public double Ik3pmax { get; set; }
        public double Ik3pmin { get; set; }
        public double CosPhi3pmax { get; set; }
        public double Rplus3pmax { get; set; }
        public double Xplus3pmax { get; set; }

        public double Ik2pmax { get; set; }
        public double Ik2pmin { get; set; }
        public double CosPhi2pmin { get; set; }
        public double Rplus2pmin { get; set; }
        public double Xplus2pmin { get; set; }

        public Complex Z3pmax { get; set; }
        public Complex Z3pmin { get; set; }
        public Complex Z2pmax { get; set; }
        public Complex Z2pmin { get; set; }
    }
}
