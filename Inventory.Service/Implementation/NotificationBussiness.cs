using inventory.Engines.LdapAuth;
using Inventory.CrossCutting.NotificationHub;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class NotificationBussiness : INotificationBussiness
    {

        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IUserBusiness _userBusiness;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        private readonly IRepository<Notification, Guid> _notificationRepository;
        private readonly IRepository<UserNotification, Guid> _userNotificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IRepository<NotificationTemplate, int> _notificationTemplateRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILogger<NotificationBussiness> _logger;

        public NotificationBussiness(
            IStringLocalizer<SharedResource> Localizer,
            IUserBusiness userBusiness,
            ILdapAuthenticationService ldapAuthenticationService,
            IRepository<Notification, Guid> notificationRepository,
            IRepository<UserNotification, Guid> userNotificationRepository,
            IHubContext<NotificationHub> hubContext,
            ILogger<NotificationBussiness> logger,
        IRepository<NotificationTemplate, int> notificationTemplateRepository,
            ITenantProvider tenantProvider
            )
        {
            _Localizer = Localizer;
            _userBusiness = userBusiness;
            _ldapAuthenticationService = ldapAuthenticationService;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _hubContext = hubContext;
            _logger = logger;
            _notificationTemplateRepository = notificationTemplateRepository;
            _tenantProvider = tenantProvider;
        }

        private string ReplaceTemplate(string template, Dictionary<int, string> notificationValues)
        {
            foreach (var item in notificationValues)
            {
                template = template.Replace("[" + item.Key + "]", item.Value);
            }
            return template;
        }

        private List<NotificationUserVM> FindUsers(NotificationTemplateEnum notificationTemplateEnum, Dictionary<int, string> notificationValues)
        {
            var isDelegated = true;
            var users = new List<NotificationUserVM>();
            if (notificationValues.ContainsKey((int)NotificationValuesEnum.StoreId) && notificationValues[(int)NotificationValuesEnum.StoreId] != null)
            {
                var storeId = notificationValues[(int)NotificationValuesEnum.StoreId];
                if (_tenantProvider.ChcekIsDelegated())
                {
                    isDelegated = false;
                    users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), false)));
                }
                switch (notificationTemplateEnum)
                {
                    case NotificationTemplateEnum.NTF_Addition:
                    case NotificationTemplateEnum.NTF_Deduction:
                    case NotificationTemplateEnum.NTF_RobbingOrder_RequestFrom:
                    case NotificationTemplateEnum.NTF_Transformation_RequestFrom:
                    case NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestFrom:
                    case NotificationTemplateEnum.NTF_Cancel_Transformation_RequestFrom:
                    case NotificationTemplateEnum.NTF_Transformation_Addition:
                    case NotificationTemplateEnum.NTF_Invoice:
                    case NotificationTemplateEnum.NTF_Addition_Edit:
                    case NotificationTemplateEnum.NTF_Cancel_Execution_Request:
                    case NotificationTemplateEnum.NTF_Create_Execution_After_Review_Request:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_RobbingOrder_Addition:
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_RobbingOrder_RequestTo:
                        //users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated), "/robbing-order/addition"));//مدير المخزن+ URL
                        break;
                    case NotificationTemplateEnum.NTF_Cancel_RobbingOrder_RequestTo:
                        //users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated)));//مدير المخزن+ URL
                        break;
                    case NotificationTemplateEnum.NTF_Transformation_RequestTo:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        //users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated), "/transformation/addition"));//مدير المخزن+URL
                        break;
                    case NotificationTemplateEnum.NTF_Cancel_Transformation_RequestTo:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        //users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated)));//مدير المخزن
                        break;
                    case NotificationTemplateEnum.NTF_Transformation_Addition_To_Sender:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated)));//مدير المخزن
                        break;
                    case NotificationTemplateEnum.NTF_RobbingOrder_Addition_To_Sender:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated)));//مدير المخزن
                        break;
                    case NotificationTemplateEnum.NTF_Delegation_Store:
                    case NotificationTemplateEnum.NTF_Cancel_Delegation_Store:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated)));//مدير المخزن
                        break;
                    case NotificationTemplateEnum.NTF_Create_ExchangeOrder:
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_Create_RefundOrder:
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_Review_ExchangeOrder:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated), "/invoice/create"));//مدير المخزن+URL
                        break;
                    case NotificationTemplateEnum.NTF_Review_RefundOrder:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated), "/refund-order/refundinvoice"));//مدير المخزن+URL
                        break;
                    case NotificationTemplateEnum.NTF_RefundOrder_Review_TechManager:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        break;
                    case NotificationTemplateEnum.NTF_ExchangeOrder_Review_TechManager:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        break;
                    case NotificationTemplateEnum.NTF_Invoice_Edit:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_DirectOrder_ExchangeOrder:
                    case NotificationTemplateEnum.NTF_DirectOrder_RefundOrder:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated)));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_Create_Execution_Request:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetTechnicalUserName(int.Parse(storeId), isDelegated),"/execution-order/review"));//مدير الاداره الفنية
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    case NotificationTemplateEnum.NTF_Review_Execution_Request:
                        users.AddRange(MapUsersToNotificationUser(_userBusiness.GetStoreKeeperUserName(int.Parse(storeId), isDelegated), "/execution-order/result"));//مدير المخزن+URL
                        users.AddRange(MapUsersToNotificationUser(_ldapAuthenticationService.GetWarehouseManager().Select(o => o.Username).ToList()));//مديرين المخازن
                        break;
                    default:
                        break;
                }
            }
            return users;
        }

        private List<NotificationUserVM> MapUsersToNotificationUser(List<string> Users, string URL = null)
        {
            var NotificationUsers = new List<NotificationUserVM>();
            foreach (var User in Users)
            {
                NotificationUsers.Add(new NotificationUserVM { User = User, URL = URL });
            }
            return NotificationUsers;
        }

        private async Task SendPushNotification(NotificationTemplateEnum notificationTemplateEnum, List<NotificationUserVM> users, List<Notification> notifications)
        {
            foreach (var username in users)
            {
                var notification = notifications.FirstOrDefault(o => o.UserNotification.Any(u => u.UserName == username.User));
                var userConnectionIds = _userBusiness.GetUserConnectionIds(username.User);
                if (userConnectionIds != null && userConnectionIds.Count > 0)
                {
                    foreach (var userConnectionId in userConnectionIds)
                    {
                        try
                        {
                            await _hubContext.Clients.Client(userConnectionId).SendAsync("pushnotification",
                          notification.Body,
                          notificationTemplateEnum,
                          notification.NotificationValues.Select(o => new { o.Key, o.Value }).ToList(),
                          notification.UserNotification.First().Id,
                          username.URL
                          );
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Notification : Error Sending Push Notification to " + userConnectionId + " Message : " + ex.Message);
                        }

                    }
                }
            }

        }
        private async Task PerformHandlers(NotificationTemplateEnum notificationTemplateEnum,
            List<NotificationUserVM> users, List<NotificationHandler> notificationHandler,
            Dictionary<int, string> notificationValues,
            List<Notification> notifications)
        {
            foreach (var handler in notificationHandler.ToList())
            {
                switch (handler.HandlerId)
                {
                    case (int)NotificationHandlersEnum.SendPushNotification:
                        await SendPushNotification(notificationTemplateEnum, users, notifications);
                        break;
                    default:
                        continue;
                }
            }
        }
        private async Task<List<Notification>> SaveNotification(NotificationTemplateEnum notificationTemplateEnum, List<NotificationUserVM> users, string body, Dictionary<int, string> notificationValues)
        {
            List<Notification> result = new List<Notification>();
            foreach (var user in users)
            {
                Notification notification = new Notification()
                {
                    NotificationTemplateId = (int)notificationTemplateEnum,
                    Body = body,
                    UserNotification = new List<UserNotification>() { new UserNotification() { IsRead = false, UserName = user.User, URL = user.URL } }
                };
                notification.NotificationValues = new List<NotificationValues>();
                foreach (var notificationValue in notificationValues)
                {
                    notification.NotificationValues.Add(new NotificationValues()
                    {
                        Key = (notificationValue.Key).ToString(),
                        Value = notificationValue.Value
                    });
                }
                _notificationRepository.Add(notification);
                result.Add(notification);
            }
            await _notificationRepository.SaveChanges();
            return result;
        }
        private async Task SendNotification(NotificationTemplateEnum notificationTemplateEnum, Dictionary<int, string> notificationValues, List<string> Users)
        {

            //fetch notification template
            var notificationTemplate = _notificationTemplateRepository.GetAll().Include(o => o.NotificationHandler).FirstOrDefault(o => o.Id == (int)notificationTemplateEnum);
            //replace body and url
            string body = ReplaceTemplate(notificationTemplate.Body, notificationValues);
            string URL = ReplaceTemplate(notificationTemplate.URL, notificationValues);

            //find users 
            var users = new List<NotificationUserVM>();
            users = FindUsers(notificationTemplateEnum, notificationValues);
            if (Users != null)
            {
                users.AddRange(MapUsersToNotificationUser(Users));
            }

            //perform handlers
            if (users.Count > 0)
            {
                //save notification in db
                List<Notification> result = await SaveNotification(notificationTemplateEnum, users, body, notificationValues);
                await PerformHandlers(notificationTemplateEnum, users, notificationTemplate.NotificationHandler.ToList(), notificationValues, result);
            }
        }




        //public async Task SendNotification(string storeId, string code, string Id)
        //{
        //    Dictionary<int, string> notificationValues = new Dictionary<int, string>();
        //    notificationValues.Add((int)NotificationValuesEnum.StoreId, storeId);
        //    notificationValues.Add((int)NotificationValuesEnum.AdditionCode, code);
        //    notificationValues.Add((int)NotificationValuesEnum.AdditionId, Id);
        //    await SendNotification(NotificationTemplateEnum.Addition, notificationValues);
        //}
        public async Task SendNotification(NotificationVM Notification)
        {
            Dictionary<int, string> notificationValues = new Dictionary<int, string>();
            notificationValues.Add((int)NotificationValuesEnum.StoreId, Notification.storeId);
            notificationValues.Add((int)NotificationValuesEnum.Code, Notification.code);
            notificationValues.Add((int)NotificationValuesEnum.Id, Notification.Id);
            notificationValues.Add((int)NotificationValuesEnum.FromStore, Notification.FromStore);
            notificationValues.Add((int)NotificationValuesEnum.ToStore, Notification.ToStore);
            notificationValues.Add((int)NotificationValuesEnum.FromStoreAdmin, Notification.FromStoreAdmin);
            notificationValues.Add((int)NotificationValuesEnum.ToStoreAdmin, Notification.ToStoreAdmin);
            notificationValues.Add((int)NotificationValuesEnum.TechManager, Notification.TechManager);
            await SendNotification(Notification.notificationTemplateEnum, notificationValues, Notification.Users);
        }




        public IQueryable<UserNotification> GetAll()
        {
            return _userNotificationRepository.GetAll();
        }

        //public IQueryable<UserNotification> GetAllUserNotifications()
        //{
        //    return _userNotificationRepository.GetAll();
        //}

        public async Task<bool> MarkAsRead(Guid id)
        {
            var userNotification = _userNotificationRepository.GetFirst(o => o.Id == id);
            userNotification.IsRead = true;
            _userNotificationRepository.Update(userNotification);
            return await _userNotificationRepository.SaveChanges() > 0;
        }


    }
}
