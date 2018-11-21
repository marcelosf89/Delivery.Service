using DeliveryService.Application.Command;
using DeliveryService.Crosscutting.Request.RouteManagement;
using DeliveryService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Application.Query;
using DeliveryService.Crosscutting.Response;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DeliveryService.Api.Controllers
{
    /// <summary>
    /// Route Management
    /// </summary>
    [Route("routes")]
    public class RoutesController : Controller
    {
        private readonly IQuery<GetRoutesByFromPointCodeQueryRequest, IEnumerable<Route>> _getRoutesByFromPointCodeQuery;
        private readonly ICommand<SaveRouteRequest> _saveRouteCommand;
        private readonly ICommand<UpdateRouteRequest> _updateRouteCommand;
        private readonly ICommand<DeleteRouteRequest> _deleteRouteCommand;

        public RoutesController
            (
              IQuery<GetRoutesByFromPointCodeQueryRequest, IEnumerable<Route>> getRoutesByFromPointCodeQuery,
              ICommand<SaveRouteRequest> saveRouteCommand,
              ICommand<UpdateRouteRequest> updateRouteCommand,
              ICommand<DeleteRouteRequest> deleteRouteCommand)
        {
            _getRoutesByFromPointCodeQuery = getRoutesByFromPointCodeQuery;
            _saveRouteCommand = saveRouteCommand;
            _updateRouteCommand = updateRouteCommand;
            _deleteRouteCommand = deleteRouteCommand;
        }

        /// <summary>
        /// Get all routes by from point 
        /// </summary>
        /// <param name="pointCode">Point code</param>
        /// <returns>All routes from point code</returns>
        /// <response code="400">TThe routes does not exist</response>
        [ProducesResponseType(typeof(IEnumerable<Route>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{pointCode}")]
        public IActionResult Get(string pointCode)
        {
            IEnumerable<Route> routes = _getRoutesByFromPointCodeQuery.Execute(new GetRoutesByFromPointCodeQueryRequest
            {
                PointFromCode = pointCode
            });

            return Ok(routes);
        }


        /// <summary>
        /// Create new route if it does not exist
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// </remarks>
        /// <param name="request">Model to delete the route</param>
        /// <response code="400">The route already exists / It`s required a cost bigger than 0 / It`s required a time bigger than 0</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Save([FromBody] SaveRouteRequest request)
        {
            _saveRouteCommand.Execute(request);

            return NoContent();
        }

        /// <summary>
        /// Update route 
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// * Parameters that can be updated:
        ///   Cost,
        ///   Time
        /// </remarks>
        /// <param name="request">Model to update the route</param>
        /// <response code="400">The route does not exist / Time or Cost has been fill</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPut()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Update([FromBody] UpdateRouteRequest request)
        {
            _updateRouteCommand.Execute(request);

            return NoContent();
        }



        /// <summary>
        /// Delete point and its associate routes
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// </remarks>
        /// <param name="request">Model to delete the route</param>
        /// <response code="400">The route does not exist</response>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpDelete()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Delete([FromBody] DeleteRouteRequest request)
        {
            _deleteRouteCommand.Execute(request);

            return NoContent();
        }
    }
}