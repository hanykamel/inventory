using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Models.Delegation;
using Inventory.Service.Entities.DelegationRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelegationController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IDelegationBussiness iDelegationBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DelegationController(IDelegationBussiness _iDelegationBussiness,
            IMediator mediator, IStringLocalizer<SharedResource> Localizer)
        {
            iDelegationBussiness = _iDelegationBussiness;
            _mediator = mediator;
            _localizer = Localizer;

        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        public IQueryable<Delegation> Get() => iDelegationBussiness.GetAllDelegation();


        [HttpGet]
        [Route("GetDelegationView")]
        [EnableQuery(MaxExpansionDepth = 3)]
        [Authorize(Policy = InventoryAuthorizationPolicy.AllValidRoles)]
        public IQueryable<Delegation> GetDelegationView() => iDelegationBussiness.GetAllDelegationView();


        [HttpGet]
        [Route("GetAllShift")]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        public IQueryable<Shift> GetAllShift() => iDelegationBussiness.GetAllShift();


        [HttpPost("[action]")]
        public IActionResult Create([FromBody] AddDelegationCommand addDelegationCommand)
        {
            if (!ModelState.IsValid)
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            return Ok(new
            {
                Success = "true",
                id = _mediator.Send(addDelegationCommand).Result
            });
        }


        [HttpGet]
        [Route("GetAllStore")]
         [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        public List<StoreDelegationVM> GetAllStore() => iDelegationBussiness.GetStore();

        


        [HttpGet("[action]")]
        public IActionResult StopDelegation([FromQuery] StopDelegationCommand stopDelegationCommand)
        {
            return Ok(new
            {
                Success = "true",
                result = _mediator.Send(stopDelegationCommand).Result
            });
        }


        [HttpGet("[action]")]
        [EnableQuery]
        [Route("GetDelegationTrack")]
        public IQueryable<DelegationTrack> GetDelegationTrack()
        {

            return iDelegationBussiness.GetDelegationTrack();
         
        }



    
    }
}