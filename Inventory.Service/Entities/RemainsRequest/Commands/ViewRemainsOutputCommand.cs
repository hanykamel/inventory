using System.ComponentModel.DataAnnotations;


namespace Inventory.Service.Entities.RemainsRequest.Commands
{
    public class ViewRemainsOutputCommand
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
