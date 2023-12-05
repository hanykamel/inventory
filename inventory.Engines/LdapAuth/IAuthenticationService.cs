using inventory.Engines.LdapAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory.Engines.LdapAuth
{
    public interface ILdapAuthenticationService
    {
        LdapUser Login(string username, string password);
        List<string> GetUserGroups(string userName);

        List<LdapUser> GetUserNames(string userSearchToken);

        List<LdapUser> GetTechnicanUserNames(string userSearchToken);
        List<LdapUser> GetAssistantTechnicanUserNames(string userSearchToken);

        List<LdapUser> GetStoreKeeperUserNames(string userSearchToken);
        List<LdapUser> GetWarehouseManager();



    }
}
