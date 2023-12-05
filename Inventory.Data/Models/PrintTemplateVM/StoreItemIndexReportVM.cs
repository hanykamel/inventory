using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class StoreItemIndexReportVM
    {
        public int BookNumber { get; set; }
        public int BookPageNumber { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class StoreItemIndexGroupVM
    {
        public int BookNumber { get; set; }
        public List<StoreItemIndexReportVM> Result { get; set; }
    }

}
