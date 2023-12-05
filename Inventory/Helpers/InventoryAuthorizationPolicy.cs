using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Web.Helpers
{
    public static class InventoryAuthorizationPolicy
    {
        public const string AllValidRoles = "AllValidRoles";
        public const string AllTransactions = "AllTransactions";
        public const string ViewData = "ViewData";
        public const string SystemLookups = "SystemLookups";
        public const string ViewADStoreAdmins = "ViewADStoreAdmins";
        public const string SystemAdmin = "SystemAdmin";
        public const string StoreKeeper = "StoreKeeper";
        public const string TechnicalDepartments = "TechnicalDepartments";
        public const string WarehouseManager = "WarehouseManager";
        public const string ChangeTenant = "ChangeTenant";
        public const string AddEmployees = "AddEmployees";






    }
}
