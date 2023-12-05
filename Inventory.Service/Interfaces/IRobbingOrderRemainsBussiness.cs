using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;


namespace Inventory.Service.Interfaces
{
    public interface IRobbingOrderRemainsBussiness
    {
        List<BaseItemStoreItemVM> GetFormStoreItems(List<Guid> storeItems);
    }
}
