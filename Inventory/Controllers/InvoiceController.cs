using System.Linq;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using Microsoft.AspNetCore.Authorization;
using Inventory.Web.Attributes;
using Inventory.Data.Enums;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IInvoiceBussiness _invoiceBussiness;

        public InvoiceController(IInvoiceBussiness invoiceBussiness,
            IMediator mediator)
        {
            _invoiceBussiness = invoiceBussiness;
            _mediator = mediator;
        }

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        public IQueryable<Invoice> Get() => _invoiceBussiness.GetAllInvoice();

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("GetInvoiceDetails")]
        public IQueryable<Invoice> GetInvoiceDetails() => _invoiceBussiness.GetInvoiceDetails();

        [HttpGet]
        [Route("getInvoiceCode")]
        public IActionResult getInvoiceCode() => Ok(new { Result = _invoiceBussiness.GetCode() });

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        public IActionResult Post([FromBody]AddInvoiceCommand _AddInvoiceCommand) => Ok(new { result = _mediator.Send(_AddInvoiceCommand).Result });

        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        [Route("EditInvoiceData")]
        public IActionResult EditInvoiceData([FromBody]EditInvoiceDataCommand _EditInvoiceDataCommand) => Ok(new { result = _mediator.Send(_EditInvoiceDataCommand).Result });


        [HttpPost]
        [Route("editInvoice")]
        public IActionResult editInvoice([FromBody]EditInvoiceCommand _EditInvoiceCommand) => Ok(new { Result = _mediator.Send(_EditInvoiceCommand).Result });

        [HttpGet]
        [Route("[action]/{EmployeeId?}")]
        public IActionResult getBaseItems(int EmployeeId) => Ok( _invoiceBussiness.GetBaseItemsFromInvoice(EmployeeId) );
    }
}