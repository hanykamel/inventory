using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DepartmentRequest.Commands
{
  public  class GetAllDepartmentCommand : IRequest<List<DepartmentOutputCommand>>
    {
    }
}
