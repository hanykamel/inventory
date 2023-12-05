using Inventory.Service.Entities.TechnicalDepartmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Handlers
{
    public class ActivateTechnicalDepartmentHandlers : IRequestHandler<ActivateTechnicalDepartmentCommand, bool>
    {
        private readonly ITechnicalDepartmentBussiness _ITechnicalDepartmentBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateTechnicalDepartmentHandlers(
            ITechnicalDepartmentBussiness ITechnicalDepartmentBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _ITechnicalDepartmentBussiness = ITechnicalDepartmentBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateTechnicalDepartmentCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_ITechnicalDepartmentBussiness.checkDeactivation(request.Id))
                {
                    return _ITechnicalDepartmentBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);

                }
            }
            else
            {
                return _ITechnicalDepartmentBussiness.Activate(request.Id, request.ActivationType);

            }

        }
    }
}
