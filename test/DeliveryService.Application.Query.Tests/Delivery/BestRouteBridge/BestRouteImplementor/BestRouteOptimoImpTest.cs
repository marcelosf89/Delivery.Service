using DeliveryService.Application.Query.Delivery.BestRouteBridge.BestRouteImplementor;
using DeliveryService.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Application.Query.Tests.Delivery.BestRouteBridge.BestRouteImplementor
{
    [TestClass]
    public class BestRouteOptimoImpTest
    {
        private readonly IList<Route> _grafo;

        public BestRouteOptimoImpTest()
        {
            _grafo = GetRouteGrafo();
        }

        [TestMethod]
        public void Get_NotExistRoutes_ResultBestTimeNullAndBestCostNull()
        {
            //Arrenge
            BestRouteOptimoImp act = new BestRouteOptimoImp();

            //Act
            var result = act.Get(_grafo, "a", "x");

            //Assert
            Assert.IsNull(result.BestCost);
            Assert.IsNull(result.BestTime);
        }

        [TestMethod]
        public void Get_SamePointCode_ResultNull()
        {
            //Arrenge
            BestRouteOptimoImp act = new BestRouteOptimoImp();

            //Act
            var result = act.Get(_grafo, "a", "a");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Get_WithDistanceWithOneVertice_ResultBestTimeNullAndBestCostNull()
        {
            //Arrenge
            BestRouteOptimoImp act = new BestRouteOptimoImp();

            //Act
            var result = act.Get(_grafo, "a", "c");

            //Assert
            Assert.IsNull(result.BestCost);
            Assert.IsNull(result.BestTime);
        }

        [TestMethod]
        public void Get_Ok_ResultBestTimeDiferentBestCost()
        {
            //Arrenge
            BestRouteOptimoImp act = new BestRouteOptimoImp();

            //Act
            var result = act.Get(_grafo, "a", "b");

            //Assert
            Assert.IsNotNull(result.BestTime);
            Assert.AreEqual(4, result.MinTime);
            Assert.AreEqual("c", result.BestTime.ElementAt(0).PointToCode);
            Assert.AreEqual("b", result.BestTime.ElementAt(1).PointToCode);

            Assert.IsNotNull(result.BestCost);
            Assert.AreEqual(2, result.MinCost);
            Assert.AreEqual("z", result.BestCost.ElementAt(0).PointToCode);
            Assert.AreEqual("b", result.BestCost.ElementAt(1).PointToCode);
        }

        [TestMethod]
        public void Get_Ok_TwoRoutesSameCostAndTime_ResultOneBestTimeAndBestCost()
        {
            //Arrenge
            BestRouteOptimoImp act = new BestRouteOptimoImp();

            //Act
            var result = act.Get(_grafo, "a", "m");

            //Assert
            Assert.IsNotNull(result.BestTime);
            Assert.IsNotNull(result.BestCost);
            Assert.AreEqual(20, result.MinTime);
            Assert.AreEqual(2, result.MinCost);

            Assert.AreEqual("a", result.BestTime.ElementAt(0).PointFromCode);
            Assert.AreEqual("m", result.BestTime.ElementAt(1).PointToCode);
            if (result.BestTime.ElementAt(0).PointToCode.Equals("x"))
            {
                Assert.AreEqual("x", result.BestTime.ElementAt(0).PointToCode);
                Assert.AreEqual("x", result.BestTime.ElementAt(1).PointFromCode);
            }
            else
            {
                Assert.AreEqual("y", result.BestTime.ElementAt(0).PointToCode);
                Assert.AreEqual("y", result.BestTime.ElementAt(1).PointFromCode);
            }
        }

        private IList<Route> GetRouteGrafo()
        {
            return new List<Route>()
            {
                new Route(){ PointFromCode = "a", PointToCode = "c", Cost = 20, Time= 1},
                new Route(){ PointFromCode = "a", PointToCode = "h", Cost = 1, Time= 10},
                new Route(){ PointFromCode = "a", PointToCode = "e", Cost = 5, Time= 30},
                new Route(){ PointFromCode = "h", PointToCode = "e", Cost = 1, Time= 30},
                new Route(){ PointFromCode = "e", PointToCode = "d", Cost = 5, Time= 3},
                new Route(){ PointFromCode = "d", PointToCode = "f", Cost = 50, Time= 4},
                new Route(){ PointFromCode = "f", PointToCode = "i", Cost = 50, Time= 45},
                new Route(){ PointFromCode = "f", PointToCode = "g", Cost = 50, Time= 40},
                new Route(){ PointFromCode = "c", PointToCode = "b", Cost = 12, Time= 3},
                new Route(){ PointFromCode = "i", PointToCode = "b", Cost = 5, Time= 65},
                new Route(){ PointFromCode = "g", PointToCode = "b", Cost = 73, Time= 64},
                new Route(){ PointFromCode = "a", PointToCode = "z", Cost = 1, Time= 64},
                new Route(){ PointFromCode = "z", PointToCode = "b", Cost = 1, Time= 100},

                new Route(){ PointFromCode = "y", PointToCode = "m", Cost = 1, Time= 10},
                new Route(){ PointFromCode = "x", PointToCode = "m", Cost = 1, Time= 10},
                new Route(){ PointFromCode = "a", PointToCode = "y", Cost = 1, Time= 10},
                new Route(){ PointFromCode = "a", PointToCode = "x", Cost = 1, Time= 10},

            };
        }
    }
}
