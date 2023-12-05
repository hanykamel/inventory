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
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;

namespace Inventory.Service.Entities.EcecutionOrderRequest.Handlers
{
    class PrintExecutionOrderHandler : IRequestHandler<PrintExecutionOrderCommand, MemoryStream>
    {
        private readonly IExecutionOrderBussiness _executionBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IStoreBussiness _storeBussiness;


        public PrintExecutionOrderHandler(
            IExecutionOrderBussiness executionBussiness,
            IWordBusiness wordBusiness,
            IStoreItemBussiness storeItemBussiness,
            IStoreBussiness storeBussiness)
        {
            _executionBussiness = executionBussiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _storeBussiness = storeBussiness;
        }



        public Task<MemoryStream> Handle(PrintExecutionOrderCommand request, CancellationToken cancellationToken)
        {
            //get Addition
            ExecutionOrder execution = _executionBussiness.GetExecutionOrderById(request.ExecutionOrderId);
            if (execution != null&& execution.Subtraction!=null&& execution.Subtraction.Count()>0)
            {
                Subtraction subtraction = execution.Subtraction.FirstOrDefault();
                var StoreId = execution.TenantId;
                //pobulate form2MainData object
                TransactionsVM form2MainData = new TransactionsVM();
                form2MainData.StoreName = _storeBussiness.GetStoreName(StoreId);
                form2MainData.AdditionDocumentType = "";
                form2MainData.BudgetName = execution.Budget.Name;
                form2MainData.CreationDate = subtraction.Date.ToString("yyyy/MM/dd");
                form2MainData.RequestDate = subtraction.RequestDate.ToString("yyyy/MM/dd");
                form2MainData.RequesterName = subtraction.RequesterName;
                form2MainData.Serial = subtraction.SubtractionNumber.ToString();
                form2MainData.IsAddition = false;
                form2MainData.Currencies = new List<string>();
                //get storeitemsVm
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = _storeItemBussiness.GetFormStoreItems(execution.ExecutionOrderStoreItem.Where(x => x.IsApproved==true).Select(x => x.StoreItemId).ToList());
                form2MainData.Currency = baseItemStoreItemVMs!=null&& baseItemStoreItemVMs.Count>0?baseItemStoreItemVMs[0].Currency:"";
                return Task.FromResult(_wordBusiness.printAdditionMultiDocument(form2MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
