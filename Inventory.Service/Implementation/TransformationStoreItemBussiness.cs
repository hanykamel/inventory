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
    public class TransformationStoreItemBussiness : ITransformationStoreItemBussiness
    {
        readonly private IRepository<TransformationStoreItem, Guid> _transformationStoreItemRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<StoreItemBussiness> _logger;
        public TransformationStoreItemBussiness(
            ITenantProvider tenantProvider,
            IMapper mapper,
            ILogger<StoreItemBussiness> logger,
            IStringLocalizer<SharedResource> localizer,
            IRepository<TransformationStoreItem, Guid> transformationStoreItemRepository
            )
        {
            _transformationStoreItemRepository = transformationStoreItemRepository;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public TransformationStoreItemBussiness()
        {
        }

        //get store item grouped by baseitemId
        public List<BaseItemStoreItemVM> GetFormStoreItems(List<Guid> transformationStoreItems)
        {
            return _transformationStoreItemRepository.GetAll(x => transformationStoreItems.Contains(x.Id), true)
                .Include(x => x.StoreItem)
               .ThenInclude(x => x.StoreItemStatus)
               .Include(x => x.StoreItem)
               .ThenInclude(x => x.BaseItem)
               .Include(x => x.StoreItem)
               .ThenInclude(x => x.Unit)
               .Include(x => x.StoreItem)
               .ThenInclude(x => x.Currency)
               .Include(x => x.StoreItem)
               .ThenInclude(x => x.Addition).ThenInclude(x => x.ExaminationCommitte)
               .GroupBy(x => new
               {
                   contractnum=x.StoreItem.Addition.ExaminationCommitte.ContractNumber,
                   pagenum=x.StoreItem.BookPageNumber,
                   BaseItemId = x.StoreItem.BaseItemId,
                   StoreItemStatus = x.StoreItem.StoreItemStatus.Name,
                   Description = x.StoreItem.BaseItem.Description,
                   BaseItemName = x.StoreItem.BaseItem.Name,
                   Unit = x.StoreItem.Unit.Name,
                   Price = x.StoreItem.Price,
                   Note = x.Note,
                   Currency = x.StoreItem.Currency.Name,

               })
               .Select(a => new BaseItemStoreItemVM
               {
                   ContractNumber=a.Key.contractnum,
                   PageNumber=a.Key.pagenum,
                   ItemStatus = a.Key.StoreItemStatus,
                   BaseItemDesc = a.Key.Description,
                   BaseItemName = a.Key.BaseItemName,
                   BaseItemId = a.Key.BaseItemId,
                   UnitName = a.Key.Unit,
                   Quantity = a.Count(),
                   UnitPrice = a.Key.Price,
                   FullPrice = a.Key.Price * a.Count(),
                   Currency = a.Key.Currency,
                   Notes = a.Key.Note
               }).ToList();
        }
    }

}
