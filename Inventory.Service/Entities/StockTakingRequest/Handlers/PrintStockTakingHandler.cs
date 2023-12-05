using Inventory.Service.Interfaces;
using Inventory.Data.Models.StoreItemVM;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Data.Models.PrintTemplateVM;
using System.Linq;
using System;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Repository;
using Inventory.Data.Enums;

namespace Inventory.Service.Entities.StockTakingRequest.Handlers
{
    class PrintStockTakingHandler : IRequestHandler<PrintStockTakingCommand, MemoryStream>
    {
        private readonly IStockTakingBussiness _stockTakingBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IStoreItemCopyBussiness _storeItemCopyBussiness;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IRobbedStoreItemBussiness _robbedStoreItemBussiness;
        public PrintStockTakingHandler(
            IStockTakingBussiness stockTakingBussiness,
            IWordBusiness wordBusiness,
            IStoreItemBussiness storeItemBussiness,
            IStoreBussiness storeBussiness,
            IStoreItemCopyBussiness storeItemCopyBussiness,
            IRobbedStoreItemBussiness robbedStoreItemBussiness
            )
        {
            _stockTakingBussiness = stockTakingBussiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _storeBussiness = storeBussiness;
            _storeItemCopyBussiness = storeItemCopyBussiness;
            _robbedStoreItemBussiness = robbedStoreItemBussiness;
        }



        public Task<MemoryStream> Handle(PrintStockTakingCommand request, CancellationToken cancellationToken)
        {
            var stockTakingStoreItems = _stockTakingBussiness.GetStockTakingStoreItems(request.StockTakingId);
            var StockTakingRobbedStoreItem = _stockTakingBussiness.GetStockTakingRobbedStoreItem(request.StockTakingId);

            if (stockTakingStoreItems != null && stockTakingStoreItems.Count > 0)
            {
                Form6VM MainData = new Form6VM();
                MainData.StoreName = _storeBussiness.GetStoreName(stockTakingStoreItems.FirstOrDefault().StoreItem.Store.Id);
                MainData.BudgetName = stockTakingStoreItems.FirstOrDefault().StoreItem.Addition.Budget.Name;
                MainData.StockTakingDate = stockTakingStoreItems.FirstOrDefault().StockTaking.Date.ToString("yyyy/MM/dd");
                MainData.StoreKeeper = stockTakingStoreItems.FirstOrDefault().StoreItem.Store.AdminId;
                MainData.CurrencyList = new List<string>();
                MainData.TotalPriceList = new List<string>();
                MainData.Currency = stockTakingStoreItems.FirstOrDefault().StoreItem.Currency != null ? stockTakingStoreItems.FirstOrDefault().StoreItem.Currency.Name : "";
                List<Guid> Ids = stockTakingStoreItems.Select(x => x.StoreItemId).ToList();
                List<FormNo6StoreItemVM> baseItemStoreItemVMs = _storeItemCopyBussiness.GetStocktackingStoreItems(Ids);
                if(StockTakingRobbedStoreItem != null && StockTakingRobbedStoreItem.Count > 0)
                {
                    List<Guid> IdstockTakens = StockTakingRobbedStoreItem.Select(x => x.RobbedStoreItemId).ToList();
                    List<FormNo6StoreItemVM> baseItemRobbedStoreItemVMs = _robbedStoreItemBussiness.GetStocktackingRobbedStoreItems(IdstockTakens);
                    baseItemStoreItemVMs.AddRange(baseItemRobbedStoreItemVMs);
                }

                //call here the function  GetStocktackingRobbedStoreItems then union with the storeitems
                return Task.FromResult(_wordBusiness.printStocktakingMultiDocument(MainData, baseItemStoreItemVMs, PrintDocumentTypesEnum.StocktakingForm.ToString()));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
