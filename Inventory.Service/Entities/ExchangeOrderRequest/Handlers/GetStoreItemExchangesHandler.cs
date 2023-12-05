using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Handlers
{
    public class GetStoreItemExchangesHandler : IRequestHandler<GetStoreItemExchangesCommand, List<StoreItemExchangesVM>>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchStoreItemsExchangeOrderHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public GetStoreItemExchangesHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IUnitOfWork unitOfWork,
            ILogger<SearchStoreItemsExchangeOrderHandler> logger,
            IMapper mapper,
             IStringLocalizer<SharedResource> localizer 
            )
        {
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _localizer = localizer;
        }
        public async Task<List<StoreItemExchangesVM>> Handle(GetStoreItemExchangesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.id == null || request.id == Guid.Empty )
                    throw new NullInputsException(_localizer["InvalidModelStateException"]);
                List<StoreItemExchangesVM> result = _storeItemBussiness.SearchStoreItemExchanges(request.id);                  
                return Task.FromResult(result).Result;
            }
            catch (InventoryExceptionBase e)
            {
                throw new NotSavedException(_localizer["NotSavedException"]);
            }
        }

    }
}