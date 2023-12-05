using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Commands
{
    public class CreateExecutionOrderAfterReviewCommand : IRequest<bool>
    {
        public CreateExecutionOrderAfterReviewCommand()
        {
            ExecutionOrderResultRemainList = new List<ExecutionOrderResultRemainModel>();
            ExecutionOrderResultItemList = new List<ExecutionOrderResultItemModel>();
            
        }
        public Guid ExecutionOrderId { get; set; }

        public List<ExecutionOrderResultRemainModel> ExecutionOrderResultRemainList { get; set; }
        public List<ExecutionOrderResultItemModel> ExecutionOrderResultItemList { get; set; }

        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }



    }



    public class ExecutionOrderResultRemainModel
    {
        public long RemainsId { get; set; }
        public Guid ExecutionOrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Note { get; set; }

        public int CurrencyId { get; set; }

    }


 public   class ExecutionOrderResultItemModel
    {
        public long BaseItemId { get; set; }
        public Guid ExecutionOrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Note { get; set; }

        public int CurrencyId { get; set; }
    }
}
