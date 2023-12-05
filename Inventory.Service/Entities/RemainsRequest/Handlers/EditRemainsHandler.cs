using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Service.Entities.RemainsRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RemainsRequest.Handlers
{
    public class EditRemainsHandler : IRequestHandler<EditRemainsCommand, bool>
    {
        private readonly IRemainsBussiness _RemainsBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public EditRemainsHandler(IRemainsBussiness RemainsBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _RemainsBussiness = RemainsBussiness;
            _Localizer = Localizer;
        }
        public Task<bool> Handle(EditRemainsCommand request, CancellationToken cancellationToken)
        {
            // 1. check object is exist 
            // 2. check object after edit is existed 
            // 3. check if the book is used from other lists 

            if (_RemainsBussiness.checkValidEdit(request.Id))
            {
                Remains entity = new Remains();
                entity.Id = request.Id;
                entity.Name = request.Name;
                entity.Description = request.Description != null ? request.Description : "";
                entity.Consumed = request.Consumed;

                List<Remains> lstExistance = _RemainsBussiness.CheckRemainsExistance(entity).ToList();


                if (lstExistance.Count > 0)
                    throw new InvalidRemainException(_Localizer["RemainsNameAlreadyExist"]);
                return _RemainsBussiness.UpdateRemains(entity);
            }
            else
            {
                throw new InvalidBookException(_Localizer["invalidBookEdit"]);

            }
        }
    }
}
