using Inventory.Data.Enums;

namespace Inventory.Service.Entities.StoreRequest.Commands
{
    public class ViewStoreOutputCommand
    {
        public string storeCode { get; set; }
        public StoreTypeEnum storeType { get; set; }
        public string RobbingBudget { get; set; }
        public int? RobbingBudgetId { get; set; }
        public int? TechnicalDepartmentId { get; set; }
        public string TechnicalDepartment { get; set; }
    }
}
