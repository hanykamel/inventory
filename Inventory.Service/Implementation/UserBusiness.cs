using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.TenantVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class UserBusiness : IUserBusiness
    {
        readonly private IRepository<Store, int> _storeRepository;
        readonly private IRepository<TechnicalDepartment, int> _technicanRepository;
        private readonly IRepository<UserConnection, Guid> _userConnectionRepository;
        private readonly IRepository<DelegationStore, Guid> _delegationStoreRepository;

        public UserBusiness(IRepository<Store, int> storeRepository,
            IRepository<TechnicalDepartment, int> technicanRepository,
            IRepository<UserConnection, Guid> userConnectionRepository,
            IRepository<DelegationStore, Guid> delegationStoreRepository
            )
        {
            _storeRepository = storeRepository;
            _technicanRepository = technicanRepository;
            _userConnectionRepository = userConnectionRepository;
            _delegationStoreRepository=delegationStoreRepository;
        }

        public async Task<string> ConnectUserConnectionId(string username, string connectionId)
        {
            var userConnection = _userConnectionRepository.GetAll().Where(o => o.UserName == username).FirstOrDefault();
            if (userConnection == null)
            {
                userConnection = new UserConnection()
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    ConnectionId = connectionId,
                };
                _userConnectionRepository.Add(userConnection);
            }
            else
            {
                userConnection.ConnectionId = connectionId;
                _userConnectionRepository.Update(userConnection);
            }
            if (await _userConnectionRepository.SaveChanges() > 0)
                return Task.FromResult(userConnection.ConnectionId).Result;
            return "";
        }
        public async Task<bool> DeleteUserConnectionId(string username)
        {
            var userConnection = _userConnectionRepository.GetAll().Where(o => o.UserName == username).FirstOrDefault();
            if (userConnection != null)
            {
                userConnection.IsActive = false;
                _userConnectionRepository.Update(userConnection);
                if (await _userConnectionRepository.SaveChanges() > 0)
                    return Task.FromResult(true).Result;
                else
                    return Task.FromResult(false).Result;
            }

            return Task.FromResult(true).Result;
        }
        public List<string> GetUserConnectionIds(string username)
        {
            var userConnection = _userConnectionRepository.GetAll().Where(o => o.UserName == username).ToList();
            if (userConnection != null && userConnection.Count > 0)
            {
                return userConnection.Select(o => o.ConnectionId).ToList();
            }
            return new List<string>();
        }

      
        public List<TenantVM> GetUserOriginalTenants(string userName)
        {
            var tenants = _storeRepository.
                 GetAll(x => (x.AdminId == userName
            || x.TechnicalDepartment.TechnicianId == userName) && x.IsActive == true
            && (x.TechnicalDepartment == null || x.TechnicalDepartment.IsActive == true), true)
                .Include(y => y.TechnicalDepartment)
                .Include(y => y.RobbingBudget)
                .Include(y => y.StoreType)

                .Select(y => new TenantVM
                {
                    Id = y.Id,
                    Name = y.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                      (y.RobbingBudget != null ?
                  (y.StoreType.Name + " " + y.RobbingBudget.Name) : "") :
                    (y.TechnicalDepartment != null ?
                    y.StoreType.Name + " " + y.TechnicalDepartment.Name : ""),
                    IsDelegated = false,
                    StoreType = y.StoreTypeId,
                      userName=y.AdminId

                }).ToList().Distinct(new TenantComparer()).ToList();

            
            return tenants;

        }

        public List<TenantVM> GetUserTenants(string userName)
        {
            var tenants = _storeRepository.
                 GetAll(x => (x.AdminId == userName
            || x.TechnicalDepartment.TechnicianId == userName ||
            x.TechnicalDepartment.AssistantTechnician == userName)
            && x.IsActive == true
            && (x.TechnicalDepartment == null || x.TechnicalDepartment.IsActive == true), 
            true)
                .Include(y => y.TechnicalDepartment)
                .Include(y => y.RobbingBudget)
                .Include(y => y.StoreType)

                .Select(y => new TenantVM
                {
                    Id = y.Id,
                    Name = y.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                      (y.RobbingBudget != null ?
                  (y.StoreType.Name + " " + y.RobbingBudget.Name) : "") :
                    (y.TechnicalDepartment != null ?
                    y.StoreType.Name + " " + y.TechnicalDepartment.Name : ""),
                    IsDelegated = false,
                    StoreType = y.StoreTypeId

                }).ToList().Distinct(new TenantComparer()).ToList();

            var delegatedUsers = _delegationStoreRepository.GetAll(x =>
                  x.Store.IsActive == true
                  && (x.Store.TechnicalDepartment == null ||
                  x.Store.TechnicalDepartment.IsActive == true)
                  &&
                  x.Delegation.IsSuspended != true
                  && x.Delegation.UserName == userName
                  && DateTime.Now.Date >= x.Delegation.DateFrom.Date
                  && DateTime.Now.Date <= x.Delegation.DateTo.Date
                  && DateTime.Now.TimeOfDay >= x.Delegation.TimeFrom
                  && DateTime.Now.TimeOfDay <= x.Delegation.TimeTo
                  , true)
                .Include(y => y.Delegation)
                .Include(y => y.Store)
                .ThenInclude(y => y.TechnicalDepartment)
                .Include(y => y.Store)
                .ThenInclude(y => y.RobbingBudget)
                .Include(y => y.Store)
                .ThenInclude(y => y.StoreType)
            .Select(y => new TenantVM
            {
                Id = y.StoreId,
                Name = y.Store.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                  (y.Store.RobbingBudget != null ?
              (y.Store.StoreType.Name + " " + y.Store.RobbingBudget.Name) : "") :
                (y.Store.TechnicalDepartment != null ?
                y.Store.StoreType.Name + " " + y.Store.TechnicalDepartment.Name : ""),
                IsDelegated = true,
                EndDate = new DateTime(y.Delegation.DateTo.Year,
                 y.Delegation.DateTo.Month, y.Delegation.DateTo.Day,
                 y.Delegation.TimeTo.Hours, y.Delegation.TimeTo.Minutes,
                 y.Delegation.TimeTo.Seconds),
                StoreType = y.Store.StoreTypeId
            }).ToList().Distinct(new TenantComparer()).ToList();

            if (delegatedUsers != null && delegatedUsers.Count() > 0)
                tenants = tenants.Union(delegatedUsers).ToList();

            return tenants;

        }

        public TenantVM GetTenantData(int tenantId,string userName)
        {
            var tenantData = _storeRepository.
                 GetAll(x => 
                  x.Id==tenantId &&
           ( x.AdminId == userName || 
           x.TechnicalDepartment.TechnicianId == userName
           || x.TechnicalDepartment.AssistantTechnician == userName) 
           && x.IsActive == true
            && (x.TechnicalDepartment == null || x.TechnicalDepartment.IsActive == true)
            , true)
                .Include(y => y.TechnicalDepartment)
                .Include(y => y.RobbingBudget)
                .Include(y => y.StoreType)

                .Select(y => new TenantVM
                {
                    Id = y.Id,
                    Name = y.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                      (y.RobbingBudget != null ?
                  (y.StoreType.Name + " " + y.RobbingBudget.Name) : "") :
                    (y.TechnicalDepartment != null ?
                    y.StoreType.Name + " " + y.TechnicalDepartment.Name : ""),
                    IsDelegated = false,
                    StoreType = y.StoreTypeId

                }).FirstOrDefault();

            if (tenantData == null)
            {
                return _delegationStoreRepository.GetAll(x =>
                   x.Store.IsActive == true
                   && x.StoreId == tenantId
                   && (x.Store.TechnicalDepartment == null ||
                   x.Store.TechnicalDepartment.IsActive == true)
                   &&
                   x.Delegation.IsSuspended != true
                   && x.Delegation.UserName == userName
                   && DateTime.Now.Date >= x.Delegation.DateFrom.Date
                   && DateTime.Now.Date <= x.Delegation.DateTo.Date
                   && DateTime.Now.TimeOfDay >= x.Delegation.TimeFrom
                   && DateTime.Now.TimeOfDay <= x.Delegation.TimeTo
                   , true)
                 .Include(y => y.Delegation)
                 .Include(y => y.Store)
                 .ThenInclude(y => y.TechnicalDepartment)
                 .Include(y => y.Store)
                 .ThenInclude(y => y.RobbingBudget)
                 .Include(y => y.Store)
                 .ThenInclude(y => y.StoreType)
             .Select(y => new TenantVM
             {
                 Id = y.StoreId,
                 Name = y.Store.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                   (y.Store.RobbingBudget != null ?
               (y.Store.StoreType.Name + " " + y.Store.RobbingBudget.Name) : "") :
                 (y.Store.TechnicalDepartment != null ?
                 y.Store.StoreType.Name + " " + y.Store.TechnicalDepartment.Name : ""),
                 IsDelegated = true,
                 EndDate = new DateTime(y.Delegation.DateTo.Year,
                  y.Delegation.DateTo.Month, y.Delegation.DateTo.Day, y.Delegation.TimeTo.Hours, y.Delegation.TimeTo.Minutes,
                  y.Delegation.TimeTo.Seconds),
                 StoreType = y.Store.StoreTypeId
             }).FirstOrDefault();
            }
            else
                return tenantData;
           

        }

        public List<string> GetTechnicalUserName(int storeId, bool getDelegate = true)
        {
            var OriginalTechnicalUser= _storeRepository.
                 GetAll(x => x.Id == storeId,true)
                .Include(y => y.TechnicalDepartment)
                .Include(y => y.StoreType)

                .Select(
                    y => y.StoreTypeId != (int)StoreTypeEnum.Robbing ?
                    (y.TechnicalDepartment.TechnicianId) : ""

                ).FirstOrDefault();

            var usersList = new List<string>();
            usersList.Add(OriginalTechnicalUser);

            if (getDelegate)
            {
                var delegatedUser = _delegationStoreRepository.GetAll(
                   x => x.Store.IsActive == true
                   && x.StoreId == storeId
                   && (
                   x.Store.TechnicalDepartment.IsActive == true)
                   && x.Delegation.DelegationTypeId==(int)DelegationTypeEnum.Technican
                   && x.Delegation.IsSuspended != true
                   && DateTime.Now.Date >= x.Delegation.DateFrom.Date
                   && DateTime.Now.Date <= x.Delegation.DateTo.Date
                   && DateTime.Now.TimeOfDay >= x.Delegation.TimeFrom
                   && DateTime.Now.TimeOfDay <= x.Delegation.TimeTo
                   , true).Select(x => x.Delegation.UserName).FirstOrDefault();
                if (delegatedUser != null)
                {
                    usersList.Add(delegatedUser);
                }
            }




            return usersList;

        }

        public List<string> GetStoreKeeperUserName(int storeId,bool getDelegate=true)
        {
            var store = _storeRepository.
                 GetAll(x => x.Id == storeId, true)

                .Select(
                    y => y.AdminId

                ) .FirstOrDefault();

            var usersList = new List<string>();
            usersList.Add(store);

            if (getDelegate)
            {
                var delegatedUser = _delegationStoreRepository.GetAll(
                   x =>x.Store.IsActive == true
                   && x.StoreId == storeId
                   && (x.Store.TechnicalDepartment == null ||
                   x.Store.TechnicalDepartment.IsActive == true)
                   && x.Delegation.DelegationTypeId == (int)DelegationTypeEnum.StoreAdmin
                   && x.Delegation.IsSuspended != true
                   && DateTime.Now.Date >= x.Delegation.DateFrom.Date
                   && DateTime.Now.Date <= x.Delegation.DateTo.Date
                   && DateTime.Now.TimeOfDay >= x.Delegation.TimeFrom
                   && DateTime.Now.TimeOfDay <= x.Delegation.TimeTo
                   , true).Select(x => x.Delegation.UserName).FirstOrDefault();
                if (delegatedUser!=null)
                {
                  usersList.Add(delegatedUser);
                }
            }


           
            
            return usersList;


        }

     

        internal class TenantComparer : IEqualityComparer<TenantVM>
        {
            public bool Equals(TenantVM x, TenantVM y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(TenantVM obj)
            {
                return (int)obj.Id;
            }
        }

    }
}
