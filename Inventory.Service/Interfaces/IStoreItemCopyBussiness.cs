using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.BaseItem;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.InquiryRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IStoreItemCopyBussiness
    {
        List<FormNo6StoreItemVM> GetStocktackingStoreItems(List<Guid> storeItems);
    }
}
