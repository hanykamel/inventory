using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UnitRequest.Commands
{
   public class EditUnitCommand : IRequest<bool>
    {

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
