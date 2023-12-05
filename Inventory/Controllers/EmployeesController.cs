using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.EmployeeRequest.Commands;
using Inventory.Service.Entities.EmployeesRequest.Commands;
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
    public class EmployeesController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IEmployeeBussiness _employeeBussiness;

        public EmployeesController(IEmployeeBussiness _IEmployeeBussiness, IMediator mediator,
            IStringLocalizer<SharedResource> Localizer)
        {
            _employeeBussiness = _IEmployeeBussiness;
            _mediator = mediator;
            _localizer = Localizer;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        public IActionResult Get() => Ok(_employeeBussiness.GetActiveEmployees());

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_employeeBussiness.GetAllEmployees());

        [Authorize(Policy = InventoryAuthorizationPolicy.AddEmployees)]
        [HttpPost]
        public IActionResult Post([FromBody]AddEmployeeCommand _AddEmployeeCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            return Ok(new { Success = "true", result = _mediator.Send(_AddEmployeeCommand).Result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditEmployeeCommand _EditEmployeeCommand)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
            }
            return Ok(new { Success = "true", result = _mediator.Send(_EditEmployeeCommand).Result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateEmployeeCommand _ActivateEmployeeCommand) => Ok(new { result = _mediator.Send(_ActivateEmployeeCommand).Result });

       
    }
}