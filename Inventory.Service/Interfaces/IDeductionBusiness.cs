using Inventory.Data.Entities;
using Inventory.Data.Models.DeductionVM;
using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{  
   public interface IDeductionBusiness
    {
        IQueryable<DeductionVM> GetDeductedItems();

        List<DeductionstoreItemVM> getStoreItemId(List<long> baseItemIds);

        List<BaseItemStoreItemVM> GetDamagedItemsByDeductionId(Guid id);
        Deduction GetById(Guid id);

        Task<Guid> AddNewDeductItem(Deduction _Deduction);
        IQueryable<Deduction> PrintDeductionsList();
        string GetCode();

        string GetLastCode();
        int GetMax();

        IQueryable<StoreItem>  GetStoreItems(long baseItemId);
        IQueryable<Deduction> GetAllDeduction();
        IQueryable<Deduction> GetDeductionDetails();
    }
}
