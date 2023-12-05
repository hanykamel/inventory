using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.RemainsRequest.Commands
{
    public class ActivateRemainsCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool ActivationType { get; set; }
    }
}
