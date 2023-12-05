using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobbingOrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;

        public RobbingOrderController( IMediator mediator , IRobbingOrderBussiness robbingOrderBussiness)
        {
            this.mediator = mediator;
            _robbingOrderBussiness = robbingOrderBussiness;
        }
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        public IQueryable<RobbingOrder> Get() => _robbingOrderBussiness.GetAllRobbingOrder();

       

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("GetRobbingOrderList")]
        public IQueryable<RobbingOrder> GetRobbingOrderList() => _robbingOrderBussiness.GetAllRobbingOrderList();

        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _robbingOrderBussiness.GetCode() });



        [HttpPost]
        public IActionResult Post([FromForm]AddRobbingOrderCommand _robbingOrderCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Transformation;
                var uploadedAttachmentsResult = mediator.Send(uploadAttachmentCommand).Result;
                _robbingOrderCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = mediator.Send(_robbingOrderCommand).Result
            });

        }


        
       



        [HttpGet]
        [Route("CancelRobbingOrder")]
        public bool CancelRobbingOrder([FromQuery]CancelRobbingOrderCommand cancelRobbingOrderCommand) => mediator.Send(cancelRobbingOrderCommand).Result;







        [HttpPost]
        [Route("SaveRobbingExecutionRemainItem")]
        public IActionResult SaveRobbingExecutionRemainItem([FromForm]RobbingExecutionOrderRemainsCommand _RobbingExecutionOrderRemainsCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {

            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.Transformation;
                var uploadedAttachmentsResult = mediator.Send(uploadAttachmentCommand).Result;
                _RobbingExecutionOrderRemainsCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new
            {
                Success = "true",
                result = mediator.Send(_RobbingExecutionOrderRemainsCommand).Result
            });

        }
    }
}