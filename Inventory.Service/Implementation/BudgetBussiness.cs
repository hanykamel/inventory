using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Implementation
{
    public class BudgetBussiness : IBudgetBussiness
    {
        readonly private IRepository<Budget, int> _BudgetRepository;
        public BudgetBussiness(IRepository<Budget, int> BudgetRepository)
        {
            _BudgetRepository = BudgetRepository;
        }
        public bool AddNewBudget(Budget _Budget)
        {
            _BudgetRepository.Add(_Budget);
            return true;
        }

        public IQueryable<Budget> GetAllBudget()
        {
            var BudgetList = _BudgetRepository.GetAll();
            return BudgetList;
        }

        public bool Activate(int BudgetId)
        {
            var BudgetEntity = _BudgetRepository.Get(BudgetId);
            BudgetEntity.IsActive = BudgetEntity.IsActive == true ? false : true;
            return true;
        }

        public bool UpdateBudget(Budget _Budget)
        {
            _BudgetRepository.Update(_Budget);
            return true;

        }

        public Budget ViewBudget(int BudgetId)
        {
            var BudgetEntity = _BudgetRepository.Get(BudgetId);
            return BudgetEntity;
        }
    }
}
