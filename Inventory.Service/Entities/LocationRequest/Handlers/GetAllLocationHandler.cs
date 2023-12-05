using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.LocationRequest.Handlers
{
    public class GetAllLocationHandler : IRequestHandler<GetAllLocationCommand, List<LocationOutputCommand>>
    {
        private readonly ILocationBussiness _ILocationBussiness;
        public GetAllLocationHandler(ILocationBussiness ILocationBussiness)
        {
            _ILocationBussiness = ILocationBussiness;
        }
        public Task<List<LocationOutputCommand>> Handle(GetAllLocationCommand request, CancellationToken cancellationToken)
        {
            List<LocationOutputCommand> _GetAllLocationCommand = new List<LocationOutputCommand>();
            var LocationList = _ILocationBussiness.GetAllLocation();
            foreach (var item in LocationList)
            {
                LocationOutputCommand _entity = new LocationOutputCommand();
                _entity.Id = item.Id;
                _entity.IsActive = item.IsActive;
                _entity.Name = item.Name;
                _GetAllLocationCommand.Add(_entity);
            }
            return Task.FromResult(_GetAllLocationCommand);
        }
    }
}
