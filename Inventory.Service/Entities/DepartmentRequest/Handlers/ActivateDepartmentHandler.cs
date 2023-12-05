using Inventory.Service.Entities.DepartmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
namespace Inventory.Service.Entities.DepartmentRequest.Handlers
{
    public class ActivateDepartmentHandler : IRequestHandler<ActivateDepartmentCommand, bool>
    {

        private readonly IStringLocalizer<SharedResource> _Localizer;

        private readonly IDepartmentBussiness _IDepartmentBussiness;
        public ActivateDepartmentHandler(IDepartmentBussiness IDepartmentBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IDepartmentBussiness = IDepartmentBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateDepartmentCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IDepartmentBussiness.checkDeactivation(request.Id))
                {
                    return _IDepartmentBussiness.Activate(request.Id, request.ActivationType);

                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);

                }
            }
            else
            {
                return _IDepartmentBussiness.Activate(request.Id, request.ActivationType);
            }
        }
    }
}
