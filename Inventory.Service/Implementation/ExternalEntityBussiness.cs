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
    public class ExternalEntityBussiness : IExternalEntityBussiness
    {
        readonly private IRepository<ExternalEntity, int> _ExternalEntityRepository;
        public ExternalEntityBussiness(IRepository<ExternalEntity, int> ExternalEntityRepository)
        {
            _ExternalEntityRepository = ExternalEntityRepository;
        }

        public bool checkDeactivation(int ExternalEntityId)
        {
            var checkConnections = _ExternalEntityRepository.GetAll()
                .Where(x => x.Id == ExternalEntityId)
                .Where(x => x.ExaminationCommitte.Any() || x.Transformation.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int ExternalEntityId,bool ActivationType)
        {
            if (ActivationType)
                _ExternalEntityRepository.Activate(new ExternalEntity() { Id = ExternalEntityId });
            else
                _ExternalEntityRepository.DeActivate(new ExternalEntity() { Id = ExternalEntityId });

            var added = await _ExternalEntityRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<ExternalEntity> AddNewExternalEntity(ExternalEntity _ExternalEntity)
        {
            _ExternalEntityRepository.Add(_ExternalEntity);
            int added = await _ExternalEntityRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_ExternalEntity);
            else
                return await Task.FromResult<ExternalEntity>(null);
        }

        public IQueryable<ExternalEntity> GetAllExternalEntity()
        {
            var ExternalEntityList = _ExternalEntityRepository.GetAll(true);
            return ExternalEntityList;
        }
        public IQueryable<ExternalEntity> GetActiveExternalEntities()
        {
            var ExternalEntityList = _ExternalEntityRepository.GetAll();
            return ExternalEntityList;
        }
        public async Task<bool> UpdateExternalEntity(ExternalEntity _ExternalEntity)
        {
            _ExternalEntityRepository.PartialUpdate(_ExternalEntity, x => x.Name);
            int added = await _ExternalEntityRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public ExternalEntity ViewExternalEntity(int ExternalEntityId)
        {
            var StoreEntity = _ExternalEntityRepository.Get(ExternalEntityId);
            return StoreEntity;
        }
    }
}
