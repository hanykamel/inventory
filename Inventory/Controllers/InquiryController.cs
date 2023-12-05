using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]

    public class InquiryController : Controller
    {
        private readonly IStoreItemBussiness _storeItemBussiness;
        readonly IMediator _mediator;
        private IStringLocalizer<SharedResource> _localizer;
        public InquiryController(IStoreItemBussiness storeItemBussiness, IMediator mediator, IStringLocalizer<SharedResource> localizer)
        {
            _storeItemBussiness = storeItemBussiness;
            _mediator = mediator;
            _localizer = localizer;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        public IActionResult GetStagnantStoreItems(string stagnantDate) => Ok(_storeItemBussiness.GetStagnantStoreItems(Convert.ToDateTime(stagnantDate)));
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        public IActionResult getStagnantStoreItemsByBaseItemId() => Ok(_storeItemBussiness.getStagnantStoreItemsByBaseItemId());

        [HttpPost]
        [Route("SaveStagnantStoreItems")]
        public IActionResult SaveStagnantStoreItems([FromBody] SaveStagnantCommand saveStagnantModel)
        {
            if (!ModelState.IsValid || (saveStagnantModel != null && saveStagnantModel.StoreItems.Count == 0))
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            return Ok(new
            {
                Success = _mediator.Send(saveStagnantModel).Result
            });
        }
        
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpPost]
        [Route("GetInquiryStoreItems")]
        public IActionResult GetInquiryStoreItems([FromBody] InquiryStoreItemsCommand inquiryStoreItemsCommand)
        {

            return Ok(new
            {
                Success = "true",
                storeItems = _mediator.Send(inquiryStoreItemsCommand).Result
            });
        }

        [HttpPost]
        [Route("GetInquiryBaseItems")]
        public IActionResult GetInquiryBaseItems([FromBody] InquiryBaseItemsCommand inquiryBaseItemsCommand)
        {

            return Ok(new
            {
                Success = "true",
                BaseItems = _mediator.Send(inquiryBaseItemsCommand).Result
            });
        }

        [HttpPost]
        [Route("EditStoreItemsBook")]
        public IActionResult EditStoreItemsBook([FromBody] EditStoreItemsBookCommand inquiryBaseItemsCommand)
        {
            if (!ModelState.IsValid || (inquiryBaseItemsCommand.BaseItems == null && inquiryBaseItemsCommand.BaseItems.Count == 0))
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            return Ok(new
            {
                Success = _mediator.Send(inquiryBaseItemsCommand).Result
            });
        }

    }
}