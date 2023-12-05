using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface IUnitBussiness
    {
        Task<Unit> AddNewUnit(Unit _Unit);
        Task<bool> UpdateUnit(Unit _Unit);
        Unit ViewUnit(int UnitId);
        Task<bool> Activate(int UnitId, bool ActivationType);
        bool checkDeactivation(int UnitId);

        IQueryable<Unit> GetAllUnit();

        IQueryable<Unit> GetActiveUnits();
        Task<IQueryable<Unit>> GetByName(string Name);
    }
}
