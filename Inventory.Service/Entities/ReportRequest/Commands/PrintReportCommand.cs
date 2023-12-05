using Inventory.Data.Enums;
using Inventory.Data.Models.Inquiry;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.ReportRequest.Commands
{
    public class PrintReportCommand : IRequest<MemoryStream>
    {
        public PrintDocumentTypesEnum ReportType { get; set; }
        public ObjectResult Result { get; set; }
        public String Params { get; set; }
    }


}
