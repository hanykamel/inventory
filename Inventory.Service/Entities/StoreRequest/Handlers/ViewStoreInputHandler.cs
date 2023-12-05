using Inventory.Data.Enums;
using Inventory.Service.Entities.StoreRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.StoreRequest.Handlers
{
    public class ViewStoreInputHandler : IRequestHandler<ViewStoreInputCommand, ViewStoreOutputCommand>
    {

        private readonly IStoreBussiness _IStoreBussiness;
        public ViewStoreInputHandler(IStoreBussiness IStoreBussiness)
        {
            _IStoreBussiness = IStoreBussiness;
        }

        public Task<ViewStoreOutputCommand> Handle(ViewStoreInputCommand request, CancellationToken cancellationToken)
        {
            ViewStoreOutputCommand _ViewStoreOutputCommand = new ViewStoreOutputCommand();
           var storeEntity= _IStoreBussiness.ViewStore(request.storeId);
            _ViewStoreOutputCommand.RobbingBudget = storeEntity.RobbingBudget.Name;
            _ViewStoreOutputCommand.TechnicalDepartment = storeEntity.TechnicalDepartment.Name;
            
            _ViewStoreOutputCommand.storeType =(StoreTypeEnum) storeEntity.StoreTypeId;
            return Task.FromResult(_ViewStoreOutputCommand);
        }
    }
}
