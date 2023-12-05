using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RobbingOrderRequest.Handlers
{
  public  class AddRobbingOrderHandler : IRequestHandler<AddRobbingOrderCommand, Guid>
    {
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper; 
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private IServiceScopeFactory factory;
        private readonly ISubtractionBussiness _subtractionBussiness;
        public AddRobbingOrderHandler(IRobbingOrderBussiness robbingOrderBussiness,
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

        public async Task<Guid> Handle(AddRobbingOrderCommand request, CancellationToken cancellationToken)
        {


            var robbingOrder = new RobbingOrder();
            robbingOrder = _mapper.Map<RobbingOrder>(request);
            robbingOrder.Id = Guid.NewGuid();


            robbingOrder.RobbingOrderStoreItem = new List<RobbingOrderStoreItem>();
            if (request.RobbingOrderBaseItemList != null && request.RobbingOrderBaseItemList.Count > 0)
            {
                foreach (var Storeitem in request.RobbingOrderBaseItemList)
                {

                    //var getAvailableStoreItem = _storeItemBussiness.GetAllStoreItems()
                    // .Where(x => x.BaseItemId == BaseItem.BaseItemId&&
                    // x.Addition.BudgetId == request.BudgetId&&
                    // x.CurrentItemStatusId == (int)ItemStatusEnum.Available 
                    // && x.UnderDelete != true)
                    //  .Take(BaseItem.Count)
                    //    .Select(x => x.Id);

                    //foreach (var storeItemId in getAvailableStoreItem)
                    //{
                        robbingOrder.RobbingOrderStoreItem.Add
                                       (new RobbingOrderStoreItem()
                                       {
                                           Id = Guid.NewGuid(),
                                           StoreItemId = Storeitem.storeitemId,
                                           Notes = Storeitem.Note,
                                           Price= Storeitem.price,
                                           ExaminationReport= Storeitem.ExaminationReport,
                                           StoreItemStatusId= Storeitem.RobbingStoreItemStatusId,
                                       });
                   // }

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


            //_storeItemBussiness.DeActivateStoreItem(request.robbingOrderStorelist.Select(x => x.StoreItemId).ToList());

            _storeItemBussiness.MakeStoreItemUnderDelete(robbingOrder.RobbingOrderStoreItem.Select(x => x.StoreItemId).ToList());


            var id  = await _robbingOrderBussiness.AddNewRobbingOrder(robbingOrder);
          


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