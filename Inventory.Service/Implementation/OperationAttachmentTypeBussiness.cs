using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class OpertaionAttachmentTypeBussiness : IOperationAttachmentTypeBussiness
    {
        private readonly IRepository<OperationAttachmentType, int> operationAttachmentTypeRepository;

        public OpertaionAttachmentTypeBussiness(IRepository<OperationAttachmentType, int> operationAttachmentTypeRepository)
        {
            this.operationAttachmentTypeRepository = operationAttachmentTypeRepository;
        }
       
        IQueryable<OperationAttachmentType> IOperationAttachmentTypeBussiness.getAll() => operationAttachmentTypeRepository.GetAll();
    }
}
