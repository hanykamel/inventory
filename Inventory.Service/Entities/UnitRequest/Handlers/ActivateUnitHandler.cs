using Inventory.Service.Entities.UnitRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.UnitRequest.Handlers
{
    public class ActivateUnitHandler : IRequestHandler<ActivateUnitCommand, bool>
    {


        private readonly IUnitBussiness _IUnitBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateUnitHandler(IUnitBussiness IUnitBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IUnitBussiness = IUnitBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateUnitCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IUnitBussiness.checkDeactivation(request.Id))
                {
                    return _IUnitBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _IUnitBussiness.Activate(request.Id, request.ActivationType);
            }
        }
    }
}
