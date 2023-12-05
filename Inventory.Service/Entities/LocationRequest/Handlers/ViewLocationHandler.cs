using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.LocationRequest.Handlers
{
    public class ViewLocationHandler : IRequestHandler<ViewLocationCommand, LocationOutputCommand>
    {

        private readonly ILocationBussiness _ILocationBussiness;
        public ViewLocationHandler(ILocationBussiness ILocationBussiness)
        {
            _ILocationBussiness = ILocationBussiness;
        }
        public Task<LocationOutputCommand> Handle(ViewLocationCommand request, CancellationToken cancellationToken)
        {
        
            var Locationentity= _ILocationBussiness.ViewLocation(request.Id);
          
               LocationOutputCommand _entity = new LocationOutputCommand();
            _entity.Id = Locationentity.Id;
            _entity.Name = Locationentity.Name;
            _entity.IsActive = Locationentity.IsActive;
            return Task.FromResult(_entity);

        }
    }
}
