using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.SupplierRequest.Commands
{
 public   class GetAllSupplierCommand : IRequest<List<SupplierOutputCommand>>
    {
    }
}
