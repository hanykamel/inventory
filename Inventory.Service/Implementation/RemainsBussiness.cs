using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.CrossCutting.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Service.Implementation
{
    public class RemainsBussiness : IRemainsBussiness
    {
        readonly private IRepository<Remains, long> _RemainsRepository;
        readonly private IRepository<RemainsDetails, Guid> _RemainsDetailsRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        readonly private ITenantProvider _tenantProvider;
        public RemainsBussiness(IRepository<Remains, long> RemainsRepository,
            IStringLocalizer<SharedResource> Localizer, ITenantProvider tenantProvider, IRepository<RemainsDetails, Guid> RemainsDetailsRepository)
        {
            _RemainsRepository = RemainsRepository;
            _Localizer = Localizer;
            _tenantProvider = tenantProvider;
            _RemainsDetailsRepository = RemainsDetailsRepository;
        }
        public async Task<Remains> AddNewRemains(Remains _Remains)
        {
            _RemainsRepository.Add(_Remains);
            int added = await _RemainsRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Remains);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<Remains> GetAllRemains()
        {
           var RemainsList = _RemainsRepository.GetAll(true);
            return RemainsList;
        }

        public IQueryable<Remains> GetActiveRemains()
        {
            var RemainsList = _RemainsRepository.GetAll();
            return RemainsList;
        }

        public IQueryable<Remains> CheckRemainsExistance(Remains _Remains)
        {
            if (_Remains.Id != 0)
                return _RemainsRepository.GetAll
                (b => b.Id != _Remains.Id && b.Name == _Remains.Name , true);
            else
                return _RemainsRepository.GetAll
                (b => b.Name == _Remains.Name, true);

        }

        public async Task<bool> Activate(int RemainsId, bool ActivationType)
        {
            if (ActivationType)
                _RemainsRepository.Activate(new Remains() { Id = RemainsId });
            else
                _RemainsRepository.DeActivate(new Remains() { Id = RemainsId });

            var added = await _RemainsRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public async Task<bool> UpdateRemains(Remains _Remains)
        {
            _RemainsRepository.PartialUpdate(_Remains, d => d.Name, d => d.Description,d=>d.Consumed);
            int added = await _RemainsRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);

        }

        public bool checkValidEdit(long RemainsId)
        {
            var checkConnections = _RemainsRepository.GetAll()
                .Where(x => x.Id == RemainsId && x.RemainsDetails.Any()).Count();

            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Remains ViewRemains(int RemainsId)
        {
            var RemainsEntity = _RemainsRepository.Get(RemainsId);
            if (RemainsEntity != null)
            {
                return RemainsEntity;
            }
            else
                throw new InvalidException(_Localizer["InvalidException"]);
        }



        public IQueryable<RemainsDetails> GetAllRemainsDetails()
        {
            return _RemainsDetailsRepository.GetAll();
        }
        public IQueryable<RemainsDetails> GetAllRemainsDetailsList()
        {
            return _RemainsDetailsRepository.GetAll().
                Include(r=>r.Remains)
                .Include(r=>r.Unit);
        }
    }
}