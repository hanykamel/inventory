using Inventory.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface ISubtractionBussiness
    {
        Task<Subtraction> AddNewSubtraction(Subtraction _Subtraction);
        Task<bool> UpdateSubtraction(Subtraction _Subtraction);
        Subtraction ViewSubtraction(Guid SubtractionId);
        Task<bool> Activate(Guid SubtractionId, bool ActivationType);
        IQueryable<Subtraction> GetAllSubtractions();
        IQueryable<Subtraction> GetActiveSubtractions();
        //int GetMax();
        //string GetCode();
        Subtraction PrepareSubtraction(Subtraction subtraction);
        int GetMaxSubtractionNumber();
        bool CheckSubtractionExist(int subtractionNumber);
    }
}
