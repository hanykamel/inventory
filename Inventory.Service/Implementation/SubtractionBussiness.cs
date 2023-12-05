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
using inventory.Engines.CodeGenerator;
using Inventory.Data.Enums;

namespace Inventory.Service.Implementation
{
    public class SubtractionBussiness : ISubtractionBussiness
    {
        readonly private IRepository<Subtraction, Guid> _SubtractionRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        readonly private ITenantProvider _tenantProvider;
        private readonly ICodeGenerator _codeGenerator;
        public SubtractionBussiness(IRepository<Subtraction, Guid> subtractionRepository,
            IStringLocalizer<SharedResource> Localizer,ICodeGenerator codeGenerator,ITenantProvider tenantProvider)
        {
            _SubtractionRepository = subtractionRepository;
            _Localizer = Localizer;
            _tenantProvider = tenantProvider;
            _codeGenerator = codeGenerator;
        }
        public async Task<Subtraction> AddNewSubtraction(Subtraction _Subtraction)
        {
            _SubtractionRepository.Add(_Subtraction);
            int added = await _SubtractionRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Subtraction);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<Subtraction> GetAllSubtractions()
        {
            var SubtractionList = _SubtractionRepository.GetAll();
            return SubtractionList;
        }


        public bool CheckSubtractionExist(int subtractionNumber)
        {
            var count = _SubtractionRepository.GetAll().Where(a => a.SubtractionNumber == subtractionNumber).Count();
            if (count > 0)
                return true;
            else
                return false;

        }
        public IQueryable<Subtraction> GetActiveSubtractions()
        {
            var SubtractionList = _SubtractionRepository.GetAll();
            return SubtractionList;
        }



        public async Task<bool> Activate(Guid SubtractionId, bool ActivationType)
        {
            if (ActivationType)
                _SubtractionRepository.Activate(new Subtraction() { Id = SubtractionId });
            else
                _SubtractionRepository.DeActivate(new Subtraction() { Id = SubtractionId });

            var added = await _SubtractionRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public async Task<bool> UpdateSubtraction(Subtraction _Subtraction)
        {
            //_SubtractionRepository.PartialUpdate(_Subtraction, d => d.PageCount, d => d.Consumed, d => d.StoreId, d => d.SubtractionNumber);
            int added = await _SubtractionRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);

        }

        public Subtraction ViewSubtraction(Guid SubtractionId)
        {
            var SubtractionEntity = _SubtractionRepository.Get(SubtractionId);
            if (SubtractionEntity != null)
            {
                return SubtractionEntity;
            }
            else
                throw new InvalidException(_Localizer["InvalidException"]);
        }

        public int GetMax()
        {
            return _SubtractionRepository.GetMax(null, x => x.Serial) + 1;
        }
        public int GetMaxSubtractionNumber()
        {
            return _SubtractionRepository.GetMax(null,x => Convert.ToInt32(x.SubtractionNumber)) + 1;
        }
        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }
        public Subtraction PrepareSubtraction(Subtraction subtraction)
        {
            subtraction.Id = Guid.NewGuid();
            subtraction.Code = GetCode();
            subtraction.OperationId = (int)OperationEnum.Subtraction;
            subtraction.Serial = GetMax();
            return subtraction;
        }
    }
}