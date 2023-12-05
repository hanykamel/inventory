using inventory.Engines.CodeGenerator;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class TransformationRequestBussiness : ITransformationRequestBussiness
    {

        readonly private IRepository<Transformation, Guid> _transformationRepository;
        readonly private IRepository<TransformationStoreItem, Guid> _transformationStoreItemRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        ITenantProvider _tenantProvider;

        public TransformationRequestBussiness(IRepository<Transformation, Guid> TransformationRepository,
            ICodeGenerator codeGenerator,
             ITenantProvider tenantProvider,
            IStringLocalizer<SharedResource> Localizer,
            IRepository<TransformationStoreItem, Guid> TransformationStoreItemRepository)
        {
            _transformationRepository = TransformationRepository;
            _codeGenerator = codeGenerator;
            _localizer = Localizer;
            _tenantProvider = tenantProvider;
            _transformationStoreItemRepository = TransformationStoreItemRepository;
        } 
        public IQueryable<Transformation> GetAllTransformation()
        {
            return _transformationRepository.GetAll(x => x.IsActive == true, true);
        }

        public IQueryable<Transformation> GetAllTransformationView()
        {
            return _transformationRepository.GetAll(x => (x.ToStoreId == _tenantProvider.GetTenant() ||
            x.FromStoreId == _tenantProvider.GetTenant() ||
            _tenantProvider.GetTenant() == 0)
            && x.IsActive == true, true).Include(x=>x.Subtraction);
        }
        public IQueryable<Transformation> PrintTransformationsList()
        {
            return _transformationRepository
                .GetAll(x => (x.ToStoreId == _tenantProvider.GetTenant() || 
                                x.FromStoreId == _tenantProvider.GetTenant() ||
                                _tenantProvider.GetTenant() == 0) && 
                                x.IsActive == true, true)
                .Include(t => t.TransformationStatus)
                .Include(t => t.FromStore)
                .ThenInclude(t => t.TechnicalDepartment)
                .Include(t=>t.Subtraction);
        }
        public int GetMax()
        {
            return _transformationRepository.GetMax(null, x => x.Serial) + 1;
        }

        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }

        public string GetLastCode()
        {
            var lastAddedObj = _transformationRepository.GetAll().OrderByDescending(o => o.CreationDate).FirstOrDefault();
            if (lastAddedObj != null)
            {
                return lastAddedObj.Code;
            }
            return "";
        }

        public Transformation ViewTransformation(Guid TransformationId)
        {
            var Transformation = _transformationRepository.Get(TransformationId);
            return Transformation;
        }

        public void UpdateTransformationStatus(Guid requestId)
        {
            var Transformation = _transformationRepository.Get(requestId, true);
            if (Transformation.TransformationStatusId == (int)TransformationOrderStatusEnum.Added)
                throw new InvalidException(_localizer["transformationRequestAddedBefore"]);
            if (Transformation.TransformationStatusId == (int)TransformationOrderStatusEnum.Cancel)
                throw new InvalidException(_localizer["transformationRequestCanselBefore"]);
            Transformation.TransformationStatusId = (int)TransformationOrderStatusEnum.Added;
            _transformationRepository.Update(Transformation);
        }

        public async Task<bool> AddNewTransformation(Transformation transformation)
        {
            transformation.OperationId = (int)OperationEnum.Transformation;
            transformation.TransformationStatusId = (int)TransformationOrderStatusEnum.Requested;
            transformation.Id = Guid.NewGuid();
            transformation.FromStoreId = _tenantProvider.GetTenant();
            transformation.Serial = GetMax();
            transformation.Code = GetCode();

            _transformationRepository.Add(transformation);

            int added = await _transformationRepository.SaveChanges();
            if (added > 0) 
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_localizer["NotSavedException"]); ;
        }

        public Transformation GetById(Guid Id)
        {
            return _transformationRepository.GetAll(true)
                .Include(a=>a.Subtraction)
               .Include(a => a.Budget)
               .Include(a => a.TransformationStoreItem)
               .ThenInclude(s => s.StoreItem)
               .Include(e => e.FromStore)
               .ThenInclude(s => s.TechnicalDepartment)
               .Include(e => e.FromStore)
               .ThenInclude(s => s.RobbingBudget)
               .Where(e => e.Id == Id)
                .FirstOrDefault();
        }

        public bool CancelTransformation(Transformation transformation )
        {
            if (transformation != null)
                transformation.TransformationStatusId =
               (int)TransformationOrderStatusEnum.Cancel;
           

            return true;

        }



        public async Task<bool> Save()
        {

              int added = await _transformationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_localizer["NotSavedException"]);
    }


        public List<Guid> GetTransformationStoreItem(Guid Id)
        {
            return _transformationStoreItemRepository.GetAll().Where(x => x.TransformationId == Id).Select(x => x.StoreItemId).ToList();

         
        }
    }
}
