using System;
using System.Collections.Generic;
using System.Text;
using Inventory.Data.Entities;
using Inventory.Service.Interfaces;
using Inventory.Repository;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Service.Implementation
{
    public class EmployeeBussiness : IEmployeeBussiness
    {

        readonly private IRepository<Employees, int> _EmployeesRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public EmployeeBussiness(IRepository<Employees, int> EmployeesRepository, IStringLocalizer<SharedResource> Localizer)
        {
            _EmployeesRepository = EmployeesRepository;
            _Localizer = Localizer;
        }
        public async Task<Employees> AddNewEmployees(Employees _Employees)
        {
            _EmployeesRepository.Add(_Employees);
            int added = await _EmployeesRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Employees);
            else
                return await Task.FromResult<Employees>(null);
        }

        public IQueryable<Employees> GetAllEmployees()
        {
            var EmployeesList = _EmployeesRepository.GetAll(true);
            return EmployeesList;
        }

        public IQueryable<Employees> GetAllPrintEmployees()
        {
            var EmployeesList = _EmployeesRepository.GetAll(true).Include(e=>e.Department);
            return EmployeesList;
        }

        public IQueryable<Employees> GetActiveEmployees()
        {
            var EmployeesList = _EmployeesRepository.GetAll();
            return EmployeesList;
        }

        public bool checkDeactivation(int EmployeeId)
        {
            var checkConnections = _EmployeesRepository
                .GetAll()
                .Where( x => x.Id == EmployeeId)
                .Where( x => x.CommitteeEmployee.Any() || x.ExchangeOrder.Any()
                     || x.ExaminationRefundOrder.Any() || x.EmployeeRefundOrder.Any()
                     || x.Invoice.Any() ).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> Activate(int EmployeeId,bool ActivationType)
        {
            if (ActivationType)
                _EmployeesRepository.Activate(new Employees() { Id = EmployeeId });
            else
                _EmployeesRepository.DeActivate(new Employees() { Id = EmployeeId });

            var added = await _EmployeesRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new InvalidException(_Localizer["InvalidException"]);
        }

        public async Task<bool> UpdateEmployees(Employees _Employees)
        {
            _EmployeesRepository.PartialUpdate(_Employees, x => x.CardCode, x => x.Name, x => x.DepartmentId);
            int added = await _EmployeesRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public Employees ViewEmployees(int EmployeesId)
        {
            var EmployeesEntity = _EmployeesRepository.Get(EmployeesId);
            return EmployeesEntity;
        }

        public Employees GetEmployees(string CardNum)
        {
            var EmployeesEntity = _EmployeesRepository.GetFirst(e=>e.CardCode== CardNum);
            return EmployeesEntity;
        }

        public Employees IsCardCodeExist(String emp)
        {
            return _EmployeesRepository.GetAll(true).Where(e => e.CardCode == emp).AsNoTracking().FirstOrDefault();
        }
    }
}