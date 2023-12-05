using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.DelegationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.DelegationRequest.Handlers
{
  public  class StopDelegationHandler : IRequestHandler<StopDelegationCommand, bool>
    {

        private readonly IDelegationBussiness _iDelegationBussiness;
        private readonly IStoreBussiness _storeBuissness;
        private readonly INotificationBussiness _notificationBussiness;
        public StopDelegationHandler(IDelegationBussiness iDelegationBussiness,
            IStoreBussiness storeBuissness,
            INotificationBussiness notificationBussiness
            )
        {
            _iDelegationBussiness = iDelegationBussiness;
            _storeBuissness = storeBuissness;
            _notificationBussiness = notificationBussiness;
        }

        public async Task<bool> Handle(StopDelegationCommand request, CancellationToken cancellationToken)
        {
            var result = await _iDelegationBussiness.StopDelegation(request.Id);
            if (result)
            {
                var delegation = _iDelegationBussiness.GetAllDelegation()
               .Include(d => d.DelegationStore)
               .ThenInclude(s => s.Store)
               .Where(d => d.Id == request.Id).FirstOrDefault();
                if (delegation.DelegationTypeId == (int)DelegationTypeEnum.Technican)
                {
                    NotificationVM notificationTOTechManager = new NotificationVM();
                    //notificationTOTechManager.TechManager = _userBusiness.GetTechnicalUserName(delegation.DelegationStore.FirstOrDefault().StoreId);
                    foreach (var store in delegation.DelegationStore)
                    {
                        notificationTOTechManager.Id = delegation.Id.ToString();
                        notificationTOTechManager.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Delegated_User;
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
                        notificationTOStoreAdmin.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Delegation_Store;
                        notificationTOStoreAdmin.storeId = store.StoreId.ToString();
                        notificationTOStoreAdmin.ToStore = _storeBuissness.GetStoreName(store.StoreId);
                        notificationTOStoreAdmin.FromStoreAdmin = delegation.UserName; //delegated User's Name;
                        await _notificationBussiness.SendNotification(notificationTOStoreAdmin);

                        NotificationVM notificationToDelegatedUser = new NotificationVM();
                        notificationToDelegatedUser.Id = delegation.Id.ToString();
                        notificationToDelegatedUser.notificationTemplateEnum = NotificationTemplateEnum.NTF_Cancel_Delegated_User;
                        notificationToDelegatedUser.storeId = null;
                        notificationToDelegatedUser.FromStore = _storeBuissness.GetStoreName(store.StoreId);
                        notificationToDelegatedUser.FromStoreAdmin = delegation.UserName; //delegated User's Name;
                        notificationToDelegatedUser.ToStoreAdmin = store.Store.AdminId;
                        notificationToDelegatedUser.Users = new List<string>();
                        notificationToDelegatedUser.Users.Add(delegation.UserName);
                        await _notificationBussiness.SendNotification(notificationToDelegatedUser);
                    }
                }
            }
            return result;
        }
    }
}
