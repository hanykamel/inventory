using Inventory.Data.Entities;
using MediatR;

namespace Inventory.Service.Entities.DepartmentRequest.Commands
{
   public class AddDepartmentCommand : IRequest<Department>
    {
        public string name { get; set; }
    }
}
