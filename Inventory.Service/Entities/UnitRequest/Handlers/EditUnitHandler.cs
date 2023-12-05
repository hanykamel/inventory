using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.UnitRequest.Handlers
{
    public class EditUnitHandler : IRequestHandler<EditUnitCommand, bool>
    {

        private readonly IUnitBussiness _IUnitBussiness;
        public EditUnitHandler(IUnitBussiness IUnitBussiness)
        {
            _IUnitBussiness = IUnitBussiness;
        }
        public Task<bool> Handle(EditUnitCommand request, CancellationToken cancellationToken)
        {
    
            return _IUnitBussiness.UpdateUnit(new Inventory.Data.Entities.Unit { Id=request.Id,Name=request.Name });
        }
    }
}
