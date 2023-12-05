using System.Linq;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
    [Route("api/[controller]")]
    
    public class BudgetController : Controller
    {
        private readonly IBudgetBussiness _budgetBussiness;

        public BudgetController(IBudgetBussiness _IBudgetBussiness, IMediator mediator)
        {
            _budgetBussiness = _IBudgetBussiness;

        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<Budget> Get() => _budgetBussiness.GetAllBudget();

    }
}