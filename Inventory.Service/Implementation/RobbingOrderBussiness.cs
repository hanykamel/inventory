using inventory.Engines.CodeGenerator;
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
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class RobbingOrderBussiness : IRobbingOrderBussiness
    {

        private readonly IRepository<RobbingOrder, Guid> _robbingOrderRepository;
        private readonly IRepository<RobbingOrderStoreItem, Guid> _RobbingOrderStoreItemRepository;
        private readonly IRepository<RobbingOrderRemainsDetails, Guid> _robbingOrderRemainsDetailsRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ITenantProvider _tenantProvider;

        public RobbingOrderBussiness(
            IRepository<RobbingOrder, Guid> RobbingOrderRepository,
            IStringLocalizer<SharedResource> Localizer,
            ICodeGenerator codeGenerator, ITenantProvider tenantProvider,
            IRepository<RobbingOrderStoreItem, Guid> RobbingOrderStoreItemRepository,
            IRepository<RobbingOrderRemainsDetails, Guid> robbingOrderRemainsDetails)
        {
            _robbingOrderRepository = RobbingOrderRepository;
            _localizer = Localizer;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _RobbingOrderStoreItemRepository = RobbingOrderStoreItemRepository;
            _robbingOrderRemainsDetailsRepository = robbingOrderRemainsDetails;
        }



        public IQueryable<RobbingOrder> GetAllRobbingOrder()
        {
            return _robbingOrderRepository.GetAll(x => x.IsActive == true, true);
        }

        public IQueryable<RobbingOrder> PrintRobbingOrdersList()
        {
            return _robbingOrderRepository.GetAll(x =>
                    (x.ToStoreId == _tenantProvider.GetTenant() || x.FromStoreId == _tenantProvider.GetTenant() ||
                    _tenantProvider.GetTenant() == 0) &&
                    x.IsActive == true, true)
                .Include(r => r.RobbingOrderStatus)
                .Include(r => r.FromStore)
                .ThenInclude(s => s.TechnicalDepartment)
                .Include(s=>s.Subtraction);
        }
        public IQueryable<RobbingOrder> GetAllRobbingOrderList()
        {
            return _robbingOrderRepository.GetAll(x => (x.ToStoreId == _tenantProvider.GetTenant() ||
            x.FromStoreId == _tenantProvider.GetTenant()
            || _tenantProvider.GetTenant() == 0) && x.IsActive == true, true);
        }

        public int GetMax()
        {
            return _robbingOrderRepository.GetMax(null, x => x.Serial) + 1;
        }

        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }

        public string GetLastCode()
        {
            var lastAddedObj = _robbingOrderRepository.GetAll().OrderByDescending(o => o.CreationDate).FirstOrDefault();
            if (lastAddedObj != null)
            {
                return lastAddedObj.Code;
            }
            return "";
        }

        public void UpdateRobbingOrderStatus(Guid robbingOrderId)
        {
            var obj = _robbingOrderRepository.Get(robbingOrderId, true);
            if (obj.RobbingOrderStatusId == (int)RobbingOrderStatusEnum.Added)
                throw new InvalidException(_localizer["transformationRequestAddedBefore"]);
            if (obj.RobbingOrderStatusId == (int)RobbingOrderStatusEnum.Cancel)
                throw new InvalidException(_localizer["RobbingRequestCanselBefore"]);
            obj.RobbingOrderStatusId = (int)TransformationOrderStatusEnum.Added;
            _robbingOrderRepository.Update(obj);
        }
        public void DeActivateRobbingRemainsItem(Guid robbingId)
        {
            var _robbingRemainsItems = _robbingOrderRemainsDetailsRepository.GetAll(x => x.RobbingOrderId == robbingId, true);

            if (_robbingRemainsItems != null)
            {
                foreach (var _robbingRemainItem in _robbingRemainsItems)
                {
                    _robbingRemainItem.IsActive = false;
                }
            }
        }


        public async Task<Guid> AddNewRobbingOrder(RobbingOrder robbingOrder)
        {
            robbingOrder.OperationId = (int)OperationEnum.Addition;
            robbingOrder.RobbingOrderStatusId = (int)RobbingOrderStatusEnum.Requested;
            robbingOrder.Id = Guid.NewGuid();
            robbingOrder.FromStoreId = _tenantProvider.GetTenant();
            robbingOrder.Serial = GetMax();
            robbingOrder.Code = GetCode();
            _robbingOrderRepository.Add(robbingOrder);
            int added = await _robbingOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(robbingOrder.Id);
            else
                throw new Exception("No records saved to database");
        }

        public async Task<Guid> AddNewRobbingOrderExecutionRemainItem(RobbingOrder robbingOrder)
        {
            robbingOrder.OperationId = (int)OperationEnum.Execution;
            robbingOrder.RobbingOrderStatusId = (int)RobbingOrderStatusEnum.Requested;
            robbingOrder.Id = Guid.NewGuid();
            robbingOrder.FromStoreId = _tenantProvider.GetTenant();
            robbingOrder.Serial = GetMax();
            robbingOrder.Code = GetCode();
            _robbingOrderRepository.Add(robbingOrder);
            int added = await _robbingOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(robbingOrder.Id);
            else
                throw new Exception("No records saved to database");
        }



        public RobbingOrder GetById(Guid Id)
        {
            return _robbingOrderRepository.GetAll(true)
               .Include(a=>a.Subtraction)
               .Include(a => a.Budget)
               .Include(a => a.RobbingOrderStoreItem)
               .Include(a => a.RobbingOrderRemainsDetails)
               .Include(r => r.FromStore)
               .ThenInclude(f => f.TechnicalDepartment)
               .Include(r => r.FromStore)
               .ThenInclude(f => f.RobbingBudget)
               .Where(e => e.Id == Id)
                .FirstOrDefault();
        }

        public bool CancelRobbingOrder(RobbingOrder robbingOrder)
        {
            if (robbingOrder != null)
                robbingOrder.RobbingOrderStatusId =
               (int)RobbingOrderStatusEnum.Cancel;


            return true;
        }

        public List<Guid> GetRobbingOrderStoreItem(Guid Id)
        {
            return _RobbingOrderStoreItemRepository.GetAll().Where(x => x.RobbingOrderId == Id).Select(x => x.StoreItemId).ToList();
        }

        public async Task<bool> Save()
        {
            int added = await _robbingOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_localizer["NotSavedException"]);
        }
    }
}
