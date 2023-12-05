using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;


namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingController : ControllerBase
    {
        private readonly IStockTakingBussiness _stockTakingBussiness;
        readonly IMediator _mediator;

        public StockTakingController(IStockTakingBussiness stockTakingBussiness,
            IMediator mediator)
        {
            _stockTakingBussiness = stockTakingBussiness;
            _mediator = mediator;
        }
        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _stockTakingBussiness.GetCode(), lastGeneratedCode = _stockTakingBussiness.GetLastCode() });
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        public IQueryable<StockTaking> Get() => _stockTakingBussiness.GetAllStockTaking();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("GetStockTakingView")]
        public IQueryable<StockTaking> GetStockTakingView() => _stockTakingBussiness.GetStockTakingview();

        [HttpPost("[action]")]
        public IActionResult SearchStockTaking([FromBody]SearchStockTakingCommand searchStockTakingCommand) => Ok(new { Success = "true", result = _mediator.Send(searchStockTakingCommand).Result , count = searchStockTakingCommand.Count});
        [HttpPost("[action]")]
        public IActionResult create([FromForm]CreateStockTakingCommand createStockTakingCommand, [FromForm] UploadAttachmentCommand uploadAttachmentCommand)
        {
            if (uploadAttachmentCommand.myFormFiles != null && uploadAttachmentCommand.myFormFiles.Length > 0 && uploadAttachmentCommand.myFormTypes.Length > 0)
            {
                uploadAttachmentCommand.operation = OperationEnum.StockTaking;
                var uploadedAttachmentsResult = _mediator.Send(uploadAttachmentCommand).Result;
                createStockTakingCommand.AdditionAttachment = uploadedAttachmentsResult.attachments;
            }
            return Ok(new { Success = "true", result = _mediator.Send(createStockTakingCommand).Result});
        }
    }
}