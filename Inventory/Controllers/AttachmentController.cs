using System.IO;
using System.Linq;
using Inventory.Data.Entities;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Inventory.Web.Controllers
{
    [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
    [Route("api/[controller]")]
    public class AttachmentController : Controller
    {
        private readonly IAttachmentBussiness _attachmentBussiness;
        private readonly IMediator _mediator;

        public AttachmentController(IAttachmentBussiness attachmentBussiness, IMediator mediator)
        {
            _attachmentBussiness = attachmentBussiness;
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [EnableQuery]
        public IQueryable<Attachment> Get() => _attachmentBussiness.getAll();


        [HttpGet]
        [Route("download")]
        public IActionResult Download([FromQuery] string fileurl, [FromQuery] string filename, [FromQuery] string type)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileurl, type, filename);
            if (!System.IO.File.Exists(uploads))
                return NotFound();
            DownLoadAttachmentCommand _DownLoadAttachmentCommand = new DownLoadAttachmentCommand();
            _DownLoadAttachmentCommand.fileurl = uploads;
            return File(_mediator.Send(_DownLoadAttachmentCommand).Result, GetContentType(uploads), filename);
        }


        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


    }
}