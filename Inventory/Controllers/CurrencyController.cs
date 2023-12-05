using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyBussiness _currencyBussiness;

        public CurrencyController(ICurrencyBussiness currencyBussiness, IMediator mediator)
        {
            _currencyBussiness = currencyBussiness;

        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<Currency> Get() => _currencyBussiness.GetAllCurrency();

    }
}