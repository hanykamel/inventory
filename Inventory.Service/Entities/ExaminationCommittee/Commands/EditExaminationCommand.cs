using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExaminationCommittee.Commands
{
  public  class EditExaminationCommand : IRequest<EditExaminationCommandVM>
    {
        public Guid Id { get; set; }
        public string ExaminationNum { get; set; }

        public DateTime ExaminationDate { get; set; }

        public int Budget { get; set; }

        public string ContractNum { get; set; }

        public DateTime? ContractDate { get; set; }

        public int? Supplier { get; set; }

        public int? ExternalEntity { get; set; }

        public int SupplierType { get; set; }
        public int Currency { get; set; }
        public DateTime? SupplyApprovalDate { get; set; }

        public string SupplyApprovalNumber { get; set; }

        public string SupplyOrderNumber { get; set; }

        public DateTime? SupplyOrderDate { get; set; }

        public bool Examinationtype { get; set; }

        public List<members> Members { get; set; }

        public List<Category> AllCategory { get; set; }
        public List<AttachmentVM> Attachments { get; set; }
        public bool SaveData { get; set; }

        public List<Guid> FileDelete { get; set; }


    }
}
