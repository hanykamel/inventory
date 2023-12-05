using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface ISupplierBussiness
    {
        Task<Supplier> AddNewSupplier(Supplier _Supplier);
        Task<bool> UpdateSupplier(Supplier _Supplier);
        Supplier ViewSupplier(int SupplierId);
        Task<bool> Activate(int SupplierId, bool ActivationType);

        bool checkDeactivation(int SupplierId);

        IQueryable<Supplier> GetAllSupplier();
        IQueryable<Supplier> GetActives();


    }
}
