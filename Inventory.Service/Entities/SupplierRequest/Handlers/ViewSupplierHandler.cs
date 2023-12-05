using Inventory.Service.Entities.SupplierRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.SupplierRequest.Handlers
{
    public class ViewSupplierHandler : IRequestHandler<ViewSupplierCommand, SupplierOutputCommand>
    {
        private readonly ISupplierBussiness _ISupplierBussiness;
        public ViewSupplierHandler(ISupplierBussiness ISupplierBussiness)
        {
            _ISupplierBussiness = ISupplierBussiness;
        }

        public Task<SupplierOutputCommand> Handle(ViewSupplierCommand request, CancellationToken cancellationToken)
        {
            SupplierOutputCommand _SupplierOutputCommand = new SupplierOutputCommand();
            var storeEntity = _ISupplierBussiness.ViewSupplier(request.Id);
            _SupplierOutputCommand.Name = storeEntity.Name;

            return Task.FromResult(_SupplierOutputCommand);
        }
    }

    }