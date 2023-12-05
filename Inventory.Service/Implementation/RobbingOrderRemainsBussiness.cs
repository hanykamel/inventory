using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Service.Implementation
{
    public class RobbingOrderRemainsBussiness : IRobbingOrderRemainsBussiness
    {
        readonly private IRepository<RobbingOrderRemainsDetails, Guid> _robbingOrderRemainsRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<StoreItemBussiness> _logger;
        public RobbingOrderRemainsBussiness(
            ITenantProvider tenantProvider,
            IMapper mapper,
            ILogger<StoreItemBussiness> logger,
            IStringLocalizer<SharedResource> localizer,
            IRepository<RobbingOrderRemainsDetails, Guid> robbingOrderRemainsRepository
            )
        {
            _robbingOrderRemainsRepository = robbingOrderRemainsRepository;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public RobbingOrderRemainsBussiness()
        {
        }

        //get store item grouped by baseitemId
        public List<BaseItemStoreItemVM> GetFormStoreItems(List<Guid> robbingOrderRemains)
        {

            return _robbingOrderRemainsRepository.GetAll(x => robbingOrderRemains.Contains(x.Id), true)
                .Include(x => x.ExecutionOrderResultRemain)
               .ThenInclude(x => x.Unit)
               .Include(x => x.ExecutionOrderResultRemain)
               .ThenInclude(x => x.Currency)
               .Include(x => x.ExecutionOrderResultRemain)
               .ThenInclude(x => x.Remains)
               .ThenInclude(r=>r.RemainsDetails)
               .Include(x => x.ExecutionOrderResultRemain)
               .Select(a => new BaseItemStoreItemVM
               {
                   ItemStatus = "",
                   BaseItemDesc = a.ExecutionOrderResultRemain.Remains.Description,
                   BaseItemName = a.ExecutionOrderResultRemain.Remains.Name,
                   BaseItemId = a.ExecutionOrderResultRemain.Remains.Id,
                   UnitName = a.ExecutionOrderResultRemain.Unit.Name,
                   Quantity = a.ExecutionOrderResultRemain.Quantity,
                   UnitPrice = a.Price,
                   FullPrice = a.Price,
                   Currency = a.ExecutionOrderResultRemain.Currency.Name,
                   Notes = a.ExaminationReport,
                   ExaminationReport = a.ExaminationReport
               }).ToList();
        }
    }

}
