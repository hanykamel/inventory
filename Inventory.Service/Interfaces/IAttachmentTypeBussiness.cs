using Inventory.Data.Entities;
using Inventory.Data.Models.AttachmentVM;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Interfaces
{
 
    public interface IAttachmentTypeBussiness
    {
        IQueryable<AttachmentType> getAll();
    }
}
