using Inventory.Data.Entities;
using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.DeductionVM
{
    public class DeductionVM 
    {

        public long Id { get; set; }
        public string Code { get; set; }

        public string BaseItemName { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public string UnitName { get; set; }

        public int? UnitId { get; set; }
        public long BaseItemId { get; set; }
        //    public Guid StoreItemId { get; set; }
        public int BudgetId { get; set; }
        public int StoreItemStatusId { get; set; }
        public int Count { get; set; }
        public int? CurrancyId { get; set; }

        public DateTime CreationDate { set; get; }
        public string Note { get; set; }
    }

    public class DeductionstoreItemVM
    {

        public long BaseItemId { get; set; }

        public Guid storeItemId { get; set; }
    }


}
