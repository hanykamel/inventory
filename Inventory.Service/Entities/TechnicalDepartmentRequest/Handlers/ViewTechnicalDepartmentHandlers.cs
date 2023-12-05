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
    public class ViewTechnicalDepartmentHandlers : IRequestHandler<ViewTechnicalDepartmentInputCommand, TechnicalDepartmentOutputCommands>
    {

        private readonly ITechnicalDepartmentBussiness _ITechnicalDepartmentBussiness;
        public ViewTechnicalDepartmentHandlers(ITechnicalDepartmentBussiness ITechnicalDepartmentBussiness)
        {
            _ITechnicalDepartmentBussiness = ITechnicalDepartmentBussiness;
        }
        public Task<TechnicalDepartmentOutputCommands> Handle(ViewTechnicalDepartmentInputCommand request, CancellationToken cancellationToken)
        {
            TechnicalDepartmentOutputCommands _entity = new TechnicalDepartmentOutputCommands();
            var TechnicalDepartment = _ITechnicalDepartmentBussiness.ViewTechnicalDepartment(request.Id);
            _entity.Id = TechnicalDepartment.Id;
            _entity.TechnicalDepartmentName = TechnicalDepartment.Name;
            _entity.IsActive = TechnicalDepartment.IsActive;

            return Task.FromResult(_entity);

        }
    }
}
