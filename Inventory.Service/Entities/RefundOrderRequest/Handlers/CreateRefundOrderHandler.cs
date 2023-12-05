using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.NotificationVM;
using Inventory.Repository;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RefundOrderRequest.Handlers
{
    public class CreateRefundOrderHandler : IRequestHandler<CreateRefundOrderCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly IInvoiceBussiness _invoiceBussiness;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;

        public CreateRefundOrderHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IRefundOrderBussiness refundOrderBussiness,
            IRepository<StoreItem, Guid> storeItemRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IInvoiceBussiness invoiceBussiness
            )
        {
            _storeItemRepository = storeItemRepository;
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _refundOrderBussiness = refundOrderBussiness;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _Localizer = localizer;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _invoiceBussiness = invoiceBussiness;
        }
        public async Task<Guid> Handle(CreateRefundOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.items == null || request.items.Count == 0 || request.ExaminationEmployeeId <= 0)
                throw new NullInputsException();
            RefundOrder refund = _mapper.Map<RefundOrder>(request);
            if (refund.IsDirectOrder)
            {
                refund.RefundOrderStatusId = (int)RefundOrderStatusEnum.Reviewed;
            }
            if (request.RefundAttachment != null && request.RefundAttachment.Count > 0)
            {
                refund.RefundOrderAttachment = new List<RefundOrderAttachment>();

                foreach (var attachment in request.RefundAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    refund.RefundOrderAttachment.Add(new RefundOrderAttachment() { Attachment = obj });
                }
            }
            refund.RefundOrderStoreItem = new List<RefundOrderStoreItem>();
            foreach (var item in request.items)
            {
                refund.RefundOrderStoreItem.Add(new RefundOrderStoreItem()
                {
                    Id = Guid.NewGuid(),
                    RefundOrderId = refund.Id,
                    StoreItemId = item.StoreItemId,
                    Notes = item.Notes != null ? item.Notes : null,
                    StoreItemStatusId = item.storeItemStatus.Id != 0 ? item.storeItemStatus.Id : throw new NotSavedException(_Localizer["InvalidModelStateException"])
                });

               _invoiceBussiness.EditInvoiceStoreItemUnderRefund(new InvoiceStoreItem() { Id = item.InvoicestoreItemId });
            }
            RefundOrder result = await _refundOrderBussiness.Create(refund);
            //send notification to (مدير المخازن)
            var storeId = _tenantProvider.GetTenant();
            NotificationVM notification = new NotificationVM();
            notification.Id = result.Id.ToString();
            notification.code = result.Code;
            notification.storeId = storeId.ToString();
            notification.FromStore = _tenantProvider.GetTenantName();
            notification.notificationTemplateEnum = refund.IsDirectOrder?NotificationTemplateEnum.NTF_DirectOrder_RefundOrder: NotificationTemplateEnum.NTF_Create_RefundOrder;
            await _notificationBussiness.SendNotification(notification);
            return Task.FromResult(result.Id).Result;
        }

    }
}