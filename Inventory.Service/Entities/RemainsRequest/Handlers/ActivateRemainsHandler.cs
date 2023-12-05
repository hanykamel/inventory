using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Service.Entities.RemainsRequest.Commands;

namespace Inventory.Service.Entities.RemainsRequest.Handlers
{
    public class ActivateRemainsHandler : IRequestHandler<ActivateRemainsCommand, bool>
    {

        private readonly IRemainsBussiness _IRemainsBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateRemainsHandler(IRemainsBussiness IRemainsBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _IRemainsBussiness = IRemainsBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateRemainsCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IRemainsBussiness.checkValidEdit(request.Id))
                {
                    return _IRemainsBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _IRemainsBussiness.Activate(request.Id, request.ActivationType);

            }
        }
    }
}
