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
    public class ViewUnitHandler : IRequestHandler<ViewUnitCommand, UnitOutputCommand>
    {

        private readonly IUnitBussiness _IUnitBussiness;
        public ViewUnitHandler(IUnitBussiness IUnitBussiness)
        {
            _IUnitBussiness = IUnitBussiness;
        }
        public Task<UnitOutputCommand> Handle(ViewUnitCommand request, CancellationToken cancellationToken)
        {
        
            var Unitentity= _IUnitBussiness.ViewUnit(request.Id);
          
               UnitOutputCommand _entity = new UnitOutputCommand();
            _entity.Id = Unitentity.Id;
            _entity.Name = Unitentity.Name;
            _entity.IsActive = Unitentity.IsActive;
            return Task.FromResult(_entity);

        }
    }
}
