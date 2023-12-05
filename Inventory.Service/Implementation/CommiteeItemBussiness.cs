using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class CommiteeItemBussiness : ICommiteeItemBussiness
    {
        readonly private IRepository<CommitteeItem, Guid> _committeeItemRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly ITenantProvider tenantProvider;

        public CommiteeItemBussiness(
            IRepository<CommitteeItem, Guid> committeeItemRepository,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider

            )
        {
            _committeeItemRepository = committeeItemRepository;
            this.codeGenerator = codeGenerator;
            this.tenantProvider = tenantProvider;
        }

        public async Task UpdateNotesAsync(List<Guid> ids, List<string> notes)
        {
            var update = false;
            if (ids != null)
                for (int i = 0; i < ids.Count; i++)
                {
                    if (string.IsNullOrEmpty(notes[i]) || ids[i] == null || ids[i] == Guid.Empty)
                        continue;
                    update = true;
                    var commiteeItem = _committeeItemRepository.GetFirst(o => o.Id == ids[i]);
                    commiteeItem.AdditionNotes = notes[i];
                }
            if (update)
                await _committeeItemRepository.SaveChanges();
        }
        public List<CommitteeItem> GetAdditionCommitteeItem(Guid additionId, List<BaseItemStoreItemVM> baseItemStoreItemList)
        {
            var baseItemList = baseItemStoreItemList.Select(x => x.BaseItemId);
            return _committeeItemRepository.
                GetAll(x => x.ExaminationCommitte.Addition.Any(y => y.Id == additionId))
                .Include(x=>x.BaseItem)
                .Include(x=>x.Unit)

                .Where(x => baseItemList.Contains(x.BaseItemId)).ToList();
        }
        public List<CommitteeItem> GetBystoreItem(List<StoreItem> storeItems)
        {
            var baseItemIds = storeItems.Select(x=>x.BaseItemId);
            var examinationIds = storeItems.Select(x => x.Addition.ExaminationCommitteId);

            return _committeeItemRepository.GetAll
                (c => baseItemIds.Any(i => i == c.BaseItemId)
                && examinationIds.Any(i=>i ==c.ExaminationCommitteId )
                ).Include(c=>c.ExaminationCommitte)
                  .Include(c => c.Unit).Distinct()
                  .ToList();
        }
       
    }
}
