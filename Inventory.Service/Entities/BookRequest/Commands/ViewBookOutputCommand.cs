using System.ComponentModel.DataAnnotations;


namespace Inventory.Service.Entities.BookRequest.Commands
{
    public class ViewBookOutputCommand
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        public bool Consumed { get; set; }
        [Required]
        public int PageCount { get; set; }
    }
}
