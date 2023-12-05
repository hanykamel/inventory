using Inventory.Service.Entities.TechnicalDepartmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Handlers
{
    public class GetAllTechnicalDepartmentHandler : IRequestHandler<GetAllTechnicalDepartmentCommand, List<TechnicalDepartmentOutputCommands>>
    {


        private readonly ITechnicalDepartmentBussiness _ITechnicalDepartmentBussiness;
        public GetAllTechnicalDepartmentHandler(ITechnicalDepartmentBussiness ITechnicalDepartmentBussiness)
        {
            _ITechnicalDepartmentBussiness = ITechnicalDepartmentBussiness;
        }
        public Task<List<TechnicalDepartmentOutputCommands>> Handle(GetAllTechnicalDepartmentCommand request, CancellationToken cancellationToken)
        {
            List<TechnicalDepartmentOutputCommands> _ViewTechnicalDepartmentOutputCommands = new List<TechnicalDepartmentOutputCommands>();
            var AllTechnicalDepartment = _ITechnicalDepartmentBussiness.GetAllTechnicalDepartment();
            foreach (var item in AllTechnicalDepartment)
            {
                TechnicalDepartmentOutputCommands _entity = new TechnicalDepartmentOutputCommands();

                _entity.Id = item.Id;
                _entity.TechnicalDepartmentName = item.Name;
                _entity.IsActive = item.IsActive;

                _ViewTechnicalDepartmentOutputCommands.Add(_entity);
            }
            return Task.FromResult(_ViewTechnicalDepartmentOutputCommands);
        }
    }
}
