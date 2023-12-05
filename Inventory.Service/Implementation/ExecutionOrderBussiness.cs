using AutoMapper;
using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Resources;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.ExecutionOrderVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Repository;
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
    public class ExecutionOrderBussiness : IExecutionOrderBussiness
    {
        private readonly IRepository<ExecutionOrder, Guid> _executionOrderRepository;
        private readonly IRepository<RobbingOrderRemainsDetails, Guid> _robbingOrderRemainsDetailsRepository;
        private readonly IRepository<ExecutionOrderStoreItem, Guid> _executionOrderStoreItemRepository;
        private readonly IRepository<StoreItem, Guid> _StoreItemRepository;
        private readonly IRepository<ExecutionOrderResultItem, Guid> _executionOrderResultItemRepository;
        private readonly IRepository<ExecutionOrderResultRemain, Guid> _executionOrderResultRemainRepository;



        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IRepository<Attachment, Guid> _attachmentRepositoryRepository;
        private readonly IRepository<ExecutionOrderAttachment, Guid> _executionOrderAttachmentRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ITenantProvider _tenantProvider;

        public ExecutionOrderBussiness(
            IRepository<ExecutionOrder, Guid> executionOrderRepository,
            IStringLocalizer<SharedResource> localizer,
            IRepository<Attachment, Guid> attachmentRepositoryRepository,
            IRepository<ExecutionOrderAttachment, Guid> executionOrderAttachment,
            ICodeGenerator codeGenerator,
            ITenantProvider tenantProvider,
            IRepository<ExecutionOrderStoreItem, Guid> executionOrderStoreItemRepository,
            IRepository<ExecutionOrderResultItem, Guid> executionOrderResultItemRepository,
            IRepository<ExecutionOrderResultRemain, Guid> executionOrderResultRemainRepository,
            IRepository<StoreItem, Guid> StoreItemRepository,
            IRepository<RobbingOrderRemainsDetails, Guid> robbingOrderRemainsDetailsRepository
            )
        {
            _executionOrderRepository = executionOrderRepository;
            _executionOrderAttachmentRepository = executionOrderAttachment;
            _localizer = localizer;
            _attachmentRepositoryRepository = attachmentRepositoryRepository;
            _codeGenerator = codeGenerator;
            _tenantProvider = tenantProvider;
            _executionOrderStoreItemRepository = executionOrderStoreItemRepository;
            _executionOrderResultRemainRepository = executionOrderResultRemainRepository;
            _executionOrderResultItemRepository = executionOrderResultItemRepository;
            _StoreItemRepository = StoreItemRepository;
            _robbingOrderRemainsDetailsRepository = robbingOrderRemainsDetailsRepository;
        }

        public IQueryable<ExecutionOrder> GetAllExecutionOrders()
        {
            return _executionOrderRepository.GetAll();
        }

        public IQueryable<ExecutionOrder> GetAllExecutionOrdersView()
        {
            return _executionOrderRepository.GetAll(true);
        }
        public IQueryable<CustomeExecutionOrderVM> GetCustomeExecutionOrdersList()
        {
            var _RobbingOrderRemainsDetailsIds = (_robbingOrderRemainsDetailsRepository.GetAll()).Select(e => e.ExecutionOrderResultRemainId);

            return _executionOrderRepository.GetAll()
                .Select(s => new CustomeExecutionOrderVM()
                {
                    TenantId = s.TenantId,
                    Id = s.Id,
                    Code = s.Code,
                    Date = s.Date,
                    ExecutionOrderStatusId = s.ExecutionOrderStatusId,
                    SubtractionModel=s.Subtraction.Select(x=>new SubtractionVM()
                    {
                        Id=x.Id,
                        date=x.RequestDate,
                        SubtractionNum=x.SubtractionNumber,
                    }).AsQueryable(),
                    SubtractionNum=s.Subtraction.FirstOrDefault().SubtractionNumber,
                    ExecutionOrderStatus = new ExecutionOrderStatusVM()
                    {
                        Id = s.ExecutionOrderStatus.Id,
                        Name = s.ExecutionOrderStatus.Name
                    },
                    ItemsNotAddedCount = s.ExecutionOrderResultItem.Where(c=>c.IsAdded != true).Any(),
                    RemainItemAdded= s.ExecutionOrderResultRemain.Where(x=> _RobbingOrderRemainsDetailsIds.Contains(x.Id)).Count()== s.ExecutionOrderResultRemain.Count()?false:true,
                    CreationDate = s.CreationDate
                });
        }
        public IQueryable<ExecutionListVM> GetExecutionOrdersList()
        {
            var x= _executionOrderRepository.GetAll()
                .Include(e => e.ExecutionOrderStatus)
                .Include(e=>e.Subtraction).Select(e=>new ExecutionListVM() { 
                    Code = e.Code,
                    RequestDate = e.Subtraction.FirstOrDefault().RequestDate,
                    SubtractionNum = e.Subtraction.FirstOrDefault()!=null&&e.Subtraction.Count>0? e.Subtraction.FirstOrDefault().SubtractionNumber:null,
                    CreationDate = e.CreationDate,
                    ExchangeOrderStatusName =  e.ExecutionOrderStatus.Name
                });;
            return x;
        }

        public ExecutionOrder GetById(Guid Id)
        {
            return _executionOrderRepository.GetAll(true)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }

        public ExecutionOrder GetExecutionOrderById(Guid Id)
        {
            return _executionOrderRepository.GetAll(true)
                .Include(ex => ex.ExecutionOrderStoreItem)
                .Include(ex => ex.Budget)
                .Include(ex=>ex.Subtraction)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }


        public async Task<bool> CancelExecutionOrderAsync(Guid Id)
        {

            var executionOrder = _executionOrderRepository.GetAll(true)
                    .Where(e => e.Id == Id).Include(x => x.ExecutionOrderStoreItem).FirstOrDefault();
            if (executionOrder != null & executionOrder.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Pending)
            {
                executionOrder.ExecutionOrderStatusId = (int)ExecutionOrderStatusEnum.Canceled;

                var storeitemlist = _StoreItemRepository.GetAll().Where(x => executionOrder.ExecutionOrderStoreItem.Select(s => s.StoreItemId).Contains(x.Id));

                foreach (var item in storeitemlist)
                {
                    item.UnderExecution = false;
                }
                await _executionOrderRepository.SaveChanges();

                return true;
            }
            else
            {
                throw new InvalidCanceledExecutionOrder(_localizer["CanceledExecutionOrder"]);
            }


        }



        public bool ReviewExecutionOrderAsync(Guid Id, string reviewNote, List<Guid> storeitemReviewId, List<Guid> storeitemUnReviewId, List<Attachment> allAttachment)
        {

            var executionOrder = _executionOrderRepository.GetAll(true)
                    .Where(e => e.Id == Id).Include(x=>x.ExecutionOrderAttachment).Include(x => x.ExecutionOrderStoreItem).FirstOrDefault();
            if (executionOrder != null & executionOrder.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Pending)
            {
                executionOrder.ReviewNotes = reviewNote;
                executionOrder.ExecutionOrderStatusId = (int)ExecutionOrderStatusEnum.Reviewed;
                foreach (var itemAttachment in allAttachment)
                {
                    executionOrder.ExecutionOrderAttachment.Add(new ExecutionOrderAttachment() { Id = Guid.NewGuid(), Attachment = itemAttachment });

                }
                //foreach (var item in storeitemReviewId)
                //{
                //    var ExecutionOrderStoreItem = executionOrder.ExecutionOrderStoreItem.FirstOrDefault(x => x.StoreItemId == item);
                //    ExecutionOrderStoreItem.IsApproved = true;
                //}
                if (storeitemReviewId != null)
                {
                    var ExecutionOrderStoreItem = executionOrder.ExecutionOrderStoreItem.Where(x => storeitemReviewId.Contains(x.StoreItemId));
                    foreach (var item in ExecutionOrderStoreItem)
                    {
                        item.IsApproved = true;
                    }
                }
                if (storeitemUnReviewId != null)
                {
                    var storeitemlistUnReview = _StoreItemRepository.GetAll().Where(x => storeitemUnReviewId.Contains(x.Id));
                    foreach (var item in storeitemlistUnReview)
                    {
                        item.UnderExecution = false;
                    }
                }

                return true;
            }
            else
            {
                throw new InvalidCanceledExecutionOrder(_localizer["ReviewedExecutionOrder"]);
            }


        }



        public async Task<bool> Save()
        {
            int added = await _executionOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_localizer["NotSavedException"]);
        }


        public int GetMax()
        {
            return _executionOrderRepository.GetMax(null,x => x.Serial) + 1;
        }

        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }


        public async Task<Guid> AddNewExecutionOrder(ExecutionOrder ExecutionOrder)
        {
            ExecutionOrder.OperationId = (int)OperationEnum.Execution;
            ExecutionOrder.ExecutionOrderStatusId = (int)ExecutionOrderStatusEnum.Pending;
            ExecutionOrder.Id = Guid.NewGuid();
            ExecutionOrder.TenantId = _tenantProvider.GetTenant();
            ExecutionOrder.Serial = GetMax();
            ExecutionOrder.Code = GetCode();
            _executionOrderRepository.Add(ExecutionOrder);
            int added = await _executionOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(ExecutionOrder.Id);
            else
                throw new Exception(_localizer["NotSavedException"]);
        }


        public IEnumerable<ExecutionOrderBaseItemModel> GetExecutionOrderStoreItemReview(Guid Id)
        {
            List<ExecutionOrderBaseItemModel> listModel = new List<ExecutionOrderBaseItemModel>();
            return _executionOrderStoreItemRepository.GetAll().Where(x => x.ExecutionOrderId == Id).Include(x => x.StoreItem).ThenInclude(x => x.BaseItem)
                   .Include(x => x.StoreItem).ThenInclude(x => x.StoreItemStatus)
                   .GroupBy(x => new
                   { baseItemId = x.StoreItem.BaseItemId, BaseItemName = x.StoreItem.BaseItem.Name, disc = x.StoreItem.BaseItem.Description,
                   Price=x.StoreItem.Price})
                   .Select(x => new ExecutionOrderBaseItemModel
                   {
                       BaseItemId = x.Key.baseItemId,
                       BaseItemName = x.Key.BaseItemName,
                       Discription = x.Key.disc,
                       Quantity = x.Count(),
                       Price=x.Key.Price,
                       ExecutionOrderStoreItems = x.Select(a => new ExecutionOrderStoreItemModel() { StoreItemId = a.StoreItemId, StoreItemCode = a.StoreItem.Code, status = a.StoreItem.StoreItemStatus.Name, isApproved = (bool)a.IsApproved,CurrancyId=a.StoreItem.CurrencyId }).OrderBy(s => s.StoreItemCode)
                   }) ;

        }




        public async Task<bool> AddNewExecutionOrderReminsAndItem(Guid executionOrdeid, List<ExecutionOrderResultItem> AllExecutionOrderResultItem, List<ExecutionOrderResultRemain> AllExecutionOrderResultRemain, List<Attachment> allAttachment)
        {
            foreach (var item in AllExecutionOrderResultItem)
            {
                item.Id = Guid.NewGuid();
                _executionOrderResultItemRepository.Add(item);
            }
            foreach (var item in AllExecutionOrderResultRemain)
            {
                item.Id = Guid.NewGuid();
                _executionOrderResultRemainRepository.Add(item);
            }


            var executionOrde = _executionOrderRepository.GetAll(true)
              .Where(e => e.Id == executionOrdeid).Include(x => x.ExecutionOrderStoreItem).Include(x=>x.ExecutionOrderAttachment)
               .FirstOrDefault();
            executionOrde.ExecutionOrderStatusId = (int)ExecutionOrderStatusEnum.Resulted;
            foreach (var itemAttachment in allAttachment)
            {
                executionOrde.ExecutionOrderAttachment.Add(new ExecutionOrderAttachment() { Id = Guid.NewGuid(), Attachment = itemAttachment });

            }
            var storeitemlist = _StoreItemRepository.GetAll().Where(x => executionOrde.ExecutionOrderStoreItem.Where(e => e.IsApproved == true).Select(s => s.StoreItemId).Contains(x.Id));
            foreach (var item in storeitemlist)
            {
                item.IsActive = false;
            }
            int added = await _executionOrderRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new Exception(_localizer["NotSavedException"]);
        }




        public IQueryable<RemainItemsModel> GetRobbingRemainItems(long? RemainId, Guid[] SelectExecutionOrderRemainItemId,int  BudgetId)
        {
            var _RobbingOrderRemainsDetailsIds = (_robbingOrderRemainsDetailsRepository.GetAll()).Select(e => e.ExecutionOrderResultRemainId);
            return _executionOrderResultRemainRepository.GetAll().Where(x => (RemainId == null || x.RemainsId == RemainId)
            && (SelectExecutionOrderRemainItemId == null || !SelectExecutionOrderRemainItemId.Any() 
            || !SelectExecutionOrderRemainItemId.Contains(x.Id))
            && (!_RobbingOrderRemainsDetailsIds.Any()||!_RobbingOrderRemainsDetailsIds.Contains(x.Id))&& x.ExecutionOrder.BudgetId== BudgetId)
                .Include(x => x.Remains).Include(x => x.Unit).Include(x=>x.ExecutionOrder)
                .Select(x => new RemainItemsModel()
            {
                Id = x.Id,
                ExecutionOrderId = x.ExecutionOrderId,
                RemainsId = x.RemainsId,
                Note = x.Note,
                Quantity = x.Quantity,
                UnitId = x.UnitId,
                UnitName = x.Unit.Name,
                Remainsname = x.Remains.Name,
                    ExecutionOrderCode=x.ExecutionOrder.Code,

                }).OrderBy(x=>x.RemainsId);
        }

        public void UpdateExecutionOrderAfterAddition(Guid? executionOrderId, List<Guid> committeeItemids, List<string> notes)
        {
            ExecutionOrder executionOrderObj = _executionOrderRepository
               .GetAll(o => o.Id == executionOrderId)
               .Include(o => o.ExecutionOrderResultItem)
               .FirstOrDefault();

            if (committeeItemids != null && notes.Any())
                for (int i = 0; i < committeeItemids.Count; i++)
                {
                    var commiteeItem = executionOrderObj.ExecutionOrderResultItem.FirstOrDefault(o => o.Id == committeeItemids[i]);
                    commiteeItem.IsAdded = true;
                }


            _executionOrderRepository.Update(executionOrderObj);
        }

        public IQueryable<RemainModel> getRemainItemRobbing(Guid ExecutionOrderId)
        {
            var _RobbingOrderRemainsDetailsIds = (_robbingOrderRemainsDetailsRepository.GetAll()).Select(e => e.ExecutionOrderResultRemainId);

            return _executionOrderResultRemainRepository.GetAll().Where(x => x.ExecutionOrderId == ExecutionOrderId &&
            (!_RobbingOrderRemainsDetailsIds.Any() || !_RobbingOrderRemainsDetailsIds.Contains(x.Id)))
                .Include(x => x.Remains).Include(x => x.Unit).Include(x => x.ExecutionOrder).ThenInclude(b => b.Budget)
                .GroupBy(r => new { BudgetId = r.ExecutionOrder.BudgetId, budgetName = r.ExecutionOrder.Budget.Name })
                .Select(z => new RemainModel()
                {
                    BudgetId = z.Key.BudgetId,
                    BudgetName = z.Key.budgetName
                ,
                    RemainItemsModels = z.Select(x => new RemainItemsModel()
                    {
                        Id = x.Id,
                        ExecutionOrderId = x.ExecutionOrderId,
                        RemainsId = x.RemainsId,
                        Note = x.Note,
                        Quantity = x.Quantity,
                        UnitId = x.UnitId,
                        UnitName = x.Unit.Name,
                        Remainsname = x.Remains.Name,
                        ExecutionOrderCode = x.ExecutionOrder.Code,

                    }).OrderBy(x => x.RemainsId)
                });




               
        }

        
    }
}
