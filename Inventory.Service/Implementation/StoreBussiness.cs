using inventory.Engines.LdapAuth;
using inventory.Engines.LdapAuth.Entities;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class StoreBussiness : IStoreBussiness
    {

        private readonly IRepository<Store, int> _StoreRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public StoreBussiness(IRepository<Store, int> StoreRepository,
             IStringLocalizer<SharedResource> Localizer)
        {
            _StoreRepository = StoreRepository;
            _Localizer = Localizer;
        }
        public async Task<Store> AddNewStore(Store _Store)
        {

            var count = 0;
            if (_Store.StoreTypeId == (int)StoreTypeEnum.Robbing)
            {
                count = _StoreRepository.GetAll().Where(s => s.RobbingBudgetId == _Store.RobbingBudgetId).Count();
                if (count>0)
                {
                    throw new MoreeThanOneException(_Localizer["MoreThanOneStorePerBudgetException"]);
                }
            }

            else
            {
                count = _StoreRepository.GetAll().Where(s => s.TechnicalDepartmentId == _Store.TechnicalDepartmentId && s.StoreTypeId ==
                _Store.StoreTypeId).Count();
                if (count > 0)
                {
                    throw new MoreeThanOneException(_Localizer["MoreeThanOneException"]);
                }
            }
                _StoreRepository.Add(_Store);
                int added = await _StoreRepository.SaveChanges();
                if (added > 0)
                {
                    _Store.TenantId = _Store.Id;
                    _StoreRepository.Update(_Store);
                    await _StoreRepository.SaveChanges();
                    return await Task.FromResult(_Store);
                }
                else
                    throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public string GetStoreName(int storeId)
        {
            Store store = _StoreRepository.GetAll(x => x.IsActive == true, true)
                .Include(s => s.TechnicalDepartment)
                .Include(s => s.RobbingBudget)
                .Where(s => s.Id == storeId).FirstOrDefault();
            if (store.TechnicalDepartment != null)
            {
               return (store.StoreTypeId == (int)StoreTypeEnum.Store ? _Localizer["Store"] : _Localizer["Custody"]) +" "+ store.TechnicalDepartment.Name;
            }
            else
            {
                return _Localizer["robbing"] + " " + store.RobbingBudget.Name;
            }
        }

        public string GetAllStoreName(int storeId)
        {
            Store store = _StoreRepository.GetAll( true)
                .Include(s => s.TechnicalDepartment)
                .Include(s => s.RobbingBudget)
                .Where(s => s.Id == storeId).FirstOrDefault();
            if (store.TechnicalDepartment != null)
            {
                return (store.StoreTypeId == (int)StoreTypeEnum.Store ? _Localizer["Store"] : _Localizer["Custody"]) + " " + store.TechnicalDepartment.Name;
            }
            else
            {
                return _Localizer["robbing"] + " " + store.RobbingBudget.Name;
            }
        }

        public IQueryable<Store> GetAllStore()
        {
            var StoreList = _StoreRepository.GetAll(true);
            return StoreList;
        }

        public IQueryable<Store> GetAllPrintStore()
        {
            var StoreList = _StoreRepository.GetAll(true).Include(s=>s.StoreType);
            return StoreList;
        }

        public IQueryable<Store> GetActiveStores()
        {
            var StoreList = _StoreRepository.GetAll(s => s.IsActive == true && (s.TechnicalDepartment == null || s.TechnicalDepartment.IsActive == true), true);
            return StoreList;
        }

        public bool checkDeactivation(int StoreId)
        {
            var checkConnections = _StoreRepository
                .GetAll().Where(x=>x.Id==StoreId)
                .Where(
                   x=>x.Book.Any() || x.ExaminationCommitte.Any() 
                || x.StoreItem.Any() || x.FromTransformation.Any()
                || x.FromRobbingOrder.Any() || x.ToTransformation.Any() 
                || x.ToRobbingOrder.Any() || x.DelegationStore.Any()
                     ).Count();

            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public async Task<bool> Activate(int StoreId, bool ActivationType)
        {
            if (ActivationType)
                _StoreRepository.Activate(new Store() { Id = StoreId });
            else
                _StoreRepository.DeActivate(new Store() { Id = StoreId });

            var added = await _StoreRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new InvalidException(_Localizer["InvalidException"]);
        }

        public async Task<bool> UpdateStore(Store _Store)
        {
            _StoreRepository.PartialUpdate(_Store, x => x.AdminId, x => x.RobbingBudgetId, x => x.StoreTypeId, x => x.TechnicalDepartmentId);
            int added = await _StoreRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);

        }

        public Store ViewStore(int StoreId)
        {
            var StoreEntity = _StoreRepository.Get(StoreId);
            return StoreEntity;
        }


        public IQueryable<Store> GetTransformationStores()
        {
            return _StoreRepository.GetAll(x => x.StoreTypeId != (int)StoreTypeEnum.Robbing
            && x.IsActive == true, true);
        }
        public IQueryable<Store> GetRobbingStores()
        {
            return _StoreRepository.GetAll(x => x.StoreTypeId == (int)StoreTypeEnum.Robbing
            && x.IsActive == true, true);
        }

        public bool CheckIsRobbingStore()
        {
            
            var store = _StoreRepository.GetAll(o => o.StoreTypeId == (int)StoreTypeEnum.Robbing).ToList();
            if (store != null && store.Count() > 0)
                return true;
            else
                return false;

        }

    }
}
