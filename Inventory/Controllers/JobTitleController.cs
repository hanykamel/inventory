using System.Linq;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Service.Entities.JobTitleRequest.Commands;
using Inventory.Service.Interfaces;
using Inventory.Web.Attributes;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class JobTitleController : ControllerBase
    {

        readonly IMediator _mediator;

        private readonly IJobTitleBussiness _jobTitleBussiness;

        public JobTitleController(IJobTitleBussiness _IJobTitleBussiness, IMediator mediator)
        {
            _jobTitleBussiness = _IJobTitleBussiness;
            _mediator = mediator;
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        public IActionResult Get() => Ok(_jobTitleBussiness.GetActiveJobTitles());

        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet]
        [EnableQuery]
        [Route("GetAll")]
        public IActionResult GetAll() => Ok(_jobTitleBussiness.GetAllJobTitle());

        [Authorize(Policy = InventoryAuthorizationPolicy.SystemLookups)]
        [HttpPost]
        public IActionResult Post([FromBody]AddJobTitleCommand _AddJobTitleCommand) => Ok(new { Success = "true", result = _mediator.Send(_AddJobTitleCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPut]
        public IActionResult Put([FromBody]EditJobTitleCommand _EditJobTitleCommand) => Ok(new { Success = "true", result = _mediator.Send(_EditJobTitleCommand).Result });
        [Authorize(Policy = InventoryAuthorizationPolicy.SystemAdmin)]
        [HttpPost]
        [Route("Activate")]
        public IActionResult Activate([FromBody]ActivateJobTitleCommand _ActivateJobTitleCommand) => Ok(new { result = _mediator.Send(_ActivateJobTitleCommand).Result });

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintJobTitlesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.JobsTitlesList })]
        public IActionResult PrintJobTitlesList() => Ok(_jobTitleBussiness.GetAllJobTitle());
    }
}