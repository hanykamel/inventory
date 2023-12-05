using Inventory.Service.Entities.ExternalEntityRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.ExternalEntityRequest.Handlers
{
   public class ActivateExternalEntityHandler : IRequestHandler<ActivateExternalEntityCommand, bool>
    {
        private readonly IExternalEntityBussiness _IExternalEntityBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateExternalEntityHandler
            (IExternalEntityBussiness IExternalEntityBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _IExternalEntityBussiness = IExternalEntityBussiness;
            _Localizer = Localizer;

        }

        public Task<bool> Handle(ActivateExternalEntityCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IExternalEntityBussiness.checkDeactivation(request.id))
                {
                    return _IExternalEntityBussiness.Activate(request.id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
            return _IExternalEntityBussiness.Activate(request.id, request.ActivationType);
            }
        }
    }
}
