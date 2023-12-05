using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class AttachmentBussiness : IAttachmentBussiness
    {
        private readonly IRepository<Attachment, Guid> _attachmentRepository;
        private readonly ILogger<AttachmentBussiness> _logger;
        private readonly IRepository<AttachmentType, int> attachmentTypeRepository;
        private readonly IRepository<OperationAttachmentType, int> _operationAttachmentTypeRepository;

        public AttachmentBussiness(
            IRepository<Attachment, Guid> attachmentRepository,
            ILogger<AttachmentBussiness> logger,
            IRepository<AttachmentType, int> attachmentTypeRepository,
            IRepository<OperationAttachmentType, int> operationAttachmentTypeRepository)
        {
            _attachmentRepository = attachmentRepository;
            _logger = logger;
            this.attachmentTypeRepository = attachmentTypeRepository;
            _operationAttachmentTypeRepository = operationAttachmentTypeRepository;
        }
        private List<string> allowedExtensions { get; set; } = new List<string>() { "jpg", "jpeg", "png", "docb", "dotm", "dotx", "docm", "doc", "docx", "pdf", "txt" };
        private int maxFileSize { get; set; } = 5242880;
        private bool meetSize(IFormFile item)
        {
            return item.Length <= maxFileSize;
        }

        private bool meetExtension(IFormFile item, out string extension)
        {
            var dotssplit = item.FileName.Split(".");
            extension = "";
            if (dotssplit != null && dotssplit.Length > 0)
            {
                extension = dotssplit[dotssplit.Length - 1];
            }
            return allowedExtensions.Contains(extension.ToLower());
        }

        private byte[] getFileBytes(IFormFile item)
        {
            using (Stream stream = item.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    return binaryReader.ReadBytes((int)item.Length);
                }
            }
        }

        private bool SaveFile(byte[] content, string path)
        {
            var file = new FileInfo(path);
            file.Directory.Create();
            try
            {
                System.IO.File.WriteAllBytesAsync(file.FullName, content);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "File Upload Error");
                return false;
            }
        }

        public AttachmentOutputVM Upload(IFormFile[] myFormFiles, string[] myFormTypes, Data.Enums.OperationEnum operation)
        {
            AttachmentOutputVM ouput = new AttachmentOutputVM();
            if (myFormFiles != null && myFormFiles.Length > 0)
                for (int i = 0; i < myFormFiles.Length; i++)
                {

                    var item = myFormFiles[i];
                    if (item == null) continue;
                    if (item.Length == 0) continue;
                    if (string.IsNullOrEmpty(myFormTypes[i])) continue;

                    //validate filesize 
                    if (!meetSize(item))
                    {
                        if (!ouput.rejectedFilesClientFileSize.Any(o => o.FileName == item.FileName))
                            ouput.rejectedFilesClientFileSize.Add(item);
                        continue;
                    }

                    //validate extensions
                    string extension;
                    if (!meetExtension(item, out extension))
                    {
                        if (!ouput.rejectedFilesClientExtensions.Any(o => o.FileName == item.FileName))
                            ouput.rejectedFilesClientExtensions.Add(item);
                        continue;
                    }


                    var filecontent = getFileBytes(item);
                    if (filecontent != null)
                    {
                        AttachmentVM attachment = new AttachmentVM();
                        var attachmentType = attachmentTypeRepository.GetFirst(o => o.Name == myFormTypes[i]);
                        if (attachmentType == null) continue;
                        attachment.AttachmentTypeId = attachmentType.Id;
                        string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "uploads", operation.ToString(), attachmentType.Name);
                        attachment.FileName = attachment.Id.ToString()+ "//" + item.FileName;
                        var path = Path.Combine(directory, attachment.FileName);
                        attachment.FileSize = item.Length;
                        attachment.FileUrl = "assets/uploads/" + operation.ToString();
                        attachment.FileExtention = extension;

                        if (SaveFile(filecontent, path))
                            ouput.attachments.Add(attachment);
                    }
                }
            return ouput;
        }

        public bool Delete(List<Guid> attachmentIds, Data.Enums.OperationEnum operation)
        {
            if (attachmentIds.Count > 0)
            {
                var attachments = _attachmentRepository.GetAll().Include(o => o.AttachmentType).Where(o => attachmentIds.Contains(o.Id)).ToList();
                if (attachments != null && attachments.Count > 0)
                    foreach (var item in attachments)
                    {
                        string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "uploads", operation.ToString(), item.AttachmentType.Name);
                        var path = Path.Combine(directory, item.FileName);
                        try
                        {
                            if (File.Exists(path))
                                File.Delete(path);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Delete File Error");
                        }
                    }
            }
            return true;
        }

        public List<Attachment> GetAttachments(List<Guid> attachmentIds)
        {
            return _attachmentRepository.GetAll().Include(o => o.AttachmentType).Where(o => attachmentIds.Contains(o.Id)).ToList();
        }

        public bool DeleteFromDatabase(List<Guid> attachmentIds)
        {
            var attachments = GetAttachments(attachmentIds);
            if (attachments != null && attachments.Count > 0)
                foreach (var item in attachments)
                {
                    _attachmentRepository.Delete(item);
                }
            return true;
        }

        public List<AttachmentType> getAttachmentTypes(Data.Enums.OperationEnum operation)
        {
            int operationId = (int)operation;
            return _operationAttachmentTypeRepository.GetAll().Include(o => o.AttachmentType).Where(o => o.OperationId == operationId).ToList().Select(o => o.AttachmentType).ToList(); ;
        }

        public IQueryable<Attachment> getAll() => _attachmentRepository.GetAll();

 

        public async Task<MemoryStream> download(string uploadFile)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(uploadFile, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return await Task.FromResult(memory);
        }
    }
}
