using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DepartmentRequest.Commands
{
   public class EditDepartmentCommand : IRequest<bool>
    {

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
