using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.Model
{
    public class CableProperties
    {
        public CableData CableData { get; set; }
        public int NumberOfCables { get; set; }
        public int Conductors { get; set; }
        public double ImpedanceBehind { get; set; }
    }
}
