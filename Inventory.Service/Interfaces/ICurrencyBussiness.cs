using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Interfaces
{
  public  interface ICurrencyBussiness
    {
        IQueryable<Currency> GetAllCurrency();
    }
}
