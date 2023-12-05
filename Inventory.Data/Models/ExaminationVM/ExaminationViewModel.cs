using System;
using System.Collections.Generic;


namespace Inventory.Data.Models.ExaminationVM
{
  public  class ExaminationViewModel
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

        public bool SaveData { get; set; }

        public bool Examinationtype { get; set; }
        public List<ItemsVM> AllItems { get; set; }
        public List<membersVM> Allmembers { get; set; }
        public List<EditAttachment> _Attachments { get; set; }
        public List<Guid> FileDelete { get; set; }
    }


    public class ItemsVM
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

    public class membersVM
    {
        public Guid Id { get; set; }
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int JobId { get; set; }
        public bool IsHead { get; set; }
    }

    public class EditAttachment
    {
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public long FileSize { get; set; }
    public string FileExtention { get; set; }
    public int AttachmentTypeId { get; set; }
}





    public class ExaminationItemResult
    {
        public string BaseItemName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsSuccess { get; set; }
        public long Id { get; set; }
        public string Message { get; set; }

    }


}
