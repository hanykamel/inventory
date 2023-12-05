using System;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class ExecutionListVM
    {
        public string Code { get; set; }
        public int? SubtractionNum { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string ExchangeOrderStatusName { get; set; }
    }
}
