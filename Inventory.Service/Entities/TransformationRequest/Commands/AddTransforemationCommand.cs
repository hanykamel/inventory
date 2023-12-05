using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TransformationRequest.Commands
{
  public  class AddTransforemationCommand : IRequest<Guid>
    {
        public int? ToStoreId { get; set; }
        public string Code { get; set; }
        public int? BudgetId { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public int? SubtractionNumber { get; set; }
        public virtual ICollection<AttachmentVM> AdditionAttachment { get; set; }
        public List<TransformationBaseItem> TransformationBaseItemList { get; set; }
    }

     
    public class TransformationBaseItem
    {
         public long BaseItemId { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
        public int StatusId { get; set; }
        public string ContractNum { get; set; }
        public int PageNum { get; set; }
    }
}
