using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.BookRequest.Commands
{
    public class EditBookCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Consumed { get; set; }
        [Required]
        public int BookNumber { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        public int StoreId { get; set; }
    }
}
