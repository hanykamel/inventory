using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
namespace Inventory.Service.Entities.AdditionRequest.Commands
{
    public class CreateAdditionCommand : IRequest<Guid>
    { 
        public Guid AdditionId { get; set; }
        public Guid? ExaminationCommitteId { get; set; }
        public Guid? ExecutionOrderId { get; set; }
        public string ExaminationCode { get; set; }
        public Guid? RobbingOrderId { get; set; }
        public string InputRobbingOrderNumber { get; set; }
        public Guid? RequestId { get; set; }
        public string InputRequestNumber { get; set; }
        public int? CurrencyId { get; set; }

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
        [Required]
        public DateTime Date { get; set; }
        public int? AdditionNumber { get; set; }
        public DateTime AdditionDocumentDate { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public virtual ICollection<AdditionItemVM> Items { get; set; }
        public virtual ICollection<AdditionRemainsItemVM> RemainsItems { get; set; }
        public virtual ICollection<RobbingStoreItemVm> RobbingStoreItem { get; set; }


        public bool ValidateCreate()
        {
            //if (RequestDate == default(DateTime) || RequestDate > DateTime.Now)
            //    return false;
            return true;
        }



    }

  public  class RobbingStoreItemVm
    {
        public long BaseItemId { get; set; }
        public long StoreBaseItemId { get; set; }
        public int StoreItemStatusId { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string Note { get; set; }
        public int UnitId { get; set; }
        public int CurrencyId { get; set; }
    }
}
