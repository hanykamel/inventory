using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.SupplierRequest.Commands;
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
    public class SupplierController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly ISupplierBussiness _supplierBussiness;

        public SupplierController(ISupplierBussiness _ISupplierBussiness, IMediator mediator)
        {
            _supplierBussiness = _ISupplierBussiness;
            _mediator = mediator;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        public IActionResult Get() => Ok(_supplierBussiness.GetActives());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_supplierBussiness.GetAllSupplier());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddSupplierCommand _AddSupplierCommand) => Ok(new { result = _mediator.Send(_AddSupplierCommand).Result });

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditSupplierCommand _editSupplierCommand) => Ok(new { result = _mediator.Send(_editSupplierCommand).Result });

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateSupplierCommand _ActivateSupplierCommand) => Ok(new { result = _mediator.Send(_ActivateSupplierCommand).Result });

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintSuppliersList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.SuppliersList })]
        public IActionResult PrintSuppliersList() => Ok(_supplierBussiness.GetAllSupplier());
    }
}