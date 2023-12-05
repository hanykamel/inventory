using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Data.Models.Shared;
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
    public class GetBudgetItemsHandler : IRequestHandler<GetBudgetItemsCommand, BudgetCategoryItemsVM>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchStoreItemsExchangeOrderHandler> _logger;


        public GetBudgetItemsHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IUnitOfWork unitOfWork,
            ILogger<SearchStoreItemsExchangeOrderHandler> logger
            )
        {
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<BudgetCategoryItemsVM> Handle(GetBudgetItemsCommand request, CancellationToken cancellationToken) 
        {
            try
            {
                List<BaseItem> baseItems = _storeItemBussiness.GetBaseItemsByBudgetId(request.budgetId);
                //;
                List<ItemCategory> itemCategories = baseItems.Select(o => o.ItemCategory).Distinct(new ItemCategoryComparer()).ToList();
                BudgetCategoryItemsVM result = new BudgetCategoryItemsVM()
                {
                    Categories = itemCategories.Select(o => new LookupVM<int>() { Id = o.Id, Name = o.Name }).ToList(),
                    BaseItems = baseItems.Select(o => new { o.Id, o.Name , o.ItemCategoryId , o.WarningLevel }).ToList(),
                };
                return Task.FromResult(result).Result;
            }
            catch (InventoryExceptionBase)
            {
                throw new NotSavedException();
            }
        }
        internal class ItemCategoryComparer : IEqualityComparer<ItemCategory>
        {
            public bool Equals(ItemCategory x, ItemCategory y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(ItemCategory obj)
            {
                return obj.Id;
            }
        }

    }
}