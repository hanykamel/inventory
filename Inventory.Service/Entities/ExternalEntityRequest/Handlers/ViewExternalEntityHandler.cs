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
  public  class ViewExternalEntityHandler : IRequestHandler<ViewExternalEntityCommand, ExternalEntityOutputCommand>
    {
        private readonly IExternalEntityBussiness _IExternalEntityBussiness;
        public ViewExternalEntityHandler(IExternalEntityBussiness IExternalEntityBussiness)
        {
            _IExternalEntityBussiness = IExternalEntityBussiness;
        }

        public Task<ExternalEntityOutputCommand> Handle(ViewExternalEntityCommand request, CancellationToken cancellationToken)
        {
            ExternalEntityOutputCommand _ExternalEntityOutputCommand = new ExternalEntityOutputCommand();
            var storeEntity = _IExternalEntityBussiness.ViewExternalEntity(request.id);
            _ExternalEntityOutputCommand.Name = storeEntity.Name;
           
            return Task.FromResult(_ExternalEntityOutputCommand);
        }
    }
}
