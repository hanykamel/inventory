using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
    [Route("api/[controller]")]
    public class ExaminationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IExaminationBusiness _examinationBussiness;

        public ExaminationController(IExaminationBusiness _IExaminationBussiness,
            IMediator mediator)
        {
            _examinationBussiness = _IExaminationBussiness;
            _mediator = mediator;
        }
        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        public IQueryable<ExaminationCommitte> Get() => _examinationBussiness.GetAllExamination();

        

        [HttpGet]
        [Route("getExaminationCode")]
        public IActionResult getExaminationCode() => Ok(new { Result = _examinationBussiness.GetCode() });
        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        public IActionResult Post([FromForm]AddExaminationCommand _addExaminationCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {
            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.ExaminationCommittee;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                _addExaminationCommand.Attachments = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(_addExaminationCommand).Result
            });

        }


        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPut]
        public IActionResult Put([FromForm]EditExaminationCommand _editExaminationCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            
            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 )
            {
                uploadAttachmentCommand.operation = OperationEnum.ExaminationCommittee;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                _editExaminationCommand.Attachments = uploadedAttachmentsResult.attachments;
                
            }
            _editExaminationCommand.FileDelete = uploadAttachmentCommand.deletedAttachments;
            var resultEditExamination = _mediator.Send(_editExaminationCommand).Result;
             if (uploadAttachmentCommand.deletedAttachments!=null&&uploadAttachmentCommand.deletedAttachments.Any()&& resultEditExamination.isEditSuccess==true)
            {
                var result = _mediator.Send(new DeleteAttachmentCommand() { attachmentIds = uploadAttachmentCommand.deletedAttachments, operation = OperationEnum.ExaminationCommittee }).Result;
                
            }
            return Ok(new
            {
                Success = "true",
                result = resultEditExamination
            });

        }




    }
}
