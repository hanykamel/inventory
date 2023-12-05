using Inventory.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IStoreBussiness
    {
        Task<Store> AddNewStore(Store _Store);
        Task<bool> UpdateStore(Store _Store);
        string GetStoreName(int _StoreId);
        Store ViewStore(int StoreId);
        Task<bool> Activate(int StoreId, bool ActivationType);
        bool checkDeactivation(int StoreId);
        IQueryable<Store> GetAllStore();
        IQueryable<Store> GetActiveStores();
        IQueryable<Store> GetTransformationStores();
        IQueryable<Store> GetRobbingStores();
        string GetAllStoreName(int storeId);
        IQueryable<Store> GetAllPrintStore();

        bool CheckIsRobbingStore();

    }
}
