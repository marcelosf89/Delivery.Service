using DeliveryService.Application.Command;
using DeliveryService.Application.Command.PointManager.Models;
using DeliveryService.Application.Query.PointManager.Models;
using DeliveryService.Crosscutting.Request.PointManagement;
using DeliveryService.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Application.Query;
using DeliveryService.Crosscutting.Response;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DeliveryService.Api.Controllers
{
    /// <summary>
    /// Point Management 
    /// </summary>
    [Route("points")]
    public class PointsController : Controller
    {
        private readonly IQuery<GetPointByCodeQueryRequest, Point> _getPointByCodeQuery;
        private readonly ICommand<SavePointRequest> _savePointCommand;
        private readonly ICommand<UpdatePointRequest> _updatePointCommand;
        private readonly ICommand<DeletePointCommnadRequest> _deletePointCommand;

        public PointsController(IQuery<GetPointByCodeQueryRequest, Point> getPointByCodeQuery,
                                ICommand<SavePointRequest> savePointCommand,
                                ICommand<UpdatePointRequest> updatePointCommand,
                                ICommand<DeletePointCommnadRequest> deletePointCommand)
        {
            _getPointByCodeQuery = getPointByCodeQuery;
            _savePointCommand = savePointCommand;
            _deletePointCommand = deletePointCommand;
            _updatePointCommand = updatePointCommand;
        }

        /// <summary>
        /// Get Point by Code
        /// </summary>
        /// <param name="code">Point code</param>
        /// <returns>Point</returns>
        /// <response code="400">The point does not exist</response>
        [ProducesResponseType(typeof(Point), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{code}")]
        public IActionResult Get(string code)
        {
            Point point = _getPointByCodeQuery.Execute(new GetPointByCodeQueryRequest
            {
                Code = code
            });

            return Ok(point);
        }

        /// <summary>
        /// Create new point if it does not exist
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// </remarks>
        /// <param name="request">Model to create the point</param>
        /// <response code="400">The code already exists</response>
        [ProducesResponseType(typeof(Point), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Save([FromBody] SavePointRequest request)
        {
            _savePointCommand.Execute(request);

            return NoContent();
        }


        /// <summary>
        /// Update point 
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// * Parameters that can be updated:
        ///   Description
        /// </remarks>
        /// <param name="request">Model to update the point</param>
        /// <response code="400">The code does not exist</response>
        [ProducesResponseType(typeof(Point), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPut()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Update([FromBody] UpdatePointRequest request)
        {
            _updatePointCommand.Execute(request);

            return NoContent();
        }

        /// <summary>
        /// Delete point and its associate routes
        /// </summary>
        /// <remarks>
        /// * Request Authorization : Admin
        /// </remarks>
        /// <param name="code">Model to create the point</param>
        /// <response code="400">The point code does not exist</response>
        [ProducesResponseType(typeof(Point), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpDelete("{code}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Delete(string code)
        {
            _deletePointCommand.Execute(new DeletePointCommnadRequest
            {
                Code = code
            });

            return NoContent();
        }
    }
}