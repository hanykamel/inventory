using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.AttachmentVM
{
    public interface IAttachmentFormDataVM
    {
        IFormFile[] myFormFiles { get; set; }
        string[] myFormTypes { get; set; }

        List<Guid> deletedAttachments { get; set; }
    }
}
