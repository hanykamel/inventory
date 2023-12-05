using Inventory.Service.Entities.ItemCategoryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization; 

namespace Inventory.Service.Entities.ItemCategoryRequest.Handlers
{
    public class ActivateItemCategoryHandler : IRequestHandler<ActivateItemCategoryCommand, bool>
    {
        private readonly IItemCategoryBussiness _IItemCategoryBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public ActivateItemCategoryHandler(IItemCategoryBussiness IItemCategoryBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IItemCategoryBussiness = IItemCategoryBussiness;
            _Localizer = Localizer;

        }
        public Task<bool> Handle(ActivateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.ActivationType)
            {
                if (_IItemCategoryBussiness.checkDeactivation(request.Id))
                {
                    return _IItemCategoryBussiness.Activate(request.Id, request.ActivationType);
                }
                else
                {
                    throw new InvalidDeactivationException(_Localizer["InvalidDeactivation"]);
                }
            }
            else
            {
                return _IItemCategoryBussiness.Activate(request.Id, request.ActivationType);
            }
        }
    }
}
