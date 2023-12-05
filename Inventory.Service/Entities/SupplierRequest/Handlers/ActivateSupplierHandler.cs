using Inventory.Service.Entities.SupplierRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.SupplierRequest.Handlers
{
    class ActivateSupplierHandler : IRequestHandler<ActivateSupplierCommand, bool>
    {
        private readonly ISupplierBussiness _ISupplierBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateSupplierHandler(ISupplierBussiness ISupplierBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _ISupplierBussiness = ISupplierBussiness;
            _Localizer = Localizer;

        }

        public Task<bool> Handle(ActivateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_ISupplierBussiness.checkDeactivation(request.Id))
                {
                    return _ISupplierBussiness.Activate(request.Id, request.ActivationType);

                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _ISupplierBussiness.Activate(request.Id, request.ActivationType);
            }
        }

    }
}
