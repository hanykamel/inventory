using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.BookRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    
    [Route("api/[controller]")]
    public class SubtractionController : Controller
    {
        readonly IMediator _mediator;
        private readonly ISubtractionBussiness _SubtractionBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public SubtractionController(IMediator mediator, ISubtractionBussiness SubtractionBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _mediator = mediator;
            _SubtractionBussiness = SubtractionBussiness;
            _localizer = Localizer;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        [EnableQuery]
        public IActionResult Get() => Ok(_SubtractionBussiness.GetActiveSubtractions());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_SubtractionBussiness.GetAllSubtractions());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddBookCommand _AddBookCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new { Success = "true", result = _mediator.Send(_AddBookCommand).Result });
        }

        [Authorize(Policy = InventoryAuthorizationPolicy.AllTransactions)]
        [HttpGet("[action]")]
        public IActionResult GetSubtrationNumber() => Ok(new {  subtractionNumber = _SubtractionBussiness.GetMaxSubtractionNumber()});


        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPut]
        public IActionResult Put([FromBody]EditBookCommand _EditBookCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new { Success = "true", result = _mediator.Send(_EditBookCommand).Result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateBookCommand _ActivateBookCommand) => Ok(new { result = _mediator.Send(_ActivateBookCommand).Result });

       
    }
}