using Inventory.Data.Models.Inquiry;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.InquiryRequest.Commands
{
    public class InquiryBaseItemsCommand : IRequest<InquiryBaseItems>
    {
        public int? categoryId { get; set; }
        public int? baseItemId { get; set; }
        public bool? consumed { get; set; }
        public int? StoreItemStatus { get; set; }
        public int? budgetId { get; set; }
        public int? BookNumberFrom { get; set; }
        public int? BookNumberTo { get; set; }

        public int? BookPageNumberFrom { get; set; }
        public int? BookPageNumberTo { get; set; }
        public int? skip { get; set; }
        public int? take { get; set; }
    }
}
