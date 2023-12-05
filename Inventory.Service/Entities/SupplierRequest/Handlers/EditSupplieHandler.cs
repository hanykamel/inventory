using Inventory.Data.Entities;
using Inventory.Service.Entities.SupplierRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.SupplierRequest.Handlers
{
    public class EditSupplierHandler : IRequestHandler<EditSupplierCommand, bool>
    {

        private readonly ISupplierBussiness _ISupplierBussiness;
        public EditSupplierHandler(ISupplierBussiness ISupplierBussiness)
        {
            _ISupplierBussiness = ISupplierBussiness;
        }
   

        public Task<bool> Handle(EditSupplierCommand request, CancellationToken cancellationToken)
        {
            return _ISupplierBussiness.UpdateSupplier(new Supplier { Id = request.Id, Name = request.Name });
        }
    }
}