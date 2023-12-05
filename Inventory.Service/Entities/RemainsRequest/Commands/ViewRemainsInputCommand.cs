using MediatR;

namespace Inventory.Service.Entities.RemainsRequest.Commands
{
    public class ViewRemainsInputCommand : IRequest<ViewRemainsOutputCommand>
    {
        public int Id { get; set; }
    }
}
