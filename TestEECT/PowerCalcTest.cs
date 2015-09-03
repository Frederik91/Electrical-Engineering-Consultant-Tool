using EECT.ElectricalCalculations;
using EECT.Model;
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
        public void TestTransformerImpedance_WithArgumentFromExaple_ReturnsAnswerFromExample()
        {
            var PC = new PowerCalc();

            var GTD = new GridAndTransformerData
                {
                GridVoltage = 20000,
                GridIk = 10000,
                TransformerPowerRating = 400000,
                TransformerVoltageLow = 410,
                Ek = 0.04,
                TransformerFullLoadLoss = 4600,
                TransfomerX0 = 0.95,
                TransformerRo = 1
            };

            double Tolerance = 1.1;

            double Cmax = 1.05;

            var Impedance = new Complex(0.00464, 0.01546);

            var res = PC.TransformerImpedance(GTD, Cmax, PC.GridImpedance(GTD, Tolerance).Magnitude);

            Assert.AreEqual(Math.Round(Impedance.Magnitude,4), Math.Round(res.Magnitude,4));

        }

        [TestMethod]
        public void CableImpedance_WithArgumentFromExaple_ReturnsAnswerFromExample()
        {
            var CDH = new CableDataHandler();

            var CP = new CableProperties
            {
                CableData = new CableData
                {
                    Phases = 3,
                    Rpos = 0.208,
                    Lpos = 0.068,
                },
                NumberOfCables = 2,
                Length = 4,
            };


            var Impedance = new Complex(0.000416, 0.000136);

            var res = CDH.GetCableImpedance(CP);

            Assert.AreEqual(Math.Round(Impedance.Magnitude, 4), Math.Round(res.Magnitude, 4));

        }

        [TestMethod]
        public void Ik3pPeak_WithArgumentFromExaple_ReturnsAnswerFromExample()
        {
            var PC = new PowerCalc();

            var CP = new CableProperties
            {
                CableData = new CableData
                {
                    Phases = 3,
                    Rpos = 0.208,
                    Lpos = 0.068,
                },
                NumberOfCables = 2,
                Length = 4,
            };


            double expected = 27947;

            var res = PC.Ik3pPeak(new Complex(0.00518, 0.01637),PC.Ik3p(400, new Complex(0.00518, 0.01637),1.05));

            Assert.AreEqual(Math.Round(expected, 0), Math.Round(res, 0));

        }

        //private CableProperties getCableList()
        //{
        //    var CablePr

        //}

        [TestMethod]

        public void CableValues()
        {

        }

    }
}
