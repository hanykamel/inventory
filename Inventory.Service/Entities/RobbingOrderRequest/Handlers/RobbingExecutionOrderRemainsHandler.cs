using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Models.NotificationVM;
using Inventory.Data.Enums;

namespace Inventory.Service.Entities.RobbingOrderRequest.Handlers
{
    public class RobbingExecutionOrderRemainsHandler : IRequestHandler<RobbingExecutionOrderRemainsCommand, Guid>
    {

        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private IServiceScopeFactory factory;
        private readonly ISubtractionBussiness _subtractionBussiness;
        public RobbingExecutionOrderRemainsHandler(IRobbingOrderBussiness robbingOrderBussiness,
        IStoreItemBussiness storeItemBussiness,
            IMapper mapper,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IStoreBussiness storeBussiness,
             IServiceScopeFactory _factory,
             ISubtractionBussiness subtractionBussiness
            )
        {
            _robbingOrderBussiness = robbingOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _mapper = mapper;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
            factory = _factory;
            _subtractionBussiness = subtractionBussiness;
        }

        public async Task<Guid> Handle(RobbingExecutionOrderRemainsCommand request, CancellationToken cancellationToken)
        {

            var robbingOrder = _mapper.Map<RobbingOrder>(request);
            robbingOrder.Id = Guid.NewGuid();


            robbingOrder.RobbingOrderStoreItem = new List<RobbingOrderStoreItem>();
            if (request.RobbingOrderRemainItems != null && request.RobbingOrderRemainItems.Count > 0)
            {
                foreach (var remain in request.RobbingOrderRemainItems)
                {
                    robbingOrder.RobbingOrderRemainsDetails.Add
                                   (new RobbingOrderRemainsDetails()
                                   {
                                       Id = Guid.NewGuid(),
                                       ExecutionOrderResultRemainId = remain.ExecutionOrderResultRemainId,
                                       Notes = remain.Notes,
                                       Price = remain.price,
                                       ExaminationReport = remain.ExaminationReport
                                   });


                }
            }

            robbingOrder.Subtraction = new List<Subtraction>();
            Subtraction subtraction = _mapper.Map<Subtraction>(request);
            subtraction = _subtractionBussiness.PrepareSubtraction(subtraction);
            robbingOrder.Subtraction.Add(subtraction);


            robbingOrder.RobbingOrderAttachment = new List<RobbingOrderAttachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    robbingOrder.RobbingOrderAttachment.Add(new RobbingOrderAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }


            var id = await _robbingOrderBussiness.AddNewRobbingOrderExecutionRemainItem(robbingOrder);

            #region Background_thread_notification
            // Create new  Background thread for notification
            new Thread(() =>
            {
                using (var scope = this.factory.CreateScope())
                {
                    // create  service For this Scope and  Dispose when scope finish and thread finish
                    var _tenantProviderSer = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                    var _notificationSer = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();
                    var _storeBussinessSer = scope.ServiceProvider.GetRequiredService<IStoreBussiness>();

                    // Use service


                    string fromStoreName = _tenantProviderSer.GetTenantName();
                    string toStoreName = _storeBussinessSer.GetStoreName(robbingOrder.ToStoreId);
                    NotificationVM fromNotification = new NotificationVM();
                    fromNotification.Id = id.ToString();
                    fromNotification.code = robbingOrder.Code;
                    fromNotification.notificationTemplateEnum = NotificationTemplateEnum.NTF_RobbingOrder_RequestFrom;
                    fromNotification.storeId = robbingOrder.FromStoreId.ToString();
                    fromNotification.FromStore = fromStoreName;
                    fromNotification.ToStore = toStoreName;
                    //send notifications to( مدير الإدارة الفنية و مدير المخازن )for the sending store
                    _notificationSer.SendNotification(fromNotification).Wait();
                    //send notifications to( مدير الإدارة الفنية و مدير المخازن و أمين المخزن )for the recieving store
                    NotificationVM toNotification = new NotificationVM();
                    toNotification.Id = id.ToString();
                    toNotification.code = robbingOrder.Code;
                    toNotification.notificationTemplateEnum = NotificationTemplateEnum.NTF_RobbingOrder_RequestTo;
                    toNotification.storeId = robbingOrder.ToStoreId.ToString();
                    toNotification.FromStore = fromStoreName;
                    toNotification.ToStore = toStoreName;
                    _notificationSer.SendNotification(toNotification).Wait();

                    // Dispose scope
                }
            }).Start();
            #endregion

            return id;

        }
    }
}
