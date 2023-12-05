using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExternalEntityRequest.Commands
{
  public class AddExternalEntityCommand : IRequest<ExternalEntity>
    {
        public string Name { get; set; }
    }
}
