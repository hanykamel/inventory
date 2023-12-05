using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Commands
{
   public class ActivateTechnicalDepartmentCommand: IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
