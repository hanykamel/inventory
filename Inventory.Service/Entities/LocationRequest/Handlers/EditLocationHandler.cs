using Inventory.Data.Entities;
using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.LocationRequest.Handlers
{
    public class EditLocationHandler : IRequestHandler<EditLocationCommand, bool>
    {

        private readonly ILocationBussiness _ILocationBussiness;
        public EditLocationHandler(ILocationBussiness ILocationBussiness)
        {
            _ILocationBussiness = ILocationBussiness;
        }
        public Task<bool> Handle(EditLocationCommand request, CancellationToken cancellationToken)
        {
            return _ILocationBussiness.UpdateLocation(new Location { Id = request.Id, Name = request.Name });
        }
    }
}
