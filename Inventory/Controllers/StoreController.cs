
using inventory.Engines.LdapAuth;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Enums;
using Inventory.Service.Entities.StoreRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {
        readonly IMediator _mediator;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;

        public StoreController(IMediator mediator, IStoreBussiness _IStoreBussiness, IStringLocalizer<SharedResource> Localizer,
                         ILdapAuthenticationService ldapAuthenticationService)
        {
            _storeBussiness = _IStoreBussiness;

            _mediator = mediator;
            _localizer = Localizer;
            _ldapAuthenticationService = ldapAuthenticationService;
        }

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get() => Ok(_storeBussiness.GetActiveStores());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [EnableQuery]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_storeBussiness.GetAllStore());



        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [Route("GetStoreName")]
        public IActionResult GetStoreName(int StoreId) => Ok(new { result= _storeBussiness.GetStoreName(StoreId) });


        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [EnableQuery]
        [HttpGet]
        [Route("GetTransformationStores")]
        public IActionResult GetTransformationStores() => Ok(_storeBussiness.GetTransformationStores());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [EnableQuery]
        [HttpGet]
        [Route("GetRobbingStores")]
        public IActionResult GetRobbingStores() => Ok(_storeBussiness.GetRobbingStores());


        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        public IActionResult Post([FromBody]AddStoreCommand _AddStoreCommand)
        {
                if (!ModelState.IsValid)
                {
                    throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
                }
                if (_ldapAuthenticationService.GetUserNames(_AddStoreCommand.Admin).Count == 0)
                {
                    throw new NullSearchValueException(_localizer["UserNotFoundException"]);
                }
            return Ok(_mediator.Send(_AddStoreCommand).Result);
         
        }

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditStoreCommand _EditStoreCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            if (_ldapAuthenticationService.GetUserNames(_EditStoreCommand.Admin).Count == 0)
            {
                throw new NullSearchValueException(_localizer["UserNotFoundException"]);
            }
            return Ok(new { _mediator.Send(_EditStoreCommand).Result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateStoreCommand _ActivateStoreCommand) => Ok(new { result = _mediator.Send(_ActivateStoreCommand).Result });

       
    }
}