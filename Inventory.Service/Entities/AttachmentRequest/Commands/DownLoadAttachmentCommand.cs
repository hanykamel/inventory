using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.AttachmentRequest.Commands
{
  public  class DownLoadAttachmentCommand : IRequest<MemoryStream>
    {
        public string fileurl { get; set; }
    }
}
