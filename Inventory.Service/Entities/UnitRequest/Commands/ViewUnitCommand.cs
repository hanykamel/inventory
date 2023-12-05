using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UnitRequest.Commands
{
   public class ViewUnitCommand : IRequest<UnitOutputCommand>
    {
        public int Id { get; set; }
    }
}
