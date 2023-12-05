using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Service.Entities.RemainsRequest.Commands;
using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Inventory.Service.Entities.RemainsRequest.Handlers
{
    public class AddRemainsHandler : IRequestHandler<AddRemainsCommand,Remains>
    {
        private readonly IRemainsBussiness _RemainsBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public AddRemainsHandler(IRemainsBussiness RemainsBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _RemainsBussiness = RemainsBussiness;
            _Localizer = Localizer;
        }
        public Task<Remains> Handle(AddRemainsCommand request, CancellationToken cancellationToken)
        {
            Remains entity = new Remains();
            entity.Name = request.Name;
            entity.Description = request.Description!=null? request.Description:"";
            entity.Consumed = request.Consumed;
            List<Remains> lstExistance = _RemainsBussiness.CheckRemainsExistance(entity).ToList();
            if (lstExistance.Count > 0)
                throw new InvalidBookException(_Localizer["InvalidBookException"]);

            return _RemainsBussiness.AddNewRemains(entity);
        }
    }
}
