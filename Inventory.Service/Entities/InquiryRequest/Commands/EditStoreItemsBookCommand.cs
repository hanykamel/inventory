using Inventory.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Service.Entities.InquiryRequest.Commands
{
    public class EditStoreItemsBookCommand : IRequest<bool>
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

        public List<BaseItemsBooksVM> BaseItems { get; set; }
    }

    public class BaseItemsBooksVM
    {
        public long BaseItemId { get; set; }
        [Required]
        public int NewBookId { get; set; }
        public int OldBookId { get; set; }
        public int? UnitId { get; set; }
        [Required]
        public int NewBookPageNumber { get; set; }
        public int OldBookPageNumber { get; set; }

    }
}