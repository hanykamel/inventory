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
    public class AttachmentTypeBussiness : IAttachmentTypeBussiness
    {
        private readonly IRepository<AttachmentType, int> attachmentTypeRepository;

        public AttachmentTypeBussiness(IRepository<AttachmentType, int> attachmentTypeRepository)
        {
            this.attachmentTypeRepository = attachmentTypeRepository;
        }
       

        public IQueryable<AttachmentType> getAll() => attachmentTypeRepository.GetAll();
    }
}
