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
    public class SupplierBussiness : ISupplierBussiness
    {
        readonly private IRepository<Supplier, int> _SupplierRepository;
        public SupplierBussiness(IRepository<Supplier, int> SupplierRepository)
        {
            _SupplierRepository = SupplierRepository;
        }

        public async Task<bool> Activate(int SupplierId, bool ActivationType)
        {
            if (ActivationType)
                _SupplierRepository.Activate(new Supplier() { Id = SupplierId });
            else
                _SupplierRepository.DeActivate(new Supplier() { Id = SupplierId });

            var added = await _SupplierRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public bool checkDeactivation(int SupplierId)
        {
            var checkConnections = _SupplierRepository
                .GetAll().Where(x => x.Id == SupplierId && x.ExaminationCommitte.Any()).Count();

            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<Supplier> AddNewSupplier(Supplier _Supplier)
        {
            _SupplierRepository.Add(_Supplier);
            int added = await _SupplierRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Supplier);
            else
                return await Task.FromResult<Supplier>(null);
        }

        public IQueryable<Supplier> GetAllSupplier()
        {
            var SupplierList = _SupplierRepository.GetAll(true);
            return SupplierList;
        }
        public IQueryable<Supplier> GetActives()
        {
            var SupplierList = _SupplierRepository.GetAll();
            return SupplierList;
        }

        public async Task<bool> UpdateSupplier(Supplier _Supplier)
        {
            _SupplierRepository.PartialUpdate(_Supplier,s=>s.Name);
            int added = await _SupplierRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public Supplier ViewSupplier(int SupplierId)
        {
            var StoreEntity = _SupplierRepository.Get(SupplierId);
            return StoreEntity;
        }
    }
}