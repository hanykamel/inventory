using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.DeductionVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using inventory.Engines.CodeGenerator;
using System.Threading.Tasks;  
using Microsoft.Extensions.Localization;
using Inventory.CrossCutting.ExceptionHandling;

namespace Inventory.Service.Implementation
{
    public class DeductionBusiness: IDeductionBusiness
    {
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        readonly private IRepository<Deduction, Guid> _deductionRepository;
        readonly private IRepository<DeductionStoreItem, Guid> _deductionStoreItemRepository;
        private readonly ICodeGenerator _codeGenerator;
        readonly private IRepository<Budget, int> _budgetRepository;
        readonly private IStringLocalizer<SharedResource> _Localizer;



        public DeductionBusiness(IRepository<StoreItem, Guid> storeItemRepository,
            IRepository<Deduction, Guid> deductionRepository,
            IRepository<Budget, int> budgetRepository,
             IStringLocalizer<SharedResource> Localizer,
            IRepository<DeductionStoreItem, Guid> deductionStoreItemRepository, ICodeGenerator codeGenerator)
        {
            _storeItemRepository = storeItemRepository;
            _deductionRepository = deductionRepository;
            _deductionStoreItemRepository = deductionStoreItemRepository;
            _codeGenerator = codeGenerator;
            _budgetRepository = budgetRepository;
            _Localizer = Localizer;
        }


        public IQueryable<Deduction> GetAllDeduction()
        {
            return _deductionRepository.GetAll();
        }
        public IQueryable<Deduction> PrintDeductionsList()
        {
            return _deductionRepository.GetAll()
                .Include(d=>d.Budget)
                .Include(d=>d.Subtraction);
        }

        public IQueryable<Deduction> GetDeductionDetails()
        {

            var DeductionStoreItemsList = _deductionRepository.GetAll(true);
            return DeductionStoreItemsList;
        }

        public Deduction GetById(Guid id)
        {
            return _deductionRepository.GetAll().Where(a=>a.Id == id).
                Include(d=>d.Budget)
           .FirstOrDefault();
        }

        public List<BaseItemStoreItemVM> GetDamagedItemsByDeductionId(Guid id)
        {
            return _deductionStoreItemRepository.GetAll(true)
                .Include(d => d.Deduction)
                .ThenInclude(d=>d.Budget)
                .Include(d => d.Deduction)
                .ThenInclude(d => d.Subtraction)
                .Include(d => d.StoreItem)
                .ThenInclude(s => s.BaseItem)
                .Include(d => d.StoreItem)
                .ThenInclude(s => s.Unit)
                .Include(d => d.StoreItem)
                .ThenInclude(s => s.Currency)
                .Where(a=>a.DeductionId == id)
                .GroupBy(x => new
                {
                    BaseItemId = x.StoreItem.BaseItemId,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    UnitName = x.StoreItem.Unit.Name,
                    Price = x.StoreItem.Price,
                    BaseItemDesc = x.StoreItem.BaseItem.Description,
                    Currency = x.StoreItem.Currency.Name,
                    Note  = x.Note
                })
                .Select(a => new BaseItemStoreItemVM
                {
                    deduction = a.FirstOrDefault().Deduction,
                    BaseItemId = a.Key.BaseItemId,
                    BaseItemName = a.Key.BaseItemName,
                    UnitName = a.Key.UnitName,
                    Quantity = a.Count(),
                    UnitPrice = a.Key.Price,
                    FullPrice = a.Count() * a.Key.Price,
                    Currency = a.Key.Currency,
                    Notes = a.Key.Note
                }).ToList();
        }

        public IQueryable<DeductionVM> GetDeductedItems()
        {
            var DeductedStoreItems =
                _storeItemRepository.GetAll()
                .Include(x=>x.BaseItem)
                .Include(x=>x.Unit)
                .Include(x=>x.Addition)
               .Where(x => x.StoreItemStatusId == (int)StoreItemStatusEnum.Tainted )
                //    .Where(x => x.StoreItemStatusId == (int)StoreItemStatusEnum.Tainted && x.Addition.BudgetId == budgetId)
                .Select(x=> new
                {
                    baseItemId = x.BaseItemId,
                    baseItemName = x.BaseItem.Name,
                    unitName = x.Unit.Name,
                    unitId = x.UnitId,
                    PageNumber = x.BookPageNumber,
                    ExaminationCommitte = x.Addition.ExaminationCommitte,
                    itemStatus = x.StoreItemStatusId,
                    statusTained = x.StoreItemStatusId == (int)StoreItemStatusEnum.Tainted,
                    BudgetId = x.Addition.BudgetId,
                    CurrancyId=x.CurrencyId,
                })
                .GroupBy(
                    x =>new
                    {
                       baseItemId =   x.baseItemId,
                       baseItemName= x.baseItemName,
                        PageNumber = x.PageNumber,
                        ExaminationCommitte = x.ExaminationCommitte,
                        unitName = x.unitName,
                       unitId= x.unitId,          
                       itemStatus = x.itemStatus,
                        BudgetId = x.BudgetId,
                        CurrancyId=x.CurrancyId

                    }
                )
                .Select(x=> new DeductionVM
                {
                    PageNumber = x.Key.PageNumber,
                    ContractNumber = x.Key.ExaminationCommitte!=null? x.Key.ExaminationCommitte.ContractNumber!=null? x.Key.ExaminationCommitte.ContractNumber : "":"",
                    Id = x.Key.baseItemId,
                    BaseItemName = x.Key.baseItemName,
                    BaseItemId = x.Key.baseItemId,               
                    UnitId= x.Key.unitId,
                    UnitName = x.Key.unitName,
                    StoreItemStatusId = x.Key.itemStatus,
                    Count= x.Count(i=>i.statusTained),
                    BudgetId=x.Key.BudgetId,
                    CurrancyId=x.Key.CurrancyId,
                    Note=""
                });
                
            return DeductedStoreItems;
        }


        public List<DeductionstoreItemVM> getStoreItemId(List<long> baseItemIds)
        {
            return _storeItemRepository
                .GetAll()
                .Where(x=> baseItemIds.Contains(x.BaseItemId) 
                && x.StoreItemStatusId == (int)StoreItemStatusEnum.Tainted)
                .Select(x=>new DeductionstoreItemVM { storeItemId= x.Id,BaseItemId=x.BaseItemId }).ToList();
        }

       public IQueryable<StoreItem> GetStoreItems(long baseItemId)
        {
            var storeItems= 
             _storeItemRepository
            .GetAll().Include(x=>x.BaseItem).Include(x=>x.Unit)
            .Where(x => x.BaseItemId == baseItemId 
            && x.StoreItemStatusId == (int)StoreItemStatusEnum.Tainted);
            return storeItems;
        }

        public async Task<Guid> AddNewDeductItem(Deduction _Deduction)
        {
            _Deduction.Serial = GetMax();
            _deductionRepository.Add(_Deduction);
            int added = await _deductionRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Deduction.Id);
            else
                throw new NoChangesInEdit(_Localizer["NotSavedException"]);

        }
  

        public int GetMax()
        {
            return _deductionRepository.GetMax(null, x => x.Serial) + 1;

        }
        public string GetCode()
        {
            var serial = GetMax();
            return _codeGenerator.Generate(serial);
        }

        public string GetLastCode()
        {
            var lastAddedObj = _deductionRepository.GetAll().OrderByDescending(o => o.CreationDate).FirstOrDefault();
            if (lastAddedObj != null)
            {
                return lastAddedObj.Code;
            }
            return "";
        }
    }
}
