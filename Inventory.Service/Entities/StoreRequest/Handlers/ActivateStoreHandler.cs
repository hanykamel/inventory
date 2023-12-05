using Inventory.Service.Entities.StoreRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.StoreRequest.Handlers
{
    public class ActivateStoreHandler : IRequestHandler<ActivateStoreCommand, bool>
    {

        private readonly IStoreBussiness _IStoreBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateStoreHandler(IStoreBussiness IStoreBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IStoreBussiness = IStoreBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateStoreCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IStoreBussiness.checkDeactivation(request.Id))
                {
                    return _IStoreBussiness.Activate(request.Id, request.ActivationType);

                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);

                }
            }
            else
            {
                return _IStoreBussiness.Activate(request.Id, request.ActivationType);

            }
        }
    }
}
