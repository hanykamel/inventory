using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.RefundOrderVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundOrderController : ControllerBase
    {
        //readonly IMediator _mediator;
        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly IInvoiceBussiness _invoiceBussiness;

        readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public RefundOrderController(
            IRefundOrderBussiness RefundOrderBussiness,
            IMediator mediator,
            IStringLocalizer<SharedResource> localizer,
            IInvoiceBussiness invoiceBussiness
            )
        {
            _invoiceBussiness = invoiceBussiness;
            _refundOrderBussiness = RefundOrderBussiness;
            _mediator = mediator;
            _localizer = localizer;
        }


        [EnableQuery(MaxExpansionDepth = 7, MaxAnyAllExpressionDepth = 5)]
        [HttpGet]
        [Route("[action]")]
        public IQueryable<RefundOrder> Get()
        {
            return _refundOrderBussiness.GetAllRefundOrderView();  
        }

        [EnableQuery(MaxExpansionDepth = 7, MaxAnyAllExpressionDepth = 5)]
        [HttpGet]
        public IQueryable<RefundOrder> GetList()
        {
            return _refundOrderBussiness.GetAllRefundOrder();
        }
       
        [HttpPost("[action]")]
        public IActionResult Create([FromForm]CreateRefundOrderCommand refund,
            [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.RefundOrder;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                refund.RefundAttachment = uploadedAttachmentsResult.attachments;
            }
            var refundId = _mediator.Send(refund).Result;
            return Ok(new
            {
                Success = "true",
                id = refundId,
                refund.RefundAttachment
            });
        }

        [HttpGet("[action]")]
        public OkObjectResult GetInitalCode() => Ok(new
        { code = _refundOrderBussiness.GetCode(_refundOrderBussiness.GetMax()) });

        [HttpGet("[action]")]
        [Route("RefundOrderReview")]
        public IActionResult RefundOrderReview([FromQuery] RefundOrderReviewCommand refundOrderReviewCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(refundOrderReviewCommand).Result
        });

        [HttpGet("[action]")]
        [Route("CancelRefundOrder")]
        public IActionResult CancelRefundOrder([FromQuery] CancelRefundOrderCommand cancelRefundOrderCommand) => Ok(new
        {
            Success = "true",
            result = _mediator.Send(cancelRefundOrderCommand).Result
        });

    }

   
}