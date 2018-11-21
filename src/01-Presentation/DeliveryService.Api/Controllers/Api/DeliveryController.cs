using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DeliveryService.Application.Query;
using DeliveryService.Crosscutting.Response;
using System.Net;

namespace DeliveryService.Api.Controllers
{
    /// <summary>
    /// Controller responsable to get the best route to delivery
    /// </summary>
    [Route("delivery")]
    public class DeliveryController : Controller
    {
        private readonly IQuery<GetRouteByPointOriginAndDestinationQueryRequest, GetRouteByPointOriginAndDestinationQueryResponse> _getRouteByPoint;
        private readonly IQuery<GetBestTimeByPointOriginAndDestinationQueryRequest, IEnumerable<Route>> _getBestTimeByPoint;
        private readonly IQuery<GetBestCostByPointOriginAndDestinationQueryRequest, IEnumerable<Route>> _getBestCostByPoint;

        /// <summary>
        ///  Controller responsable to get the best route to delivery
        /// </summary>
        /// <param name="getRouteByPoint"></param>
        /// <param name="getBestTimeByPoint"></param>
        /// <param name="getBestCostByPoint"></param>
        public DeliveryController(
            IQuery<GetRouteByPointOriginAndDestinationQueryRequest, GetRouteByPointOriginAndDestinationQueryResponse> getRouteByPoint,
            IQuery<GetBestTimeByPointOriginAndDestinationQueryRequest, IEnumerable<Route>> getBestTimeByPoint,
            IQuery<GetBestCostByPointOriginAndDestinationQueryRequest, IEnumerable<Route>> getBestCostByPoint)
        {
            _getRouteByPoint  = getRouteByPoint;
            _getBestTimeByPoint = getBestTimeByPoint;
            _getBestCostByPoint = getBestCostByPoint;
        }

        /// <summary>
        /// Get the best route time and best route cost to delivery starting origin and destination
        /// </summary>
        /// <param name="originCode">Point code where your origin delivers</param>
        /// <param name="destinationCode">Point code where your destination delivers</param>
        /// <returns>The best route time and best route cost </returns>
        /// <response code="400">There is no route for this points / Origin and Destination have the same code</response>
        [ProducesResponseType(typeof(GetRouteByPointOriginAndDestinationQueryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{originCode}/{destinationCode}")]
        public IActionResult Get(string originCode, string destinationCode)
        {
            return Ok(_getRouteByPoint.Execute(new GetRouteByPointOriginAndDestinationQueryRequest
            {
                PointOriginCode = originCode,
                PointDestinationCode = destinationCode
            }));
        }

        /// <summary>
        /// Get the best route time to deliver starting origin and destination
        /// </summary>
        /// <param name="originCode">Point code where your origin delivers</param>
        /// <param name="destinationCode">Point code where your destination delivers</param>
        /// <returns>The best route time </returns>
        /// <response code="400">There is no route for this points / Origin and Destination have the same code</response>
        [ProducesResponseType(typeof(IEnumerable<Route>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{originCode}/{destinationCode}/bestTime")]
        public IActionResult GetBestTime(string originCode, string destinationCode)
        {
            return Ok(_getBestTimeByPoint.Execute(new GetBestTimeByPointOriginAndDestinationQueryRequest
            {
                PointOriginCode = originCode,
                PointDestinationCode = destinationCode
            }));
        }

        /// <summary>
        /// Get the best route cost to deliver starting origin and destination
        /// </summary>
        /// <param name="originCode">Point code where your origin delivers</param>
        /// <param name="destinationCode">Point code where your destination delivers</param>
        /// <returns>The best route cost </returns>
        /// <response code="400">There is no route for this points / Origin and Destination have the same code</response>
        [ProducesResponseType(typeof(IEnumerable<Route>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{originCode}/{destinationCode}/bestCost")]
        public IActionResult GetBestCost(string originCode, string destinationCode)
        {
            return Ok(_getBestCostByPoint.Execute(new GetBestCostByPointOriginAndDestinationQueryRequest
            {
                PointOriginCode = originCode,
                PointDestinationCode = destinationCode
            }));
        }

    }
}