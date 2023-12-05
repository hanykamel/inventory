using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Enums;
using Inventory.Service.Entities.ReportRequest.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Web.Attributes
{
    public class WordActionFilterAttribute : ActionFilterAttribute
    {
        private IMediator _mediator;
        private readonly IMapper _mapper;
        private IStringLocalizer<SharedResource> _localizer;
        public PrintDocumentTypesEnum _documentType { get; set; }
        public WordActionFilterAttribute(
            IMediator mediator,
            IMapper mapper,
            PrintDocumentTypesEnum documentType,
            IStringLocalizer<SharedResource> localizer)
        {
            _mediator = mediator;
            _mapper = mapper;
            _documentType = documentType;
            _localizer = localizer;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            if (result != null)
            {
                PrintReportCommand printCommand = new PrintReportCommand();
                printCommand.ReportType = _documentType;
                printCommand.Result = result;
                printCommand.Params = ((Microsoft.AspNetCore.Http.DefaultHttpContext)context.HttpContext)?.Request?.Path.Value;
                MemoryStream memoryStream = _mediator.Send(printCommand).Result;
                context.Result = new FileContentResult(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                base.OnResultExecuting(context);
            }
            else
            {
                throw new NoDataException(_localizer["NoDataException"]);
            }
        }
    }
}
