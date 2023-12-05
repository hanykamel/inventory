using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExternalEntityRequest.Commands
{
   public class ActivateExternalEntityCommand : IRequest<bool>
    {
        public int id { get; set; }
        public bool ActivationType { get; set; }
    }
}
