using Inventory.Data.Entities;
using Inventory.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Data.Models.AdditionVM
{
    public class AdditionRemainsItemVM
    {
        public Guid Id { get; set; }
        public long RemainsId { get; set; }
        public int? UnitId { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public int StoreItemStatusId { get; set; }
        public string Note { get; set; }
        public int CurrencyId { get; set; }
        public string RobbingName { get; set; }
        public decimal RobbingPrice { get; set; }
    }
}
