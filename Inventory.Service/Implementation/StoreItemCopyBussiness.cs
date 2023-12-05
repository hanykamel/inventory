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
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class StoreItemCopyBussiness : IStoreItemCopyBussiness
    {
        readonly private IRepository<StoreItemCopy, Guid> _storeItemCopyRepository;
        readonly private IStringLocalizer<SharedResource> _localizer;
        readonly private ILogger<StoreItemBussiness> _logger;

        public StoreItemCopyBussiness(
            IRepository<StoreItemCopy, Guid> storeItemCopyRepository,
            ILogger<StoreItemBussiness> logger,
            IStringLocalizer<SharedResource> localizer

            )
        {
            _storeItemCopyRepository = storeItemCopyRepository;
            _localizer = localizer;
            _logger = logger;
        }

        public StoreItemCopyBussiness()
        {
        }



        public List<FormNo6StoreItemVM> GetStocktackingStoreItems(List<Guid> storeItems)
        {

            return _storeItemCopyRepository.GetAll(x => storeItems.Contains(x.HistoryId) && x.AuditAction== "Insert",true)
               .Include(x => x.StoreItemStatus)
               .Include(x => x.History)
               .ThenInclude(x => x.BaseItem)
               .Include(x => x.History)
               .ThenInclude(x => x.Unit)
               .Include(x => x.History)
               .ThenInclude(x => x.Book)
               .Include(x => x.History)
               .ThenInclude(x => x.Currency)
               .Include(x=>x.History).ThenInclude(x=> x.Addition).ThenInclude(x=>x.ExaminationCommitte)
               .GroupBy(x => new
               {
                   contractnum=x.History.Addition.ExaminationCommitte.ContractNumber,
                   BaseItemId = x.BaseItemId,
                   StoreItemStatus = x.StoreItemStatus.Name,
                   Description = x.History.BaseItem.Description,
                   BaseItemName = x.History.BaseItem.Name,
                   Unit = x.History.Unit.Name,
                   Price = x.History.Price,
                   Note = x.Note,
                   Currency = x.History.Currency.Name,
                   BookNumber = x.History.Book.BookNumber,
                   BookId = x.BookId,
                   UnitId = x.UnitId,
                   PageNumber = x.BookPageNumber,
                   CurrencyId = x.CurrencyId,
                   
               })
               .Select(a => new FormNo6StoreItemVM
               {
                   ContractNumber=a.Key.contractnum,
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
    }
}
