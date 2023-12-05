using MediatR;
using System.Collections.Generic;

namespace Inventory.Service.Entities.LocationRequest.Commands
{
  public  class GetAllLocationCommand : IRequest<List<LocationOutputCommand>>
    {
    }
}
