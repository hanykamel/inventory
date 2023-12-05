using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Implementation
{
  public  class CurrencyBussiness : ICurrencyBussiness
    {
        readonly private IRepository<Currency, int> _CurrencyRepository;
        public CurrencyBussiness(IRepository<Currency, int> CurrencyRepository)
        {
            _CurrencyRepository = CurrencyRepository;
        }


        public IQueryable<Currency> GetAllCurrency()
        {
            var CurrencyList = _CurrencyRepository.GetAll();
            return CurrencyList;
        }
    }
}
