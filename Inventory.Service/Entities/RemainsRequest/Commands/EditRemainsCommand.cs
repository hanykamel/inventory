using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.RemainsRequest.Commands
{
    public class EditRemainsCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Consumed { get; set; }
    }
}
