using MediatR;

namespace Inventory.Service.Entities.StoreRequest.Commands
{
    public class ViewStoreInputCommand : IRequest<ViewStoreOutputCommand>
    {
        public int storeId { get; set; }
    }
}
