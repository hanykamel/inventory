using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.ExecutionOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Handlers
{
    class CreateExecutionOrderHandler : IRequestHandler<CreateExecutionOrderCommand, Guid>
    {
        private readonly IExecutionOrderBussiness _ExecutionOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private IServiceScopeFactory factory;
        private readonly ISubtractionBussiness _subtractionBussiness;
        public CreateExecutionOrderHandler(IExecutionOrderBussiness ExecutionOrderBussiness,
        IStoreItemBussiness storeItemBussiness,
            IMapper mapper,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IStoreBussiness storeBussiness,
            IServiceScopeFactory _factory,
            ISubtractionBussiness subtractionBussiness
            )
        {
            _ExecutionOrderBussiness = ExecutionOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _mapper = mapper;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
            factory = _factory;
            _subtractionBussiness = subtractionBussiness;
        }


        public async Task<Guid> Handle(CreateExecutionOrderCommand request, CancellationToken cancellationToken)
        {
            var executionOrder = new ExecutionOrder();
            executionOrder = _mapper.Map<ExecutionOrder>(request);
            executionOrder.Id = Guid.NewGuid();
            //executionOrder.Date = DateTime.Now;
            executionOrder.StoreId = _tenantProvider.GetTenant();
            executionOrder.ExecutionOrderStoreItem = new List<ExecutionOrderStoreItem>();
            if (request.ExecutionOrderBaseItemList != null && request.ExecutionOrderBaseItemList.Count > 0)
            {
                foreach (var Storeitem in request.ExecutionOrderBaseItemList)
                {
                    executionOrder.ExecutionOrderStoreItem.Add
                                   (new ExecutionOrderStoreItem()
                                   {
                                       Id = Guid.NewGuid(),
                                       StoreItemId = Storeitem.storeitemId,
                                       Note = Storeitem.Note,
                                       IsApproved=false

                                   });
  
                }
            }

            executionOrder.Subtraction = new List<Subtraction>();
            Subtraction subtraction = _mapper.Map<Subtraction>(request);
            subtraction = _subtractionBussiness.PrepareSubtraction(subtraction);
            executionOrder.Subtraction.Add(subtraction);

            executionOrder.ExecutionOrderAttachment = new List<ExecutionOrderAttachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    executionOrder.ExecutionOrderAttachment.Add(new ExecutionOrderAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }


            // _storeItemBussiness.DeActivateStoreItem(request.robbingOrderStorelist.Select(x => x.StoreItemId).ToList());

            _storeItemBussiness.MakeStoreItemUnderExecution(request .ExecutionOrderBaseItemList.Select(x => x.storeitemId).ToList());


            var id = await _ExecutionOrderBussiness.AddNewExecutionOrder(executionOrder);



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

                    NotificationVM fromNotification = new NotificationVM();
                    fromNotification.Id = id.ToString();
                    fromNotification.code = executionOrder.Code;
                    fromNotification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Create_Execution_Request;
                    fromNotification.storeId = executionOrder.TenantId.ToString();
                    fromNotification.FromStore = _tenantProviderSer.GetTenantName();
                    _notificationSer.SendNotification(fromNotification).Wait();
                    // Dispose scope
                }
            }).Start();
            #endregion

            return id;
        }
    }
}