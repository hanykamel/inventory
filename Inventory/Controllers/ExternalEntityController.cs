using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.ExternalEntityRequest.Commands;
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
    
    public class ExternalEntityController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IExternalEntityBussiness _externalEntityBussiness;

        public ExternalEntityController(IExternalEntityBussiness _IExternalEntityBussiness, IMediator mediator)
        {
            _externalEntityBussiness = _IExternalEntityBussiness;
            _mediator = mediator;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        public IQueryable<ExternalEntity> Get() => _externalEntityBussiness.GetActiveExternalEntities();
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        [Route("GetAll")]
        public IQueryable<ExternalEntity> GetAll() => _externalEntityBussiness.GetAllExternalEntity();

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddExternalEntityCommand _AddExternalEntityCommand) => Ok(new { Success = "true", result = _mediator.Send(_AddExternalEntityCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditExternalEntityCommand _EditExternalEntityCommand) => Ok(new { Success = "true", result = _mediator.Send(_EditExternalEntityCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateExternalEntityCommand _ActivateExternalEntityCommand) => Ok(new { result = _mediator.Send(_ActivateExternalEntityCommand).Result });

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintExternalEntities")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExternalEntityList })]
        public IActionResult PrintExternalEntities() => Ok(_externalEntityBussiness.GetAllExternalEntity());
    }
}