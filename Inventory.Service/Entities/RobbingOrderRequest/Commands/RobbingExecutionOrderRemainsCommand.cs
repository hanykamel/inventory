using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.RobbingOrderRequest.Commands
{
  public  class RobbingExecutionOrderRemainsCommand : IRequest<Guid>
    {
        public int? ToStoreId { get; set; }
        public string Code { get; set; }
        public string SubtractionNumber { get; set; }
        public int? BudgetId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public List<RobbingOrderRemainItem> RobbingOrderRemainItems { get; set; }
    }


    public class RobbingOrderRemainItem
    {
        public Guid ExecutionOrderResultRemainId { get; set; }
        public string Notes { get; set; }
        public decimal price { get; set; }
        public string ExaminationReport { get; set; }
    }
}
