using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UnitRequest.Commands
{
  public  class GetAllUnitCommand : IRequest<List<UnitOutputCommand>>
    {
    }
}
