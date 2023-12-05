using Inventory.Service.Entities.DepartmentRequest.Commands;
using Inventory.Service.Entities.DepartmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.DepartmentRequest.Handlers
{
    public class GetAllDepartmentHandler : IRequestHandler<GetAllDepartmentCommand, List<DepartmentOutputCommand>>
    {
        private readonly IDepartmentBussiness _IDepartmentBussiness;
        public GetAllDepartmentHandler(IDepartmentBussiness IDepartmentBussiness)
        {
            _IDepartmentBussiness = IDepartmentBussiness;
        }
        public Task<List<DepartmentOutputCommand>> Handle(GetAllDepartmentCommand request, CancellationToken cancellationToken)
        {
            List<DepartmentOutputCommand> _GetAllDepartmentCommand = new List<DepartmentOutputCommand>();
            var DepartmentList = _IDepartmentBussiness.GetAllDepartment();
            foreach (var item in DepartmentList)
            {
                DepartmentOutputCommand _entity = new DepartmentOutputCommand();
                _entity.Id = item.Id;
                _entity.IsActive = item.IsActive;
                _entity.DepartmentName = item.Name;
                _GetAllDepartmentCommand.Add(_entity);
            }
            return Task.FromResult(_GetAllDepartmentCommand);
        }
    }
}
