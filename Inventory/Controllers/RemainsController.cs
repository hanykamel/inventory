using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Service.Entities.RemainsRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    
    [Route("api/[controller]")]
    public class RemainsController : Controller
    {
        readonly IMediator _mediator;
        private readonly IRemainsBussiness _RemainsBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public RemainsController(IMediator mediator, IRemainsBussiness RemainsBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _mediator = mediator;
            _RemainsBussiness = RemainsBussiness;
            _localizer = Localizer;
        }
        //[Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
       
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get() => Ok(_RemainsBussiness.GetActiveRemains());

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Route("GetRemainsDetails")]
        public IActionResult GetRemainsDetails() => Ok(_RemainsBussiness.GetAllRemainsDetails());

        

        //[Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_RemainsBussiness.GetAllRemains());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpPost]
        public IActionResult Post([FromBody]AddRemainsCommand _AddRemainsCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new { Success = "true", result = _mediator.Send(_AddRemainsCommand).Result });
        }

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpPut]
        public IActionResult Put([FromBody]EditRemainsCommand _EditRemainsCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new { Success = "true", result = _mediator.Send(_EditRemainsCommand).Result });
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateRemainsCommand _ActivateRemainsCommand) => Ok(new { result = _mediator.Send(_ActivateRemainsCommand).Result });

      
    }
}