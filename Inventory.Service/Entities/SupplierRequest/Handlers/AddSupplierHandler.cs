using Inventory.Data.Entities;
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
    class AddSupplierHandler : IRequestHandler<AddSupplierCommand, Supplier>
    {
        private readonly ISupplierBussiness _ISupplierBussiness;
        public AddSupplierHandler(ISupplierBussiness ISupplierBussiness)
        {
            _ISupplierBussiness = ISupplierBussiness;
        }
        public Task<Supplier> Handle(AddSupplierCommand request, CancellationToken cancellationToken)
        {
            Supplier entity = new Supplier();
            entity.Name = request.Name;


            return _ISupplierBussiness.AddNewSupplier(entity);
        }
    }
}
