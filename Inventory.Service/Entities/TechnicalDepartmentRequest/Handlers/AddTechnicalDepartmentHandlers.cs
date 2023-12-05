using Inventory.Data.Entities;
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
    public class AddTechnicalDepartmentHandlers : IRequestHandler<AddTechnicalDepartmentCommand, TechnicalDepartment>
    {

        private readonly ITechnicalDepartmentBussiness _ITechnicalDepartmentBussiness;
        public AddTechnicalDepartmentHandlers(ITechnicalDepartmentBussiness ITechnicalDepartmentBussiness)
        {
            _ITechnicalDepartmentBussiness = ITechnicalDepartmentBussiness;
        }
        public Task<TechnicalDepartment> Handle(AddTechnicalDepartmentCommand request, CancellationToken cancellationToken)
        {
            TechnicalDepartment _TechnicalDepartment = new TechnicalDepartment();
            _TechnicalDepartment.Name = request.TechnicalDepartmentName;
            _TechnicalDepartment.TechnicianId = request.TechnicalId;
            _TechnicalDepartment.AssistantTechnician = request.SecandTechnicalId;
            return _ITechnicalDepartmentBussiness.AddNewTechnicalDepartment(_TechnicalDepartment);
        }
    }
}
