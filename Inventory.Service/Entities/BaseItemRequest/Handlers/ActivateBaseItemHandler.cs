using Inventory.Service.Entities.BaseItemRequest.Commands;
using Inventory.Service.Implementation;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.BaseItemRequest.Handlers
{
   public class ActivateBaseItemHandler : IRequestHandler<ActivateBaseItemCommand, bool>
    {
        private readonly IStringLocalizer<SharedResource> _Localizer;

        private readonly IBaseItemBussiness _IBaseItemBussiness;
        public ActivateBaseItemHandler(
            IBaseItemBussiness IBaseItemBussiness
            ,IStringLocalizer<SharedResource> Localizer)
        {
            _IBaseItemBussiness = IBaseItemBussiness;
            _Localizer = Localizer;
        }

        public Task<bool> Handle(ActivateBaseItemCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IBaseItemBussiness.checkDeactivation(request.Id))
                    return _IBaseItemBussiness.Activate(request.Id, request.ActivationType);
                else
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
            }
            else
            {
                return _IBaseItemBussiness.Activate(request.Id, request.ActivationType);
            }

        }
    }
}
