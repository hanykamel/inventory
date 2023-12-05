using Inventory.Data.Enums;
using System;

namespace Inventory.Data.Models.ExchangeOrderVM
{
    public class StoreItemExchangesVM
    {
        public OperationEnum Operation{ get; set; }
        public string OperatedTo{ get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
    }

}

