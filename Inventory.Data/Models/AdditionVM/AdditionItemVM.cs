using Inventory.Data.Entities;
using Inventory.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Data.Models.AdditionVM
{
    public class AdditionItemVM
    {
        public bool Checked { get; set; }
        [Required]
        public int BaseItemId { get; set; }


        public int UnitId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Guid Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public long BookId { get; set; }
        [Required]
        public int BookPageNumber { get; set; }
        [Required]
        public int StoreItemStatusId { get; set; }
        public string Note { get; set; }
        public int? CurrencyId { get; set; }
        public ICollection<StoreItemVM> StoreItems { get; set; }
        //public string RobbingName { get; set; }
        public decimal RobbingPrice { get; set; }
        //public int? RobbingStoreItemStatusId { get; set; }
    }
}
