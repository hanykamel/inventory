using Inventory.Data.Entities;
using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Inventory.Service.Entities.LocationRequest.Handlers
{
    public class AddLocationHandler : IRequestHandler<AddLocationCommand,Location>
    {
        
        private readonly ILocationBussiness _ILocationBussiness;
        public AddLocationHandler(ILocationBussiness ILocationBussiness)
        {
            _ILocationBussiness = ILocationBussiness;
        }
        public Task<Location> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            Location entity = new Location();
            entity.Name = request.Name;
            return(_ILocationBussiness.AddNewLocation(entity));

     }
    }
}
