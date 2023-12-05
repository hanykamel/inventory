using Inventory.Service.Entities.LocationRequest.Commands;
using MediatR;


namespace Inventory.Service.Entities.LocationRequest.Commands
{
   public class ViewLocationCommand : IRequest<LocationOutputCommand>
    {
        public int Id { get; set; }
    }
}
