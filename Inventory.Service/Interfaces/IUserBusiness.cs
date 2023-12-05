
using Inventory.Data.Models.TenantVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IUserBusiness
    {
        List<TenantVM> GetUserTenants(string userName);
        List<TenantVM> GetUserOriginalTenants(string userName);
        
        Task<string> ConnectUserConnectionId(string username, string connectionId);
        List<string> GetUserConnectionIds(string username);
        List<string> GetTechnicalUserName(int storeId, bool getDelegated);
        List<string> GetStoreKeeperUserName(int storeId, bool getDelegated);
        Task<bool> DeleteUserConnectionId(string username);
        TenantVM GetTenantData(int tenantId, string userName);

    }
}
