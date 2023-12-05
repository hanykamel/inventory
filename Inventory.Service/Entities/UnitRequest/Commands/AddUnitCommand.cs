using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unit = Inventory.Data.Entities.Unit;
namespace Inventory.Service.Entities.UnitRequest.Commands
{
   public class AddUnitCommand : IRequest<Unit>
    {
        public string Name { get; set; }
    }
}
