using Inventory.Service.Entities.LocationRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
namespace Inventory.Service.Entities.LocationRequest.Handlers
{
    public class ActivateLocationHandler : IRequestHandler<ActivateLocationCommand, bool>
    {


        private readonly ILocationBussiness _ILocationBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateLocationHandler(ILocationBussiness ILocationBussiness
            ,IStringLocalizer<SharedResource> Localizer)
        {
            _ILocationBussiness = ILocationBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateLocationCommand request, CancellationToken cancellationToken)
        {

            if (!request.ActivationType)
            {
                if (_ILocationBussiness.checkDeactivation(request.Id))
                {
                    return _ILocationBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);

                }
            }
            else
            {
                return _ILocationBussiness.Activate(request.Id, request.ActivationType);
            }
            
        }
    }
}
