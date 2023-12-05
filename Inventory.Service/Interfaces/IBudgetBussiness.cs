using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Interfaces
{
   public interface IBudgetBussiness
    {

        bool AddNewBudget(Budget _Budget);
        bool UpdateBudget(Budget _Budget);
        Budget ViewBudget(int BudgetId);
        bool Activate(int BudgetId);
        IQueryable<Budget> GetAllBudget();
    }
}
