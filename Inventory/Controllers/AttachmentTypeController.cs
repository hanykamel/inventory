using System.Linq;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
    [Route("api/[controller]")]
    public class AttachmentTypeController : Controller
    {
        private readonly IAttachmentTypeBussiness _attachmentTypeBussiness;

        public AttachmentTypeController(IAttachmentTypeBussiness attachmentTypeBussiness)
        {
           _attachmentTypeBussiness = attachmentTypeBussiness;
        }

        [HttpGet("[action]")]
        [EnableQuery]
        public IQueryable<AttachmentType> Get() => _attachmentTypeBussiness.getAll();
      

    }
}