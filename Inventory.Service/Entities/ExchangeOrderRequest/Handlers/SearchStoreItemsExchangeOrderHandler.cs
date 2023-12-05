using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Handlers
{
    public class SearchStoreItemsExchangeOrderHandler : IRequestHandler<SearchStoreItemsExchangeOrderCommand, ExchangeorderStoreItemVM>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchStoreItemsExchangeOrderHandler> _logger;
        private readonly IMapper _mapper;

        public SearchStoreItemsExchangeOrderHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IUnitOfWork unitOfWork,
            ILogger<SearchStoreItemsExchangeOrderHandler> logger,
            IMapper mapper
            )
        {
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ExchangeorderStoreItemVM> Handle(SearchStoreItemsExchangeOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //if (request.BaseItemId <= 0)
                //{
                //    throw new NullInputsException();
                //}

                int count;
                int takeitem;
                int available;
                int Expenses;
                List<StoreItemVM> result = new List<StoreItemVM>();
                ExchangeorderStoreItemVM model = new ExchangeorderStoreItemVM();
                model.AllStoreItemVM = new List<StoreItemVM>();
                // if (request.statusitem == null)
                // {
                result = _exchangeOrderBussiness.SearchStoreItems(request.BaseItemId, request.BudgetId, request.ItemCategoryId,request.PageNumber, request.ContractNumber,request.SelectStoreItemId, out count, out takeitem, out Expenses,out available)
                    .Select(o => _mapper.Map<StoreItemVM>(o)).ToList();
                model.count = count;
                model.itemUnAvailable = takeitem;
                model.itemAvailable = available;
                model.ExpensesItem = Expenses;


                model.AllStoreItemVM = result;



                return Task.FromResult(model).Result;
            }
            catch (InventoryExceptionBase)
            {
                throw new NotSavedException();
            }
        }

    }
}