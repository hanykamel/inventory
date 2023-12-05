using Inventory.Data.Entities;
using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Unit = Inventory.Data.Entities.Unit;
using Inventory.Service.Entities.DepartmentRequest.Commands;

namespace Inventory.Service.Entities.DepartmentRequest.Handlers
{
    public class AddDepartmentHandler : IRequestHandler<AddDepartmentCommand, Department>
    {
        
        private readonly IDepartmentBussiness _departmentBussiness;
        public AddDepartmentHandler(IDepartmentBussiness IDepartmentBussiness)
        {
            _departmentBussiness = IDepartmentBussiness;
        }

        public Task<Department> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            Department entity = new Department();
            entity.Name = request.name;
            return (_departmentBussiness.AddNewDepartment(entity));
        }
    }
}
