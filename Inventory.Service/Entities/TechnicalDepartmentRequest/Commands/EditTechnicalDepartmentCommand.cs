using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Commands
{
   public class EditTechnicalDepartmentCommand : IRequest<bool>
    {

        public int Id { get; set; }
        public string TechnicalDepartmentName { get; set; }
        public string TechnicalId { get; set; }

        public string SecandTechnicalId { get; set; }
    }
}
