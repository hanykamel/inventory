using Inventory.Data.Entities;
using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface ICommiteeItemBussiness
    {
        Task UpdateNotesAsync(List<Guid> ids, List<string> notes);
        List<CommitteeItem> GetAdditionCommitteeItem(Guid additionId, List<BaseItemStoreItemVM> baseItemStoreItemList);

        List<CommitteeItem> GetBystoreItem(List<StoreItem> baseItems);
    }
}
