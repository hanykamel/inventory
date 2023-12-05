using Inventory.Service.Entities.RemainsRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RemainsRequest.Handlers
{
    public class GetAllRemainsHandler : IRequestHandler<GetAllRemainsCommand, List<ViewRemainsOutputCommand>>
    {

        private readonly IRemainsBussiness _RemainsBussiness;
        public GetAllRemainsHandler(IRemainsBussiness RemainsBussiness)
        {
            _RemainsBussiness = RemainsBussiness;
        }
        public Task<List<ViewRemainsOutputCommand>> Handle(GetAllRemainsCommand request, CancellationToken cancellationToken)
        {
            List<ViewRemainsOutputCommand> _ViewRemainsOutputCommand = new List<ViewRemainsOutputCommand>();
            var BookList = _RemainsBussiness.GetAllRemains();
            foreach (var item in BookList)
            {
                ViewRemainsOutputCommand _entity = new ViewRemainsOutputCommand();

                _entity.Name = item.Name;
                _entity.Id = item.Id;
                _entity.Description = item.Description;
                _ViewRemainsOutputCommand.Add(_entity);
            }
            return Task.FromResult(_ViewRemainsOutputCommand);

        }
    }
}
