using System;
using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class AdditionController : Controller
    {
        readonly IMediator _mediator;
        private readonly IAdditionBussiness _additionBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AdditionController(IAdditionBussiness additionBussiness, IMediator mediator,
            IStringLocalizer<SharedResource> localizer)
        {
            _additionBussiness = additionBussiness;
            _mediator = mediator;
            _localizer = localizer;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet("[action]")]
        [Route("GetALL")]
        [EnableQuery(MaxExpansionDepth = 4)]
        public IQueryable<Addition> GetALL() => _additionBussiness.GetAllAddition();




        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet("[action]")]
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("GetAddition")]
        public IQueryable<Addition> GetAddition() => _additionBussiness.GetAdditionView();

      

        [Authorize(Policy = InventoryAuthorizationPolicy.AllTransactions)]
        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _additionBussiness.GetCode(), lastGeneratedCode = _additionBussiness.GetLastCode(),additionNumber  = _additionBussiness.GetMaxAdditionNumber() });

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost("[action]")]
        public IActionResult Create([FromForm]CreateAdditionCommand addition, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {
            if (!ModelState.IsValid || !addition.ValidateCreate())
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Addition;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                addition.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            var additionId = _mediator.Send(addition).Result;
            GenerateStoreItemsBarCodeCommand generateStoreItemsBarCodeCommand = new GenerateStoreItemsBarCodeCommand()
            {
                AdditionId = additionId
            };
            return Ok(new
            {
                Success = "true",
                id = additionId,
                addition.AdditionAttachment,
                storeItems = _mediator.Send(generateStoreItemsBarCodeCommand).Result
            });
        }

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost("[action]")]
        public IActionResult Edit([FromForm]EditAdditionCommand addition, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            AttachmentOutputVM uploadedAttachmentsResult = new AttachmentOutputVM();
            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Addition;
                uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                addition.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }

            addition.FileDelete = uploadAttachmentCommand.deletedAttachments;
            var additionId = _mediator.Send(addition).Result;
           
            return Ok(new
            {
                Success = "true",
                id = additionId,
                AdditionAttachment = uploadedAttachmentsResult
            });
        }
       


    }
}