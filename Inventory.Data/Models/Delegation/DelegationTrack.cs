using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.Delegation
{
  public  class DelegationTrack
    {
        public Guid Id { get; set; }

        public int TenantId { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan CreationTime { get; set; }
        public string DelegationUserName { get; set; }
        public string Operation { get; set; }
        public string OperationNum { get; set; }
    }
}
