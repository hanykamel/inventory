using Inventory.Service.Entities.JobTitleRequest.Commands;
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
    public class ActivateJobTitleHandler : IRequestHandler<ActivateJobTitleCommand, bool>
    {


        private readonly IJobTitleBussiness _IJobTitleBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateJobTitleHandler(IJobTitleBussiness IJobTitleBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IJobTitleBussiness = IJobTitleBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateJobTitleCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IJobTitleBussiness.checkDeactivation(request.Id))
                {
                    return _IJobTitleBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _IJobTitleBussiness.Activate(request.Id, request.ActivationType);
            }
        }
    }
}
