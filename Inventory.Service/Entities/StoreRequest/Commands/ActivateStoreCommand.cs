using MediatR;

namespace Inventory.Service.Entities.StoreRequest.Commands
{
    public class ActivateStoreCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
