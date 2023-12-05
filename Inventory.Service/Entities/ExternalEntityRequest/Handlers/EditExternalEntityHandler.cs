using Inventory.Data.Entities;
using Inventory.Service.Entities.ExternalEntityRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExternalEntityRequest.Handlers
{
    public class EditExternalEntityHandler : IRequestHandler<EditExternalEntityCommand, bool>
    {

        private readonly IExternalEntityBussiness _IExternalEntityBussiness;
        public EditExternalEntityHandler(IExternalEntityBussiness IExternalEntityBussiness)
        {
            _IExternalEntityBussiness = IExternalEntityBussiness;
        }
        public Task<bool> Handle(EditExternalEntityCommand request, CancellationToken cancellationToken)
        {
            return _IExternalEntityBussiness.UpdateExternalEntity(new ExternalEntity { Id = request.Id, Name = request.Name });
        }
    }
}
