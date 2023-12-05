using Inventory.Data.Entities;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.RefundOrderVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Service.Entities.StockTakingRequest.Commands
{
    public class CreateStockTakingCommand : IRequest<Guid>
    {
        //  public List<BaseItem> BaseItems { get; set; }

        public string Date { get; set; }
        public List<BaseItemsVM> BaseItems { get; set; }
        public List<AttachmentVM> AdditionAttachment { get; set; }
    }
    public class BaseItemsVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UnitName { get; set; }
        //public string StoreItemStatus { get; set; }
        public int? UnitId { get; set; }
        public int Price { get; set; }
        public int BookPageNumber { get; set; }
        public int BookNumber { get; set; }
        public int BookId { get; set; }

        public string ContractNum { get; set; }

        public bool Isrobbing { get; set; }


    }
}
