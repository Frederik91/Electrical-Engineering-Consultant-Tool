using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectCostEstimator.ViewModel;
using Moq;
using ProjectCostEstimator.Power;

namespace TestEECT
{
    [TestClass]
    public class LogicTests
    {
        //[TestMethod]
        //public void CalculationAdd_Numbers2And3_Returns5()
        //{
        //    var l = new Logic();

        //    var res = l.CalculationAdd(2,3);

        //    Assert.AreEqual(5,res);
        //}
        ////TODO: Se på mock (moq) NugetPAkke
        //[TestMethod]
        //public void CalculationAdd_Numbers4And4_Returns8()
        //{
        //    var l = new Logic();

        //    var res = l.CalculationAdd(4, 4);

        //    Assert.AreEqual(8, res);
        //}

        [TestMethod]
        public void Calculate_Args_Returns5()
        {
            var PowerCalculationMock = new Mock<IPowerCalculations>();
            PowerCalculationMock.Setup(p => p.Current(1, 2, 1)).Returns(0.5);

            var Logic = new Logic(PowerCalculationMock.Object);

           
            var result = Logic.Calculate(1,2,1);

            Assert.AreEqual(0.5,result);
        }
    }
}
