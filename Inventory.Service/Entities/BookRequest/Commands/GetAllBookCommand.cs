using MediatR;
using System.Collections.Generic;


namespace Inventory.Service.Entities.BookRequest.Commands
{
  public  class GetAllBookCommand : IRequest<List<ViewBookOutputCommand>>
    {
    }
}
