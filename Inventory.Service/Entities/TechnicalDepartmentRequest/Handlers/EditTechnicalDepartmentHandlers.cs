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
    public class EditTechnicalDepartmentHandlers : IRequestHandler<EditTechnicalDepartmentCommand, bool>
    {
        private readonly ITechnicalDepartmentBussiness _ITechnicalDepartmentBussiness;
        public EditTechnicalDepartmentHandlers(ITechnicalDepartmentBussiness ITechnicalDepartmentBussiness)
        {
            _ITechnicalDepartmentBussiness = ITechnicalDepartmentBussiness;
        }
        public Task<bool> Handle(EditTechnicalDepartmentCommand request, CancellationToken cancellationToken)
        {
            TechnicalDepartment _TechnicalDepartment = new TechnicalDepartment();
            _TechnicalDepartment.Name = request.TechnicalDepartmentName;
            _TechnicalDepartment.TechnicianId = request.TechnicalId;
            _TechnicalDepartment.AssistantTechnician = request.SecandTechnicalId;
            _TechnicalDepartment.Id = request.Id;
            return _ITechnicalDepartmentBussiness.UpdateTechnicalDepartment(_TechnicalDepartment);
        }
    }
}
