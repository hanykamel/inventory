using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Service.Entities.EmployeesRequest.Commands
{
  public  class AddEmployeeCommand : IRequest<Employees>
    {
        [Required]
        public string Name { get; set; }
        public string CardCode { get; set; }
        public int? DepartmentId { get; set; }
    }
}
