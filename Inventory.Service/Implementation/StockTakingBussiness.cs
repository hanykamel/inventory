using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Shared;
using Inventory.Data.Models.StockTakingVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class StockTakingBussiness : IStockTakingBussiness
    {

        private readonly IRepository<StockTaking, Guid> _stockTakingRepository;
        private readonly IRepository<StockTakingStoreItem, Guid> _stockTakingStoreItemRepository;
        private readonly IRepository<StockTakingRobbedStoreItem, Guid> _stockTakingRobbedStoreItemRepository;
        private readonly IRepository<StoreItem, Guid> _storeItemRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ITenantProvider _tenantProvider;
        private readonly IRepository<StoreItemCopy, Guid> _storeItemCopyRepository;

        public StockTakingBussiness(IRepository<StockTaking, Guid> StockTakingRepository,
            IRepository<StockTakingStoreItem, Guid> StockTakingStoreItemRepository,
            IRepository<StoreItem, Guid> storeItemRepository,
            IStringLocalizer<SharedResource> Localizer,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            IRepository<StoreItemCopy, Guid> storeItemCopyRepository,
            IRepository<StockTakingRobbedStoreItem, Guid> stockTakingRobbedStoreItemRepository
            )
        {
            _stockTakingRepository = StockTakingRepository;
            _stockTakingStoreItemRepository = StockTakingStoreItemRepository;
            _storeItemRepository = storeItemRepository;
            _localizer = Localizer;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _storeItemCopyRepository = storeItemCopyRepository;
            _stockTakingRobbedStoreItemRepository = stockTakingRobbedStoreItemRepository;
        }

        public IQueryable<StockTaking> GetAllStockTaking()
        {
            return _stockTakingRepository.GetAll();
        }


        //public IQueryable<StockTakingViewVM> GetStockTakingview()
        //{
        //    var mainInfo = _stockTakingRepository
        //        .GetAll(true)
        //        .GroupBy(x => new
        //        {
        //            Id = x.Id,
        //            Code = x.Code,
        //            Date = x.CreationDate
        //        })
        //        .Select(x => new
        //        {
        //            Id = x.Key.Id,
        //            CreatedDate = x.Key.Date,
        //            Code = x.Key.Code,

        //        });

        //    var filteration = mainInfo.Where(x => x.Id.Equals("5aae44f2-a826-420b-a2d7-6a995dadb097"));
        //    var baseItems = _StockTakingStoreItemRepository.GetAll()
        //          .Include(x => x.StoreItem).ThenInclude(x => x.BaseItem)
        //          .Include(x => x.StoreItem).ThenInclude(x => x.Book)
        //          .Include(x => x.StoreItem).ThenInclude(x => x.Unit)
        //          .Include(x => x.StoreItem).ThenInclude(x => x.StoreItemStatus)
        //        .GroupBy(x => new
        //        {
        //            Id = x.StoreItem.BaseItemId,
        //            StockTakingId = x.StockTakingId,
        //            Name = x.StoreItem.BaseItem.Name,
        //            bookNumber = x.StoreItem.Book.BookNumber,
        //            bookPageNumber = x.StoreItem.BookPageNumber,
        //            unitName = x.StoreItem.Unit.Name,
        //            status = x.StoreItem.StoreItemStatus.Name,
        //            price = x.StoreItem.Price,

        //        })
        //        .Select(x => new 
        //        {
        //            Id = x.Key.Id,
        //            StockTakingId = x.Key.StockTakingId,
        //            Name = x.Key.Name,
        //            BookNumber = x.Key.bookNumber,
        //            BookPageNumber = x.Key.bookPageNumber,
        //            UnitName = x.Key.unitName,
        //            Status = x.Key.status,
        //            Price = x.Key.price,
        //            Quantity = (int)(x.Count() * x.Key.price),
        //        });
        //   var result = from i in mainInfo
        //           join b in baseItems 
        //           on i.Id equals b.StockTakingId
        //           select new StockTakingViewVM
        //           {
        //               Id = i.Id,
        //               Code = i.Code,
        //               CreatedDate = i.CreatedDate,
        //               baseItemId = b.Id,
        //               Name = b.Name,
        //               BookNumber = b.BookNumber,
        //               BookPageNumber = b.BookPageNumber,
        //               UnitName = b.UnitName,
        //               Status = b.Status,
        //               Price = b.Price,
        //               Quantity = b.Quantity
        //           };

        //    return result;
        //}

        public IQueryable<StockTaking> GetStockTakingview()
        {
            var result =
            _stockTakingRepository
                .GetAll(x => x.IsActive == true, true)
                .Include(x => x.StockTakingRobbedStoreItem)
                .ThenInclude(r=>r.RobbedStoreItem)
                .Include(x => x.StockTakingStoreItem)
                .ThenInclude(x => x.StoreItem);
                
            return result;

            //(x.StockTakingStoreItem.FirstOrDefault().StoreItem.StoreId == _tenantProvider.GetTenant()) &&
        }

        public int GetMax()
        {
            return _stockTakingRepository.GetMax(null, x => x.Serial) + 1;
        }

        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }

        public string GetLastCode()
        {
            var lastAddedObj = _stockTakingRepository.GetAll().OrderByDescending(o => o.CreationDate).FirstOrDefault();
            if (lastAddedObj != null)
            {
                return lastAddedObj.Code;
            }
            return "";
        }



        public StockTaking Create(CreateStockTakingCommand request, List<StockTakingAttachment> attachments)
        {
            if (request.BaseItems != null && request.BaseItems.Count > 0)
            {
                var stockTaking = new StockTaking()
                {
                    Serial = GetMax(),
                    Code = GetCode(),
                    OperationId = (int)OperationEnum.StockTaking,
                    Id = Guid.NewGuid(),
                    Date = Convert.ToDateTime(request.Date)
                };
                stockTaking.StockTakingStoreItem = new List<StockTakingStoreItem>();
                var baseItemIds = request.BaseItems.Where(o => o.Isrobbing == false).Select(o => o.Id).ToList();
                var storeItems = _storeItemRepository.GetAll()
                    .Include(x => x.BaseItem)
                    .Include(x => x.Unit)
                    .Include(x => x.StoreItemStatus)
                    .Include(x => x.Book).Include(x => x.StoreItemStatus)
                      .Include(x => x.Addition).ThenInclude(x => x.ExaminationCommitte)
                    .Where(o => baseItemIds.Contains(o.BaseItemId))
                    .ToList();

                if (storeItems != null)
                {
                    foreach (var baseItemObj in request.BaseItems)
                    {
                        var newStockStoreItem =
                            storeItems
                            .Where(
                                x =>
                                x.BaseItemId == baseItemObj.Id &&
                                // x.BaseItem.Name == baseItemObj.Name &&
                                //  x.Unit.Name == baseItemObj.UnitName &&
                                x.UnitId == baseItemObj.UnitId &&
                                x.BookPageNumber == baseItemObj.BookPageNumber &&
                                x.BookId == baseItemObj.BookId &&
                                // x.StoreItemStatus.Name == baseItemObj.StoreItemStatus &&
                                x.Price == baseItemObj.Price
                                &&
                          ( string.IsNullOrEmpty(  baseItemObj.ContractNum)|| x.Addition.ExaminationCommitte ==null|| x.Addition.ExaminationCommitte.ContractNumber== baseItemObj.ContractNum)

                                ).Select(x => x.Id);
                        foreach (var item in newStockStoreItem)
                        {
                            stockTaking.StockTakingStoreItem.Add(new StockTakingStoreItem()
                            {
                                Id = Guid.NewGuid(),
                                StoreItemId = item,
                                StockTakingId = stockTaking.Id
                            });
                        }
                    }
                }
                //foreach (var item in storeItemsIds)
                //{
                //    stockTaking.StockTakingStoreItem.Add(new StockTakingStoreItem()
                //    {
                //        Id = Guid.NewGuid(),
                //        StoreItemId = item
                //    });
                //}
                if (attachments != null && attachments.Count > 0)
                    foreach (var item in attachments)
                    {
                        stockTaking.StockTakingAttachment.Add(item);
                    }
                _stockTakingRepository.Add(stockTaking);
              //  await _stockTakingRepository.SaveChanges();
                return stockTaking;
            }
            throw new NotSavedException(_localizer["InvalidException"]);
        }


        public async Task<bool>  SaveChange()
        {
            int added = await _stockTakingRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_localizer["NotSavedException"]);
        }





        public StockTaking GetById(Guid Id)
        {
            return _stockTakingRepository.GetAll()
               .Include(a => a.StockTakingStoreItem)
               .ThenInclude(s => s.StoreItem)
               .ThenInclude(s => s.Store)
                .Include(a => a.StockTakingStoreItem)
               .ThenInclude(s => s.StoreItem)
               .ThenInclude(s => s.Addition)
               .ThenInclude(a => a.Budget)
               .Where(e => e.Id == Id)
                .FirstOrDefault();
        }

        public List<StockTakingStoreItem> GetStockTakingStoreItems(Guid Id)
        {
            return _stockTakingStoreItemRepository.GetAll(true)
         .Include(s => s.StoreItem)
         .ThenInclude(s => s.Store)
         .Include(s => s.StoreItem)
         .ThenInclude(s => s.Addition)
         .ThenInclude(a => a.Budget)
         .Include(a => a.StockTaking)
         .Include(s => s.StoreItem)
         .ThenInclude(s => s.Currency)
         .Where(e => e.StockTakingId == Id).ToList();



        }


        public List<StockTakingRobbedStoreItem> GetStockTakingRobbedStoreItem(Guid Id)
        {
            return _stockTakingRobbedStoreItemRepository.GetAll(true)
         .Where(e => e.StockTakingId == Id).ToList();



        }
        //public SearchStockTakingVM SearchStoreItems(SearchStockTakingCommand request, out int count)
        //{
        //    var result = new SearchStockTakingVM();
        //    var query = _storeItemRepository.GetAll();
        //    if (request.RandomSotckTaking != null && (bool)request.RandomSotckTaking && request.RandomSotckTakingNumber > 0)
        //    {
        //        var randomBaseItemsLookups = query.Include(o => o.BaseItem)
        //            .Include(o => o.Unit)
        //            .Include(o => o.StoreItemStatus)
        //            .Include(o => o.Book)
        //            .GroupBy(o => o.BaseItem)
        //            .Select(o => new StockTakingBaseItemVM
        //            {
        //                BookNumber = o.First().Book.BookNumber,
        //                BookPageNumber = o.First().BookPageNumber,
        //                Status = o.First().StoreItemStatus != null ? o.First().StoreItemStatus.Name : "",
        //                Id = o.Key.Id,
        //                Name = o.Key.Name,
        //                UnitName = o.First().Unit != null ? o.First().Unit.Name : "",
        //                Quantity = o.Count(),
        //                Price = o.First().Price,
        //            })
        //            .Take((int)request.RandomSotckTakingNumber)
        //            .ToList();
        //        result.BaseItems = randomBaseItemsLookups;
        //        count = result.BaseItems.Count;
        //        var randomBaseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
        //        result.TotalPrice = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Select(o => o.Price).ToList().Sum();
        //        result.TotalUnitsCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Count();
        //        result.TotalConsumedCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
        //        result.TotalConsumedCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();
        //        return result;
        //    }
        //    else
        //    {
        //        if (request.BookNumberTo > 0)
        //            query = query.Include(o => o.Book).Where(o => o.Book.BookNumber <= request.BookNumberTo);
        //        if (request.BookNumberFrom > 0 && request.BookNumberTo < 0)
        //            query = query.Include(o => o.Book).Where(o => o.Book.BookNumber == request.BookNumberFrom);
        //        else if (request.BookNumberFrom > 0)
        //            query = query.Include(o => o.Book).Where(o => o.Book.BookNumber >= request.BookNumberFrom);

        //        if (request.BookPageNumberTo > 0)
        //            query = query.Where(o => o.BookPageNumber <= request.BookPageNumberTo);
        //        if (request.BookPageNumberFrom > 0 && request.BookPageNumberTo < 0)
        //            query = query.Where(o => o.BookPageNumber == request.BookPageNumberFrom);
        //        else if (request.BookPageNumberFrom > 0)
        //            query = query.Where(o => o.BookPageNumber >= request.BookPageNumberFrom);
        //        if (!string.IsNullOrEmpty(request.BaseItemName))
        //            if (request.baseItemSearchCriteria == "contains")
        //                query = query.Where(o => o.BaseItem.Name.Contains(request.BaseItemName));
        //            else if (request.baseItemSearchCriteria == "startwith")
        //                query = query.Where(o => o.BaseItem.Name.StartsWith(request.BaseItemName));
        //        if (!string.IsNullOrEmpty(request.Description))
        //            if (request.DescriptionSearchCriteria == "contains")
        //                query = query.Where(o => o.BaseItem.Description.Contains(request.Description));
        //            else if (request.DescriptionSearchCriteria == "startwith")
        //                query = query.Where(o => o.BaseItem.Description.StartsWith(request.Description));
        //        if (request.Consumed != null)
        //        {
        //            if (request.Consumed != null && request.Consumed == true)
        //                query = query.Include(o => o.BaseItem).Where(o => o.BaseItem.Consumed);
        //            else
        //                query = query.Include(o => o.BaseItem).Where(o => !o.BaseItem.Consumed);
        //        }
        //        if (request.BaseItemId > 0)
        //        {
        //            query = query.Where(o => o.BaseItemId == request.BaseItemId);
        //        }
        //        else
        //        {
        //            if (request.CategoryId > 0)
        //                query = query.Include(o => o.BaseItem).Where(o => o.BaseItem.ItemCategoryId == request.CategoryId);
        //            if (request.BudgetId > 0)
        //                query = query.Include(o => o.Addition).Where(o => o.Addition.BudgetId == request.BudgetId);
        //        }
        //    }
        //    count = query.Count();
        //    result.BaseItems = query.Include(o => o.BaseItem)
        //          .Include(o => o.Unit)
        //          .Include(o => o.StoreItemStatus)
        //          .Include(o => o.Book)
        //          .GroupBy(o => o.BaseItem)
        //          .Select(o => new StockTakingBaseItemVM
        //          {
        //              BookNumber = o.First().Book.BookNumber,
        //              BookPageNumber = o.First().BookPageNumber,
        //              Status = o.First().StoreItemStatus != null ? o.First().StoreItemStatus.Name : "",
        //              Id = o.Key.Id,
        //              Name = o.Key.Name,
        //              UnitName = o.First().Unit != null ? o.First().Unit.Name : "",
        //              Quantity = o.Count(),
        //              Price = o.First().Price,
        //          })
        //          .ToList();
        //    count = result.BaseItems.Count;
        //    var baseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
        //    result.TotalPrice = _storeItemRepository.GetAll().Where(o => baseItemsIds.Contains(o.BaseItemId)).Select(o => o.Price).ToList().Sum();
        //    result.TotalUnitsCount = _storeItemRepository.GetAll().Where(o => baseItemsIds.Contains(o.BaseItemId)).Count();
        //    result.TotalConsumedCount = _storeItemRepository.GetAll().Where(o => baseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
        //    result.TotalNotConsumedCount = _storeItemRepository.GetAll().Where(o => baseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();

        //    return result;
        //}


        public SearchStockTakingVM SearchStoreItems(SearchStockTakingCommand request, out int count)
        {
            var result = new SearchStockTakingVM();
            var query = _storeItemRepository.GetAll().Where(x=> request.CurrancyId==null || x.CurrencyId== request.CurrancyId);
            if (request.RandomSotckTaking != null && (bool)request.RandomSotckTaking && request.RandomSotckTakingNumber > 0)
            {
                var randomBaseItemsLookups = query.Include(o => o.BaseItem)
                    .Include(o => o.Unit)
                    .Include(o => o.StoreItemStatus)
                    .Include(o => o.Book)
                    .GroupBy(o => new
                    {
                        baseItem = o.BaseItem,
                        bookNumber = o.Book.BookNumber,
                        pageNumber = o.BookPageNumber,
                        status = o.StoreItemStatus != null ? o.StoreItemStatus.Name : "",
                        unitName = o.Unit != null ? o.Unit.Name : "",
                        price = o.Price

                    })
                    .Select(o => new StockTakingBaseItemVM
                    {
                        BookNumber = o.Key.bookNumber,
                        BookPageNumber = o.Key.pageNumber,
                        Status = o.Key.status,
                        Id = o.Key.baseItem.Id,
                        Name = o.Key.baseItem.Name,
                        UnitName = o.Key.unitName,
                        Quantity = o.Count(),
                        Price = o.Key.price,
                        Isrobbing=false
                        
                    })
                    .Take((int)request.RandomSotckTakingNumber)
                    .ToList();
                result.BaseItems = randomBaseItemsLookups;
                count = result.BaseItems.Count;
                var randomBaseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
                result.TotalPrice = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Select(o => o.Price).ToList().Sum();
                result.TotalUnitsCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Count();
                result.TotalConsumedCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
                result.TotalConsumedCount = _storeItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();
                return result;
            }
            else
            {
                if (request.BookNumberTo > 0)
                    query = query.Include(o => o.Book).Where(o => o.Book.BookNumber <= request.BookNumberTo);
                if (request.BookNumberFrom > 0 && request.BookNumberTo < 0)
                    query = query.Include(o => o.Book).Where(o => o.Book.BookNumber == request.BookNumberFrom);
                else if (request.BookNumberFrom > 0)
                    query = query.Include(o => o.Book).Where(o => o.Book.BookNumber >= request.BookNumberFrom);

                if (request.BookPageNumberTo > 0)
                    query = query.Where(o => o.BookPageNumber <= request.BookPageNumberTo);
                if (request.BookPageNumberFrom > 0 && request.BookPageNumberTo < 0)
                    query = query.Where(o => o.BookPageNumber == request.BookPageNumberFrom);
                else if (request.BookPageNumberFrom > 0)
                    query = query.Where(o => o.BookPageNumber >= request.BookPageNumberFrom);
                if (!string.IsNullOrEmpty(request.BaseItemName))
                    if (request.baseItemSearchCriteria == "contains")
                        query = query.Where(o => o.BaseItem.Name.Contains(request.BaseItemName));
                    else if (request.baseItemSearchCriteria == "startwith")
                        query = query.Where(o => o.BaseItem.Name.StartsWith(request.BaseItemName));
                if (!string.IsNullOrEmpty(request.Description))
                    if (request.DescriptionSearchCriteria == "contains")
                        query = query.Where(o => o.BaseItem.Description.Contains(request.Description));
                    else if (request.DescriptionSearchCriteria == "startwith")
                        query = query.Where(o => o.BaseItem.Description.StartsWith(request.Description));

                if (!string.IsNullOrEmpty(request.ContractNum)) {
                    query = query.Include(o => o.Addition).ThenInclude(o=>o.ExaminationCommitte).Where(o => o.Addition.ExaminationCommitte.ContractNumber == request.ContractNum);

                }

                if (request.Consumed != null)
                {
                    if (request.Consumed != null && request.Consumed == true)
                        query = query.Include(o => o.BaseItem).Where(o => o.BaseItem.Consumed);
                    else
                        query = query.Include(o => o.BaseItem).Where(o => !o.BaseItem.Consumed);
                }
                if (request.BaseItemId > 0)
                {
                    query = query.Where(o => o.BaseItemId == request.BaseItemId);
                }
                //else 
                {
                    if (request.CategoryId > 0)
                        query = query.Include(o => o.BaseItem).Where(o => o.BaseItem.ItemCategoryId == request.CategoryId);
                    if (request.BudgetId > 0)
                        query = query.Include(o => o.Addition).Where(o => o.Addition.BudgetId == request.BudgetId);
                }
            }
            var queryList = query.Include(o => o.BaseItem).ToList();
            count = queryList.Count();
            result.BaseItems = query.Include(o => o.BaseItem)
                  .Include(o => o.Unit)
                  .Include(o => o.StoreItemStatus)
                  .Include(o => o.Book)
                  .Include(o => o.Addition).ThenInclude(o => o.ExaminationCommitte)
                  .GroupBy(o => new
                  {
                      baseItem = o.BaseItem,
                      BookNumber = o.Book.BookNumber,
                      PageNumber = o.BookPageNumber,
                      //   status = o.StoreItemStatus!=null?o.StoreItemStatus.Name:"",
                      UnitName = o.Unit != null ? o.Unit.Name : "",
                      UnitId = o.UnitId,
                      bookId = o.BookId,
                      price = o.Price,
                      ExaminationCommitte=o.Addition.ExaminationCommitte

                  })
                  .Select(o => new StockTakingBaseItemVM
                  {
                      BookNumber = o.Key.BookNumber,
                      BookPageNumber = o.Key.PageNumber,
                      //   Status = o.Key.status,
                      Id = o.Key.baseItem.Id,
                      Name = o.Key.baseItem.Name,
                      UnitName = o.Key.UnitName,
                      UnitId = o.Key.UnitId,
                      BookId = o.Key.bookId,
                      Quantity = o.Count(),
                      Price = o.Key.price,
                      Isrobbing = false,
                      ContractNum = o.Key.ExaminationCommitte != null ? (o.Key.ExaminationCommitte.ContractNumber != null ? o.Key.ExaminationCommitte.ContractNumber : "") : "",

                  })
                  .ToList();
            count = result.BaseItems.Count;
            var baseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
            result.TotalPrice = queryList.Where(o => baseItemsIds.Contains(o.BaseItemId))
                .Select(o => o.Price).Sum();
            result.TotalUnitsCount = queryList.Where(o => baseItemsIds.Contains(o.BaseItemId)).Count();
            result.TotalConsumedCount = queryList.Where(o => baseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
            result.TotalNotConsumedCount = queryList.Where(o => baseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();

            return result;
        }

    }
}
