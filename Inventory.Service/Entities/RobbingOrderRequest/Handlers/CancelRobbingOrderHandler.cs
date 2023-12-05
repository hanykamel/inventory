using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RobbingOrderRequest.Handlers
{
   public class CancelRobbingOrderHandler : IRequestHandler<CancelRobbingOrderCommand, bool>
    {
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private IServiceScopeFactory _factory;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public CancelRobbingOrderHandler(IRobbingOrderBussiness robbingOrderBussiness,
        IStoreItemBussiness storeItemBussiness,
          IServiceScopeFactory factory,
          IStringLocalizer<SharedResource> Localizer
            )
        {
            _robbingOrderBussiness = robbingOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _factory = factory;
            _Localizer = Localizer;
        }


        public async Task<bool> Handle(CancelRobbingOrderCommand request, CancellationToken cancellationToken)
        {
            var robbingOrder = _robbingOrderBussiness.GetById(request.RobbingOrderId);
            if (robbingOrder.RobbingOrderStatusId ==(int) RobbingOrderStatusEnum.Requested)
            {
            _storeItemBussiness.CancelStoreItemUnderDelete(robbingOrder.RobbingOrderStoreItem.Select(R=>R.StoreItemId).ToList());
            _robbingOrderBussiness.CancelRobbingOrder(robbingOrder);
            var result = await _robbingOrderBussiness.Save();
            #region Background_thread_notification
            // Create new  Background thread for notification
            new Thread(() =>
            {
                using (var scope = this._factory.CreateScope())
                {
                    // create  service For this Scope and  Dispose when scope finish and thread finish
                    var _tenantProviderSer = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                    var _notificationSer = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();
                    var _storeBussinessSer = scope.ServiceProvider.GetRequiredService<IStoreBussiness>();

                    // Use service
                    string fromStoreName = _tenantProviderSer.GetTenantName();
                    string toStoreName = _storeBussinessSer.GetStoreName(robbingOrder.ToStoreId);
                    NotificationVM Notification = new NotificationVM();
                    Notification.Id = robbingOrder.Id.ToString();
                    Notification.code = robbingOrder.Code;
                    Notification.FromStore = fromStoreName;
                    Notification.ToStore = toStoreName;
                    //send notifications to( مدير الإدارة الفنية و مدير المخازن )for the sending store
                    Notification.storeId = robbingOrder.FromStoreId.ToString();
                    Notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestFrom;
                    _notificationSer.SendNotification(Notification).Wait();
                    //send notifications to( مدير الإدارة الفنية و مدير المخازن و أمين المخزن )for the recieving store
                    Notification.storeId = robbingOrder.ToStoreId.ToString();
                    Notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestTo;
                    _notificationSer.SendNotification(Notification).Wait();

                    // Dispose scope
                }
            }).Start();
            #endregion

            return result;
            }
            throw new InvalidCanceledRobbingOrder(_Localizer["CanceledRobbingOrder"]);

        }
    }
}