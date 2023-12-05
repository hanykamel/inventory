using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IRemainsBussiness
    {
        Task<Remains> AddNewRemains(Remains _Remains);
        Task<bool> UpdateRemains(Remains _Remains);
        Remains ViewRemains(int RemainsId);
        Task<bool> Activate(int BookId, bool ActivationType);
        IQueryable<Remains> GetAllRemains();
        IQueryable<Remains> GetActiveRemains();
        bool checkValidEdit(long RemainsId);
        IQueryable<Remains> CheckRemainsExistance(Remains _Remains);
        IQueryable<RemainsDetails> GetAllRemainsDetails();
        IQueryable<RemainsDetails> GetAllRemainsDetailsList();
    }
}
