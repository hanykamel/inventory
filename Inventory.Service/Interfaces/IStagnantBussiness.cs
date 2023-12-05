using Inventory.Data.Entities;
using Inventory.Service.Entities.InquiryRequest.Commands;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IStagnantBussiness
    {
        Task<bool> saveStagnantStoreItem(Stagnant request);
    }
}
