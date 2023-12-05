using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class RobbedStoreItemBussiness : IRobbedStoreItemBussiness
    {
        private readonly IRepository<StockTaking, Guid> _stockTakingRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ITenantProvider _tenantProvider;
        private readonly IRepository<RobbedStoreItem, Guid> _robbedStoreItemRepository;

        public RobbedStoreItemBussiness(
            IStringLocalizer<SharedResource> Localizer,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
           IRepository<RobbedStoreItem, Guid> robbedStoreItemRepository,
           IRepository<StockTaking, Guid> stockTakingRepository
            )
        {

            _localizer = Localizer;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _robbedStoreItemRepository = robbedStoreItemRepository;
            _stockTakingRepository = stockTakingRepository;
        }
        public SearchStockTakingVM GetRobbedStoreItem(SearchStockTakingCommand request, out int count)
        {
            var result = new SearchStockTakingVM();
            var query = _robbedStoreItemRepository.GetAll().Where(x => request.CurrancyId == null || x.CurrencyId == request.CurrancyId);
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
                        price = o.Price,
                        Quantity=o.Quantity


                    })
                    .Select(o => new StockTakingBaseItemVM
                    {
                        BookNumber = o.Key.bookNumber,
                        BookPageNumber = o.Key.pageNumber,
                        Status = o.Key.status,
                        Id = o.Key.baseItem.Id,
                        Name = o.Key.baseItem.Name,
                        UnitName = o.Key.unitName,
                        Quantity = o.Key.Quantity,
                        Price = o.Key.price,
                        Isrobbing = true
                    })
                    .Take((int)request.RandomSotckTakingNumber)
                    .ToList();
                result.BaseItems = randomBaseItemsLookups;
                count = result.BaseItems.Count;
                var randomBaseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
                result.TotalPrice = _robbedStoreItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Select(o => o.Price).ToList().Sum();
                result.TotalUnitsCount = _robbedStoreItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId)).Count();
                result.TotalConsumedCount = _robbedStoreItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
                result.TotalConsumedCount = _robbedStoreItemRepository.GetAll().Where(o => randomBaseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();
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
            count = query.Count();
            result.BaseItems = query.Include(o => o.BaseItem)
                  .Include(o => o.Unit)
                  .Include(o => o.StoreItemStatus)
                  .Include(o => o.Book)
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
                      Quantity=o.Quantity
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
                      Quantity = o.Key.Quantity,
                      Price = o.Key.price,
                      Isrobbing = true
                  })
                  .ToList();

            
            count = result.BaseItems.Count;
            var baseItemsIds = result.BaseItems.Select(o => o.Id).ToList();
            result.TotalPrice = query.Where(o => baseItemsIds.Contains(o.BaseItemId)).Select(o => o.Price).ToList().Sum();
            result.TotalUnitsCount = query.Where(o => baseItemsIds.Contains(o.BaseItemId)).Count();
            result.TotalConsumedCount = query.Where(o => baseItemsIds.Contains(o.BaseItemId) && o.BaseItem.Consumed).Count();
            result.TotalNotConsumedCount = query.Where(o => baseItemsIds.Contains(o.BaseItemId) && !o.BaseItem.Consumed).Count();

            return result;
        }


        public  StockTaking CreateStockTakingRobbing(CreateStockTakingCommand request, StockTaking NewStockTaking)
        {
            if (request.BaseItems != null && request.BaseItems.Count > 0)
            {
             
                var RobbingItemIds = request.BaseItems.Where(o=>o.Isrobbing == true).Select(o => o.Id).ToList();
                var RobbingItem = _robbedStoreItemRepository.GetAll()
                    .Include(x => x.BaseItem)
                    .Include(x => x.Unit)
                    .Include(x => x.StoreItemStatus)
                    .Include(x => x.Book)
                    .Where(o => RobbingItemIds.Contains(o.BaseItemId))
                    .ToList();

                if (RobbingItem != null)
                {
                    foreach (var baseItemObj in request.BaseItems)
                    {
                        var newStockStoreItem =
                            RobbingItem
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

                                ).Select(x => x.Id);
                        foreach (var item in newStockStoreItem)
                        {
                            NewStockTaking.StockTakingRobbedStoreItem.Add(new StockTakingRobbedStoreItem()
                            {
                                Id = Guid.NewGuid(),
                                RobbedStoreItemId = item,
                                StockTakingId = NewStockTaking.Id,
                            });
                        }
                    }
                }
            }
            return NewStockTaking;

        }

        public List<FormNo6StoreItemVM> GetStocktackingRobbedStoreItems(List<Guid> RobbedstoreItems)
        {

            return _robbedStoreItemRepository.GetAll(x => RobbedstoreItems.Contains(x.Id) )
               .Include(x => x.StoreItemStatus)
               .Include(x => x.BaseItem)
               .Include(x => x.Unit)
               .Include(x => x.Book)
               .Include(x => x.Currency)
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
                   CurrencyId = x.CurrencyId,
                   Quantity=x.Quantity

               })
               .Select(a => new FormNo6StoreItemVM
               {
                   BookNumber = a.Key.BookNumber,
                   PageNumber = a.Key.PageNumber,
                   ItemStatus = a.Key.StoreItemStatus,
                   BaseItemName = a.Key.BaseItemName,
                   BaseItemId = a.Key.BaseItemId,
                   UnitName = a.Key.Unit,
                   Quantity = a.Key.Quantity,
                   Price = a.Key.Price,
                   Currency = a.Key.Currency,
                   Description = a.Key.Description,
                   Notes = a.Key.Note,
                   BookId = a.Key.BookId,
                   UnitId = a.Key.UnitId,
                   CurrencyId = a.Key.CurrencyId
               }).ToList();
        }


        public List<RobbedStoreItem> GetRobbedStoreItemInquiry(InquiryStoreItemsRequest inquiryRequest, out int count)
        {
            count = 0;
            var result = new SearchStockTakingVM();
            var query = _robbedStoreItemRepository.GetAll();
            query = query
         .Where(o => inquiryRequest.baseItemId == null || o.BaseItemId == inquiryRequest.baseItemId)
         .Where(o => inquiryRequest.categoryId == null || o.BaseItem.ItemCategoryId == inquiryRequest.categoryId)
         .Where(o => inquiryRequest.storeId == null || o.TenantId == inquiryRequest.storeId)
         //.Where(o => inquiryRequest.StoreItemAvailibilityStatusId == null || o.CurrentItemStatusId == inquiryRequest.StoreItemAvailibilityStatusId)
         .Where(o => inquiryRequest.StoreItemStatus == null || o.StoreItemStatusId == inquiryRequest.StoreItemStatus)
         .Where(o => inquiryRequest.budgetId == null || o.Addition.BudgetId == inquiryRequest.budgetId)
         .Where(o => inquiryRequest.consumed == null || o.BaseItem.Consumed == inquiryRequest.consumed)
         .Where(o => inquiryRequest.BookPageNumberTo <= 0 || o.BookPageNumber <= inquiryRequest.BookPageNumberTo)
         .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || inquiryRequest.BookPageNumberTo > 0 || o.BookPageNumber == inquiryRequest.BookPageNumberFrom)
         .Where(o => inquiryRequest.BookPageNumberFrom <= 0 || o.BookPageNumber >= inquiryRequest.BookPageNumberFrom)
         .Where(o => inquiryRequest.BookNumberTo <= 0 || o.Book.BookNumber <= inquiryRequest.BookNumberTo)
         .Where(o => inquiryRequest.BookNumberFrom <= 0 || inquiryRequest.BookNumberTo > 0 || o.Book.BookNumber == inquiryRequest.BookNumberFrom)
         .Where(o => inquiryRequest.BookNumberFrom <= 0 || o.Book.BookNumber >= inquiryRequest.BookNumberFrom);
            if (query.Any())
            {
                count = query.Count();
                query = query
                    .Include(a => a.BaseItem)
                    .Include(x => x.Unit)
                    .Include(a => a.StoreItemStatus)
                    .Include(a => a.Addition)
                    .OrderBy(o => o.BaseItem.Name)
                    .Skip((int)inquiryRequest.skip).Take((int)inquiryRequest.take);



            }

            return query.ToList();
        }
    }
}
