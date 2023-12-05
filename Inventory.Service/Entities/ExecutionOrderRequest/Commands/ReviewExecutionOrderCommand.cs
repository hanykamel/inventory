using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Commands
{
   public class ReviewExecutionOrderCommand : IRequest<bool>
    {
        public Guid ExecutionOrderId { get; set; }
        public string NotesReview { get; set; }
        public List<Guid> storeitemReviewId { get; set; }
        public List<Guid> storeitemUnReviewId { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
    }



}

