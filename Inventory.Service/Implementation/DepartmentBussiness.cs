using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
   public class DepartmentBussiness : IDepartmentBussiness
    {
        readonly private IRepository<Department, int> _DepartmentRepository;
        public DepartmentBussiness(IRepository<Department, int> DepartmentRepository)
        {
            _DepartmentRepository = DepartmentRepository;
        }
        public async Task<Department> AddNewDepartment(Department _Department)
        {
            _DepartmentRepository.Add(_Department);
            int added = await _DepartmentRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Department);
            else
                return await Task.FromResult<Department>(null);
        }

        public IQueryable<Department> GetAllDepartment()
        {
            return _DepartmentRepository.GetAll(true);
        }
     
        public IQueryable<Department> GetActiveDepartments()
        {
            var DepartmentList = _DepartmentRepository.GetAll();
            return DepartmentList;
        }

        public bool checkDeactivation(int DepartmentId)
        {
            var checkConnections = _DepartmentRepository.GetAll()
                .Where(x=>x.Id == DepartmentId)
                .Where(x => x.Employees.Any() || x.Invoice.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else 
            { 
                return true;
            }
        }
        public async Task<bool> Activate(int DepartmentId, bool ActivationType )
        {
            if (ActivationType)
                _DepartmentRepository.Activate(new Department() { Id = DepartmentId });
            else
                _DepartmentRepository.DeActivate(new Department() { Id = DepartmentId });

            var added = await _DepartmentRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> UpdateDepartment(Department _Department)
        {
            _DepartmentRepository.PartialUpdate(_Department,d=>d.Name);
            int added = await _DepartmentRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public Department ViewDepartment(int DepartmentId)
        {
            var DepartmentEntity = _DepartmentRepository.Get(DepartmentId);
            return DepartmentEntity;
        }
    }
}