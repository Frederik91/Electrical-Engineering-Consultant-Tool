using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using EECT.ElectricalCalculations;
using ProjectCostEstimator.ElectricalCalculations;

namespace TestEECT
{
    [TestClass]
    public class PowerCalculationsTests
    {
        //[TestMethod]
        //public void CalculationAdd_Numbers2And3_Returns5()
        //{
        //    var l = new PowerCalculations();

        //    var res = l.CalculationAdd(2, 3);

        //    Assert.AreEqual(5, res);
        //}
        ////TODO: Se på mock (moq) NugetPAkke
        //[TestMethod]
        //public void CalculationAdd_Numbers4And4_Returns8()
        //{
        //    var l = new Logic();

        //    var res = l.CalculationAdd(4, 4);

        //    Assert.AreEqual(8, res);
        //}

        //[TestMethod]
        //public void Calculate_Args_Returns5()
        //{
        //    var PowerCalculationMock = new Mock<PowerCalc>();
        //    PowerCalculationMock.Setup(p => p.Current(1, 2, 1)).Returns(0.5);

        //    var Logic = new Logic(PowerCalculationMock.Object);

           
        //    var result = Logic.Calculate(1,2,1);

        //    Assert.AreEqual(0.5,result);
        //}

        [TestMethod]
        public void Calculate_Args_IkTransformer()
        {
            var Trans = new TransformerCalc();
            var res = Trans.Ik(Trans.Sk(1600000, 0.057), 400);

            Assert.AreEqual(40516, Math.Round(res));
        }


        [TestMethod]
        public void Calculate_Args_Zk3p()
        {
            var Trans = new TransformerCalc();

            PowerCalc calc = new PowerCalc();
            var res = calc.Zk3p(400, Trans.Ik(Trans.Sk(1600000,0.057),400), 0.057);

            Assert.AreEqual(0.0057, Math.Round(res, 4));
        }

        [TestMethod]
        public void Calculate_Args_Ex()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Ex(0.057);

            Assert.AreEqual(0.0561, Math.Round(res,4));
        }

        [TestMethod]
        public void Calculate_Args_X3p()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Xk3p(400,1600000,calc.Ex(0.057));

            Assert.AreEqual(0.0056116, Math.Round(res,7));
        }

        [TestMethod]
        public void Calculate_Args_R3p()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Rk3p(400, 1600000, 0.01);

            Assert.AreEqual(0.001, Math.Round(res, 7));
        }

        [TestMethod]
        public void Calculate_Args_CosPhi()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.CosPhi(0.01,0.057);

            Assert.AreEqual(0.1754386, Math.Round(res, 7));
        }

        [TestMethod]
        public void Calculate_Args_Z3p()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Z3p(1, 1);

            Assert.AreEqual(new Complex(1,1), res);
        }

        [TestMethod]
        public void Calculate_Args_Z2p()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Z2p(1, 1);

            Assert.AreEqual(new Complex(2, 2), res);
        }

        [TestMethod]
        public void Calculate_Args_Z3Phase1polePE()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Z3Phase1polePE(1, 1, 1, 1);

            Assert.AreEqual(new Complex(3,3), res);
        }

        [TestMethod]
        public void Calculate_Args_Z1Phase1PolePE()
        {
            PowerCalc calc = new PowerCalc();
            var res = calc.Z1Phase1PolePE(1, 1);

            Assert.AreEqual(new Complex(3, 3), res);
        }
    }
}
