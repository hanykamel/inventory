using AutoMapper;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.TransformationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.TransformationRequest.Handlers
{
   public class AddTransforemationHandler : IRequestHandler<AddTransforemationCommand, Guid>
    {
        private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private readonly ISubtractionBussiness _subtractionBussiness;

        public AddTransforemationHandler(ITransformationRequestBussiness transformationRequestBussiness,
            IStoreItemBussiness storeItemBussiness,
            IMapper mapper,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IStoreBussiness storeBussiness,
            ISubtractionBussiness subtractionBussiness
            )
        {
            _transformationRequestBussiness = transformationRequestBussiness;
            _storeItemBussiness = storeItemBussiness;
            _mapper = mapper;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
            _subtractionBussiness = subtractionBussiness;
        }

        public async Task<Guid> Handle(AddTransforemationCommand request, CancellationToken cancellationToken)
        {
            var transformation = new Transformation();
            transformation=_mapper.Map<Transformation>(request);

            //prepare StoreItems and add it to the transformation
            transformation.TransformationStoreItem = new List<TransformationStoreItem>();
            if (request.TransformationBaseItemList != null && request.TransformationBaseItemList.Count > 0)
                foreach (var BaseItem in request.TransformationBaseItemList)
                {

                    var getAvailableStoreItem = _storeItemBussiness.GetTransformationAvailableStoreItems
                        (BaseItem.BaseItemId, (int)request.BudgetId, BaseItem.StatusId, BaseItem.Count,BaseItem.ContractNum,BaseItem.PageNum);

                    foreach (var storeItem in getAvailableStoreItem)
                    {
                        transformation.TransformationStoreItem.Add
                                       (new TransformationStoreItem()
                                       {
                                           Id = Guid.NewGuid(),
                                           StoreItemId = storeItem.Id,
                                           Note = BaseItem.Note
                                       });
                    }
           
                }

            //Add subtraction details to transformation
            transformation.Subtraction = new List<Subtraction>();
            Subtraction subtraction = _mapper.Map<Subtraction>(request);
            subtraction = _subtractionBussiness.PrepareSubtraction(subtraction);
            transformation.Subtraction.Add(subtraction);

            //Add Attachment details to transformation
            transformation.TransformationAttachment = new List<TransformationAttachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    transformation.TransformationAttachment.Add(new TransformationAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }

            //Make the storeitems underdelete(so no ather operation can use it)
            _storeItemBussiness.MakeStoreItemUnderDelete(transformation.TransformationStoreItem.Select(x => x.StoreItemId).ToList());

            //save the transformation object to the database
            var succeed = await _transformationRequestBussiness.AddNewTransformation(transformation);

            
            //send notification to requested Store Managers
            var notification = PrepareNotification(transformation);
            await _notificationBussiness.SendNotification(notification);

            //send notification to requested to Store Managers
            notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Transformation_RequestTo;
            notification.storeId = Convert.ToString(transformation.ToStoreId);

            await _notificationBussiness.SendNotification(notification);
           

            return transformation.Id;
        }

        NotificationVM PrepareNotification(Transformation transformation)
        {
            string fromStoreName = _tenantProvider.GetTenantName();
            string toStoreName = _storeBussiness.GetStoreName(transformation.ToStoreId);
            NotificationVM notification = new NotificationVM();
            notification.Id =Convert.ToString( transformation.Id);
            notification.code = transformation.Code;
            notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Transformation_RequestFrom;
            notification.storeId = Convert.ToString( transformation.FromStoreId);
            notification.FromStore = fromStoreName;
            notification.ToStore = toStoreName;

            return notification;
        }
    }
}
