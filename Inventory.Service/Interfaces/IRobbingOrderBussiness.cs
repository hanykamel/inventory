using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IRobbingOrderBussiness
    {
        IQueryable<RobbingOrder> GetAllRobbingOrder();
        IQueryable<RobbingOrder> PrintRobbingOrdersList();
        RobbingOrder GetById(Guid Id);
        void UpdateRobbingOrderStatus(Guid robbingOrderId);
        string GetCode();
        string GetLastCode();
        IQueryable<RobbingOrder> GetAllRobbingOrderList();
        Task<Guid> AddNewRobbingOrder(RobbingOrder robbingOrder);
        bool CancelRobbingOrder(RobbingOrder robbingOrder);
        List<Guid> GetRobbingOrderStoreItem(Guid Id);
        Task<bool> Save();
        Task<Guid> AddNewRobbingOrderExecutionRemainItem(RobbingOrder robbingOrder);
        void DeActivateRobbingRemainsItem(Guid robbingId);
    }
}
