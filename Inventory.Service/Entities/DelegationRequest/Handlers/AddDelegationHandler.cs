using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.DelegationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.DelegationRequest.Handlers
{
   public class AddDelegationHandler : IRequestHandler<AddDelegationCommand, AddDelegationReturnVm>
    {

        private readonly IDelegationBussiness _iDelegationBussiness;
        private readonly IMapper _mapper;
        private readonly IStoreBussiness _storeBuissness;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IUserBusiness _userBusiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public AddDelegationHandler(IDelegationBussiness iDelegationBussiness, IMapper mapper,
            IStoreBussiness storeBuissness, INotificationBussiness notificationBussiness , ITenantProvider tenantProvider,
            IUserBusiness userBusiness, IStringLocalizer<SharedResource> Localizer)
        {
            _iDelegationBussiness = iDelegationBussiness;
            _mapper = mapper;
            _storeBuissness = storeBuissness;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _userBusiness = userBusiness;
            _Localizer = Localizer;
        }

        public async Task<AddDelegationReturnVm> Handle(AddDelegationCommand request, CancellationToken cancellationToken)
        {
            if (request.DelegationStores.Count == 0)
            {
                throw new NotSavedException(_Localizer["NotDelegationStores"]);
            }
            Delegation delegation = new Delegation();
            AddDelegationReturnVm model = new AddDelegationReturnVm();
             delegation = _mapper.Map<Delegation>(request);
            delegation.Id= Guid.NewGuid();
            IQueryable<Delegation> checkDateInterception = null;
            if (request.DelegationType==(int) DelegationTypeEnum.StoreAdmin)
            {
                 checkDateInterception = _iDelegationBussiness.checkDelegation(delegation, request.DelegationStores);
            }
            else
            {
                 _iDelegationBussiness.checkDelegationTechnican(delegation, request.DelegationStores);
            }
            if (checkDateInterception != null && checkDateInterception.Any())
            {
                model.storeName = new List<string>();
                foreach (var item in checkDateInterception.ToList())
                {
                    foreach (var store in item.DelegationStore)
                    {
                        if (request.DelegationStores.Contains(store.StoreId))
                        {
                            model.storeName.Add(store.Store.RobbingBudgetId != null ? store.Store.StoreType.Name + " " + store.Store.RobbingBudget.Name : store.Store.StoreType.Name + " " + store.Store.TechnicalDepartment.Name);
                        }

                    }
                }

            }
            else
            {
                foreach (var item in request.DelegationStores)
                {
                    var delegationstore = new DelegationStore();
                    delegationstore.StoreId = item;
                    delegationstore.Id = Guid.NewGuid();
                    delegationstore.DelegationId = delegation.Id;
                    delegation.DelegationStore.Add(delegationstore);
                }
                var returndelegation = await _iDelegationBussiness.SaveDelegation(delegation);
                if (returndelegation.DelegationTypeId == (int)DelegationTypeEnum.Technican)
                {
                    NotificationVM notificationTOTechManager = new NotificationVM();
                    //notificationTOTechManager.TechManager = _userBusiness.GetTechnicalUserName(delegation.DelegationStore.FirstOrDefault().StoreId);
                    foreach (var store in delegation.DelegationStore)
                    {
                        notificationTOTechManager.Id = delegation.Id.ToString();
                        notificationTOTechManager.notificationTemplateEnum = NotificationTemplateEnum.NTF_Delegation_Technician;
                        notificationTOTechManager.FromStore = _storeBuissness.GetStoreName(store.StoreId);
                        notificationTOTechManager.Users = new List<string>();
                        notificationTOTechManager.Users.Add(delegation.UserName);
                        await _notificationBussiness.SendNotification(notificationTOTechManager);
                    }
                }
                else
                {
                    foreach (var store in delegation.DelegationStore)
                    {
                        NotificationVM notificationTOStoreAdmin = new NotificationVM();
                        notificationTOStoreAdmin.Id = delegation.Id.ToString();
                        notificationTOStoreAdmin.notificationTemplateEnum = NotificationTemplateEnum.NTF_Delegation_Store;
                        notificationTOStoreAdmin.storeId = store.StoreId.ToString();
                        notificationTOStoreAdmin.ToStore = _storeBuissness.GetStoreName(store.StoreId);
                        notificationTOStoreAdmin.FromStoreAdmin = delegation.UserName; //delegated User's Name;
                        await _notificationBussiness.SendNotification(notificationTOStoreAdmin);

                        NotificationVM notificationToDelegatedUser = new NotificationVM();
                        notificationToDelegatedUser.Id = delegation.Id.ToString();
                        notificationToDelegatedUser.notificationTemplateEnum = NotificationTemplateEnum.NTF_Delegated_User;
                        notificationToDelegatedUser.storeId = null;
                        notificationToDelegatedUser.FromStore = _storeBuissness.GetStoreName(store.StoreId);
                        notificationToDelegatedUser.FromStoreAdmin = delegation.UserName; //delegated User's Name;
                        notificationToDelegatedUser.ToStoreAdmin = store.Store.AdminId;
                        notificationToDelegatedUser.Users = new List<string>();
                        notificationToDelegatedUser.Users.Add(delegation.UserName);
                        await _notificationBussiness.SendNotification(notificationToDelegatedUser);
                    }
                }
                model.Id = returndelegation.Id;
            }
            return model;
        }
    }
}
