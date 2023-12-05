using MediatR;

namespace Inventory.Service.Entities.UnitRequest.Commands
{
  public  class ActivateUnitCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
