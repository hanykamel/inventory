using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IItemCategoryBussiness
    {
        Task<ItemCategory> AddNewItemCategory(ItemCategory _ItemCategory);
        Task<bool> UpdateItemCategory(ItemCategory _ItemCategory);
        ItemCategory ViewItemCategory(int ItemCategoryId);
        Task<bool> Activate(int ItemCategoryId, bool ActivationType);

        bool checkDeactivation(int ItemCategoryId);

        IEnumerable<ItemCategory> GetAllItemCategory();
        IEnumerable<ItemCategory> GetActiveItemCategories();
    }
}
