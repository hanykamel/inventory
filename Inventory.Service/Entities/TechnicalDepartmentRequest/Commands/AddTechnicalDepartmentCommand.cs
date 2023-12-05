using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Commands
{
 public   class AddTechnicalDepartmentCommand : IRequest<TechnicalDepartment>
    {
        [Required]
        public string TechnicalDepartmentName { get; set; }
        [Required]
        public string TechnicalId { get; set; }
        public string SecandTechnicalId { get; set; }
    }
}
