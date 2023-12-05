using Inventory.Service.Entities.UnitRequest.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.OData.Query;
using Inventory.Web.Attributes;
using Inventory.Data.Enums;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class UnitController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IUnitBussiness _unitBussiness;

        public UnitController(IMediator mediator, IUnitBussiness _UnitBussiness)
        {
            _mediator = mediator;
            _unitBussiness = _UnitBussiness;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get() => Ok(_unitBussiness.GetActiveUnits());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_unitBussiness.GetAllUnit());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddUnitCommand _AddUnitCommand) => Ok(new { result = _mediator.Send(_AddUnitCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditUnitCommand _EditUnitCommand)
        {
            return Ok(new { result = _mediator.Send(_EditUnitCommand).Result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateUnitCommand _ActivateUnitCommand) => Ok(new { result = _mediator.Send(_ActivateUnitCommand).Result });

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintUnitsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.UnitsList })]
        public IActionResult PrintUnitsList() => Ok(_unitBussiness.GetAllUnit());
    }
}