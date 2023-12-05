using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Service.Entities.StoreRequest.Commands;

namespace Inventory.Service.Entities.StoreRequest.Handlers
{
    public class AddStoreHandler : IRequestHandler<AddStoreCommand, Store>
    {
        private readonly IStoreBussiness _IStoreBussiness;
        public AddStoreHandler(IStoreBussiness IStoreBussiness
)
        {
            _IStoreBussiness = IStoreBussiness;
        }
        public Task<Store> Handle(AddStoreCommand request, CancellationToken cancellationToken)
        {
           
                Store entity = new Store();
                entity.StoreTypeId = request.StoreTypeId;
                entity.TechnicalDepartmentId = request.TechnicalDepartmentId;
                entity.RobbingBudgetId = request.RobbingBudgetId;
                entity.AdminId = request.Admin;
                return _IStoreBussiness.AddNewStore(entity);
        }
    }
}
