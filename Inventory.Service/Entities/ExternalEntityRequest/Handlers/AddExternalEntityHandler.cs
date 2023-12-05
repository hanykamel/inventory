using Inventory.Data.Entities;
using Inventory.Service.Entities.ExternalEntityRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExternalEntityRequest.Handlers
{
    public class AddExternalEntityHandler : IRequestHandler<AddExternalEntityCommand, ExternalEntity>
    {
        private readonly IExternalEntityBussiness _IExternalEntityBussiness;
        public AddExternalEntityHandler(IExternalEntityBussiness IExternalEntityBussiness)
        {
            _IExternalEntityBussiness = IExternalEntityBussiness;
        }
        public Task<ExternalEntity> Handle(AddExternalEntityCommand request, CancellationToken cancellationToken)
        {
            ExternalEntity entity = new ExternalEntity();
            entity.Name = request.Name;


            return _IExternalEntityBussiness.AddNewExternalEntity(entity);
        }
    }
}
