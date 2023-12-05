using MediatR;

namespace Inventory.Service.Entities.LocationRequest.Commands
{
   public class EditLocationCommand : IRequest<bool>
    {

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
