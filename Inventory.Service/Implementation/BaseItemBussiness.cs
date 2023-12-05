using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Inventory.Data.Models.BaseItem;
using Inventory.Data.Enums;
using Inventory.CrossCutting.Tenant;

namespace Inventory.Service.Implementation
{
    public class BaseItemBussiness : IBaseItemBussiness
    {
         readonly private IRepository<BaseItem, long> _baseItemRepository;
        private readonly ITenantProvider _tenantProvider;

        public BaseItemBussiness(IRepository<BaseItem, long> baseItemRepository,
             ITenantProvider tenantProvider)
        {
            _baseItemRepository = baseItemRepository;
            _tenantProvider = tenantProvider;
        }

        public bool checkDeactivation(long BaseItemId)
        {
         
            var checkConnections = _baseItemRepository.GetAll()

                .Where(x => x.Id == BaseItemId)
                .Where(x => x.StoreItem.Any() || x.CommitteeItem.Any())
                .Count();

            if (checkConnections > 0)
            {
                return false; 
            }
            else
            { 
                return true; 
            }
        }
        public async Task<bool> Activate(long baseItemId, bool ActivationType)
        {
            if (ActivationType)
                _baseItemRepository.Activate(new BaseItem() { Id = baseItemId });
            else           
                _baseItemRepository.DeActivate(new BaseItem() { Id = baseItemId });           
            var added = await _baseItemRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<long> AddNewBaseItem(BaseItem _BaseItem)
        {

            _baseItemRepository.Add(_BaseItem);
            int added = await _baseItemRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_BaseItem.Id);
            else
                return await Task.FromResult(0);

        }

        public IQueryable<BaseItem> GetAllBaseItem()
        {
            return _baseItemRepository.GetAll(true);
        }
        public IQueryable<BaseItem> GetAllPrintBaseItem()
        {
            return _baseItemRepository.GetAll(true)
                .Include(b=>b.ItemCategory)
                .Include(b=>b.DefaultUnit);
        }
        public IQueryable<BaseItem> GetActiveBaseItems()
        {
            var tenant = _tenantProvider.GetTenant();
            var x = _baseItemRepository.GetAll();
            return x;
        }


        //public IQueryable<BaseItemBudgetVM> GetActiveBaseItemsBudget(int BudgetID)
        //{ 

        ////    &&
        ////        s.StoreItem.Any(d => d.StoreItemStatusId == (int)ItemStatusEnum.Available)


        //    var result= _baseItemRepository.GetAll().Include(s => s.StoreItem).ThenInclude(s => ((StoreItem)s).Addition)
        //        .Where(s => s.StoreItem.Select(d => d.Addition.BudgetId).Contains(BudgetID))
        //        .Select(a => new BaseItemBudgetVM() { BaseItemID = a.Id, BaseItemName = a.Name, CountStoreItem = a.StoreItem.Count(s=>s.CurrentItemStatusId == (int)ItemStatusEnum.Available) });
        //    return result;
        //}
        public async Task<bool> UpdateBaseItem()
        {

            //_baseItemRepository.Update(_BaseItem);
            int added = await _baseItemRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);



        }

        public BaseItem ViewBaseItem(long BaseItemId)
        {
            var StoreEntity = _baseItemRepository.Get(BaseItemId);
            return StoreEntity;
        }

    

    }
}
