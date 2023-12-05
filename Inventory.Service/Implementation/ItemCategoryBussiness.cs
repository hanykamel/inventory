using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Inventory.Service.Implementation
{
    public class ItemCategoryBussiness : IItemCategoryBussiness
    {

        readonly private IRepository<ItemCategory, int> _ItemCategoryRepository;
        public ItemCategoryBussiness(IRepository<ItemCategory, int> ItemCategoryRepository)
        {
            _ItemCategoryRepository = ItemCategoryRepository;
        }

        public bool checkDeactivation(int ItemCategoryId)
        {
            var checkConnections = _ItemCategoryRepository.GetAll().Where(x => x.Id == ItemCategoryId && x.BaseItem.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int ItemCategoryId, bool ActivationType)
        {
            if (ActivationType)
                _ItemCategoryRepository.Activate(new ItemCategory() { Id = ItemCategoryId });
            else
                _ItemCategoryRepository.DeActivate(new ItemCategory() { Id = ItemCategoryId });

            var added = await _ItemCategoryRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<ItemCategory> AddNewItemCategory(ItemCategory _ItemCategory)
        {
            _ItemCategoryRepository.Add(_ItemCategory);
            int added = await _ItemCategoryRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_ItemCategory);
            else
                return await Task.FromResult<ItemCategory>(null);
        }

        public IEnumerable<ItemCategory> GetAllItemCategory()
        {
            return _ItemCategoryRepository.GetAll(true) ;
        }

        public IEnumerable<ItemCategory> GetActiveItemCategories()
        {
            return _ItemCategoryRepository.GetAll();
        }

        public async Task<bool> UpdateItemCategory(ItemCategory _ItemCategory)
        {
            _ItemCategoryRepository.PartialUpdate(_ItemCategory, x => x.Name);
            int added = await _ItemCategoryRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public ItemCategory ViewItemCategory(int ItemCategoryId)
        {
            return _ItemCategoryRepository.Get(ItemCategoryId);
        }
    }
}
