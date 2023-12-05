using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.DepartmentRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        readonly IMediator _mediator;
        private readonly IDepartmentBussiness _departmentBussiness;

        public DepartmentController(IDepartmentBussiness _iDepartmentBussiness,
            IMediator mediator)
        {
            _departmentBussiness = _iDepartmentBussiness;
            _mediator = mediator;
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        public IActionResult Get() => Ok( _departmentBussiness.GetActiveDepartments() );

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_departmentBussiness.GetAllDepartment());

       
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddDepartmentCommand _AddDepartmentCommand) => Ok(new { Success = "true", result = _mediator.Send(_AddDepartmentCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditDepartmentCommand _EditDepartmentCommand) => Ok(new { Success = "true", result = _mediator.Send(_EditDepartmentCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateDepartmentCommand _ActivateDepartmentCommand) => Ok(new { result = _mediator.Send(_ActivateDepartmentCommand).Result });
    
    
    }
}