using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
namespace Inventory.Service.Entities.AdditionRequest.Commands
{
    public class EditAdditionCommand : IRequest<bool>
    { 
        public Guid AdditionId { get; set; }
        public Guid? ExaminationCommitteId { get; set; }
        public string ExaminationCode { get; set; }
        public Guid? RobbingOrderId { get; set; }
        public string InputRobbingOrderNumber { get; set; }
        public Guid? RequestId { get; set; }
        public string InputRequestNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int? AdditionNumber { get; set; }
        public string Code { get; set; }
        public int BudgetId { get; set; }
        public int StoreId { get; set; }
        public int? AdditionDocumentTypeId { get; set; }
        public string AdditionDocumentNumber { get; set; }
        public string Note { get; set; }
        [Required]
        public string RequesterName { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        public DateTime AdditionDocumentDate { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public virtual ICollection<AdditionItemVM> Items { get; set; }
        public List<Guid> FileDelete { get; set; }
    }
}
