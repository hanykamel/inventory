using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.NotificationVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AdditionRequest.Handlers
{
    public class EditAdditionHandler : IRequestHandler<EditAdditionCommand, bool>
    {
        //private readonly IMediator _mediator;
        private readonly IAdditionBussiness additionBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly INotificationBussiness _notificationBussiness;
        //private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        //private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        //private readonly ICommiteeItemBussiness _commiteeItemBussiness;
        //private readonly IExaminationBusiness _examinationBusiness;
        //private readonly IStoreItemBussiness _storeItemBussiness;
        //private readonly INotificationBussiness _notificationBussiness;
        //private readonly ILogger<CteateAdditionHandler> _logger;
        //private readonly ITenantProvider _tenantProvider;
        //private readonly IMapper _mapper;

        public IUnitOfWork UnitOfWork { get; }

        public EditAdditionHandler(IAdditionBussiness additionBussiness,
            ITenantProvider tenantProvider,
            INotificationBussiness notificationBussiness)
        {
            this.additionBussiness = additionBussiness;
            _tenantProvider = tenantProvider;
            _notificationBussiness = notificationBussiness;
        }

        public async Task<bool> Handle(EditAdditionCommand request, CancellationToken cancellationToken)
        {
            var result = await additionBussiness.UpdateAddition(request);
            if (result)
            {
                NotificationVM notification = new NotificationVM();
                notification.Id = request.AdditionId.ToString();
                notification.code = request.Code;
                notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Addition_Edit;
                notification.storeId = Convert.ToString(request.StoreId);
                notification.FromStore = _tenantProvider.GetTenantName();
                await _notificationBussiness.SendNotification(notification);
            }
            return result;
        }

    }
}