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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Handlers
{
  public  class CreateExecutionOrderAfterReviewHandler : IRequestHandler<CreateExecutionOrderAfterReviewCommand, bool>
    {
        private readonly IExecutionOrderBussiness _ExecutionOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private IServiceScopeFactory _factory;
        public CreateExecutionOrderAfterReviewHandler(IExecutionOrderBussiness ExecutionOrderBussiness,
        IStoreItemBussiness storeItemBussiness,
            IMapper mapper,
            ITenantProvider tenantProvider,
            IStoreBussiness storeBussiness,
            IServiceScopeFactory factory
            )
        {
            _ExecutionOrderBussiness = ExecutionOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
            _factory = factory;
        }

        public async Task<bool> Handle(CreateExecutionOrderAfterReviewCommand request, CancellationToken cancellationToken)
        {
            List<ExecutionOrderResultRemain> modelRemains = new List<ExecutionOrderResultRemain>();
            List<ExecutionOrderResultItem> modelItems = new List<ExecutionOrderResultItem>();
            List<Attachment> allAttachment = new List<Attachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    allAttachment.Add(obj);
                }
            foreach (var item in request.ExecutionOrderResultRemainList)
            {
                modelRemains.Add(_mapper.Map<ExecutionOrderResultRemain>(item));
            }

            foreach (var item in request.ExecutionOrderResultItemList)
            {
                modelItems.Add(_mapper.Map<ExecutionOrderResultItem>(item));
            }


          var result=await  _ExecutionOrderBussiness.AddNewExecutionOrderReminsAndItem(request.ExecutionOrderId,modelItems, modelRemains,  allAttachment);
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
                    fromNotification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Create_Execution_After_Review_Request
                    ;
                    fromNotification.storeId = executionOrder.TenantId.ToString();
                    fromNotification.FromStore = _tenantProviderSer.GetTenantName();
                    _notificationSer.SendNotification(fromNotification).Wait();
                    // Dispose scope
                }
            }).Start();
            #endregion
            return result;


        }
    }
}
