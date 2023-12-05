using MediatR;


namespace Inventory.Service.Entities.DepartmentRequest.Commands
{
  public  class ActivateDepartmentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public bool ActivationType { get; set; }
    }
}
