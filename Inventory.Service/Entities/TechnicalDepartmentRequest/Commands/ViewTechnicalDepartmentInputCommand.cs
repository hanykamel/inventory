using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Commands
{
   public class ViewTechnicalDepartmentInputCommand : IRequest<TechnicalDepartmentOutputCommands>
    {

        public int Id { get; set; }
    }
}
