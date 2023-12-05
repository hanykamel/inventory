using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.NotificationVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AdditionRequest.Handlers
{
    public class CteateAdditionHandler : IRequestHandler<CreateAdditionCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IAdditionBussiness additionBussiness;
        private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly ICommiteeItemBussiness _commiteeItemBussiness;
        private readonly IExaminationBusiness _examinationBusiness;
        private readonly IExecutionOrderBussiness _executionOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ILogger<CteateAdditionHandler> _logger;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public IUnitOfWork UnitOfWork { get; }

        public CteateAdditionHandler(
            IMediator _mediator,
            IAdditionBussiness additionBussiness,
            ITransformationRequestBussiness transformationRequestBussiness,
            ICommiteeItemBussiness commiteeItemBussiness,
            IRobbingOrderBussiness robbingOrderBussiness,
            IExaminationBusiness examinationBusiness,
            IExecutionOrderBussiness executionOrderBussiness,
            IStoreItemBussiness storeItemBussiness,
            INotificationBussiness notificationBussiness,
            IUnitOfWork unitOfWork,
            ILogger<CteateAdditionHandler> logger,
            ITenantProvider tenantProvider,
            IMapper mapper,
            IStoreBussiness storeBussiness,
            IStringLocalizer<SharedResource> Localizer
            )
        {
            this._mediator = _mediator;
            this.additionBussiness = additionBussiness;
            _transformationRequestBussiness = transformationRequestBussiness;
            _commiteeItemBussiness = commiteeItemBussiness;
            _robbingOrderBussiness = robbingOrderBussiness;
            _examinationBusiness = examinationBusiness;
            _executionOrderBussiness = executionOrderBussiness;
            _storeItemBussiness = storeItemBussiness;
            _notificationBussiness = notificationBussiness;
            UnitOfWork = unitOfWork;
            _logger = logger;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _storeBussiness = storeBussiness;
            _Localizer = Localizer;
        }
        private List<StoreItem> GetStoreItems(CreateAdditionCommand request)
        {
            List<StoreItem> storeItemsOutput = new List<StoreItem>();
            if (request.Items != null && request.Items.Count > 0)
                foreach (var item in request.Items)
                {
                    var res = new List<StoreItem>();
                    //for examination
                    if (request.ExaminationCommitteId != null && request.ExaminationCommitteId != Guid.Empty)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            var storeItem = _mapper.Map<StoreItem>(item);
                            storeItem.Id = Guid.NewGuid();
                            storeItem.StoreId = request.StoreId;
                            storeItem.Note = item.Note;
                            storeItem.UnitId = item.UnitId;
                            storeItem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                            storeItem.CurrencyId = item.CurrencyId;
                            res.Add(storeItem);
                        }
                    }
                    //for transformations
                    else if (request.RequestId != null && request.RequestId != Guid.Empty)
                    {
                        foreach (var storeItemModel in item.StoreItems)
                        {
                            var storeItem = _mapper.Map<StoreItem>(storeItemModel);
                            storeItem.Id = Guid.NewGuid();
                            storeItem.BookId = item.BookId;
                            storeItem.BookPageNumber = item.BookPageNumber;
                            storeItem.BaseItemId = item.BaseItemId;
                            storeItem.Note = item.Note;
                            storeItem.UnitId = storeItemModel.UnitId;
                            storeItem.StoreId = _tenantProvider.GetTenant();
                            storeItem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                            storeItem.CurrencyId = item.CurrencyId;
                            res.Add(storeItem);
                        }
                    }
                    //for robbing
                    else if (request.RobbingOrderId != null && request.RobbingOrderId != Guid.Empty)
                    {
                        foreach (var storeItemModel in item.StoreItems)
                        {
                            var storeItem = _mapper.Map<StoreItem>(storeItemModel);
                            storeItem.Id = Guid.NewGuid();
                            storeItem.BookId = item.BookId;
                            storeItem.BookPageNumber = item.BookPageNumber;
                            storeItem.BaseItemId = item.BaseItemId;
                            storeItem.Price = item.RobbingPrice;
                            storeItem.StoreItemStatusId = item.StoreItemStatusId;
                            storeItem.Note = item.Note;
                            storeItem.UnitId = storeItemModel.UnitId;
                            storeItem.StoreId = _tenantProvider.GetTenant();
                            storeItem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                            storeItem.CurrencyId = item.CurrencyId;
                            res.Add(storeItem);
                        }
                    }
                    //for execution
                    else if (request.ExecutionOrderId != null && request.ExecutionOrderId != Guid.Empty)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            var storeItem = _mapper.Map<StoreItem>(item);
                            storeItem.Id = Guid.NewGuid();
                            storeItem.StoreId = request.StoreId;
                            storeItem.Note = item.Note;
                            storeItem.UnitId = item.UnitId;
                            storeItem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                            //storeItem.CurrencyId = item.CurrencyId;
                            res.Add(storeItem);
                        }

                    }
                    storeItemsOutput.AddRange(res);
                }
            return storeItemsOutput;
        }
        private List<RemainsDetails> GetRemainsItems(CreateAdditionCommand request)
        {
            List<RemainsDetails> remainsList = new List<RemainsDetails>();
            if (request.RemainsItems != null && request.RemainsItems.Count > 0)
                foreach (var item in request.RemainsItems)
                {
                    RemainsDetails remainsObj = new RemainsDetails()
                    {
                        Id = Guid.NewGuid(),
                        BookId = item.BookId,
                        BookPageNumber = item.BookPageNumber,
                        RemainsId = item.RemainsId,
                        RobbingName = item.RobbingName,
                        Price = item.RobbingPrice,
                        Quantity = item.Quantity,
                        UnitId = item.UnitId,
                        Notes = item.Note,
                        CurrencyId=item.CurrencyId
                    };
                    remainsList.Add(remainsObj);
                }
            return remainsList;
        }

        public async Task<Guid> Handle(CreateAdditionCommand request, CancellationToken cancellationToken)
        {
            var count = additionBussiness.GetAllAddition().Where(a => a.AdditionNumber == request.AdditionNumber).Count(); 
            if (count>0 && request.AdditionNumber!=null)
            {
                throw new NotSavedException(_Localizer["InvalidAdditionNumber"]);
            }
            string transformationOrRobbingCode = null;
            int toStoreId = 0;
            var transformationOrRobbingId = new Guid();
            NotificationTemplateEnum notificationTemplateEnumFrom = new NotificationTemplateEnum();
            NotificationTemplateEnum notificationTemplateEnumTo = new NotificationTemplateEnum();

            var addition = _mapper.Map<Addition>(request);
            addition.StoreItem = new List<StoreItem>();
            //get store items from request
            var storeItems = GetStoreItems(request);
            if (storeItems != null && storeItems.Count > 0)
                (addition.StoreItem as List<StoreItem>).AddRange(storeItems);

            else if (request.RobbingOrderId != null && request.RobbingOrderId != Guid.Empty && request.RemainsItems != null && request.RemainsItems.Count > 0)
                addition.RemainsDetails = GetRemainsItems(request);

            //insert attachments
            addition.AdditionAttachment = new List<AdditionAttachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    addition.AdditionAttachment.Add(new AdditionAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }

            //update robbing
            if (request.RobbingOrderId != null && request.RobbingOrderId != Guid.Empty)
            {
                //deactivate the store items in the from store
                if (addition.StoreItem != null && addition.StoreItem.Count > 0)
                    _storeItemBussiness.DeActivateRobbingStoreItem((Guid)request.RobbingOrderId);
                //else if (request.RemainsItems != null && request.RemainsItems.Count > 0)
                //   _robbingOrderBussiness.DeActivateRobbingRemainsItem((Guid)request.RobbingOrderId);
                _robbingOrderBussiness.UpdateRobbingOrderStatus((Guid)request.RobbingOrderId);
                notificationTemplateEnumFrom = NotificationTemplateEnum.NTF_RobbingOrder_Addition;
                notificationTemplateEnumTo = NotificationTemplateEnum.NTF_RobbingOrder_Addition_To_Sender;
                var robbingOrder = _robbingOrderBussiness.GetById(request.RobbingOrderId.Value);


                transformationOrRobbingId = robbingOrder.Id;
                transformationOrRobbingCode = robbingOrder.Code;
                toStoreId = robbingOrder.FromStoreId;
            }

            //update transformation 
            if (request.RequestId != null && request.RequestId != Guid.Empty)
            {
                //deactivate the store items in the from store
                _storeItemBussiness.DeActivateTransformationStoreItem((Guid)request.RequestId);

                _transformationRequestBussiness.UpdateTransformationStatus((Guid)request.RequestId);
                notificationTemplateEnumFrom = NotificationTemplateEnum.NTF_Transformation_Addition;
                notificationTemplateEnumTo = NotificationTemplateEnum.NTF_Transformation_Addition_To_Sender;
                var transformation = _transformationRequestBussiness.GetById(request.RequestId.Value);

                transformationOrRobbingId = transformation.Id;
                transformationOrRobbingCode = transformation.Code;
                toStoreId = transformation.FromStoreId;
            }

            //update examination and committe items notes
            if (request.ExaminationCommitteId != null && request.ExaminationCommitteId != Guid.Empty)
            {
                notificationTemplateEnumFrom = NotificationTemplateEnum.NTF_Addition;
                foreach (var item in request.Items)
                {
                    if (addition.StoreItem.Count > 0)
                        _storeItemBussiness.GenerateBarcode(addition.StoreItem.Where(o => o.BaseItemId == item.BaseItemId).ToList(),
                            addition.BudgetId, item.BaseItemId);
                }

                _examinationBusiness.updateExaminationForAddition(request.ExaminationCommitteId,
                        request.Items.Select(o => o.Id).ToList()
                           , request.Items.Select(o => o.Note).ToList());

            }
            //update execution order
            if (request.ExecutionOrderId != null && request.ExecutionOrderId != Guid.Empty)
            {
                notificationTemplateEnumFrom = NotificationTemplateEnum.NTF_Addition;
                foreach (var item in request.Items)
                {
                    if (addition.StoreItem.Count > 0)
                        _storeItemBussiness.GenerateBarcode(addition.StoreItem.Where(o => o.BaseItemId == item.BaseItemId).ToList(),
                            addition.BudgetId, item.BaseItemId);
                }
                _executionOrderBussiness.UpdateExecutionOrderAfterAddition(request.ExecutionOrderId,
                        request.Items.Select(o => o.Id).ToList()
                           , request.Items.Select(o => o.Note).ToList());

            }



            if (request.RobbingStoreItem !=null && request.RobbingStoreItem.Count != 0)
            {
                foreach (var item in request.RobbingStoreItem)
                {
                    RobbedStoreItem obj = _mapper.Map<RobbedStoreItem>(item);
                    obj.Id = Guid.NewGuid();
                    obj.IsActive = true;
                    addition.RobbedStoreItem.Add(obj);
                }
                // DeActivate Store Item that replace by new Item
                _storeItemBussiness.DeActivateRobbingStoreItem((Guid)request.RobbingOrderId);

            }
            var id = await additionBussiness.AddNewAddition(addition);

            NotificationVM notificationFrom = new NotificationVM();
            notificationFrom.Id = id.ToString();
            notificationFrom.code = addition.Code;
            notificationFrom.notificationTemplateEnum = notificationTemplateEnumFrom;
            //notification.storeId = request.StoreId.ToString();
            notificationFrom.storeId = _tenantProvider.GetTenant().ToString();
            notificationFrom.FromStore = _tenantProvider.GetTenantName();
            await _notificationBussiness.SendNotification(notificationFrom);
            if ((request.RequestId != null && request.RequestId != Guid.Empty) || (request.RobbingOrderId != null && request.RobbingOrderId != Guid.Empty))
            {
                string toStoreName = _storeBussiness.GetStoreName(toStoreId);
                NotificationVM notificationTo = new NotificationVM();
                notificationTo.Id = transformationOrRobbingId.ToString();
                notificationTo.code = transformationOrRobbingCode;
                notificationTo.notificationTemplateEnum = notificationTemplateEnumTo;
                //notification.storeId = request.StoreId.ToString();
                notificationTo.storeId = Convert.ToString(toStoreId);
                notificationTo.FromStore = toStoreName;
                notificationTo.ToStore = _tenantProvider.GetTenantName();
                await _notificationBussiness.SendNotification(notificationTo);
            }

            return id;
        }

    }
}