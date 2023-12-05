using MediatR;

namespace Inventory.Service.Entities.EmployeeRequest.Commands
{
  public  class ActivateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
