using Inventory.Data.Enums;
using Inventory.Service.Entities.StoreRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.StoreRequest.Handlers
{
    public class GetAllStoreHandler : IRequestHandler<GetAllStoreCommand, List<ViewStoreOutputCommand>>
    {

        private readonly IStoreBussiness _IStoreBussiness;
        public GetAllStoreHandler(IStoreBussiness IStoreBussiness)
        {
            _IStoreBussiness = IStoreBussiness;
        }
        public Task<List<ViewStoreOutputCommand>> Handle(GetAllStoreCommand request, CancellationToken cancellationToken)
        {
            List<ViewStoreOutputCommand> _ViewStoreOutputCommand = new List<ViewStoreOutputCommand>();
            var storeList = _IStoreBussiness.GetAllStore();
            foreach (var item in storeList)
            {
                ViewStoreOutputCommand _entity = new ViewStoreOutputCommand();

                _entity.RobbingBudget = item.RobbingBudget.Name;
                _entity.TechnicalDepartment = item.TechnicalDepartment.Name;
                
                _entity.storeType = (StoreTypeEnum)item.StoreTypeId;
                _ViewStoreOutputCommand.Add(_entity);
            }
            return Task.FromResult(_ViewStoreOutputCommand);

        }
    }
}
