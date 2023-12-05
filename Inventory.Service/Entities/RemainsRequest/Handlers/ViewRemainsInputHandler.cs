using Inventory.Data.Enums;
using Inventory.Service.Entities.RemainsRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RemainsRequest.Handlers
{
    public class ViewRemainsInputHandler : IRequestHandler<ViewRemainsInputCommand, ViewRemainsOutputCommand>
    {

        private readonly IRemainsBussiness _RemainsBussiness;
        public ViewRemainsInputHandler(IRemainsBussiness RemainsBussiness)
        {
            _RemainsBussiness = RemainsBussiness;
        }

        public Task<ViewRemainsOutputCommand> Handle(ViewRemainsInputCommand request, CancellationToken cancellationToken)
        {
            ViewRemainsOutputCommand _ViewBookOutputCommand = new ViewRemainsOutputCommand();
           var RemainsEntity= _RemainsBussiness.ViewRemains(request.Id);
            _ViewBookOutputCommand.Id = RemainsEntity.Id;
            _ViewBookOutputCommand.Name = RemainsEntity.Name;
            _ViewBookOutputCommand.Description = RemainsEntity.Description;
            return Task.FromResult(_ViewBookOutputCommand);
        }
    }
}
