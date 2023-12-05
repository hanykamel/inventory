namespace Inventory.Data.Models.AdditionVM
{
    public class BaseItemVm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool Consumed { get; set; }
        public int DefaultUnitId { get; set; }
        public int? WarningLevel { get; set; }
        public int ItemCategoryId { get; set; }
    }
}
