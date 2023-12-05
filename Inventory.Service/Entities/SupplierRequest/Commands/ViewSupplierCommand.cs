using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.SupplierRequest.Commands
{
    public class ViewSupplierCommand : IRequest<SupplierOutputCommand>
    {
        public int Id { get; set; }
    }
}
