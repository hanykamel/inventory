using Inventory.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.AttachmentVM
{
    public class AttachmentVM
    {
        public AttachmentVM()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSize { get; set; }
        public string FileExtention { get; set; }
        public int AttachmentTypeId { get; set; }
    }
}
