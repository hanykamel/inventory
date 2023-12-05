using Inventory.Service.Interfaces;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data.Models.StoreItemVM;
using System;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.InvoiceRequest.Handlers
{
    class PrintInvoiceHandler : IRequestHandler<PrintInvoiceCommand, MemoryStream>
    {
        private readonly IInvoiceBussiness _invoiceBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly ICommiteeItemBussiness _commiteeItemBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IWordBusiness _wordBusiness;
        private IStringLocalizer<SharedResource> _localizer;

        public PrintInvoiceHandler(IInvoiceBussiness invoiceBussiness,
            IStoreItemBussiness storeItemBussiness,
            ICommiteeItemBussiness commiteeItemBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IStringLocalizer<SharedResource> localizer,
        IWordBusiness wordBusiness)
        {
            _invoiceBussiness = invoiceBussiness;
            _wordBusiness = wordBusiness;
            _storeItemBussiness = storeItemBussiness;
            _commiteeItemBussiness=commiteeItemBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _localizer = localizer;
        }

        

        public Task<MemoryStream> Handle(PrintInvoiceCommand request, CancellationToken cancellationToken)
        {
            List<Invoice> Invoices = _invoiceBussiness.GetMultipleInvoices(request.InvoicesIds);
            var invoiceStoreItems = _invoiceBussiness.GetInvoiceStoreItem
                (Invoices.Select(x => x.Id).ToList());
            if (invoiceStoreItems==null|| invoiceStoreItems.Count==0)
            {
                throw new InvalidInvoiceException(_localizer["InvoiceHasNoItems"]);
            }
            var StoreItemsIds = new List<Guid>();

            if (request.ShowAllInvoiceItems == null)
                StoreItemsIds = invoiceStoreItems.Where(x => x.IsRefunded != true).
                    Select(x => x.StoreItemId).ToList();
            
            else if((bool)request.ShowAllInvoiceItems==true)
                StoreItemsIds = invoiceStoreItems.
                    Select(x => x.StoreItemId).ToList();

            if (StoreItemsIds == null || StoreItemsIds.Count == 0)
            {
                throw new InvalidInvoiceException(_localizer["AllItemsInInvoiceAreRefunded"]);
            }
            var storeItems =  _storeItemBussiness.GetByInvoiceId(StoreItemsIds);
            var exchangeOrderStoreItems = _exchangeOrderBussiness.GetExchangeOrderStoreItem(StoreItemsIds);

            var committeItems =_commiteeItemBussiness.GetBystoreItem(storeItems);
            var result = storeItems.GroupBy(s => new {
                s.BaseItemId,
                BaseItemDesc = s.BaseItem.Description,
                BaseItemName = s.BaseItem.Name,
                UnitName = s.Unit.Name,
                PageNumber = s.BookPageNumber
            })
                .Select(o => new InvoiceStoreItemVM
            {
                Count = o.Count(),
                InvoiceId = invoiceStoreItems.Where(x => x.StoreItemId == o.FirstOrDefault().Id).FirstOrDefault().InvoiceId,
                BaseItemDesc = o.Key.BaseItemDesc,
                BaseItemName = o.Key.BaseItemName,
                UnitName = o.Key.UnitName != null ? o.Key.UnitName : "",
                ExchangeOrderNotes = ConcatenateNotes(exchangeOrderStoreItems, o),//exchangeOrderStoreItems.Where(x => x.StoreItemId == o.FirstOrDefault().Id)?.FirstOrDefault().Notes,
                ContractNumber = committeItems.Where(x => x.BaseItemId == o.Key.BaseItemId).FirstOrDefault()!=null ?
                 committeItems.Where(x => x.BaseItemId == o.Key.BaseItemId).FirstOrDefault().ExaminationCommitte.ContractNumber : "",
                 PageNumber=o.Key.PageNumber
            }).ToList();



            //var result = storeItems.Join(committeItems, s => s.BaseItemId,
            //    c => c.BaseItemId, (storeItem, committeItem) =>
            //    new InvoiceStoreItemVM
            //{
            //        InvoiceId= invoiceStoreItems.Where(x => x.StoreItemId == storeItem.Id).FirstOrDefault().InvoiceId,
            //        StoreItemCode = storeItem.Code,
            //        BaseItemName= storeItem.BaseItem.Name,
            //        UnitName = committeItem.Unit.Name,
            //        ExchangeOrderNotes = exchangeOrderStoreItems.Where(x=>x.StoreItemId==storeItem.Id)?.FirstOrDefault().Notes ,
            //        ContractNumber = committeItem.ExaminationCommitte.ContractNumber
            //    }).ToList();

        
            if (Invoices != null)
            { 
                return  Task.FromResult(_wordBusiness.printInvoiceDocument
                    (Invoices, result,invoiceStoreItems, request.ShowAllInvoiceItems==null?false:(bool)request.ShowAllInvoiceItems));
            }
            return  Task.FromResult(new MemoryStream());
        }

        private string ConcatenateNotes(List<ExchangeOrderStoreItem> exchangeOrderStoreItems, IGrouping<object, StoreItem> o)
        {
            string Notes = null;
            var items = exchangeOrderStoreItems.Where(x =>o.Any(i=>i.Id== x.StoreItemId));
            Notes = string.Join(" - ", items.Select(i=>i.Notes));
            if (Notes.Trim()==",")
            {
                Notes = "";
            }
            return Notes;
        }
    }
}
