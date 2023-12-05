using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.BaseItemRequest.Commands
{
  public  class ActivateBaseItemCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
