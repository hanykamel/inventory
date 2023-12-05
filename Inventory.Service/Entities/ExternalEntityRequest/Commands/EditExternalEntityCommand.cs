using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExternalEntityRequest.Commands
{
  public  class EditExternalEntityCommand:IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
