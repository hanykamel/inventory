using Inventory.Data.Models.AttachmentVM;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.AttachmentRequest.Commands
{
    public class DatabaseDeleteAttachmentCommand : IRequest<bool>
    {
        public List<Guid> attachmentIds{ get; set; }
    }
}
