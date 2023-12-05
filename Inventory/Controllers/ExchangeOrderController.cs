using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeOrderController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private IStringLocalizer<SharedResource> _localizer;
        public ExchangeOrderController(IExchangeOrderBussiness _iExchangeOrderBussiness,
            IMediator mediator, IStringLocalizer<SharedResource> localizer)
        {
            _exchangeOrderBussiness = _iExchangeOrderBussiness;
            _mediator = mediator;
            _localizer = localizer;
        }

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("[action]")]
        public IQueryable<ExchangeOrder> Get() => _exchangeOrderBussiness.GetAllExchangeOrderView();

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        public IQueryable<ExchangeOrder> GetList() => _exchangeOrderBussiness.GetAllExchangeOrder();

      
        [HttpPost("[action]")]
        public IActionResult SearchStoreItemsExchangeOrder([FromBody] SearchStoreItemsExchangeOrderCommand searchStoreItemsExchangeOrderCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new
            {
                Success = "true",
                storeItems = _mediator.Send(searchStoreItemsExchangeOrderCommand).Result
            });
        }
        [HttpPost("[action]")]
        public IActionResult Create([FromBody] CreateExchangeOrderCommand createExchangeOrderCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new
            {
                Success = "true",
                id = _mediator.Send(createExchangeOrderCommand).Result
            });
        }
        [HttpGet("[action]")]
        public IActionResult getBudgetItems([FromQuery] GetBudgetItemsCommand getBudgetItemsCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);

            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(getBudgetItemsCommand).Result
            });
        }
        [HttpGet("[action]")]
        public OkObjectResult GetInitalCode() => Ok(new { code = _exchangeOrderBussiness.GetCode(_exchangeOrderBussiness.GetMax()) });
        [HttpGet("[action]")]
        [Route("ExchangeOrderReview")]
        public IActionResult ExchangeOrderReview([FromQuery] ExchangeOrderReviewCommand exchangeOrderReviewCommand) => Ok(new
        {
            Success = "true", 
            result = _mediator.Send(exchangeOrderReviewCommand).Result
        });
        [HttpGet("[action]")]
        [Route("CancelExchangeOrder")]
        public IActionResult CancelExchangeOrder([FromQuery] CancelExchangeOrderCommand cancelExchangeOrderCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(cancelExchangeOrderCommand).Result
        });
        [HttpPost]
        [Route("[action]")]
        public IActionResult ReenableExchangeOrderStoreItems([FromBody] ReEnableExchangeOrderStoreItemsCommand reenableExchangeOrderStoreItemsCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(reenableExchangeOrderStoreItemsCommand).Result
            });
        }
     

    }
}