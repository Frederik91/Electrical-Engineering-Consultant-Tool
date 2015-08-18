using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectCostEstimator.ViewModel;

namespace TestEECT
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void CalculationAdd_Numbers2And3_Returns5()
        {
            var l = new Logic();

            var res = l.CalculationAdd(2,3);

            Assert.AreEqual(5,res);
        }
        //TODO: Se på mock (moq) NugetPAkke
        [TestMethod]
        public void CalculationAdd_Numbers4And4_Returns8()
        {
            var l = new Logic();

            var res = l.CalculationAdd(4, 4);

            Assert.AreEqual(8, res);
        }
    }
}
