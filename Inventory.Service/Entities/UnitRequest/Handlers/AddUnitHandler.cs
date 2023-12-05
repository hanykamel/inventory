using Inventory.Data.Entities;
using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unit = Inventory.Data.Entities.Unit;

namespace Inventory.Service.Entities.UnitRequest.Handlers
{
    public class AddUnitHandler : IRequestHandler<AddUnitCommand,Unit>
    {
        
        private readonly IUnitBussiness _IUnitBussiness;
        public AddUnitHandler(IUnitBussiness IUnitBussiness)
        {
            _IUnitBussiness = IUnitBussiness;
        }
        public Task<Unit> Handle(AddUnitCommand request, CancellationToken cancellationToken)
        {
            Data.Entities.Unit entity = new Data.Entities.Unit();
            entity.Name = request.Name;
            return(_IUnitBussiness.AddNewUnit(entity));

     }
    }
}
