using Inventory.CrossCutting.Tenant;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Handlers
{
   public class CancelExecutionOrderHandler : IRequestHandler<CancelExecutionOrderCommand, bool>
    {
        private readonly IExecutionOrderBussiness _ExecutionOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private IServiceScopeFactory _factory;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly ITenantProvider _tenantProvider;

        public CancelExecutionOrderHandler(IExecutionOrderBussiness ExecutionOrderBussiness,
        IStoreItemBussiness storeItemBussiness,
          IServiceScopeFactory factory,
          IStringLocalizer<SharedResource> Localizer,
          ITenantProvider tenantProvider
            )
        {
            _ExecutionOrderBussiness = ExecutionOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _factory = factory;
            _Localizer = Localizer;
            _tenantProvider = tenantProvider; 
        }


        public async Task<bool> Handle(CancelExecutionOrderCommand request, CancellationToken cancellationToken)
        {

            var result=await   _ExecutionOrderBussiness.CancelExecutionOrderAsync(request.ExecutionOrderId);
            var executionOrder = _ExecutionOrderBussiness.GetById(request.ExecutionOrderId);
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
                    NotificationVM fromNotification = new NotificationVM();
                    fromNotification.Id = request.ExecutionOrderId.ToString();
                    fromNotification.code = executionOrder.Code;
                    fromNotification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Execution_Request;
                    fromNotification.storeId = executionOrder.TenantId.ToString();
                    fromNotification.FromStore = _tenantProviderSer.GetTenantName();
                    //send notifications to( مدير الإدارة الفنية و مدير المخازن )for the sending store
                    _notificationSer.SendNotification(fromNotification).Wait();
                    // Dispose scope

                }
            }).Start();
            #endregion

            return result;
        }
    }
}