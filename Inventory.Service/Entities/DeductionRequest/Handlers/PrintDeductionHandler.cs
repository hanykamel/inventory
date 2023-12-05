using Inventory.Service.Interfaces;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data.Models.StoreItemVM;
using System;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Service.Entities.DeductionRequest.Commands;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.CrossCutting.Tenant;

namespace Inventory.Service.Entities.InvoiceRequest.Handlers
{
    class PrintDeductionHandler : IRequestHandler<PrintDeductionCommand, MemoryStream>
    {
        private readonly IDeductionBusiness _deductionBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly ICommiteeItemBussiness _commiteeItemBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IWordBusiness _wordBusiness;
        private IStringLocalizer<SharedResource> _localizer;
        private readonly ITenantProvider _tenantProvider;

        public PrintDeductionHandler(IDeductionBusiness deductionBusiness,
            IStoreItemBussiness storeItemBussiness,
            ICommiteeItemBussiness commiteeItemBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IStringLocalizer<SharedResource> localizer,
            ITenantProvider tenantProvider,
        IWordBusiness wordBusiness)
        {
            _deductionBusiness = deductionBusiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _commiteeItemBussiness=commiteeItemBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _localizer = localizer;
            _tenantProvider = tenantProvider;
        }

        

        public Task<MemoryStream> Handle(PrintDeductionCommand request, CancellationToken cancellationToken)
        {
            var deductionStoreItems = _deductionBusiness.GetDamagedItemsByDeductionId(request.DeductionId);
            Deduction deduction = deductionStoreItems.FirstOrDefault().deduction;
            TransactionsVM form2MainData = new TransactionsVM();
            form2MainData.StoreName = _tenantProvider.GetTenantName();
            Subtraction subtraction = deduction.Subtraction.FirstOrDefault();
            //form2MainData.AdditionDocumentType = addition.AdditionDocumentType != null ? addition.AdditionDocumentType.Name : "";
            form2MainData.BudgetName = deduction.Budget.Name;
            form2MainData.CreationDate = subtraction.Date.ToString("yyyy/MM/dd");
            form2MainData.RequestDate = subtraction.RequestDate.ToString("yyyy/MM/dd");
            form2MainData.RequesterName = subtraction.RequesterName;
            form2MainData.Serial = deduction.Subtraction.FirstOrDefault().SubtractionNumber.ToString();
            form2MainData.IsAddition = false;
            form2MainData.Currencies = new List<string>();
            if (deductionStoreItems == null|| deductionStoreItems.Count()==0)
            {
                throw new InvalidInvoiceException(_localizer["InvoiceHasNoItems"]);
            }

            if (deduction != null)
            { 
                return  Task.FromResult(_wordBusiness.printAdditionMultiDocument
                    (form2MainData, deductionStoreItems));
            }
            return  Task.FromResult(new MemoryStream());
        }
    }
}
