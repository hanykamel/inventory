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
    class PrintAdditionHandler : IRequestHandler<PrintAdditionCommand, MemoryStream>
    {
        private readonly IAdditionBussiness _additionBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IStoreBussiness _storeBussiness;


        public PrintAdditionHandler(IAdditionBussiness additionBussiness,
            IWordBusiness wordBusiness,
            IStoreItemBussiness storeItemBussiness,
            IStoreBussiness storeBussiness)
        {
            _additionBussiness = additionBussiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _storeBussiness = storeBussiness;
        }



        public Task<MemoryStream> Handle(PrintAdditionCommand request, CancellationToken cancellationToken)
        {
            //get Addition
            Addition addition = _additionBussiness.GetAdditionById(request.AdditionId);
            if (addition != null)
            {
                var StoreId = addition.TenantId;
                //pobulate form2MainData object
                TransactionsVM form2MainData = new TransactionsVM();
                form2MainData.StoreName = _storeBussiness.GetStoreName(StoreId);
                form2MainData.AdditionDocumentType = addition.AdditionDocumentType != null ? addition.AdditionDocumentType.Name : "";
                form2MainData.CreationDate = addition.Date.ToString("yyyy/MM/dd");
                form2MainData.RequestDate = addition.RequestDate.ToString("yyyy/MM/dd");
                form2MainData.RequesterName = addition.RequesterName;
                form2MainData.Serial = addition.AdditionNumber.ToString();
                form2MainData.IsAddition = true;
                form2MainData.Currencies = new List<string>();
                form2MainData.BudgetName = addition.Budget.Name;
                //get storeitemsVm
                //we ignore filter to get all items ,but not delete ones
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = _storeItemBussiness.GetFormStoreItems(addition.
                    StoreItem.Where(o=>o.IsDelete==null).Select(x => x.Id).ToList());
                form2MainData.Currency = baseItemStoreItemVMs[0].Currency ;
                return Task.FromResult(_wordBusiness.printAdditionMultiDocument(form2MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
