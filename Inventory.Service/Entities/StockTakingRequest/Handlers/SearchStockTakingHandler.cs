using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.StockTakingRequest.Handlers
{
    public class SearchStockTakingHandler : IRequestHandler<SearchStockTakingCommand, SearchStockTakingVM>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IRobbedStoreItemBussiness _robbedStoreItemBussiness;
        
        private readonly IStoreBussiness _storeBussiness;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStockTakingBussiness _stockTakingBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public SearchStockTakingHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IRefundOrderBussiness refundOrderBussiness,
            IRepository<StoreItem, Guid> storeItemRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStockTakingBussiness stockTakingBussiness,
            IStringLocalizer<SharedResource> localizer,
            IStoreBussiness storeBussiness,
            IRobbedStoreItemBussiness robbedStoreItemBussiness
            )
        {
            _storeItemRepository = storeItemRepository;
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stockTakingBussiness = stockTakingBussiness;
            _Localizer = localizer;
            _storeBussiness = storeBussiness;
            _robbedStoreItemBussiness = robbedStoreItemBussiness;
        }
        public async Task<SearchStockTakingVM> Handle(SearchStockTakingCommand request, CancellationToken cancellationToken)
        {
            int count;
            SearchStockTakingVM items = _stockTakingBussiness.SearchStoreItems(request, out count);
            if (_storeBussiness.CheckIsRobbingStore())
            {
                int countRobbing;
                SearchStockTakingVM robbedStoreItem =   _robbedStoreItemBussiness.GetRobbedStoreItem(request, out countRobbing);

                items.TotalConsumedCount = items.TotalConsumedCount + robbedStoreItem.TotalConsumedCount;
                items.TotalNotConsumedCount = items.TotalNotConsumedCount + robbedStoreItem.TotalNotConsumedCount;
                items.TotalPrice = items.TotalPrice + robbedStoreItem.TotalPrice;
                items.TotalUnitsCount = items.TotalUnitsCount + robbedStoreItem.TotalUnitsCount;

                items.BaseItems.AddRange(robbedStoreItem.BaseItems);
                request.Count = count+ countRobbing;
            }
            else
            {
                request.Count = count;

            }
            
            return items;
        }

    }
}