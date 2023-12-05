using Inventory.Service.Entities.AdditionRequest.Commands;
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
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.AdditionRequest.Handlers
{
    class PrintFormNo8Handler:IRequestHandler<PrintFormNo8Command, MemoryStream>
    {
        private readonly IAdditionBussiness _additionBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IStoreBussiness _storeBussiness;


        public PrintFormNo8Handler(IAdditionBussiness additionBussiness,
            IWordBusiness wordBusiness,
            IStoreItemBussiness storeItemBussiness,
            IStoreBussiness storeBussiness)
        {
            _additionBussiness = additionBussiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _storeBussiness = storeBussiness;
        }

        

        public Task<MemoryStream> Handle(PrintFormNo8Command request, CancellationToken cancellationToken)
        {
            //get Addition
            Addition addition = _additionBussiness.GetAdditionById(request.AdditionId);
            var storeItem = addition.StoreItem.Where(s => s.AdditionId == addition.Id).FirstOrDefault();

            //pobulate form2MainData object
            TransactionsVM form2MainData = new TransactionsVM();
            form2MainData.StoreName = _storeBussiness.GetStoreName(storeItem.StoreId);
            form2MainData.BudgetName = addition.Budget.Name;
            form2MainData.CreationDate = addition.CreationDate.ToString("yyyy/MM/dd");
            form2MainData.CustodyOwner = storeItem.Store.AdminId;

            if (addition!=null)
            {
                //get storeitemsVm
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = _storeItemBussiness.GetFormStoreItems(
                    addition.StoreItem.Select(x => x.Id).ToList());
                var m = baseItemStoreItemVMs.GroupBy(b => b.Currency);
                return Task.FromResult(_wordBusiness.printExchangeDocument(form2MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
