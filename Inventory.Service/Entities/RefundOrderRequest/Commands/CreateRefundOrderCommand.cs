using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.RefundOrderVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.RefundOrderRequest.Commands
{
    public class CreateRefundOrderCommand : IRequest<Guid>
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public int RefundOrderEmployeeId { get; set; }
        [Required]
        public int ExaminationEmployeeId { get; set; }
        public string DirectOrderNotes { get; set; }
        public bool IsDirectOrder { get; set; }

        [Required]
        public List<RefundStoreItemVM> items { get; set; }

        public virtual ICollection<AttachmentVM> RefundAttachment { get; set; }
        
    }
}
