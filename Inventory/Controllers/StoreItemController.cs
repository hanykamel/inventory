using Inventory.Data.Models.BaseItem;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class StoreItemController : Controller
    {
        readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;

        public StoreItemController(IMediator mediator, IStoreItemBussiness storeItemBussiness)
        {
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        public IActionResult Get() => Ok(_storeItemBussiness.GetAllStoreItems());

        [HttpPost]
        [Route("GetBaseItemsLatestBooksAndPages")]
        public IActionResult GetBaseItemsLatestBooksAndPages([FromBody]List<long> baseItemsIds) => Ok(_storeItemBussiness.GetBaseItemsLatestBooksAndPages(baseItemsIds));

        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        [Route("GetAvailable")]
        public IActionResult GetAvailable() => Ok(_storeItemBussiness.GetAvailableStoreItems());

        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        [Route("GetRobbingStoreItems")]
        public IActionResult GetRobbingStoreItems() => Ok(_storeItemBussiness.GetRobbingStoreItems());

        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("GetActiveRobbingBaseItemsBudget")]
        public IQueryable<BaseItemBudgetVM> GetActiveRobbingBaseItemsBudget([FromQuery] int BudgetId,[FromQuery]int CurrencyId, [FromQuery] int? Status, [FromQuery] int? CategoryId, [FromQuery]string SelectBaseItem,[FromQuery] string ContractNum ,[FromQuery] int? PageNum) => _storeItemBussiness.GetActiveRobbingBaseItemsBudget(BudgetId, CurrencyId, Status, CategoryId, JsonConvert.DeserializeObject<long[]>(SelectBaseItem), ContractNum, PageNum);


        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("[action]")]
        public IActionResult getStoreItemExchanges([FromQuery]GetStoreItemExchangesCommand getStoreItemExchangesCommand) => Ok(new { Success = "true", result = _mediator.Send(getStoreItemExchangesCommand).Result });


        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("GetAllBaseItemByBudget")]
        public IQueryable<BaseItemBudgetVM> GetAllBaseItemByBudget([FromQuery] int BudgetId, [FromQuery] int CurrencyId, [FromQuery] int? Status, [FromQuery] int? CategoryId, [FromQuery] string ContractNum, [FromQuery] int? PageNum) => _storeItemBussiness.GetActiveBaseItemsBudget(BudgetId, CurrencyId, Status, CategoryId, ContractNum, PageNum);

        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("[action]")]
        public List<ExchangeOrderStoreItemVM> GetByExchangeOrderId([FromQuery] Guid ExchangeOrderId) => _storeItemBussiness.GetByExchangeOrderId(ExchangeOrderId);

         [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("GetExecutionOrderBaseItemsBudget")]
        public IQueryable<BaseItemBudgetVM> GetExecutionOrderBaseItemsBudget([FromQuery] int BudgetId, [FromQuery] int CurrancyId, [FromQuery] int? CategoryId, [FromQuery]string SelectBaseItem) => _storeItemBussiness.GetExecutionOrderBaseItemsBudget(BudgetId, CurrancyId, CategoryId, JsonConvert.DeserializeObject<long[]>(SelectBaseItem) );



    }
}