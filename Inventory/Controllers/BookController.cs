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
    public class BookController : Controller
    {
        readonly IMediator _mediator;
        private readonly INoteBooksBussiness _noteBooksBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public BookController(IMediator mediator, INoteBooksBussiness noteBooksBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _mediator = mediator;
            _noteBooksBussiness = noteBooksBussiness;
            _localizer = Localizer;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        [EnableQuery]
        public IActionResult Get() => Ok(_noteBooksBussiness.GetActiveBooks());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_noteBooksBussiness.GetAllBook());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddBookCommand _AddBookCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new { Success = "true", result = _mediator.Send(_AddBookCommand).Result });
        }

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