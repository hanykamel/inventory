using Inventory.Data.Enums;

using Inventory.Data.Models.AttachmentVM;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.AttachmentRequest.Commands
{
    public class UploadAttachmentCommand : IRequest<AttachmentOutputVM>, IAttachmentFormDataVM
    {
        public IFormFile[] myFormFiles { get; set; }
        public string[] myFormTypes { get; set; }
        public OperationEnum operation { get; set; }
        public List<Guid> deletedAttachments { get; set; }
}
}
