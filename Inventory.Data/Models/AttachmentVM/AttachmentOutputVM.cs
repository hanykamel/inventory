using Inventory.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.AttachmentVM
{
    public class AttachmentOutputVM
    {
        public AttachmentOutputVM()
        {
            attachments = new List<AttachmentVM>();
            rejectedFilesClientFileSize = new List<IFormFile>();
            rejectedFilesClientExtensions = new List<IFormFile>();
        }
        public List<AttachmentVM> attachments { get; set; }
        public List<IFormFile> rejectedFilesClientFileSize { get; set; }
        public List<IFormFile> rejectedFilesClientExtensions { get; set; }
    }
}
