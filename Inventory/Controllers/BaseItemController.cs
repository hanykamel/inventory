using System.Linq;
using Inventory.Service.Entities.BaseItemRequest.Commands;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Inventory.Data.Enums;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class BaseItemController : ControllerBase
    { 
        readonly IMediator _mediator;

        private readonly IBaseItemBussiness _baseItemBussiness;

        public BaseItemController(IBaseItemBussiness _IBaseItemBussiness, IMediator mediator)
        {
            _baseItemBussiness = _IBaseItemBussiness;
            _mediator = mediator;

        }
        //[Authorize(Policy = "OwnerOnly")]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxAnyAllExpressionDepth = 2)]
        public IQueryable<BaseItem> Get() => _baseItemBussiness.GetActiveBaseItems();
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Route("GetAll")]
        public IQueryable<BaseItem> GetAll() => _baseItemBussiness.GetAllBaseItem();
       

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        public IActionResult Post([FromBody]AddBaseItemCommand _AddBaseItemCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(_AddBaseItemCommand).Result
        });

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        [Route("EditBaseItem")]
        public IActionResult EditBaseItem([FromBody]EditBaseItemCommand _EditBaseItemCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(_EditBaseItemCommand).Result
        });

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        [Route("ActivateBaseItem")]
        public IActionResult ActivateBaseItem([FromBody]ActivateBaseItemCommand _ActivateBaseItemCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(_ActivateBaseItemCommand).Result
        });

       

    }
}