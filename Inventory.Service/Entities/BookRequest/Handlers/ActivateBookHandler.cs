using Inventory.Service.Entities.BookRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization; 

namespace Inventory.Service.Entities.BookRequest.Handlers
{
    public class ActivateBookHandler : IRequestHandler<ActivateBookCommand, bool>
    {

        private readonly IBookBussiness _IBookBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateBookHandler(IBookBussiness IBookBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _IBookBussiness = IBookBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateBookCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IBookBussiness.checkValidEdit(request.Id))
                {
                    return _IBookBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _IBookBussiness.Activate(request.Id, request.ActivationType);

            }
        }
    }
}
