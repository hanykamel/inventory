using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.SupplierRequest.Commands
{
   public class AddSupplierCommand : IRequest<Supplier>
    {
        public string Name { get; set; }
    }
}
