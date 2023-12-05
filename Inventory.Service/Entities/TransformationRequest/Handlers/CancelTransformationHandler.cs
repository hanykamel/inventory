using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.TransformationRequest.Handlers
{
   public class CancelTransformationHandler : IRequestHandler<CancelTransformationCommand, bool>
    {
        private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private IServiceScopeFactory _factory;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public CancelTransformationHandler(ITransformationRequestBussiness transformationRequestBussiness,
            IStoreItemBussiness storeItemBussiness,
            IServiceScopeFactory factory,
            IStringLocalizer<SharedResource> Localizer)
        {
            _transformationRequestBussiness = transformationRequestBussiness;
            _storeItemBussiness = storeItemBussiness;
            _factory = factory;
            _Localizer = Localizer;
        }

        public async Task<bool> Handle(CancelTransformationCommand request, CancellationToken cancellationToken)
        {
           var transformation= _transformationRequestBussiness.GetById(request.TransformationId);
            if (transformation.TransformationStatusId == (int)TransformationOrderStatusEnum.Requested)
            {
            _storeItemBussiness.CancelStoreItemUnderDelete(transformation.TransformationStoreItem.Select(x=>x.StoreItemId).ToList());
            _transformationRequestBussiness.CancelTransformation(transformation);
            var result = await _transformationRequestBussiness.Save();
                #region Background_thread_notification
                // Create new  Background thread for notification
                if (result)
                {
                    new Thread(() =>
                    {
                        using (var scope = this._factory.CreateScope())
                        {
                            // create  service For this Scope and  Dispose when scope finish and thread finish
                            var _tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                            var _notificationBussiness = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();
                            var _storeBussiness = scope.ServiceProvider.GetRequiredService<IStoreBussiness>();

                            // Use service

                            string fromStoreName = _storeBussiness.GetStoreName(transformation.FromStoreId);
                            string toStoreName = _storeBussiness.GetStoreName(transformation.ToStoreId);
                            NotificationVM Notification = new NotificationVM();
                            Notification.Id = request.TransformationId.ToString();
                            Notification.code = transformation.Code;
                            Notification.FromStore = fromStoreName;
                            Notification.ToStore = toStoreName;
                            //send notifications to( مدير الإدارة الفنية و مدير المخازن )for the sending store
                            Notification.storeId = transformation.FromStoreId.ToString();
                            Notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Transformation_RequestFrom;
                            _notificationBussiness.SendNotification(Notification).Wait();
                            //send notifications to( مدير الإدارة الفنية و مدير المخازن )for the recieving store
                            Notification.storeId = transformation.ToStoreId.ToString();
                            Notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Transformation_RequestTo;
                            _notificationBussiness.SendNotification(Notification).Wait();
                            // Dispose scope
                        }
                    }).Start();
                    #endregion
                }

                return result;
            }
            throw new InvalidCanceledTransformation(_Localizer["CanceledTransformation"]);
        }
    }
}
