using Inventory.Data.Entities;
using Inventory.Service.Entities.DepartmentRequest.Commands;
using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.DepartmentRequest.Handlers
{
    public class EditDepartmentHandler : IRequestHandler<EditDepartmentCommand, bool>
    {

        private readonly IDepartmentBussiness _IDepartmentBussiness;
        public EditDepartmentHandler(IDepartmentBussiness IDepartmentBussiness)
        {
            _IDepartmentBussiness = IDepartmentBussiness;
        }
        public Task<bool> Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            return _IDepartmentBussiness.UpdateDepartment(new Department { Id = request.Id, Name = request.Name });
        }
    }
}
