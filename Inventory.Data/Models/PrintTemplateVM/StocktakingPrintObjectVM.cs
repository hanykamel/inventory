using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class StocktakingPrintObjectVM
    {
        public string Currency { get; set; }
        public List<FormNo6StoreItemVM> value { get; set; }
    }
}
