using DeliveryService.Application.Query.Delivery.BestRouteBridge.BestRouteImplementor.Model;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Application.Query.Delivery.BestRouteBridge.BestRouteImplementor
{
    public class BestRouteOptimoImp : IBestRouteImplementor
    {
        private string _origin;
        private string _destination;
        private List<BestRouteOptimoRequest> _cacheBestRouteOptimoModelStatic = new List<BestRouteOptimoRequest>();
        private List<BestRouteOptimoRequest> _cacheBestRouteOptimoModelAll = new List<BestRouteOptimoRequest>();

        public GetRouteByPointOriginAndDestinationQueryResponse Get(IEnumerable<Route> grafo, string origin, string destination)
        {
            _origin = origin;
            _destination = destination;
            if (origin.Equals(destination))
            {
                return null;
            }

            foreach (var route in grafo)
            {
                _cacheBestRouteOptimoModelAll.Add(new BestRouteOptimoRequest(route));

                var bestRouteOptimoModelToChange = _cacheBestRouteOptimoModelAll.Where(p =>
                                            p.Origin.Equals(route.PointToCode) || p.Destination.Equals(route.PointFromCode));

                if (!bestRouteOptimoModelToChange.Any())
                {
                    _cacheBestRouteOptimoModelAll.Add(new BestRouteOptimoRequest(route));
                    continue;
                }

                foreach (var changes in bestRouteOptimoModelToChange)
                {
                    _cacheBestRouteOptimoModelStatic.Add(new BestRouteOptimoRequest(changes));

                    UpdateModel(changes, route);
                }
            }
            _cacheBestRouteOptimoModelAll.AddRange(_cacheBestRouteOptimoModelStatic);

            return GetResponse();
        }

        private GetRouteByPointOriginAndDestinationQueryResponse GetResponse()
        {
            GetRouteByPointOriginAndDestinationQueryResponse result = new GetRouteByPointOriginAndDestinationQueryResponse();
            result.MinCost = double.MaxValue;
            result.MinTime = double.MaxValue;

            foreach (var modelStatic in _cacheBestRouteOptimoModelStatic)
            {
                foreach (var model in _cacheBestRouteOptimoModelAll)
                {
                    BestRouteOptimoRequest candidate = GetCandidate(modelStatic, model);
                    UpdateResult(result, candidate);
                }
            }

            _cacheBestRouteOptimoModelStatic.Clear();
            _cacheBestRouteOptimoModelAll.Clear();

            return result;
        }

        private BestRouteOptimoRequest GetCandidate(BestRouteOptimoRequest modelStatic, BestRouteOptimoRequest model)
        {
            if (model.Origin == _origin && model.Destination == _destination && model.Routes.Count > 1)
                return model;

            if (model.Origin == modelStatic.Destination &&
                modelStatic.Origin == _origin &&
                model.Destination == _destination &&
                model.Origin != _origin)
            {
                return GetAggregateResult(model, modelStatic);
            }

            if (model.Destination == modelStatic.Origin &&
                model.Origin == _origin &&
                modelStatic.Destination == _destination &&
                model.Destination != _destination)
            {
                return GetAggregateResult(modelStatic, model);
            }

            return null;
        }

        private BestRouteOptimoRequest GetAggregateResult(BestRouteOptimoRequest end, BestRouteOptimoRequest initial)
        {
            BestRouteOptimoRequest result = new BestRouteOptimoRequest(initial);
            result.Destination = end.Destination;
            result.TotalCost += end.TotalCost;
            result.TotalTime += end.TotalTime;

            foreach (var item in end.Routes)
            {
                result.Routes.AddLast(item);
            }

            return result;
        }

        private void UpdateResult(GetRouteByPointOriginAndDestinationQueryResponse result, BestRouteOptimoRequest model)
        {
            if (model is null)
                return;

            if (result.MinCost > model.TotalCost)
            {
                result.MinCost = model.TotalCost;
                result.BestCost = model.Routes;
            }

            if (result.MinTime > model.TotalTime)
            {
                result.MinTime = model.TotalTime;
                result.BestTime = model.Routes;
            }
        }

        private void UpdateModel(BestRouteOptimoRequest model, Route route)
        {
            if (model.Origin == route.PointToCode && model.Origin != _origin)
            {
                model.Routes.AddFirst(route);
                model.Origin = route.PointFromCode;
                model.TotalCost += route.Cost;
                model.TotalTime += route.Time;
            }
            else if (model.Destination == route.PointFromCode && model.Destination != _destination)
            {
                model.Routes.AddLast(route);
                model.Destination = route.PointToCode;
                model.TotalCost += route.Cost;
                model.TotalTime += route.Time;
            }
        }
    }
}