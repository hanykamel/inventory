using Inventory.Service.Entities.DeductionRequest.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Unit = Inventory.Data.Entities.Unit;
using System.Linq;
using Inventory.Data.Models.NotificationVM;
using Inventory.CrossCutting.Tenant;
using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.DeductionRequest.Handlers
{
    class SaveDeductionHandler : IRequestHandler<SaveDeductionCommand, Guid>
    {
        private readonly IDeductionBusiness _IDeductionBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IMapper _mapper;
        private readonly ITenantProvider _tenantProvider;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ISubtractionBussiness _subtractionBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public SaveDeductionHandler(IDeductionBusiness IDeductionBusiness, IStoreItemBussiness storeItemBussiness,
            ITenantProvider tenantProvider,
            INotificationBussiness notificationBussiness,
            IMapper mapper,
            ISubtractionBussiness subtractionBussiness,
            IStringLocalizer<SharedResource> localizer
            )
        {
            _IDeductionBusiness = IDeductionBusiness;
            _storeItemBussiness = storeItemBussiness;
            _tenantProvider = tenantProvider;
            _notificationBussiness = notificationBussiness;
            _mapper = mapper;
            _subtractionBussiness = subtractionBussiness;
            _Localizer = localizer;
        }
        public async Task<Guid> Handle(SaveDeductionCommand request, CancellationToken cancellationToken)
        {
            var subtractionExist=_subtractionBussiness.CheckSubtractionExist(int.Parse(request.SubtractionNumber));

            if (subtractionExist)
            {
                throw new NotSavedException(_Localizer["InvalidAdditionNumber"]);
            }
            Deduction deduction = new Deduction();
            //deduction.RequesterName = request.RequesterName;
            //deduction.RequestDate = request.RequestDate;
            deduction.Code = request.RequestCode;
            deduction.Notes = request.Notes;

            deduction.BudgetId = request.BudgetId;//(int)BudgetNamesEnum.ArmedForces;
            deduction.OperationId = (int)OperationEnum.Deduction;
            deduction.Id = Guid.NewGuid();
            List<long> baseItemIds = request.deductItems.Select(x=>x.BaseItemId).ToList();
           
         

            //get storeitems if not empty 
            //List<Guid> storeItemIds = new List<Guid>();
          var  Result = _IDeductionBusiness.getStoreItemId(baseItemIds);


            foreach (var Item in Result)
            {
                DeductionStoreItem model = new DeductionStoreItem();         
                model.Id =Guid.NewGuid();
                model.StoreItemId = Item.storeItemId;
                model.Note = request.deductItems.FirstOrDefault(x=>x.BaseItemId== Item.BaseItemId).Note;
                deduction.DeductionStoreItem.Add(model);
            }
            deduction.Subtraction = new List<Subtraction>();
            Subtraction subtraction = _mapper.Map<Subtraction>(request);
            subtraction = _subtractionBussiness.PrepareSubtraction(subtraction);
            deduction.Subtraction.Add(subtraction);

            _storeItemBussiness.DeActivateStoreItem(Result.Select(x=>x.storeItemId).ToList());
            var Id = await _IDeductionBusiness.AddNewDeductItem(deduction);

            //Send Notification
            NotificationVM notification = new NotificationVM();
            notification.Id = Id.ToString();
            notification.code = deduction.Code;
            notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Deduction;
            //notification.storeId = request.StoreId.ToString();
            notification.storeId = deduction.TenantId.ToString();
            notification.FromStore = _tenantProvider.GetTenantName();
            await _notificationBussiness.SendNotification(notification);

            return Id;
        }
    }
}
