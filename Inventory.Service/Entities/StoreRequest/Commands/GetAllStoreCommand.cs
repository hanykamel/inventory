using MediatR;
using System.Collections.Generic;


namespace Inventory.Service.Entities.StoreRequest.Commands
{
  public  class GetAllStoreCommand : IRequest<List<ViewStoreOutputCommand>>
    {
    }
}
