using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Service.Entities.EmployeesRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.EmployeesRequest.Handlers
{
   public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, Employees>
    {
        private readonly IEmployeeBussiness _IEmployeeBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public AddEmployeeHandler(IEmployeeBussiness IEmployeeBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IEmployeeBussiness = IEmployeeBussiness;
            _Localizer = Localizer;
        }
        public Task<Employees> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(request.CardCode))
            {
                Employees isCardCodeExist = _IEmployeeBussiness.IsCardCodeExist(request.CardCode);
                if (isCardCodeExist != null)
                    throw new InvalidException(_Localizer["CardCodeExist"]);
            }
            if (!String.IsNullOrEmpty(request.Name))
            {
                int Employees = _IEmployeeBussiness.GetAllEmployees().Where(e=>e.Name==request.Name).Count();
                if (Employees > 0)
                    throw new InvalidException(_Localizer["EmployeeNameExist"]);
            }
            Employees entity = new Employees();
            entity.Name = request.Name;
            entity.DepartmentId = request.DepartmentId;
            entity.CardCode = request.CardCode;

            return _IEmployeeBussiness.AddNewEmployees(entity);
        }
    }
}