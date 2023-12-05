using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransformationController : ControllerBase
    {
        private readonly ITransformationRequestBussiness _TransformationRequestBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMediator mediator;

        public TransformationController(ITransformationRequestBussiness TransformationRequestBussiness,
            IStoreItemBussiness storeItemBussiness, IMediator mediator)
        {
            _TransformationRequestBussiness = TransformationRequestBussiness;
            _storeItemBussiness = storeItemBussiness;
            this.mediator = mediator;
        }
        [EnableQuery(MaxExpansionDepth = 4)]
        [Route("[action]")]
        [HttpGet]
        public IQueryable<Transformation> Get() => _TransformationRequestBussiness.GetAllTransformation();


        [EnableQuery(MaxExpansionDepth = 7)]
        [HttpGet]
        public IQueryable<Transformation> GetAllTransformationView() => _TransformationRequestBussiness.GetAllTransformationView();
       


        [HttpPost("[action]")]
        public IActionResult GetStoreItems([FromBody]List<Guid> storeItemIds) => Ok(new { data = _storeItemBussiness.GetAllStoreItems(storeItemIds, true) });


        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _TransformationRequestBussiness.GetCode() });

        [HttpPost]
        public IActionResult Post([FromForm]AddTransforemationCommand _addTransforemationCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Transformation;
                var uploadedAttachmentsResult = mediator.Send(uploadAttachmentCommand).Result;
                _addTransforemationCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = mediator.Send(_addTransforemationCommand).Result
            });

        }



          
        [HttpGet]
        [Route("CancelTransformation")]
        public bool CancelTransformation([FromQuery]CancelTransformationCommand cancelTransformationCommand) => mediator.Send(cancelTransformationCommand).Result;


    }
}