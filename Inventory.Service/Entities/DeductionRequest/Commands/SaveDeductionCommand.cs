using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DeductionRequest.Commands
{
   public  class SaveDeductionCommand : IRequest<Guid>
    {

        public string RequestCode { get; set; }

        public string RequesterName { get; set; }
        public string Notes { get; set; }
        public string SubtractionNumber { set; get; }
        public DateTime Date { set; get; }
        public DateTime RequestDate { set; get; }

        public int BudgetId { get; set; }

        public List<DeductItems> deductItems { set; get; }

    }
    public class DeductItems
    {
        //public long Id { get; set; }
        public string Code { get; set; }
        public string BaseItemName { get; set; }
        public long BaseItemId { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public string Note { get; set; }

        //    public Guid StoreItemId { set; get; }
    }
}
