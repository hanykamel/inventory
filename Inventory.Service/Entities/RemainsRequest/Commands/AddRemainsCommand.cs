using Inventory.Data.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.RemainsRequest.Commands
{
    public class AddRemainsCommand : IRequest<Remains>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Consumed { get; set; }

    }
}
