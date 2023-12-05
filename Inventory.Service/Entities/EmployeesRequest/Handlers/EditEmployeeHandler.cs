using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Service.Entities.EmployeesRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.EmployeesRequest.Handlers
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand, bool>
    {
        private readonly IEmployeeBussiness _IEmployeeBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public EditEmployeeHandler(IEmployeeBussiness IEmployeeBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IEmployeeBussiness = IEmployeeBussiness;
            _Localizer = Localizer;
        }
        public Task<bool> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(request.CardCode))
            {
                Employees isCardCodeExist = _IEmployeeBussiness.IsCardCodeExist(request.CardCode);
                if (isCardCodeExist != null && request.Id != isCardCodeExist.Id)
                    throw new InvalidException(_Localizer["CardCodeExist"]);
            }
            Employees entity = new Employees();
            entity.Name = request.Name;
            entity.Id = request.Id;
            entity.DepartmentId = request.DepartmentId;
            entity.CardCode = request.CardCode;

            return _IEmployeeBussiness.UpdateEmployees(entity);
        }
    }
}
