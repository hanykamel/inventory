using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
namespace Inventory.Service.Interfaces
{
   public interface IBaseItemBussiness
    {
        Task<long> AddNewBaseItem(BaseItem _BaseItem);
        Task<bool>  UpdateBaseItem();
        BaseItem ViewBaseItem(long BaseItemId);
        Task<bool> Activate(long BaseItemId,bool activation);
        IQueryable<BaseItem> GetAllBaseItem();
        IQueryable<BaseItem> GetActiveBaseItems();
        IQueryable<BaseItem> GetAllPrintBaseItem();
        bool checkDeactivation(long BaseItemId);

    }
}
