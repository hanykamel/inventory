using Inventory.Data.Entities;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.AdditionRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface IAdditionBussiness
    {
        Task<Guid> AddNewAddition(Addition _Addition);
        Addition GetAdditionById(Guid additionId);

        Task<bool> UpdateAddition(EditAdditionCommand _Addition);
        Addition ViewAddition(Guid AdditionId);
        bool Activate(Guid AdditionId);
        IQueryable<Addition> GetAllAddition();
        IQueryable<Addition> GetAdditionView();

        IQueryable<AdditionListVM> PrintAdditionList();
        string GetCode(int Max);
        string GetCode();
        string GetLastCode();
        int GetMaxAdditionNumber();
    }
}

