using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DelegationRequest.Commands
{
   public class EditDelegationCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public List<int> DelegationStores { get; set; }
    }
}
