using Inventory.Service.Entities.ItemCategoryRequest.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Inventory.Web.Attributes;
using Microsoft.AspNet.OData.Query;
using Inventory.Data.Enums;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryBussiness _itemCategoryBussiness;
        readonly IMediator _mediator;

        public ItemCategoryController(IItemCategoryBussiness _IItemCategoryBussiness, IMediator mediator)
        {
            _itemCategoryBussiness = _IItemCategoryBussiness;
            _mediator = mediator;

        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        public IActionResult Get() => Ok(_itemCategoryBussiness.GetActiveItemCategories());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_itemCategoryBussiness.GetAllItemCategory());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddItemCategoryCommand _AddItemCategoryCommand) => Ok(new { Success = "true", result = _mediator.Send(_AddItemCategoryCommand).Result });

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditItemCategoryCommand _editItemCategoryCommand) => Ok(new { Success = "true", result = _mediator.Send(_editItemCategoryCommand).Result });


        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateItemCategoryCommand _activateItemCategoryCommand) => Ok(new { result = _mediator.Send(_activateItemCategoryCommand).Result });


        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintItemCategoriesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ItemsCategoriesList })]
        public IActionResult PrintItemCategoriesList() => Ok(_itemCategoryBussiness.GetAllItemCategory());
    }
}