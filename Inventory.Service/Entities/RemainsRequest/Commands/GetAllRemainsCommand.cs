using MediatR;
using System.Collections.Generic;


namespace Inventory.Service.Entities.RemainsRequest.Commands
{
  public  class GetAllRemainsCommand : IRequest<List<ViewRemainsOutputCommand>>
    {
    }
}
