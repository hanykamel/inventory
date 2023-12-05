using System.Linq;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class OperationAttachmentTypeController : Controller
    {
        private readonly IOperationAttachmentTypeBussiness _operationAttachmentTypeBussiness;

        public OperationAttachmentTypeController(IOperationAttachmentTypeBussiness operationAttachmentTypeBussiness)
        {
            _operationAttachmentTypeBussiness = operationAttachmentTypeBussiness;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet("[action]")]
        [EnableQuery]
        public IQueryable<OperationAttachmentType> Get() => _operationAttachmentTypeBussiness.getAll();
    }
}