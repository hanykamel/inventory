using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.RobbingOrderRequest.Commands
{
  public  class AddRobbingOrderCommand : IRequest<Guid>
    {
        public int? ToStoreId { get; set; }
        public string Code { get; set; }
        public int? BudgetId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int? SubtractionNumber { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public List<RobbingOrderBaseItem> RobbingOrderBaseItemList { get; set; }
    }


    public class RobbingOrderBaseItem
    {
        public Guid storeitemId { get; set; }
        public string Note { get; set; }
        public decimal price { get; set; }
        public int RobbingStoreItemStatusId { get; set; }
        public string ExaminationReport { get; set; }
        
        //public int Count { get; set; }
    }
}

