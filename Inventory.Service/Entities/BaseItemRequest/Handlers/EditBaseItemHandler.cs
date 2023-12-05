using Inventory.Data.Entities;
using Inventory.Service.Entities.BaseItemRequest.Commands;
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
    class EditBaseItemHandler : IRequestHandler<EditBaseItemCommand, bool>
    {

        private readonly IBaseItemBussiness _baseItemBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public EditBaseItemHandler(IBaseItemBussiness baseItemBussiness,IStringLocalizer<SharedResource> Localizer)
        {
            _baseItemBussiness = baseItemBussiness;
            _Localizer = Localizer;

        }

        public async Task<bool> Handle(EditBaseItemCommand request, CancellationToken cancellationToken)
        {

            if (!_baseItemBussiness.checkDeactivation(request.Id))
                throw new InvalidDeactivationException(_Localizer["CanNotEditBaseItem"]);

            BaseItem baseItemObj=_baseItemBussiness.ViewBaseItem(request.Id);
            if(baseItemObj !=null)
            {
                //BaseItem entity = new BaseItem();
                baseItemObj.Name = request.Name;
                baseItemObj.ShortName = request.ShortName;
                baseItemObj.ItemCategoryId = request.ItemCategoryId;
                baseItemObj.DefaultUnitId = request.DefaultUnitId;
                baseItemObj.WarningLevel = request.WarningLevel;
                baseItemObj.Consumed = request.Consumed;
                baseItemObj.Description = request.Description;
           
                return await _baseItemBussiness.UpdateBaseItem();
            }
            else
            {

                throw new NoDataException(_Localizer["NoDataException"]);
            }
            
        }
    }
}
