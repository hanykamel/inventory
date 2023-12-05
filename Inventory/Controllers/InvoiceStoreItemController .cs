using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceStoreItemController : Controller
    {
        private readonly IInvoiceStoreItemBussiness _invoiceStoreItemBussiness;

        public InvoiceStoreItemController( IInvoiceStoreItemBussiness invoiceStoreItemBussiness)
        {
            _invoiceStoreItemBussiness = invoiceStoreItemBussiness;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet]
        [EnableQuery]
        public IActionResult Get() => Ok(_invoiceStoreItemBussiness.GetAllInvoiceStoreItems());

    }
}