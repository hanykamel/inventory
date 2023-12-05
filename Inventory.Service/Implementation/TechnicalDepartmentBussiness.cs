using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class TechnicalDepartmentBussiness : ITechnicalDepartmentBussiness

    {

        readonly private IRepository<TechnicalDepartment, int> _TechnicalDepartmentRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public TechnicalDepartmentBussiness(IRepository<TechnicalDepartment, int> TechnicalDepartmentRepository,
            IStringLocalizer<SharedResource> Localizer)
        {
            _TechnicalDepartmentRepository = TechnicalDepartmentRepository;
            _Localizer = Localizer;
        }
        public async Task<TechnicalDepartment> AddNewTechnicalDepartment(TechnicalDepartment _TechnicalDepartment)
        {
            if (_TechnicalDepartment.TechnicianId != null)
            {
                _TechnicalDepartmentRepository.Add(_TechnicalDepartment);
                int added = await _TechnicalDepartmentRepository.SaveChanges();
                if (added > 0)
                    return await Task.FromResult(_TechnicalDepartment);
                else
                    throw new NotSavedException(_Localizer["NotSavedException"]);
            }
            else
            {
                throw new Exception();
            }
        }

        public IQueryable<TechnicalDepartment> GetAllTechnicalDepartment()
        {
            var TechnicalDepartmentList = _TechnicalDepartmentRepository.GetAll(true);
            return (TechnicalDepartmentList);
        }
        public IQueryable<TechnicalDepartment> GetActiveTechnicalDepartments()
        {
            var TechnicalDepartmentList = _TechnicalDepartmentRepository.GetAll();
            return (TechnicalDepartmentList);
        }

        public bool checkDeactivation(int TechDepartmnetId)
        {
            var checkConnections = _TechnicalDepartmentRepository.GetAll()
                .Where(x => x.Id == TechDepartmnetId && x.Store.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int TechnicalDepartmentId,bool ActivationType)
        {
            if (ActivationType)
                _TechnicalDepartmentRepository.Activate(new TechnicalDepartment() { Id = TechnicalDepartmentId });
            else
                _TechnicalDepartmentRepository.DeActivate(new TechnicalDepartment() { Id = TechnicalDepartmentId });

            var added = await _TechnicalDepartmentRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> UpdateTechnicalDepartment(TechnicalDepartment _TechnicalDepartment)
        {
            _TechnicalDepartmentRepository.PartialUpdate(_TechnicalDepartment, x => x.TechnicianId, x => x.Name,x=>x.AssistantTechnician);
            int added = await _TechnicalDepartmentRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public TechnicalDepartment ViewTechnicalDepartment(int TechnicalDepartmentId)
        {
            var TechnicalDepartment = _TechnicalDepartmentRepository.Get(TechnicalDepartmentId);
            return (TechnicalDepartment);
        }
    }
}
