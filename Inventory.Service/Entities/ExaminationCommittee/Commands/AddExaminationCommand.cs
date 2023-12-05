using Inventory.Data.Models.AttachmentVM;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExaminationCommittee.Commands
{
   public class AddExaminationCommand : IRequest<Guid>
    {

        public string ExaminationNum { get; set; }

        public DateTime ExaminationDate { get; set; }

        public int Budget { get; set; }

        public string ContractNum { get; set; }

        public DateTime ContractDate { get; set; }

        public int Supplier { get; set; }
        public int Currency { get; set; }

        public int ExternalEntity { get; set; }

        public int SupplierType { get; set; }

        public DateTime SupplyApprovalDate { get; set; }

        public string SupplyApprovalNumber { get; set; }

        public string SupplyOrderNumber { get; set; }

        public DateTime SupplyOrderDate { get; set; }

        public bool Examinationtype { get; set; }

        public List<members> Members { get; set; }

        public List<Category> AllCategory { get; set; }
        public List<AttachmentVM> Attachments { get; set; }
    }


    public class members
    {
        public Guid Id { get; set; }
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int JobId { get; set; }
        public bool IsHead { get; set; }
    }


    public class Category
    {
        public Guid itemId { get; set; }
        public int CategoryTypeId { get; set; }
        public string CategoryTypeName { get; set; }
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }
        public int UniId { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public int Accepted { get; set; }
        public int Refused { get; set; }
        public string Reasons { get; set; }
        public int ExaminationPercentage { get; set; }
    }
}
