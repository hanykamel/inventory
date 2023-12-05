using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventory.Data.Enums;
using Inventory.Data.Models.BaseItem;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Implementation
{
    public class InvoiceBussiness : IInvoiceBussiness
    {
        readonly private IRepository<Invoice, Guid> _invoiceRepository;
        readonly private ICodeGenerator _codeGenerator;
        readonly private ITenantProvider _tenantProvider;
        private readonly ILogger<InvoiceBussiness> _logger;
        readonly private IRepository<InvoiceStoreItem, Guid> _invoiceStoreItemRepository;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        private IStringLocalizer<SharedResource> _localizer;


        public InvoiceBussiness(IRepository<Invoice, Guid> InvoiceRepository,
            IRepository<InvoiceStoreItem, Guid> invoiceStoreItemRepository,
             IRepository<StoreItem, Guid> storeItemRepository,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            ILogger<InvoiceBussiness> logger,
            IStringLocalizer<SharedResource> localizer
            )
        {
            _invoiceRepository = InvoiceRepository;
            _invoiceStoreItemRepository = invoiceStoreItemRepository;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _logger = logger;
            _storeItemRepository = storeItemRepository;
            _localizer = localizer;
        }


        //public async Task<Guid> AddNewInvoice(Invoice invoice)
        //{
        //    invoice.Serial = GetMax();
        //    invoice.Code = GetCode(invoice.Serial);
        //    _invoiceRepository.Add(invoice);
        //    int added = await _invoiceRepository.SaveChanges();
        //    if (added > 0)
        //        return await Task.FromResult(invoice.Id);
        //    else
        //        throw new Exception("No records saved to database");
        //}


        public IQueryable<Invoice> GetAllInvoice()
        {
            var InvoiceList = _invoiceRepository.GetAll();
            return InvoiceList;
        }

        public IQueryable<Invoice> GetInvoiceDetails()
        {
            var InvoiceList = _invoiceRepository.GetAll(true);
            return InvoiceList;
        }

        public IQueryable<Invoice> PrintInvoicesList()
        {
            var InvoiceList = _invoiceRepository.GetAll()
                .Include(i => i.ExchangeOrder)
                .Include(i => i.ReceivedEmployee)
                .Include(i => i.Department)
                .Include(i => i.InvoiceStoreItem)
                .ThenInclude(i => i.StoreItem);
            return InvoiceList;
        }

        public List<Invoice> GetMultipleInvoices(List<Guid> invoiceIds)
        {
            List<Invoice> InvoiceList = _invoiceRepository.GetAll()
                .Include(i => i.ReceivedEmployee)
                .Include(r => r.Department)
                .Include(i => i.Location)
                .Include(i => i.ExchangeOrder)
                .ThenInclude(e => e.Budget)
                .Where(i => invoiceIds.Contains(i.Id)).ToList();
            return InvoiceList;
        }

        public Invoice GetInvoiceById(Guid invoiceId)
        {
            return _invoiceRepository.Get(invoiceId);
        }
        public int GetMax()
        {
            return _invoiceRepository.GetMax(null, x => x.Serial) + 1;

        }
        public string GetCode(int serial)
        {

            return _codeGenerator.Generate(serial);
        }

        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }

        public bool AddAllInvoice(List<Invoice> AllInvoice)
        {
            var max = GetMax();
            foreach (var item in AllInvoice)
            {
                item.Serial = max;
                item.Code = GetCode(item.Serial);
                _invoiceRepository.Add(item);
                max++;
            }
            return true;
        }

        public async Task<List<Guid>> SaveAllInvoice(List<Invoice> AllInvoice)
        {
            List<Guid> AllinvoiceId = new List<Guid>();
            int added = await _invoiceRepository.SaveChanges();
            if (added > 0)
            {
                foreach (var item in AllInvoice)
                {
                    AllinvoiceId.Add(item.Id);
                }
                return await Task.FromResult(AllinvoiceId);
            }
            else
                throw new Exception("No records saved to database");
        }
        public List<InvoiceStoreItem> GetInvoiceStoreItem(List<Guid> invoiceIds)
        {
            return _invoiceStoreItemRepository.GetAll(true)
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.Store)
                .ThenInclude(x => x.TechnicalDepartment)
                .Where(x => invoiceIds.Contains(x.InvoiceId))
                .ToList();
        }



        public async Task<bool> EditeInvoice(List<InvoiceStoreItem> InvoiceStoreItem)
        {

            foreach (var item in InvoiceStoreItem)
            {
                _invoiceStoreItemRepository.PartialUpdate(new InvoiceStoreItem()
                {
                    Id = item.Id,
                    IsRefunded = true,
                    RefundDate = DateTime.Now
                }, x => x.IsRefunded, x => x.RefundDate);

                _storeItemRepository.PartialUpdate(new StoreItem()
                { Id = item.StoreItemId, CurrentItemStatusId = (int)ItemStatusEnum.Available, StoreItemStatusId = item.StoreItem.StoreItemStatusId },
                x => x.CurrentItemStatusId, x => x.StoreItemStatusId);
            }
            return true;
         

        }

           

        public bool EditInvoiceStoreItemUnderRefund(InvoiceStoreItem invoiceStoreItem)
        {
            _invoiceStoreItemRepository.PartialUpdate(new InvoiceStoreItem()
            {
                Id = invoiceStoreItem.Id,
                UnderRefunded = true,
            }, x => x.UnderRefunded);
            return true;
        }


        public IQueryable<BaseItemVM> GetBaseItemsFromInvoice(int empId)
        {
            IQueryable<InvoiceVM> invoiceStoreItemsIds = _invoiceRepository.GetAll(i => i.ReceivedEmployeeId == empId)
                .Include(i => i.InvoiceStoreItem)
                .Select(s => new InvoiceVM()
                {
                    InvoiceStoreItems = s.InvoiceStoreItem.Select(a => a.StoreItemId).ToList()
                }).Where(a => a.InvoiceStoreItems.Count > 0);
            List<Guid> allStoreItems = new List<Guid>();
            foreach (var item in invoiceStoreItemsIds)
            {
                allStoreItems.AddRange(item.InvoiceStoreItems);
            }
            var x = _storeItemRepository.GetAll(s => allStoreItems.Contains(s.Id))
                .Include(si => si.BaseItem)
                .GroupBy(a => new { Id = a.BaseItemId, Name = a.BaseItem.Name })
                .Select(s => new BaseItemVM()
                {
                    Id = s.Key.Id,
                    Name = s.Key.Name
                });
            return x;
        }

        public async Task<bool> Savechange()
        {
            int added = await _invoiceRepository.SaveChanges();
            if (added > 0)
            {
   
                return true;
            }
            else
                throw new NoChangesInEdit(_localizer["NoChangesInEdit"]);
        }
        public bool checkInvoiceStoreItemRefunded(List<Guid> storeItems)
        {
            var notRefundedStoreItems = _invoiceStoreItemRepository.GetAll(x => storeItems.Contains(x.StoreItemId)
              && x.IsRefunded != true);

            if (notRefundedStoreItems != null && notRefundedStoreItems.Count() > 0)
                return false;
            else
                return true;

        }
    }
}
