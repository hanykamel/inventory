using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExternalEntityRequest.Commands
{
    public class ViewExternalEntityCommand : IRequest<ExternalEntityOutputCommand>
    {
        public int id { get; set; }
    }
}
