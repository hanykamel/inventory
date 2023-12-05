using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class DistributeStoreItemVM
    {
        public int Id { get; set; }

        public Guid InvoiceID { get; set; }
        public int DepartmentID { get; set; }
        public string InvoiceName { get; set; }

       

        public int Count  { get; set; }

        public int ItemsNumber { get; set; }
        public string receiverName { get; set; }
        public int amount { get; set; }
        public string location { get; set; }

        public string notes { get; set; }
        public DateTime CreationDate { get; set; }

        public int TotalExpended { get; set; }

        public int RemainingInStore { get; set; }
    }

    
}
