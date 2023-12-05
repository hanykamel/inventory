using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Inventory.Data.Models.DeductionVM;
using Microsoft.AspNet.OData;
using Inventory.Service.Entities.DeductionRequest.Commands;
using MediatR;
using Inventory.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class DeductionController : ControllerBase
    {
        private readonly IDeductionBusiness _deductionBusiness;
        private readonly IMediator _mediator;

        public DeductionController(IDeductionBusiness deductionBusiness, IMediator mediator)
        {
            _deductionBusiness = deductionBusiness;
            _mediator = mediator;

        }


        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet("[action]")]
        [EnableQuery]
        public IQueryable<Deduction> Get() => _deductionBusiness.GetAllDeduction();


        [Authorize(Policy =InventoryAuthorizationPolicy.ViewData)]
        [HttpGet("[action]")]
        [EnableQuery(MaxExpansionDepth = 5)]
        [Route("GetDeductionDetails")]

        public IQueryable<Deduction> GetDeductionDetails() => _deductionBusiness.GetDeductionDetails();


        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [EnableQuery]
        [HttpPost("[action]")]
        [Route("GetDeductionItems")]
        public IQueryable<DeductionVM> GetDeductionItems() => _deductionBusiness.GetDeductedItems();



        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpGet("[action]")]
        public IActionResult GetInitalValues() => Ok(new { generatedCode = _deductionBusiness.GetCode(), 
            lastGeneratedCode = _deductionBusiness.GetLastCode() });



        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost("[action]")]
        public IActionResult GetStoreItems([FromBody] long baseItemId) => Ok(new { data = _deductionBusiness.GetStoreItems(baseItemId) });



        [Authorize(Policy = InventoryAuthorizationPolicy.StoreKeeper )]
        [HttpPost]
        public IActionResult Post([FromBody]SaveDeductionCommand _saveDecutionCommand)
        {

          
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(_saveDecutionCommand).Result
            });

        }
    }
}