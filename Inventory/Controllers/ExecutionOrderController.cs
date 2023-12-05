using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.ExecutionOrderVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class ExecutionOrderController : ControllerBase
    {
 
        private readonly IExecutionOrderBussiness _executionOrderBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IMediator _mediator;
        public ExecutionOrderController(
            IExecutionOrderBussiness executionOrderBussiness,
            IMediator mediator,
            IStringLocalizer<SharedResource> localizer)
        {
            _executionOrderBussiness = executionOrderBussiness;
            _mediator = mediator;
            _localizer = localizer;
        }
        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("[action]")]
        public IQueryable<ExecutionOrder> Get() => _executionOrderBussiness.GetAllExecutionOrders();

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("[action]")]
        public IQueryable<ExecutionOrder> GetList() => _executionOrderBussiness.GetAllExecutionOrdersView();

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("[action]")]
        public IQueryable<CustomeExecutionOrderVM> GetExecutionOrderList() => _executionOrderBussiness.GetCustomeExecutionOrdersList();

        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _executionOrderBussiness.GetCode() });



        [HttpPost]
        public IActionResult Post([FromForm]CreateExecutionOrderCommand _ExecutionOrderCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Transformation;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                _ExecutionOrderCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(_ExecutionOrderCommand).Result
            });

        }



        [HttpGet]
        [Route("CancelExecutionOrder")]
        public bool CancelExecutionOrder([FromQuery]CancelExecutionOrderCommand CancelExecutionOrderCommand) => _mediator.Send(CancelExecutionOrderCommand).Result;


        [HttpPost]
        [Route("ReviewExecutionOrder")]
        public IActionResult ReviewExecutionOrder([FromForm] ReviewExecutionOrderCommand _ReviewExecutionOrderCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand) {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Execution;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                _ReviewExecutionOrderCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(_ReviewExecutionOrderCommand).Result
            });
        }





        [HttpGet]
        [Route("GetExecutionOrderStoreItemReview")]
        public IEnumerable<ExecutionOrderBaseItemModel> GetExecutionOrderStoreItemReview([FromQuery]Guid Id) => _executionOrderBussiness.GetExecutionOrderStoreItemReview(Id);





        [HttpPost]
        [Route("SaveExecutionItemAndRemains")]

        public IActionResult SaveExecutionItemAndRemains([FromForm]CreateExecutionOrderAfterReviewCommand _CreateExecutionOrderAfterReviewCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Execution;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                _CreateExecutionOrderAfterReviewCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(_CreateExecutionOrderAfterReviewCommand).Result
            });

        }

        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [Route("GetRobbingRemainItems")]
        public IQueryable<RemainItemsModel> GetRobbingRemainItems([FromQuery] long? RemainId, [FromQuery] string SelectRemainItem, [FromQuery] int BudgetId) => _executionOrderBussiness.GetRobbingRemainItems(RemainId, JsonConvert.DeserializeObject<Guid[]>(SelectRemainItem), BudgetId);

        [HttpGet]
        [Route("getRemainItemRobbing")]
        public IQueryable<RemainModel> getRemainItemRobbing([FromQuery] Guid ExecutionOrderId) => _executionOrderBussiness.getRemainItemRobbing(ExecutionOrderId);
        

    }
}
