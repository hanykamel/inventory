using Inventory.Data.Entities;
using Inventory.Service.Entities.StoreRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.StoreRequest.Handlers
{
    public class EditStoreHandler : IRequestHandler<EditStoreCommand, bool>
    {
        private readonly IStoreBussiness _IStoreBussiness;
        public EditStoreHandler(IStoreBussiness IStoreBussiness)
        {
            _IStoreBussiness = IStoreBussiness;
        }
        public Task<bool> Handle(EditStoreCommand request, CancellationToken cancellationToken)
        {
            Store entity = new Store();
            entity.Id = request.Id;
            entity.AdminId = request.Admin;
            entity.StoreTypeId = request.StoreType;
            entity.TechnicalDepartmentId = request.TechnicalDepartmentId;
            entity.RobbingBudgetId = request.RobbingBudgetId;
            return _IStoreBussiness.UpdateStore(entity);
        }
    }
}
