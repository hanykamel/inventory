using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.OData.Query;
using Inventory.Service.Entities.TechnicalDepartmentRequest.Commands;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using inventory.Engines.LdapAuth;
using Inventory.Data.Entities;
using System.Linq;
using Inventory.Web.Attributes;
using Inventory.Data.Enums;
using Inventory.Web.Helpers;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class TechnicalDepartmentController : ControllerBase
    {


        readonly IMediator _mediator;
        private readonly ITechnicalDepartmentBussiness _technicalDepartmentBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;

        public TechnicalDepartmentController(IMediator mediator, ITechnicalDepartmentBussiness _TechnicalDepartmentBussiness,
             IStringLocalizer<SharedResource> Localizer, ILdapAuthenticationService ldapAuthenticationService)
        {
            _mediator = mediator;
            _technicalDepartmentBussiness = _TechnicalDepartmentBussiness;
            _Localizer = Localizer;
            _ldapAuthenticationService = ldapAuthenticationService;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult Get() => Ok(_technicalDepartmentBussiness.GetActiveTechnicalDepartments());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_technicalDepartmentBussiness.GetAllTechnicalDepartment());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddTechnicalDepartmentCommand _AddTechnicalDepartmentCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_Localizer["InvalidModelStateException"]);
            }
            if (_ldapAuthenticationService.GetUserNames(_AddTechnicalDepartmentCommand.TechnicalId).Count == 0)
            {
                throw new NullSearchValueException(_Localizer["UserNotFoundException"]);
            }
            var result = _mediator.Send(_AddTechnicalDepartmentCommand).Result;
            return Ok(result);
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditTechnicalDepartmentCommand _EditTechnicalDepartmentCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_Localizer["InvalidModelStateException"]);
            }
            if (_ldapAuthenticationService.GetUserNames(_EditTechnicalDepartmentCommand.TechnicalId).Count == 0)
            {
                throw new NullSearchValueException(_Localizer["UserNotFoundException"]);
            }
            var result = _mediator.Send(_EditTechnicalDepartmentCommand).Result;
            return Ok(result);
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateTechnicalDepartmentCommand _ActivateTechnicalDepartmentCommand) => Ok(new { result = _mediator.Send(_ActivateTechnicalDepartmentCommand).Result });



        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintTechnicalDepartmentsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.TechnicalDepartmentsList })]
        public IActionResult PrintTechnicalDepartmentsList() => Ok(_technicalDepartmentBussiness.GetAllTechnicalDepartment());
    }
}