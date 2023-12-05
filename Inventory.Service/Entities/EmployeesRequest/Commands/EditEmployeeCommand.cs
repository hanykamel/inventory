using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Inventory.Service.Entities.EmployeesRequest.Commands
{
   public class EditEmployeeCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CardCode { get; set; }
        public int? DepartmentId { get; set; }
    }
}
