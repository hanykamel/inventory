using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Shared;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class StagnantBussiness : IStagnantBussiness
    {

        private readonly IRepository<Stagnant, Guid> _stagnantRepository;
        private readonly IRepository<StagnantStoreItem, Guid> _stagnantStoreItemRepository;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public StagnantBussiness(
            IRepository<Stagnant, Guid> StagnantRepository,
            IRepository<StagnantStoreItem, Guid> StagnantStoreItemRepository,
            IRepository<StoreItem, Guid> StoreItemRepository,
            IStringLocalizer<SharedResource> Localizer
            )
        {
            _stagnantRepository = StagnantRepository;
            _stagnantStoreItemRepository = StagnantStoreItemRepository;
            _storeItemRepository = StoreItemRepository;
            _Localizer = Localizer;
        }

        public async Task<bool> saveStagnantStoreItem(Stagnant stagnantModel)
        {

            _stagnantRepository.Add(stagnantModel);
            int added = await _stagnantRepository.SaveChanges();
            if (added > 0)
            {
                return await Task.FromResult(true);
            }
            else
            {
                throw new NotSavedException(_Localizer["NotSavedException"]);
            }

        }
    }
}
