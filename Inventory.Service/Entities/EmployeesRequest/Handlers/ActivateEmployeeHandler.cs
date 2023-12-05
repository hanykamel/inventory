using Inventory.Service.Entities.EmployeeRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.EmployeeRequest.Handlers
{
    public class ActivateEmployeeHandler : IRequestHandler<ActivateEmployeeCommand, bool>
    {


        private readonly IEmployeeBussiness _IEmployeeBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateEmployeeHandler(IEmployeeBussiness IEmployeeBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IEmployeeBussiness = IEmployeeBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IEmployeeBussiness.checkDeactivation(request.Id))
                {
                    return _IEmployeeBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);

                }
            }
            else
            {
                return _IEmployeeBussiness.Activate(request.Id, request.ActivationType);
            }
        }
    }
}
