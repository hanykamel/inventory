using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.DeductionRequest.Commands;
using Inventory.Service.Entities.ExaminationCommittee.Commands;
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Entities.ReportRequest.Commands;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;

namespace Inventory.Web.Controllers
{
    [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
    [Route("api/[controller]")]
    public class WordProcessingController : Controller
    {
        private IMediator _mediator;

        private IStringLocalizer<SharedResource> _localizer;

        public WordProcessingController(IMediator mediator, IStringLocalizer<SharedResource> localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{AdditionId?}")]
        public IActionResult PrintAddition([FromRoute]PrintAdditionCommand _printAdditionCommand)
        {
            if (_printAdditionCommand.AdditionId==Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printAdditionCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");

        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{ExaminationId?}")]
        public IActionResult PrintExamination([FromRoute]PrintExaminationCommand _printExaminationCommand)
        {
            if (_printExaminationCommand.ExaminationId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printExaminationCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");

        }
        //[Authorize(Policy = InventoryAuthorizationPolicy.AllTransactions)]
        [HttpPost]
        [EnableQuery]
        [Route("[action]")]
        public IActionResult PrintInvoice([FromBody]PrintInvoiceCommand _printInvoiceCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printInvoiceCommand).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{AdditionId?}")]
        public IActionResult PrintAdditionFormNo8([FromRoute]PrintFormNo8Command _printFormNo8Command)
        {
            if (_printFormNo8Command.AdditionId==Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printFormNo8Command).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{RobbingOrderId?}")]
        public IActionResult PrintRobbingOrderFormNo8([FromRoute]PrintRobbingOrderFormNo8Command _printRobbingOrderFormNo8Command)
        {
            if (_printRobbingOrderFormNo8Command.RobbingOrderId== Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printRobbingOrderFormNo8Command).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{RobbingOrderId?}")]
        public IActionResult PrintRobbingOrderFormNo2([FromRoute]PrintRobbingOrderFormNo2Command _printRobbingOrderFormNo2Command)
        {
            if (_printRobbingOrderFormNo2Command.RobbingOrderId==Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printRobbingOrderFormNo2Command).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{TransformationId?}")]
        public IActionResult PrintTransformationFormNo8([FromRoute]PrintTransformationFormNo8Command _printTransformationFormNo8Command)
        {
            if (_printTransformationFormNo8Command.TransformationId==Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printTransformationFormNo8Command).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{TransformationId?}")]
        public IActionResult PrintTransformationFormNo2([FromRoute]PrintTransformationFormNo2Command _printTransformationFormNo2Command)
        {
            if (_printTransformationFormNo2Command.TransformationId==Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printTransformationFormNo2Command).Result;
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                    ".wordprocessingml.document");
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{RefundOrderId?}")]
        public IActionResult PrintRefundOrderFromNo9([FromRoute]PrintFromNo9Command _printFromNo9Command)
        {
            if (_printFromNo9Command.RefundOrderId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printFromNo9Command).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");

        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{DeductionId?}")]
        public IActionResult PrintDeductionerFromNo2([FromRoute]PrintDeductionCommand _printDeductionCommand)
        {
            if (_printDeductionCommand.DeductionId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printDeductionCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");

        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{StockTakingId?}")]
        public IActionResult PrintStockTaking([FromRoute]PrintStockTakingCommand _printStockTakingCommand)
        {
            if (_printStockTakingCommand.StockTakingId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printStockTakingCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");
        }
        [HttpGet]
        [EnableQuery]
        [Route("[action]/{HandoverId?}")]
        public IActionResult PrintHandover([FromRoute]PrintHandoverCommand _printHandoverCommand)
        {
            if (_printHandoverCommand.HandoverId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printHandoverCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult PrintStagnantStoreItems([FromBody] PrintStagnantCommand _printStagnantModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printStagnantModel).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [HttpGet]
        [EnableQuery]
        [Route("[action]/{ExecutionOrderId?}")]
        public IActionResult PrintExecutionOrderFromNo2([FromRoute]PrintExecutionOrderCommand _printExecutionOrderCommand)
        {
            if (_printExecutionOrderCommand.ExecutionOrderId == Guid.Empty)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            var memoryStream = _mediator.Send(_printExecutionOrderCommand).Result;
            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument" +
                ".wordprocessingml.document");

        }

    }
}