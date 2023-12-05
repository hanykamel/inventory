using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.UnitRequest.Handlers
{
    public class GetAllUnitHandler : IRequestHandler<GetAllUnitCommand, List<UnitOutputCommand>>
    {
        private readonly IUnitBussiness _IUnitBussiness;
        public GetAllUnitHandler(IUnitBussiness IUnitBussiness)
        {
            _IUnitBussiness = IUnitBussiness;
        }
        public Task<List<UnitOutputCommand>> Handle(GetAllUnitCommand request, CancellationToken cancellationToken)
        {
            List<UnitOutputCommand> _GetAllUnitCommand = new List<UnitOutputCommand>();
            var unitList = _IUnitBussiness.GetAllUnit();
            foreach (var item in unitList)
            {
                UnitOutputCommand _entity = new UnitOutputCommand();
                _entity.Id = item.Id;
                _entity.IsActive = item.IsActive;
                _entity.Name = item.Name;
                _GetAllUnitCommand.Add(_entity);
            }
            return Task.FromResult(_GetAllUnitCommand);
        }
    }
}
