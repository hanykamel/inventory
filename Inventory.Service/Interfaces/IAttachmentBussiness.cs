using Inventory.Data.Entities;
using Inventory.Data.Models.AttachmentVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IAttachmentBussiness
    {
        AttachmentOutputVM Upload(IFormFile[] myFormFiles, string[] myFormTypes, Data.Enums.OperationEnum operation);
        bool Delete(List<Guid> attachmentIds, Data.Enums.OperationEnum operation);
        List<Attachment> GetAttachments(List<Guid> attachmentIds);
        IQueryable<Attachment> getAll();
        bool DeleteFromDatabase(List<Guid> attachmentIds);
        List<AttachmentType> getAttachmentTypes(Data.Enums.OperationEnum operation);

        Task <MemoryStream> download(string uploadFile);



    }
}
