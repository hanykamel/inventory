using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DelegationRequest.Commands
{
  public  class AddDelegationReturnVm
    {
        public Guid Id { get; set; }
        public List<string> storeName { get; set; }

    }
}
