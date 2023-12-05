using Inventory.Data.Entities;
using MediatR;

namespace Inventory.Service.Entities.LocationRequest.Commands
{
   public class AddLocationCommand : IRequest<Location>
    {
        public string Name { get; set; }
    }
}
