using Inventory.Data.Entities;
using Inventory.Data.Models.Delegation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface IDelegationBussiness
    {
        IQueryable<Delegation> GetAllDelegation();
        IQueryable<Delegation> GetAllDelegationList();
        Task<Delegation> SaveDelegation(Delegation delegation);
        Task<Guid> EditDelegation(Delegation delegation);
        IQueryable<Shift> GetAllShift();
        List<StoreDelegationVM> GetStore();
        Task<bool> StopDelegation(Guid DelegationId);
        IQueryable<Delegation> checkDelegation(Delegation request, List<int> storeId);
        IQueryable<DelegationTrack> GetDelegationTrack();
        IQueryable<Delegation> GetAllDelegationView();
        void checkDelegationTechnican(Delegation request, List<int> storeId);

    }
}
