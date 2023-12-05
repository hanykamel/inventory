using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class UnitBussiness : IUnitBussiness
    {

        readonly private IRepository<Unit, int> _UnitRepository;
        public UnitBussiness(IRepository<Unit, int> UnitRepository)
        {
            _UnitRepository = UnitRepository;
        }
        public bool checkDeactivation(int UnitId)
        {
            var checkConnections = _UnitRepository.GetAll()
                .Where(x => x.Id == UnitId)
                .Where(x => x.BaseItem.Any() || x.StoreItem.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        
        public async Task<bool> Activate(int UnitId,bool ActivationType)
        {
            if (ActivationType)
                _UnitRepository.Activate(new Unit() { Id = UnitId });
            else
                _UnitRepository.DeActivate(new Unit() { Id = UnitId });

            var added = await _UnitRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<Unit> AddNewUnit(Unit _Unit)
        {
            _UnitRepository.Add(_Unit);
            int added = await _UnitRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Unit);
            else
                return await Task.FromResult<Unit>(null);
        }

        public IQueryable<Unit> GetAllUnit()
        {
            return _UnitRepository.GetAll(true);
        }

        public IQueryable<Unit> GetActiveUnits()
        {
            return _UnitRepository.GetAll();
        }

        public Task<IQueryable<Unit>> GetByName(string Name)
        {
            return Task.FromResult(_UnitRepository.GetAll().Where(x=>x.Name ==Name));
        }

        public async Task<bool> UpdateUnit(Unit _Unit)
        {
            _UnitRepository.PartialUpdate(_Unit, x => x.Name);
            int added = await _UnitRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public Unit ViewUnit(int UnitId)
        {
            return _UnitRepository.Get(UnitId);
        }
    }
}
