using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DelegationRequest.Commands
{
   public class AddDelegationCommand : IRequest<AddDelegationReturnVm>
    {
        public string UserName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public houreClock TimeFrom { get; set; }
        public houreClock TimeTo { get; set; }
        public int ShiftId { get; set; }
        public int DelegationType { get; set; }
        public List<int> DelegationStores { get; set; }
    }


    public class houreClock
    {
        public int Houre { get; set; }
        public int Minute { get; set; }
        public int Secand { get; set; }
    }
}
