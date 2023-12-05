using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly ILocationBussiness _locationBussiness;

        public LocationController(ILocationBussiness _iLocationBussiness,
            IMediator mediator)
        {
            _locationBussiness = _iLocationBussiness;
            _mediator = mediator;
        }

        [EnableQuery(MaxExpansionDepth = 3, AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public IActionResult Get() => Ok(_locationBussiness.GetActiveLocations());

        [EnableQuery(MaxExpansionDepth = 3, AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_locationBussiness.GetAllLocation());


        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddLocationCommand _AddLocationCommand) => Ok(new { Success = "true", result = _mediator.Send(_AddLocationCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditLocationCommand _EditLocationCommand) => Ok(new { Success = "true", result = _mediator.Send(_EditLocationCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateLocationCommand _ActivateLocationCommand) => Ok(new { result = _mediator.Send(_ActivateLocationCommand).Result });
    }
}