using AutoMapper;
using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class RefundOrderBussiness : IRefundOrderBussiness
    {
        readonly private IRepository<RefundOrder, Guid> _refundOrderRepository;
        readonly private IRepository<RefundOrderStoreItem, Guid> _refundOrderStoreItemRepository;
        
        readonly private ICodeGenerator _codeGenerator;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IMapper _mapper;

        public RefundOrderBussiness(
            IRepository<RefundOrder, Guid> refundOrderRepository,
            IRepository<RefundOrderStoreItem, Guid> refundOrderStoreItemRepository,
            ICodeGenerator codeGenerator,
            IStringLocalizer<SharedResource> Localizer,
            IMapper mapper
            )
        {
            _refundOrderRepository = refundOrderRepository;
            _refundOrderStoreItemRepository = refundOrderStoreItemRepository;
            _codeGenerator = codeGenerator;
            _Localizer = Localizer;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStatus(Guid RefundOrderId, RefundOrderStatusEnum status)
        {
            //_refundOrderRepository.PartialUpdate(new RefundOrder()
            //{ Id = RefundOrderId, RefundOrderStatusId = (int)status }, x => x.RefundOrderStatusId);


            var _refuncOrderObj = _refundOrderRepository.GetAll
                (x => x.Id == RefundOrderId && x.RefundOrderStatusId == (int)RefundOrderStatusEnum.Pending).FirstOrDefault();
            if (_refuncOrderObj == null) { 
                throw new ReviewException(_Localizer["ChangeReviewRefund"]);
            }
                _refuncOrderObj.RefundOrderStatusId = (int)status;

            if (await _refundOrderRepository.SaveChanges() > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(); 
        }

        public  RefundOrder GetById(Guid RefundOrderId)
        {
            return _refundOrderRepository.GetAll()
                .Include(x=>x.RefundOrderEmployee)
                .Where(x => x.Id == RefundOrderId).FirstOrDefault();
        }

        public IQueryable<RefundOrder> GetAllRefundOrder()
        {
            var refundOrderList = _refundOrderRepository.GetAll();
            return refundOrderList;
        }
        public IQueryable<RefundOrder> GetAllRefundOrderView()
        {
            var refundOrderList = _refundOrderRepository.GetAll(true);
            return refundOrderList;
        }
        public IQueryable<RefundOrderVM> PrintRefundOrdersList()
        {
            return _refundOrderRepository.GetAll().Include(r => r.RefundOrderStatus)
                .Select(s => new RefundOrderVM()
                {
                    Code = s.Code,
                    Date = s.Date,
                    CreationDate = s.CreationDate,
                    TenantId = s.TenantId,
                    RefundOrderStatusId = s.RefundOrderStatusId,
                    RefundOrderStatus = new RefundOrderStatusVM() { Name = s.RefundOrderStatus.Name }
                });
        }
        public async Task<RefundOrder> Create(RefundOrder refund)
        {
            refund.Id = Guid.NewGuid();
            refund.Date = DateTime.Now;
            
            if (refund.IsDirectOrder)
                refund.RefundOrderStatusId = (int)RefundOrderStatusEnum.Reviewed;
            else
                refund.RefundOrderStatusId = (int)RefundOrderStatusEnum.Pending;
            refund.OperationId = (int)OperationEnum.RefundOrder;
            refund.Serial = GetMax();
            refund.Code = GetCode(refund.Serial);
            _refundOrderRepository.Add(refund);
            if (await _refundOrderRepository.SaveChanges() > 0)
                return refund;
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }
        public int GetMax()
        {
            return _refundOrderRepository.GetMax(null, x => x.Serial) + 1;

        }
        public string GetCode(int serial)
        {
            return _codeGenerator.Generate(serial);
        }

        public async Task<bool> CompeleteRefundOrder(Guid RefundOrderId)
        {
            var refundOrder=_refundOrderRepository.Get(RefundOrderId);
            if(refundOrder !=null)
            {
                if (refundOrder.RefundOrderStatusId != (int)RefundOrderStatusEnum.Reviewed)
                {
                    throw new InvalidCanceledExchangeOrder(_Localizer["InvalidRefundOrder"]);
                }
                else
                {
                    refundOrder.RefundOrderStatusId = (int)RefundOrderStatusEnum.Refunded;
                    return await Task.FromResult(true);
                }

            }
            else
                return await Task.FromResult(false);

        }

        public List<DedcuctionStoreItemVM> GetTaintedItemsByRefundOrderId(Guid Id)
        {
            return _refundOrderStoreItemRepository.GetAll(true)
                .Include(d => d.StoreItem)
                .ThenInclude(s => s.BaseItem)
                .Include(d => d.StoreItem)
                .ThenInclude(s => s.Unit)
                .Include(d => d.StoreItem)
                .ThenInclude(s=>s.Currency)
                .Include(d => d.StoreItem).ThenInclude(s => s.Addition).ThenInclude(s=>s.ExaminationCommitte)
                .Include(s => s.StoreItemStatus)
                .Where(a => a.RefundOrderId == Id && a.StoreItemStatusId==(int)StoreItemStatusEnum.Tainted)
                .GroupBy(x => new { 
                    contractnum=x.StoreItem.Addition.ExaminationCommitte.ContractNumber,
                    pagenum=x.StoreItem.BookPageNumber,
                    BaseItemId =x.StoreItem.BaseItemId,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    UntiName = x.StoreItem.Unit.Name,
                    Price = x.StoreItem.Price,
                    Currency = x.StoreItem.Currency.Name,
                    State = x.StoreItemStatus.Name})
                .Select(a => new DedcuctionStoreItemVM
                {
                    ContractNumber=a.Key.contractnum,
                    PageNumber=a.Key.pagenum,
                    BaseItemId = a.Key.BaseItemId,
                    BaseItemName = a.Key.BaseItemName,
                    UnitName = a.Key.UntiName,
                    Quantity = a.Count(),
                    UnitPrice = a.Key.Price,
                    Currency = a.Key.Currency,
                    TotalPrice = a.Count() * a.Key.Price,
                    ItemStatus = a.Key.State,
                    Notes = string.Join(" - ", a.Select(item => item.Notes))
        }).ToList();
        }

        public  RefundOrder CancelRefundOrder(Guid RefundOrderId)
        {
             var refundOrder = _refundOrderRepository.GetAll().
                Where(e => e.Id == RefundOrderId
             && (e.RefundOrderStatusId == (int)RefundOrderStatusEnum.Pending||e.RefundOrderStatusId == (int)RefundOrderStatusEnum.Reviewed))
                 .Include(x => x.RefundOrderStoreItem).FirstOrDefault();
            if (refundOrder != null)
            {
                refundOrder.RefundOrderStatusId = (int)RefundOrderStatusEnum.Cancelled;
                return refundOrder;
            }
            else
            {
                throw new InvalidCanceledExchangeOrder(_Localizer["InvalidCanceledRefundOrder"]);
            }
        }

        public List<Guid> GetRefundOrderStoreItems(Guid RefundOrderId,List<Guid> notIncludeStoreItems)
        {
            if(notIncludeStoreItems !=null)
                return _refundOrderStoreItemRepository.
                GetAll(o=>o.RefundOrderId==RefundOrderId && !notIncludeStoreItems.Contains(o.StoreItemId))
                .Select(o=>o.StoreItemId).ToList();
            else
                return _refundOrderStoreItemRepository.
                GetAll(o => o.RefundOrderId == RefundOrderId )
                .Select(o => o.StoreItemId).ToList();
        }
    }
}
