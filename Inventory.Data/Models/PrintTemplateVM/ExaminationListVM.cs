using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class ExaminationListVM
    {
        public String Code { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationStatusId { get; set; }
        public String ExaminationStatusName { get; set; }
        public int TenantId { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
