using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
namespace Inventory.Service.Entities.ExchangeOrderRequest.Commands
{
    public class SearchStoreItemsExchangeOrderCommand : IRequest<ExchangeorderStoreItemVM>
    {
        //public int? skipe { get; set; }
        //public int? take { get; set; }
        public int? BudgetId { get; set; }
        public string ContractNumber { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemCategoryId { get; set; }
        public long? BaseItemId { get; set; }

        public List<Guid> SelectStoreItemId { get; set; }

        // public int? statusitem { get; set; }
    }

    public class ExchangeorderStoreItemVM 
    {
        public int itemAvailable { get; set; }
        public int itemUnAvailable { get; set; }
        public int count { get; set; }
        public int ExpensesItem { get; set; }
        public List<StoreItemVM> AllStoreItemVM { get; set; }
    }


}
