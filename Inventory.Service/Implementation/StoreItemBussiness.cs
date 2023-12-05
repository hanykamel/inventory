using AutoMapper;
using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.BaseItem;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class StoreItemBussiness : IStoreItemBussiness
    {
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        readonly private IRepository<TransformationStoreItem, Guid> _transformationStoreItemRepository;
        readonly private IRepository<RobbingOrderStoreItem, Guid> _robbingOrderStoreItemRepository;
        readonly private IRepository<InvoiceStoreItem, Guid> _invoiceStoreItemRepository;
        readonly private IRepository<Addition, Guid> _additionRepository;
        readonly private IRepository<CommitteeItem, Guid> _committeItemRepository;
        readonly private ICodeGenerator _codeGenerator;
        readonly private ITenantProvider _tenantProvider;
        readonly private IMapper _mapper;
        readonly private IStringLocalizer<SharedResource> _localizer;
        readonly private ILogger<StoreItemBussiness> _logger;
        //readonly private IRepository<RefundOrderStoreItem, Guid> _refundOrderStoreItemRepository;
        //private readonly IRepository<ExchangeOrderStoreItem, Guid> _exchangeOrderStoreItemRepository;

        public StoreItemBussiness(
            IRepository<StoreItem, Guid> storeItemRepository,
            IRepository<InvoiceStoreItem, Guid> invoiceStoreItemRepository,
            IRepository<Addition, Guid> additionRepository,
            IRepository<CommitteeItem, Guid> committeItemRepository,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            IMapper mapper,
            ILogger<StoreItemBussiness> logger,
            IStringLocalizer<SharedResource> localizer,
            IRepository<TransformationStoreItem, Guid> transformationStoreItemRepository,
            IRepository<RobbingOrderStoreItem, Guid> robbingOrderStoreItemRepository
            //IRepository<RefundOrderStoreItem, Guid> refundOrderStoreItemRepository,
            //IRepository<ExchangeOrderStoreItem, Guid> exchangeOrderStoreItemRepository
            )
        {
            _storeItemRepository = storeItemRepository;
            _invoiceStoreItemRepository = invoiceStoreItemRepository;
            _transformationStoreItemRepository = transformationStoreItemRepository;
            _robbingOrderStoreItemRepository = robbingOrderStoreItemRepository;
            _additionRepository = additionRepository;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _committeItemRepository = committeItemRepository;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
            //_refundOrderStoreItemRepository = refundOrderStoreItemRepository;
            //_exchangeOrderStoreItemRepository = exchangeOrderStoreItemRepository;
        }

        public StoreItemBussiness()
        {
        }

        public List<FormNo6StoreItemVM> GetFormNo6StoreItems(List<Guid> storeItems)
        {

            return _storeItemRepository.GetAll(x => storeItems.Contains(x.Id), true)
               .Include(x => x.StoreItemStatus)
               .Include(x => x.BaseItem)
               .Include(x => x.Unit)
               .Include(x => x.Book)
               .GroupBy(x => new
               {
                   BaseItemId = x.BaseItemId,
                   StoreItemStatus = x.StoreItemStatus.Name,
                   Description = x.BaseItem.Description,
                   BaseItemName = x.BaseItem.Name,
                   Unit = x.Unit.Name,
                   Price = x.Price,
                   Note = x.Note,
                   Currency = x.Currency.Name,
                   BookNumber = x.Book.BookNumber,
                   BookId = x.BookId,
                   UnitId = x.UnitId,
                   PageNumber = x.BookPageNumber,
                   CurrencyId = x.CurrencyId
               })
               .Select(a => new FormNo6StoreItemVM
               {
                   BookNumber = a.Key.BookNumber,
                   PageNumber = a.Key.PageNumber,
                   ItemStatus = a.Key.StoreItemStatus,
                   BaseItemName = a.Key.BaseItemName,
                   BaseItemId = a.Key.BaseItemId,
                   UnitName = a.Key.Unit,
                   Quantity = a.Count(),
                   Price = a.Key.Price,
                   Currency = a.Key.Currency,
                   Description = a.Key.Description,
                   Notes = a.Key.Note,
                   BookId = a.Key.BookId,
                   UnitId = a.Key.UnitId,
                   CurrencyId = a.Key.CurrencyId
               }).ToList();
        }
        //get store item grouped by baseitemId
        public List<BaseItemStoreItemVM> GetFormStoreItems(List<Guid> storeItems)
        {

            return _storeItemRepository.GetAll(x => storeItems.Contains(x.Id), true)
               .Include(x => x.StoreItemStatus)
               .Include(x => x.BaseItem)
               .Include(x => x.Unit)
               .Include(x => x.Currency)
               .Include(x => x.Addition).ThenInclude(x => x.ExaminationCommitte)
               .GroupBy(x => new
               {
                   contractnum = x.Addition.ExaminationCommitte.ContractNumber,
                   pagenum=x.BookPageNumber,
                   BaseItemId = x.BaseItemId,
                   StoreItemStatus = x.StoreItemStatus.Name,
                   Description = x.BaseItem.Description,
                   BaseItemName = x.BaseItem.Name,
                   Unit = x.Unit.Name,
                   Price = x.Price,
                   Note = x.Note,
                   Currency = x.Currency.Name
               })
               .Select(a => new BaseItemStoreItemVM
               {
                   PageNumber=a.Key.pagenum,
                   ContractNumber=a.Key.contractnum,
                   ItemStatus = a.Key.StoreItemStatus,
                   BaseItemDesc = a.Key.Description,
                   BaseItemName = a.Key.BaseItemName,
                   BaseItemId = a.Key.BaseItemId,
                   UnitName = a.Key.Unit,
                   Quantity = a.Count(),
                   UnitPrice = a.Key.Price,
                   FullPrice = a.Key.Price * a.Count(),
                   Currency = a.Key.Currency,
                   Notes = a.Key.Note,
               }).ToList();
        }
        //get store item grouped by baseitemId
        public List<StoreItem> GetByInvoiceId(List<Guid> storeItemIds)
        {

            var items = _storeItemRepository.GetAll(true)
                .Include(x => x.BaseItem)
                .Include(x => x.InvoiceStoreItem)
                .Include(x => x.Addition)
                .Include(x => x.Unit)
                .Where(x => storeItemIds.Contains(x.Id));

            var itemsList = items.ToList();
            return itemsList;
        }

        public List<ExchangeOrderStoreItemVM> GetByExchangeOrderId(Guid exchangeOrderId)
        {
            return _storeItemRepository.GetAll()
                .Include(x => x.BaseItem)
                .Include(x => x.ExchangeOrderStoreItem)
                .Where(x => x.ExchangeOrderStoreItem.Any(e => e.ExchangeOrderId == exchangeOrderId)).Select(
                s => new ExchangeOrderStoreItemVM
                {
                    StoreItemCode = s.Code,
                    BaseItemName = s.BaseItem.Name,
                    Notes = s.ExchangeOrderStoreItem.FirstOrDefault().Notes,
                    StoreItemId = s.Id,
                    ItemStatus = s.CurrentItemStatusId
                }
                ).ToList();
        }

        public int GetMax(int budgetId, long baseItemId)
        {
            var date = DateTime.Now;
            return _storeItemRepository.GetMax(x => x.BaseItemId == baseItemId
            && x.Addition.BudgetId == budgetId
            && x.StoreId == _tenantProvider.GetTenant(), x => x.Serial, false) + 1;

        }
        public bool AddNewStoreItem(StoreItem _storeItem)
        {
            _storeItemRepository.Add(_storeItem);
            return true;
        }

        public IQueryable<StoreItem> GetAllStoreItems(bool ignoreTentant = false)
        {
            var StoreItemList = _storeItemRepository.GetAll(ignoreTentant)
                .Include(s => s.Addition).ThenInclude(s => s.ExaminationCommitte);
            return StoreItemList;
        }

        public IQueryable<StoreItem> GetTransformationAvailableStoreItems
             (long baseItemId, int budgetId, int statusId, int count, string contractNum, int pageNum)
        {

            var result = GetAllStoreItems().Where
                   (x => x.BaseItemId == baseItemId
                    && x.Addition.BudgetId == budgetId &&
                     x.UnderDelete != true && x.UnderExecution != true &&
                          x.CurrentItemStatusId == (int)ItemStatusEnum.Available &&
                          x.StoreItemStatusId == statusId &&
                          x.BookPageNumber == pageNum &&
                           x.StoreItemStatusId != (int)StoreItemStatusEnum.Robbing &&
                         x.StoreItemStatusId != (int)StoreItemStatusEnum.Tainted
                         && (string.IsNullOrEmpty(contractNum) ||
                         x.Addition.ExaminationCommitte.ContractNumber == contractNum)
                        )

        .Take(count)
        //.GroupBy(x => new
        //{
        //    Id = x.Id,

        //})
        ;
        
            return result;
        }

        public IEnumerable<StoreItem> GetBaseItemsLatestBooksAndPages(List<long> baseItemsIds)
        {
            return _storeItemRepository.GetAll()
                .Where(s => baseItemsIds.Contains(s.BaseItemId))
                .GroupBy(s => s.BaseItemId)
                .Select(g => g.OrderByDescending(c => c.CreationDate).FirstOrDefault()).ToList();
        }

        public IQueryable<StoreItem> GetAvailableStoreItems()
        {
            var StoreItemList = _storeItemRepository.GetAll(x =>
             x.UnderDelete != true &&
             x.UnderExecution !=true&&
            x.CurrentItemStatusId == (int)ItemStatusEnum.Available &&
            x.StoreItemStatusId != (int)StoreItemStatusEnum.Robbing &&
            x.StoreItemStatusId != (int)StoreItemStatusEnum.Tainted);
            return StoreItemList;
        }




        //private IQueryable<StoreItem> GetStagnantLogic(DateTime stagnantDate)
        //{
        //    IQueryable<StoreItem> query = _storeItemRepository
        //        .GetAll(x =>
        //                    (x.IsStagnant != true) &&
        //                    (x.ExchangeOrderStoreItem == null || x.ExchangeOrderStoreItem.Any(y => y.CreationDate > stagnantDate) == false) &&
        //                    (x.RefundOrderStoreItem == null || x.RefundOrderStoreItem.Any(y => y.CreationDate > stagnantDate) == false) &&
        //                    (x.Addition.Date <= stagnantDate));
        //    query.Include(s => s.BaseItem);
        //    query.Include(s => s.Unit);
        //    query.Include(s => s.StoreItemStatus);

        //    return query;
        //}
        public IQueryable<StagnantBaseItemVM> GetStagnantStoreItems(DateTime stagnantDate)
        {

            var stagnantStoreItems =
              _storeItemRepository
              .GetAll()
              .Where(c => c.CurrentItemStatusId == (int)ItemStatusEnum.Available
              && c.UnderDelete != true && c.UnderExecution!=true)
              .Where(c => c.Addition.Date < stagnantDate)
              .Where(c => c.ExchangeOrderStoreItem == null || 
              c.ExchangeOrderStoreItem.Any(y => y.ExchangeOrder.Date >=stagnantDate) == false)
              .Where(c => c.RefundOrderStoreItem == null || 
              c.RefundOrderStoreItem.Any(y => y.RefundOrder.Date >= stagnantDate) == false)


              .Select(s => new
              {
                  BookId = s.BookId,
                  BookNumber = s.Book.BookNumber,
                  BookPageNumber = s.BookPageNumber,
                  BaseItemId = s.BaseItemId,
                  BaseItemName = s.BaseItem.Name,
                  UnitId = s.UnitId,
                  UnitName = s.Unit.Name,
                  TenantId = s.TenantId,
                  AdditionDate = s.Addition.Date
              })
              .GroupBy(s => new
              {
                  BookId = s.BookId,
                  BookNumber = s.BookNumber,
                  BookPageNumber = s.BookPageNumber,
                  BaseItemId = s.BaseItemId,
                  BaseItemName = s.BaseItemName,
                  UnitId = s.UnitId,
                  UnitName = s.UnitName,
                  TenantId = s.TenantId
              })
              .Select(g => new StagnantItemsVM()
              {
                  TenantId = g.Key.TenantId,
                  BookNumber = g.Key.BookNumber,
                  PageNumber = g.Key.BookPageNumber,
                  BaseItemId = g.Key.BaseItemId,
                  Code = g.Key.BaseItemId.ToString(),
                  BaseItemName = g.Key.BaseItemName,
                  UnitName = g.Key.UnitName,
                  UnitId = g.Key.UnitId,
                  BookId = g.Key.BookId,
                  LastAdditionDate = g.Max(c => c.AdditionDate)
              });
            var allItems = _storeItemRepository.GetAll()
                .Select(s => new AllStoreItemsForStagnantInquiriesVM()
                {
                    BaseItemId = s.BaseItemId,
                    UnitId = s.UnitId,
                    BookId = s.BookId,
                    PageNumber = s.BookPageNumber,
                    TenantId = s.TenantId,
                    //LastExchangeOrderDate = s.ExchangeOrderStoreItem != null ? s.ExchangeOrderStoreItem.Max(ex => ex.ExchangeOrder.Date) : (DateTime?)null
                    //LastExchangeOrderDate = s.ExchangeOrderStoreItem.DefaultIfEmpty().Max(ex => ex.ExchangeOrder.Date)
                    //LastExchangeOrderDate = s.ExchangeOrderStoreItem.Any()? (DateTime?)s.ExchangeOrderStoreItem.DefaultIfEmpty().Max(ex => ex.ExchangeOrder.Date):null
                    LastExchangeOrderDate = s.ExchangeOrderStoreItem.Max(ex => (DateTime?)ex.ExchangeOrder.Date),
                    LastRefundOrderDate = s.RefundOrderStoreItem.Max(ex => (DateTime?)ex.RefundOrder.Date)
                })
                .GroupBy(s => new
                {
                    BaseItemId = s.BaseItemId,
                    UnitId = s.UnitId,
                    BookId = s.BookId,
                    TenantId = s.TenantId,
                    PageNumber = s.PageNumber
                })
                .Select(g => new
                {
                    BaseItemId = g.Key.BaseItemId,
                    UnitId = g.Key.UnitId,
                    BookId = g.Key.BookId,
                    PageNumber = g.Key.PageNumber,
                    TenantId = g.Key.TenantId,
                    StoreItemsCount = g.Count(),
                    LastExchangeOrderDate = g.Max(s => s.LastExchangeOrderDate),
                    LastRefundOrderDate = g.Max(s => s.LastRefundOrderDate)
                });

            var res = from ssi in 
                          stagnantStoreItems
                      join item in 
                      allItems
                      .Where(o => (o.LastExchangeOrderDate == null || o.LastExchangeOrderDate < stagnantDate)
                      && (o.LastRefundOrderDate == null || o.LastRefundOrderDate < stagnantDate))
                      on ssi.BaseItemId equals item.BaseItemId
                      where ssi.UnitId == item.UnitId &&
                            ssi.BookId == item.BookId &&
                            ssi.UnitId == item.UnitId &&
                            ssi.PageNumber == item.PageNumber &&
                            ssi.TenantId == item.TenantId
                      select new StagnantBaseItemVM()
                      {
                          TenantId = ssi.TenantId,
                          BookNumber = ssi.BookNumber,
                          PageNumber = ssi.PageNumber,
                          BaseItemId = ssi.BaseItemId,
                          Code = ssi.BaseItemId.ToString(),
                          Name = ssi.BaseItemName,
                          Unit = ssi.UnitName,
                          UnitId = ssi.UnitId,
                          BookId = ssi.BookId,
                          LastAdditionDate = ssi.LastAdditionDate,
                          LastExchangeOrderDate = item.LastExchangeOrderDate,
                          StoreItemsCount = item.StoreItemsCount
                      };


            return res;


        }
        public IQueryable<StagnantStoreItemVM> getStagnantStoreItemsByBaseItemId()
        {
            return _storeItemRepository.GetAll()
              .Select(s => new StagnantStoreItemVM()
              {
                  BaseItemId = s.BaseItemId,
                  Code = s.Code,
                  UnitId = s.UnitId,
                  BookId = s.BookId,
                  PageNumber = s.BookPageNumber,
                  CreationDate = s.CreationDate,
                  Name = s.BaseItem.Name,
                  UnitName = s.Unit.Name,
                  Price = s.Price,
                  StoreItemStatusName = s.StoreItemStatus.Name
              });


            //return _storeItemRepository
            //    .GetAll(x =>
            //                (x.Addition.Date <= stagnantDate) &&
            //                (x.ExchangeOrderStoreItem == null || x.ExchangeOrderStoreItem.Any(y => y.ExchangeOrder.Date > stagnantDate) == false) &&
            //                (x.RefundOrderStoreItem == null || x.RefundOrderStoreItem.Any(y => y.RefundOrder.Date > stagnantDate) == false))
            //   .Select(s => new StagnantStoreItemVM()
            //   {
            //       BaseItemId = s.BaseItemId,
            //       Code = s.Code,
            //       UnitId = s.UnitId,
            //       CreationDate = s.CreationDate,
            //       Name = s.BaseItem.Name,
            //       UnitName = s.Unit.Name,
            //       Price = s.Price,
            //       StoreItemStatusName = s.StoreItemStatus.Name
            //   });

        }
        public bool UpdateStoreItem(StoreItem _storeItem)
        {
            _storeItemRepository.Update(_storeItem);
            return true;

        }
        public void UpdateStagnantStoreItemAsync(List<Guid> storeItems)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    storeItem.IsStagnant = true;
                }
            }


        }
        public void GenerateBarcode(List<StoreItem> list, int budgetId, long baseItemId)
        {
            var storItemMax = GetMax(budgetId, baseItemId);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Code = _codeGenerator.GenerateBarcode(budgetId, baseItemId, storItemMax + i);
                list[i].Serial = storItemMax + i;
            }
        }

        public List<StoreItemVM> GenerateBarcodeImages(Guid additionId)
        {
            var addition = _additionRepository.GetAll().Include(o => o.StoreItem).ThenInclude(o => o.BaseItem).FirstOrDefault(o => o.Id == additionId);
            var result = new List<StoreItemVM>();
            if (addition.StoreItem != null && addition.StoreItem.Count > 0)
            {
                foreach (var item in addition.StoreItem)
                {
                    var storeItem = _mapper.Map<StoreItemVM>(item);
                    storeItem.Barcode = _codeGenerator.GenerateBarcodeBase64Image(item.Code);
                    result.Add(storeItem);
                }
            }
            return result;
        }

        public StoreItem GetStoreItemsbyId(Guid StoreItemId)
        {
            return _storeItemRepository.Get(StoreItemId);
        }
        public List<BaseItem> GetBaseItemsByBudgetId(int budgetId)
        {
            var result = _storeItemRepository.GetAll()
                .Where(o => o.Addition.BudgetId == budgetId)
                .Include(o => o.Addition)
                .Include(o => o.BaseItem)
                .ThenInclude(o => o.ItemCategory)
                .Select(c => c.BaseItem)
                .Include(o => o.ItemCategory)
                .ToList()
                .Distinct(new BaseItemComparer())
                .ToList();
            return result;
        }
        public void UpdateStoreItemStatus(List<Guid> storeItems,
            ItemStatusEnum validOldStatus, ItemStatusEnum newItemStatus)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    //check before edit that the current status is still valid
                    if (storeItem.CurrentItemStatusId != (int)validOldStatus)
                        throw new InvalidStoreItemStatusException(_localizer["InvalidStoreItemStatusException"]);
                    storeItem.CurrentItemStatusId = (int)newItemStatus;
                }
            }
        }


        public async Task<bool> ReenableStoreItem(List<Guid> storeItems)
        {
            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id) && x.CurrentItemStatusId == (int)ItemStatusEnum.Reserved);
            foreach (var item in _storeItems)
            {
                item.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                _storeItemRepository.PartialUpdate
                           (item, x => x.CurrentItemStatusId);
            }
            if (await _storeItemRepository.SaveChanges() > 0)
                return true;
            else
                throw new NotSavedException();

        }



        public void DeActivateStoreItem(List<Guid> storeItems)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    storeItem.IsActive = false;
                }
            }
        }
        public void DeActivateTransformationStoreItem(Guid transformationId)
        {
            var _transformationStoreItems = _transformationStoreItemRepository.GetAll(x => x.TransformationId == transformationId, true)
                .Include(x => x.StoreItem);

            if (_transformationStoreItems != null)
            {
                foreach (var _transformationStoreItem in _transformationStoreItems)
                {
                    _transformationStoreItem.StoreItem.IsActive = false;
                }
            }
        }
        public void DeActivateRobbingStoreItem(Guid robbingId)
        {
            var _robbingStoreItems = _robbingOrderStoreItemRepository.GetAll(x => x.RobbingOrderId == robbingId, true)
                .Include(x => x.StoreItem);

            if (_robbingStoreItems != null)
            {
                foreach (var _robbingStoreItem in _robbingStoreItems)
                {
                    _robbingStoreItem.StoreItem.IsActive = false;
                }
            }
        }
        public void MakeStoreItemUnderDelete(List<Guid> storeItems)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    storeItem.UnderDelete = true;
                }
            }
        }


        public void MakeStoreItemUnderExecution(List<Guid> storeItems)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    storeItem.UnderExecution = true;
                }
            }
        }
        public void CancelStoreItemUnderDelete(List<Guid> storeItems)
        {

            var _storeItems = _storeItemRepository.GetAll().Where(x => storeItems.Contains(x.Id));
            if (_storeItems != null)
            {
                foreach (StoreItem storeItem in _storeItems)
                {
                    storeItem.UnderDelete = false;
                }
            }
        }
        public List<StoreItemExchangesVM> SearchStoreItemExchanges(Guid id)
        {

            var storeItem = _storeItemRepository.GetAll(o => o.Id == id)
                .Include(o => o.Addition)
                .Include(O => O.RefundOrderStoreItem)
                .ThenInclude(o => o.RefundOrder)
                .ThenInclude(o => o.RefundOrderEmployee)
                .Include(o => o.ExchangeOrderStoreItem)
                .ThenInclude(o => o.ExchangeOrder)
                .ThenInclude(o => o.ForEmployee)
                .FirstOrDefault();
            if (storeItem == null)
                throw new NullInputsException(_localizer["InvalidModelStateException"]);
            List<StoreItemExchangesVM> storeItemExchanges = new List<StoreItemExchangesVM>();

            //if (storeItem.Addition != null)
            //{
            //    storeItemExchanges.Add(new StoreItemExchangesVM()
            //    {
            //        Date = storeItem.Addition.CreationDate,
            //        Operation = OperationEnum.Addition,
            //        OperatedTo = storeItem.Addition.CreatedBy,
            //        Notes = _committeItemRepository.GetAll(o => o.ExaminationCommitteId == storeItem.Addition.ExaminationCommitteId && o.BaseItemId == storeItem.BaseItemId).FirstOrDefault()?.AdditionNotes
            //    });
            //}
            if (storeItem.RefundOrderStoreItem != null && storeItem.RefundOrderStoreItem.Count > 0)
                storeItemExchanges.AddRange(storeItem
                    .RefundOrderStoreItem
                    .OrderBy(o => o.CreationDate)
                    .Select(o => new StoreItemExchangesVM()
                    {
                        Notes = o.Notes,
                        Operation = OperationEnum.RefundOrder,
                        OperatedTo = o.RefundOrder.RefundOrderEmployee.Name,
                        Date = o.CreationDate
                    })
                    .ToList());

            if (storeItem.ExchangeOrderStoreItem != null && storeItem.ExchangeOrderStoreItem.Count > 0)
                storeItemExchanges.AddRange(storeItem
                    .ExchangeOrderStoreItem
                    .OrderBy(o => o.CreationDate)
                    .Select(o => new StoreItemExchangesVM()
                    {
                        Notes = o.Notes,
                        Operation = OperationEnum.ExchangeOrder,
                        OperatedTo = o.ExchangeOrder.ForEmployee.Name,
                        Date = o.CreationDate
                    })
                    .ToList());

            storeItemExchanges = storeItemExchanges.OrderByDescending(o => o.Date).ToList();
            return storeItemExchanges;
        }


        public List<StoreItemVM> GetAllStoreItems(List<Guid> storeItemIds, bool ignoreTentant = false)
        {
            return _storeItemRepository.GetAll(ignoreTentant).Where(o => storeItemIds.Contains(o.Id))
                .Include(o => o.BaseItem)
                .Include(o => o.StoreItemStatus)
                .Include(o => o.Currency).
                Include(o=>o.Unit)
                .Select(o => _mapper.Map<StoreItemVM>(o)).ToList();
        }

        public IQueryable<StoreItem> GetRobbingStoreItems()
        {
            //            x.StoreItemStatusId == (int)StoreItemStatusEnum.Robbing &&
            //x.StoreItemStatusId != (int)StoreItemStatusEnum.Tainted &&
            var StoreItemList = _storeItemRepository.GetAll(x =>
                      x.CurrentItemStatusId == (int)ItemStatusEnum.Available && x.UnderDelete != true && x.UnderExecution != true);
            return StoreItemList;
        }

        public List<StoreItem> GetInquiryStoreItems(InquiryStoreItemsRequest inquiryRequest, out int count)
        {
            count = 0;
            var query = _storeItemRepository.GetAll();

            query = query
                .Where(o => inquiryRequest.baseItemId == null || o.BaseItemId == inquiryRequest.baseItemId)
                .Where(o => inquiryRequest.categoryId == null || o.BaseItem.ItemCategoryId == inquiryRequest.categoryId)
                .Where(o => inquiryRequest.contractNumber == null || o.Addition.ExaminationCommitte.ContractNumber.Contains(inquiryRequest.contractNumber))
                .Where(o => inquiryRequest.storeId == null || o.TenantId == inquiryRequest.storeId)
                .Where(o => inquiryRequest.StoreItemAvailibilityStatusId == null || o.CurrentItemStatusId == inquiryRequest.StoreItemAvailibilityStatusId)
                .Where(o => inquiryRequest.StoreItemStatus == null || o.StoreItemStatusId == inquiryRequest.StoreItemStatus)
                .Where(o => inquiryRequest.budgetId == null || o.Addition.BudgetId == inquiryRequest.budgetId)
                .Where(o => inquiryRequest.consumed == null || o.BaseItem.Consumed == inquiryRequest.consumed)
                .Where(o => inquiryRequest.BookPageNumberTo <= 0 || o.BookPageNumber <= inquiryRequest.BookPageNumberTo)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || inquiryRequest.BookPageNumberTo > 0 || o.BookPageNumber == inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || o.BookPageNumber >= inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookNumberTo <= 0 || o.Book.BookNumber <= inquiryRequest.BookNumberTo)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || inquiryRequest.BookNumberTo > 0 || o.Book.BookNumber == inquiryRequest.BookNumberFrom)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || o.Book.BookNumber >= inquiryRequest.BookNumberFrom)
                .Include(a => a.BaseItem)
                    .Include(x => x.Unit)
                    .Include(a => a.StoreItemStatus)
                    .Include(a => a.CurrentItemStatus)
                    .Include(a => a.Addition)
                    .ThenInclude(a => a.ExaminationCommitte);
            count = query.ToList().Count();
            if (count > 0)
            {
                query = query.Skip((int)inquiryRequest.skip)
                    .Take((int)inquiryRequest.take);
                    //.OrderBy(o => o.BaseItem.Name)
                    //.OrderBy(o => o.Serial);
            }
            return query.ToList();
        }

        public InquiryBaseItems GetInquiryBaseItems(InquiryBaseItemsCommand inquiryRequest)
        {
            var query = _storeItemRepository.GetAll();
            query = query
                .Where(o => inquiryRequest.baseItemId == null || o.BaseItemId == inquiryRequest.baseItemId)
                .Where(o => inquiryRequest.categoryId == null || o.BaseItem.ItemCategoryId == inquiryRequest.categoryId)
                .Where(o => inquiryRequest.budgetId == null || o.Addition.BudgetId == inquiryRequest.budgetId)
                .Where(o => inquiryRequest.consumed == null || o.BaseItem.Consumed == inquiryRequest.consumed)
                .Where(o => inquiryRequest.BookPageNumberTo <= 0 || o.BookPageNumber <= inquiryRequest.BookPageNumberTo)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || inquiryRequest.BookPageNumberTo > 0 || o.BookPageNumber == inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || o.BookPageNumber >= inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookNumberTo <= 0 || o.Book.BookNumber <= inquiryRequest.BookNumberTo)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || inquiryRequest.BookNumberTo > 0 || o.Book.BookNumber == inquiryRequest.BookNumberFrom)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || o.Book.BookNumber >= inquiryRequest.BookNumberFrom);

            //if (inquiryRequest.baseItemId != null)
            //    query.Where(s => s.BaseItemId == inquiryRequest.baseItemId);
            //if (inquiryRequest.categoryId != null)
            //    query.Where(s => s.BaseItem.ItemCategoryId == inquiryRequest.categoryId);
            //if (inquiryRequest.budgetId != null)
            //    query.Where(s => s.Addition.BudgetId == inquiryRequest.budgetId);
            //if (inquiryRequest.consumed != null)
            //    query.Where(s => s.BaseItem.Consumed == inquiryRequest.consumed);
            //if (inquiryRequest.BookNumberFrom != null)
            //    query.Where(s => s.Book.BookNumber >= inquiryRequest.BookNumberFrom);
            //if (inquiryRequest.BookNumberTo != null)
            //    query.Where(s => s.Book.BookNumber <= inquiryRequest.BookNumberTo);
            InquiryBaseItems baseItemsModel = new InquiryBaseItems();
            if (query.Any())
            {
                //var baseItemsQuery = query
                //    .Include(a => a.BaseItem)
                //    .Include(x => x.Unit)
                //    .GroupBy(g => new
                //    {
                //        BaseItemId = g.BaseItemId,
                //        BaseItemName = g.BaseItem.Name,
                //        UnitId = g.UnitId,
                //        UnitName = g.Unit.Name,
                //        BookId = g.BookId,
                //        BookPageNumber = g.BookPageNumber,
                //        Description = g.BaseItem.Description,
                //        Consumed = g.BaseItem.Consumed
                //    });
                baseItemsModel.Count = query
                    .Include(a => a.BaseItem)
                    .Include(x => x.Unit)
                    .GroupBy(g => new
                    {
                        BaseItemId = g.BaseItemId,
                        BaseItemName = g.BaseItem.Name,
                        UnitId = g.UnitId,
                        UnitName = g.Unit.Name,
                        BookId = g.BookId,
                        BookPageNumber = g.BookPageNumber,
                        Description = g.BaseItem.Description,
                        Consumed = g.BaseItem.Consumed
                    }).Count();
                baseItemsModel.BaseItems = query

                    .Include(a => a.BaseItem)
                    .Include(x => x.Unit)
                    .GroupBy(g => new
                    {
                        BaseItemId = g.BaseItemId,
                        BaseItemName = g.BaseItem.Name,
                        UnitId = g.UnitId,
                        UnitName = g.Unit.Name,
                        BookId = g.BookId,
                        BookPageNumber = g.BookPageNumber,
                        Description = g.BaseItem.Description,
                        Consumed = g.BaseItem.Consumed
                    })

                    .Select(g => new InquiryBaseItem()
                    {
                        BaseItemId = g.Key.BaseItemId,
                        BaseItemName = g.Key.BaseItemName,
                        UnitId = g.Key.UnitId,
                        UnitName = g.Key.UnitName,
                        BookId = g.Key.BookId,
                        BookPageNumber = g.Key.BookPageNumber,
                        Description = g.Key.Description,
                        Consumed = g.Key.Consumed ? _localizer["Consumed"] : _localizer["NotConsumed"],
                        IsConsumed = g.Key.Consumed,
                        StoreItemsCount = g.Count()
                    })
                    .OrderBy(o => o.BaseItemId)
                    .Skip((int)inquiryRequest.skip).Take((int)inquiryRequest.take).ToList();
            }
            else
                throw new NoDataException(_localizer["NoDataExist"]);
            return baseItemsModel;
        }

        public async Task<bool> EditStoreItemsBooksItems(EditStoreItemsBookCommand inquiryRequest)
        {
            var query = _storeItemRepository.GetAll();
            query = query
                .Where(o => inquiryRequest.BaseItems.Select(s => s.BaseItemId).Contains(o.BaseItemId))
                .Where(o => inquiryRequest.baseItemId == null || o.BaseItemId == inquiryRequest.baseItemId)
                .Where(o => inquiryRequest.categoryId == null || o.BaseItem.ItemCategoryId == inquiryRequest.categoryId)
                .Where(o => inquiryRequest.budgetId == null || o.Addition.BudgetId == inquiryRequest.budgetId)
                .Where(o => inquiryRequest.consumed == null || o.BaseItem.Consumed == inquiryRequest.consumed)
                .Where(o => inquiryRequest.BookPageNumberTo <= 0 || o.BookPageNumber <= inquiryRequest.BookPageNumberTo)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || inquiryRequest.BookPageNumberTo > 0 || o.BookPageNumber == inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || o.BookPageNumber >= inquiryRequest.BookPageNumberFrom)
                .Where(o => inquiryRequest.BookNumberTo <= 0 || o.Book.BookNumber <= inquiryRequest.BookNumberTo)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || inquiryRequest.BookNumberTo > 0 || o.Book.BookNumber == inquiryRequest.BookNumberFrom)
                .Where(o => inquiryRequest.BookNumberFrom <= 0 || o.Book.BookNumber >= inquiryRequest.BookNumberFrom);

            foreach (var item in query)
            {
                var editItem = inquiryRequest.BaseItems.FirstOrDefault(s => s.BaseItemId == item.BaseItemId &&
                                                                            s.UnitId == item.UnitId &&
                                                                            s.OldBookId == item.BookId &&
                                                                            s.OldBookPageNumber == item.BookPageNumber);
                if (editItem != null)
                {
                    item.BookId = editItem.NewBookId;
                    item.BookPageNumber = editItem.NewBookPageNumber;
                    _storeItemRepository.PartialUpdate
                               (item, s => s.BookId, s => s.BookPageNumber);
                }

            }
            if (await _storeItemRepository.SaveChanges() > 0)
                return true;
            else
                throw new NotSavedException(_localizer["NotSavedException"]);
        }

        public IQueryable<BaseItemBudgetVM> GetActiveBaseItemsBudget(int BudgetID,int CurrencyId, int? Status, int? CategoryId,string ContractNum,int? PageNum)
        {
            var result = _storeItemRepository.GetAll().Include(s => s.Addition).ThenInclude(s => s.ExaminationCommitte)
                .Include(s => s.BaseItem).ThenInclude(b => b.DefaultUnit).Include(s => s.StoreItemStatus)
                .Where(s => s.Addition.BudgetId == BudgetID &&
                s.UnderDelete != true && s.UnderExecution != true && s.CurrentItemStatusId == Status &&
                   s.StoreItemStatusId != (int)StoreItemStatusEnum.Robbing &&
               s.StoreItemStatusId != (int)StoreItemStatusEnum.Tainted &&
              (ContractNum == "null" || string.IsNullOrEmpty(ContractNum) || s.Addition.ExaminationCommitte.ContractNumber == ContractNum) &&
               (PageNum == null|| PageNum==0 || s.BookPageNumber == PageNum) &&
                (CategoryId == null || s.BaseItemId == CategoryId) && s.CurrencyId == CurrencyId);
            IQueryable<BaseItemBudgetVM> resultgroup = null;
            if (BudgetID!=(int) BudgetNamesEnum.Staff)
            {
                resultgroup = result.GroupBy(x => new {
                    Id = x.BaseItemId,
                    name = x.BaseItem.Name,
                    disc = x.BaseItem.Description,
                    status = x.StoreItemStatus.Name,
                    statusid = x.StoreItemStatusId,
                    unit = x.BaseItem.DefaultUnit.Name,
                    ContractKey = "",
                    pageNum = x.BookPageNumber
                }).Select(a => new BaseItemBudgetVM()
                {
                    BaseItemID = a.Key.Id,
                    BaseItemName = a.Key.name,
                    BaseItemdisc = a.Key.disc,
                    CountStoreItem = a.Count(),
                    statusItem = a.Key.status,
                    statusItemId = a.Key.statusid,
                    UnitName = a.Key.unit,
                    ContractNum = a.Key.ContractKey,
                    PageNum = a.Key.pageNum
                });
            }
            else
            {
                resultgroup = result. GroupBy(x => new {
                      Id = x.BaseItemId,
                      name = x.BaseItem.Name,
                      disc = x.BaseItem.Description,
                      status = x.StoreItemStatus.Name,
                      statusid = x.StoreItemStatusId,
                      unit = x.BaseItem.DefaultUnit.Name,
                    ExaminationCommitte = x.Addition.ExaminationCommitte ,
                      pageNum = x.BookPageNumber
                  })
            .Select(a => new BaseItemBudgetVM()
            {
                BaseItemID = a.Key.Id,
                BaseItemName = a.Key.name,
                BaseItemdisc = a.Key.disc,
                CountStoreItem = a.Count(),
                statusItem = a.Key.status,
                statusItemId = a.Key.statusid,
                UnitName = a.Key.unit,
                ContractNum = a.Key.ExaminationCommitte != null ? (a.Key.ExaminationCommitte.ContractNumber != null ? a.Key.ExaminationCommitte.ContractNumber : "") : "",
                PageNum = a.Key.pageNum
            });


                
            }



            return resultgroup;
        }



        public IQueryable<BaseItemBudgetVM> GetActiveRobbingBaseItemsBudget(int BudgetID,int CurrencyId, int? Status, int? CategoryId, long[] SelectBaseItem,string ContractNum, int? PageNum)
        {
            var result = _storeItemRepository.GetAll().Include(s => s.Addition).ThenInclude(s=>s.ExaminationCommitte)
                .Include(s => s.BaseItem).ThenInclude(b => b.DefaultUnit).Include(s => s.StoreItemStatus)
                .Where(s => s.Addition.BudgetId == BudgetID &&
                s.UnderDelete != true && s.UnderExecution != true && s.CurrentItemStatusId == Status &&
                (CategoryId == null || s.BaseItemId == CategoryId)
                && (ContractNum == "null" || string.IsNullOrEmpty(ContractNum) || s.Addition.ExaminationCommitte.ContractNumber == ContractNum) &&
               (PageNum == null || PageNum == 0 || s.BookPageNumber == PageNum)
                && (SelectBaseItem == null || !SelectBaseItem.Any() || !SelectBaseItem.Contains(s.BaseItemId)) && s.CurrencyId == CurrencyId);
               IQueryable<BaseItemBudgetVM> resultgroup = null;
            if (BudgetID != (int)BudgetNamesEnum.Staff)
            {

                resultgroup= result.GroupBy(x => new { Id = x.BaseItemId, name = x.BaseItem.Name, disc = x.BaseItem.Description, price = x.Price, unit = x.BaseItem.DefaultUnit.Name,PageNum=x.BookPageNumber })

                    .Select(a => new BaseItemBudgetVM()
                  {
                      index = Guid.NewGuid(),
                      BaseItemID = a.Key.Id,
                      BaseItemName = a.Key.name,
                      BaseItemdisc = a.Key.disc,
                      CountStoreItem = a.Count(),
                      price = a.Key.price,
                      UnitName = a.Key.unit,
                      PageNum=a.Key.PageNum,
                      ContractNum="",
                      
                      StoreItemsBudget = a.OrderBy(s => s.Serial).Select(x => new StoreItemBudgetVM()
                      {
                          code = x.Code,
                          StoreItemdisc = x.Note,
                          StoreItemId = x.Id,
                          statusItem = x.StoreItemStatus.Name,
                          statusItemId = x.StoreItemStatusId,
                      })
                  });


            }
            else
            {
                resultgroup = result.GroupBy(x => new { Id = x.BaseItemId, name = x.BaseItem.Name, disc = x.BaseItem.Description, price = x.Price, unit = x.BaseItem.DefaultUnit.Name, PageNum = x.BookPageNumber, ExaminationCommitte = x.Addition.ExaminationCommitte, })

                   .Select(a => new BaseItemBudgetVM()
                   {
                       index = Guid.NewGuid(),
                       BaseItemID = a.Key.Id,
                       BaseItemName = a.Key.name,
                       BaseItemdisc = a.Key.disc,
                       CountStoreItem = a.Count(),
                       price = a.Key.price,
                       UnitName = a.Key.unit,
                       PageNum = a.Key.PageNum,
                       ContractNum = a.Key.ExaminationCommitte != null ? (a.Key.ExaminationCommitte.ContractNumber != null ? a.Key.ExaminationCommitte.ContractNumber : "") : "",


                       StoreItemsBudget = a.OrderBy(s => s.Serial).Select(x => new StoreItemBudgetVM()
                     {
                         code = x.Code,
                         StoreItemdisc = x.Note,
                         StoreItemId = x.Id,
                         statusItem = x.StoreItemStatus.Name,
                         statusItemId = x.StoreItemStatusId,
                     })
                   });

            }



            return resultgroup;
        }


        public IQueryable<BaseItemBudgetVM> GetExecutionOrderBaseItemsBudget(int BudgetID,int CurrancyId, int? CategoryId, long[] SelectBaseItem)
        {
            var result = _storeItemRepository.GetAll().Include(s => s.Addition)
                .Include(s => s.BaseItem).Include(s => s.StoreItemStatus)
                .Where(s => s.Addition.BudgetId == BudgetID &&
                s.UnderDelete != true && s.UnderExecution != true && s.CurrentItemStatusId == (int)ItemStatusEnum.Available &&
                (CategoryId == null || s.BaseItemId == CategoryId)
                && (SelectBaseItem == null || !SelectBaseItem.Any() || !SelectBaseItem.Contains(s.BaseItemId))&& s.CurrencyId== CurrancyId)
              .GroupBy(x => new { Id = x.BaseItemId, name = x.BaseItem.Name, disc = x.BaseItem.Description, price = x.Price })
            .Select(a => new BaseItemBudgetVM()
            {
                index = Guid.NewGuid(),
                BaseItemID = a.Key.Id,
                BaseItemName = a.Key.name,
                BaseItemdisc = a.Key.disc,
                CountStoreItem = a.Count(),
                price = a.Key.price,
                StoreItemsBudget = a.OrderBy(s => s.Serial).Select(x => new StoreItemBudgetVM()
                {
                    code = x.Code,
                    StoreItemdisc = x.Note,
                    StoreItemId = x.Id,
                    statusItem = x.StoreItemStatus.Name,
                    statusItemId = x.StoreItemStatusId,
                })
            }); ;


            return result;
        }



        





    }



    internal class BaseItemComparer : IEqualityComparer<BaseItem>
    {
        public bool Equals(BaseItem x, BaseItem y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(BaseItem obj)
        {
            return (int)obj.Id;
        }
    }

}
