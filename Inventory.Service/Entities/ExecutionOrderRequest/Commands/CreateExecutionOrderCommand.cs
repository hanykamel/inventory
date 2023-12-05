using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Commands
{
    public class CreateExecutionOrderCommand : IRequest<Guid>
    {
        public string Code { get; set; }
        public int? BudgetId { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public int? SubtractionNumber { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public List<ExecutionOrderBaseItem> ExecutionOrderBaseItemList { get; set; }
    }


    public class ExecutionOrderBaseItem
    {
        public Guid storeitemId { get; set; }
        public string Note { get; set; }
    }
}

