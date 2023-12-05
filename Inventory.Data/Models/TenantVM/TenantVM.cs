using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.TenantVM
{
    public class TenantVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsDelegated { get; set; }
        public DateTime EndDate { get; set; }
        public int StoreType { get; set; }
        public string userName { get; set; }

    }
}
