using EECT.ElectricalCalculations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestEECT
{
    [TestClass]
    public class PowerCalcTest
    {
        [TestMethod]
        public void TestEqualParallelImpedances_WithArgumentRootHalf_ReturnsHalf()
        {
            PowerCalc PC = new PowerCalc();

            var Impedance = new Complex(Math.Sqrt(1), Math.Sqrt(1));

            var res = PC.EqualParallelImpedances(Impedance, 2);

            Assert.AreEqual(Math.Sqrt(1+1)/2, res.Magnitude);

        }

    }
}
