using Inventory.Data.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.StoreRequest.Commands
{
    public class AddStoreCommand : IRequest<Store>
    {
        [Required]
        public string StoreCode { get; set; }
        [Required]
        public int StoreTypeId { get; set; }
        [Required]
        public string Admin { get; set; }
        public int? RobbingBudgetId { get; set; }
        public int? TechnicalDepartmentId { get; set; }
    }
}
