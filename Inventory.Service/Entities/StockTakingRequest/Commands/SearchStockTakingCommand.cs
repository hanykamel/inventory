using Inventory.Data.Entities;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.RefundOrderVM;
using Inventory.Data.Models.StoreItemVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.StockTakingRequest.Commands
{
    public class SearchStockTakingCommand : IRequest<SearchStockTakingVM>
    {
        public int? BudgetId { get; set; }
        public int? BookNumberFrom { get; set; }
        public int? BookNumberTo { get; set; }
        public int? CurrancyId { get; set; }
        public int? BookPageNumberFrom { get; set; }
        public int? BookPageNumberTo { get; set; }
        public int? CategoryId { get; set; }
        public long? BaseItemId { get; set; } 
        public string Description { get; set; }
        public string DescriptionSearchCriteria { get; set; }
        public string BaseItemName { get; set; }
        public string baseItemSearchCriteria { get; set; }
        public bool? Consumed { get; set; }
        public bool? RandomSotckTaking { get; set; }
        public int? RandomSotckTakingNumber { get; set; }
        public int Take { get; set; } = 10;
        public int? Offset { get; set; } = 0;
        public int Count { get; set; } = 0;
        public string ContractNum { get; set; }
    }
}
