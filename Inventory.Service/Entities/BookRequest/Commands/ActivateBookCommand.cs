using MediatR;

namespace Inventory.Service.Entities.BookRequest.Commands
{
    public class ActivateBookCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
