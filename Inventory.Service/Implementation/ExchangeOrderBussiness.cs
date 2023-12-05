using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class ExchangeOrderBussiness : IExchangeOrderBussiness
    {
        readonly private IRepository<ExchangeOrder, Guid> _exchangeOrderRepository;
        private readonly IRepository<StoreItem, Guid> _storeItemRepository;
        private readonly IRepository<ExchangeOrderStoreItem, Guid> _exchangeOrderStoreItemRepository;
        readonly private ICodeGenerator _codeGenerator;
        readonly private ITenantProvider _tenantProvider;
        private readonly ILogger<ExchangeOrderBussiness> _logger;
        private readonly IStringLocalizer<SharedResource> _Localizer;


        public ExchangeOrderBussiness(
            IRepository<ExchangeOrder, Guid> exchangeOrderRepository,
            IRepository<StoreItem, Guid> storeItemRepository,
            IRepository<ExchangeOrderStoreItem,
            Guid> exchangeOrderStoreItemRepository,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            ILogger<ExchangeOrderBussiness> logger,
            IStringLocalizer<SharedResource> Localizer
            )
        {
            _exchangeOrderRepository = exchangeOrderRepository;
            _storeItemRepository = storeItemRepository;
            _exchangeOrderStoreItemRepository = exchangeOrderStoreItemRepository;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _logger = logger;
            _Localizer = Localizer;
        }

        public async Task<ExchangeOrder> Create(List<StoreItemVM> items,
            int budgetId, int forEmployeeId, bool isDirectOrder, string notes, string directNotes)
        {

            var exchangeOrder = new ExchangeOrder()
            {
                BudgetId = budgetId,
                Id = Guid.NewGuid(),
                Notes = notes,
                DirectOrderNotes = directNotes,
                Date = DateTime.Now,
                ForEmployeeId = forEmployeeId,
                IsDirectOrder = isDirectOrder,
                ExchangeOrderStatusId = isDirectOrder? (int)ExchangeOrderStatusEnum.Reviewed : (int)ExchangeOrderStatusEnum.Pending,
                OperationId = (int)OperationEnum.ExchangeOrder,
                Serial = GetMax(),
            };
            exchangeOrder.Code = GetCode(exchangeOrder.Serial);
            exchangeOrder.ExchangeOrderStoreItem = new List<ExchangeOrderStoreItem>();
            foreach (var item in items)
            {
                exchangeOrder.ExchangeOrderStoreItem.Add(new ExchangeOrderStoreItem()
                {
                    Id = Guid.NewGuid(),
                    StoreItemId = item.Id,
                    Notes = item.Notes
                });
            }
            _exchangeOrderRepository.Add(exchangeOrder);
            if (await _exchangeOrderRepository.SaveChanges() > 0)
                return exchangeOrder;
            else
                throw new NotSavedException();
        }

        public IQueryable<ExchangeOrder> GetAllExchangeOrder()
        {
            var ExchangeOrderList = _exchangeOrderRepository.GetAll();
            return ExchangeOrderList;
        }
        public IQueryable<ExchangeOrder> GetAllExchangeOrderView()
        {
            var ExchangeOrderList = _exchangeOrderRepository.GetAll(true);
            return ExchangeOrderList;
        }
        public IQueryable<ExchangeOrder> PrintExchangeOrders()
        {
            return _exchangeOrderRepository.GetAll()
                .Include(e => e.ExchangeOrderStatus)
                .Include(e => e.ForEmployee);

        }
        public int GetMax()
        {
            var date = DateTime.Now;
            return _exchangeOrderRepository.GetMax(null, x => x.Serial) + 1;

        }
        public string GetCode(int serial)
        {
            return _codeGenerator.Generate(serial);
        }
        public List<StoreItem> SearchStoreItems(long? baseItemId, int? budgetId, int? itemCategoryId,int? pageNumber,string contractNumber,List<Guid> SelectStoreItemId, out int count, out int takeitem, out int Expensesitem, out int available)
        {
            count = 0;
            takeitem = 0;
            available = 0;
            Expensesitem = 0;
            var query = _storeItemRepository
                .GetAll(
                o => (baseItemId == null || o.BaseItemId == baseItemId.Value )
                && o.UnderDelete != true 
                && o.UnderExecution!=true
                &&o.StoreItemStatusId != (int)StoreItemStatusEnum.Robbing
                && o.StoreItemStatusId != (int)StoreItemStatusEnum.Tainted
                &&(pageNumber==null||o.BookPageNumber==pageNumber.Value)
                && (budgetId == 0 || budgetId == null || o.Addition.BudgetId == budgetId)
                && (string.IsNullOrEmpty(contractNumber) || o.Addition.ExaminationCommitte.ContractNumber.Contains(contractNumber))
                && ( SelectStoreItemId == null|| SelectStoreItemId.Count==0 || !SelectStoreItemId.Contains(o.Id)))
                .Include(o => o.BaseItem)
                .Include(o=>o.Addition)
                .ThenInclude(o=>o.ExaminationCommitte)
                ;
            //if (budgetId != null && budgetId > 0)
            //    query = query.Where(o => o.Addition.BudgetId == budgetId);
            //if (itemCategoryId != null && itemCategoryId > 0)
            //    query = query.Where(o => o.BaseItem.ItemCategoryId == itemCategoryId);
            //if (!string.IsNullOrEmpty(contractNumber))
            //{  
            //    //query = query.Include(o => o.Addition)
            //    //    .ThenInclude(o => o.ExaminationCommitte);
            //    query = query.Where(o => o.Addition.ExaminationCommitte.ContractNumber == contractNumber);
            //}
            var queryList = query.ToList();
            if (queryList !=null && queryList.Count>0)
            {
                count = queryList.Count();
                available = queryList.Where(x => x.CurrentItemStatusId == (int)ItemStatusEnum.Available).Count();
                takeitem = queryList.Where(x => x.CurrentItemStatusId == (int)ItemStatusEnum.Reserved).Count();
                Expensesitem = queryList.Where(x => x.CurrentItemStatusId == (int)ItemStatusEnum.Expenses).Count();
            }

            return queryList.OrderBy(o => o.BaseItemId).ThenBy(o=>o.Serial).ToList();
        }


        //public List<StoreItem> SearchtopStoreItems(long baseItemId, int? budgetId, int? itemCategoryId, string contractNumber,int take, int statusitem)
        //{

        //    var query = _storeItemRepository
        //        .GetAll()
        //        .Include(o => o.BaseItem)
        //        .Where(o => o.BaseItemId == baseItemId && o.CurrentItemStatusId== statusitem);
        //    if (budgetId != null && budgetId > 0)
        //        query = query.Where(o => o.Addition.BudgetId == budgetId);
        //    //if (itemCategoryId != null && itemCategoryId > 0)
        //    //    query = query.Where(o => o.BaseItem.ItemCategoryId == itemCategoryId);
        //    if (!string.IsNullOrEmpty(contractNumber))
        //    {
        //        query = query.Include(o => o.Addition)
        //            .ThenInclude(o => o.ExaminationCommitte);
        //        query = query.Where(o => o.Addition.ExaminationCommitte.ContractNumber == contractNumber);
        //    }


        //    return query.OrderBy(o => o.Serial).Take((int)take).ToList();
        //}




        public bool ChangeStatus(Guid ExchangeOrderId, ExchangeOrderStatusEnum status)
        {

            _exchangeOrderRepository.PartialUpdate(new ExchangeOrder()
            { Id = ExchangeOrderId, ExchangeOrderStatusId = (int)status }, x => x.ExchangeOrderStatusId);
            return true;
        }


        public async Task<bool> CancelExchangeOrder_Service()
        {
            List<ExchangeOrder> allExchangeOrder = null;

            var today = DateTime.Now;
            _logger.LogInformation("Start Task", today);
            allExchangeOrder = _exchangeOrderRepository.
                GetAll(e => e.ExchangeOrderStatusId == (int)ExchangeOrderStatusEnum.Pending &&
                e.CreationDate.AddHours(72) <= today, true)
                .Include(x => x.ExchangeOrderStoreItem).ToList();

            if (allExchangeOrder.Any())
            {
                foreach (var item in allExchangeOrder)
                {
                    item.ExchangeOrderStatusId = (int)ExchangeOrderStatusEnum.Canceled;
                    foreach (var storeitem in item.ExchangeOrderStoreItem)
                    {
                        _storeItemRepository.PartialUpdate
                            (new StoreItem() { Id = storeitem.StoreItemId, CurrentItemStatusId = (int)ItemStatusEnum.Available },
                                    x => x.CurrentItemStatusId);

                        _logger.LogInformation("update  storeitems to Available ", storeitem.StoreItemId);

                    }
                    _logger.LogInformation("update Exchange Order to cancel ", item.ExchangeOrderStatusId);
                }
                var result = await _exchangeOrderRepository.SaveChanges();
                if (result > 0)
                {
                    _logger.LogInformation("Save Change ", result);
                    return await Task.FromResult(true);
                }

                else
                    _logger.LogInformation("Nothing saved", result);

            }
            else

                _logger.LogInformation("No exchange orders to be deleted");

            return await Task.FromResult(true);
        }


        public async Task<bool> CancelExchangeOrder(Guid exchangeOrderId)
        {
            var exchangeOrder = _exchangeOrderRepository.GetAll().Where(e => e.Id == exchangeOrderId
            && e.ExchangeOrderStatusId == (int)ExchangeOrderStatusEnum.Pending)
                .Include(x => x.ExchangeOrderStoreItem).FirstOrDefault();

            if (exchangeOrder != null)
            {
                exchangeOrder.ExchangeOrderStatusId = (int)ExchangeOrderStatusEnum.Canceled;
                var storeItemsIds = exchangeOrder.ExchangeOrderStoreItem.Select(x => x.StoreItemId);
                var storeitems= _storeItemRepository.GetAll(o => storeItemsIds.Contains(o.Id));

                foreach (var storeitem in storeitems)
                {
                    storeitem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                }
                var result = await _exchangeOrderRepository.SaveChanges();
                if (result > 0)
                    return await Task.FromResult(true);
                else
                    throw new NotSavedException(_Localizer["NotSavedException"]);
            }

            else
                throw new InvalidCanceledExchangeOrder(_Localizer["InvalidCanceledExchangeOrder"]);
        }

        public ExchangeOrder GetById(Guid ExchangeOrderId)
        {
            return _exchangeOrderRepository.GetAll()
                .Include(e => e.ExchangeOrderStoreItem)
                .ThenInclude(s => s.StoreItem)
                .Where(x => x.Id == ExchangeOrderId).FirstOrDefault();
        }
        public async Task<bool> ChangeStatusReview(Guid ExchangeOrderId, ExchangeOrderStatusEnum status)
        {
            var _exchangeOrder = _exchangeOrderRepository.GetAll(x => x.Id == ExchangeOrderId).FirstOrDefault();
            if (_exchangeOrder != null)
                _exchangeOrder.ExchangeOrderStatusId = (int)status;

            //_exchangeOrderRepository.PartialUpdate(new ExchangeOrder() { Id = ExchangeOrderId, ExchangeOrderStatusId = (int)status }, x => x.ExchangeOrderStatusId);

            if (await _exchangeOrderRepository.SaveChanges() > 0)
                return true;
            else
                throw new NotSavedException();
        }

        public List<ExchangeOrderStoreItem> GetExchangeOrderStoreItem(List<Guid> storeItemIds)
        {
            return _exchangeOrderStoreItemRepository.GetAll()
               .Where(x => storeItemIds.Contains(x.StoreItemId))
                .ToList();
        }
        public List<ExchangeOrderStoreItem> GetExchangeOrderStoreItemsByBaseItem(List<long> baseItemIds)
        {
            return _exchangeOrderStoreItemRepository.GetAll()
                .Include(x => x.StoreItem)
               .Where(x => baseItemIds.Contains(x.StoreItem.BaseItemId))
                .ToList();
        }
    }
}
