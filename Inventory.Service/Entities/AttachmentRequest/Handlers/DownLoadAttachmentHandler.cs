using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AttachmentRequest.Handlers
{
   public class DownLoadAttachmentHandler : IRequestHandler<DownLoadAttachmentCommand, MemoryStream>
    {

        private readonly IAttachmentBussiness _attachmentBussiness;
        public DownLoadAttachmentHandler(IAttachmentBussiness attachmentBussiness)
        {
            _attachmentBussiness = attachmentBussiness;

        }

        public async Task<MemoryStream> Handle(DownLoadAttachmentCommand request, CancellationToken cancellationToken)
        {
            return await _attachmentBussiness.download(request.fileurl);
        }
    }
}
