using inventory.Engines.LdapAuth;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Identity;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Delegation;
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
   public class DelegationBussiness: IDelegationBussiness
    {
        readonly private IRepository<Delegation, Guid> _DelegationRepository;

        readonly private IRepository<ExchangeOrderStatusTracker, Guid> _exchangeOrderStatusTrackerRepository;
        readonly private IRepository<ExecutionOrderStatusTracker, Guid> _executionOrderStatusTrackerRepository;
        readonly private IRepository<RefundOrderStatusTracker, Guid> _refundOrderStatusTrackerRepository;
        readonly private IRepository<TechnicalDepartment, int> _technicalDepartmentRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        readonly private IRepository<Shift, int> _shiftRepository;
        readonly private IRepository<Store, int> _storeRepository;
       private readonly IIdentityProvider _identityProvider;

        private readonly ILdapAuthenticationService _LdapAuthenticationService;
        public DelegationBussiness(IRepository<Delegation, Guid> DelegationRepository, 
            IStringLocalizer<SharedResource> Localizer, IRepository<Shift, int> shiftRepository,
            IRepository<Store, int> storeRepository,
            IRepository<ExchangeOrderStatusTracker, Guid> exchangeOrderStatusTrackerRepository,
            IRepository<ExecutionOrderStatusTracker, Guid> executionOrderStatusTrackerRepository,
            IRepository<RefundOrderStatusTracker, Guid> refundOrderStatusTrackerRepository,
            IRepository<TechnicalDepartment, int> technicalDepartmentRepository,
            IIdentityProvider identityProvider,
         ILdapAuthenticationService LdapAuthenticationService
            )
        {
            _DelegationRepository = DelegationRepository;
            _Localizer = Localizer;
           _shiftRepository = shiftRepository;
            _storeRepository = storeRepository;
            _exchangeOrderStatusTrackerRepository = exchangeOrderStatusTrackerRepository;
            _executionOrderStatusTrackerRepository = executionOrderStatusTrackerRepository;
            _refundOrderStatusTrackerRepository = refundOrderStatusTrackerRepository;
            _technicalDepartmentRepository = technicalDepartmentRepository;
            _identityProvider = identityProvider;
            _LdapAuthenticationService = LdapAuthenticationService;

    }

        public IQueryable<Delegation> GetAllDelegation()
        {
            return _DelegationRepository.GetAll();
        }

        public IQueryable<Delegation> GetAllDelegationView()
        {
            return _DelegationRepository.GetAll(true);
        }
        public IQueryable<Delegation> GetAllDelegationList()
        {
            return _DelegationRepository.GetAll()
                .Include(d => d.DelegationStore);
        }

        public async Task<Delegation> SaveDelegation(Delegation delegation)
        {
            _DelegationRepository.Add(delegation);
            int added = await _DelegationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(delegation);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }


        public IQueryable<Delegation> checkDelegation(Delegation request, List<int> storeid)
        {
            var result = _DelegationRepository.GetAll(true).Where(x =>x.DelegationTypeId== (int)DelegationTypeEnum.StoreAdmin && x.IsSuspended != true &&
                                                                    ((request.DateFrom.Date >= x.DateFrom.Date && request.DateFrom.Date <= x.DateTo.Date) ||
                                                                   (request.DateTo.Date <= x.DateTo.Date && request.DateTo.Date >= x.DateFrom.Date))&&
                                                                   ((request.TimeFrom >= x.TimeFrom && request.TimeFrom < x.TimeTo)||
                                                                   (request.TimeTo <= x.TimeTo && request.TimeTo > x.TimeFrom))&&
                                                                   x.DelegationStore.Any(d => storeid.Contains(d.StoreId)) 
                                                                 );



            if (result.Any())
            {
                result= result.Include(x => x.DelegationStore).ThenInclude(d => d.Store).ThenInclude(s => s.RobbingBudget)
                    .Include(x => x.DelegationStore).ThenInclude(d => d.Store).ThenInclude(s => s.TechnicalDepartment)
                     .Include(x => x.DelegationStore).ThenInclude(d => d.Store).ThenInclude(t=>t.StoreType);
            }

            return result;
        }


        public void checkDelegationTechnican(Delegation request, List<int> storeId)
        {
            var result = _DelegationRepository.GetAll(true).Where(x => x.UserName == request.UserName
            && x.TimeFrom == request.TimeFrom
            && x.TimeTo == request.TimeTo
            && x.DateFrom == request.DateFrom
            && x.DateTo == request.DateTo
            && x.DelegationStore.Any(d => storeId.Contains(d.StoreId))
            && x.IsSuspended==false);
            if (result.Any())
            {
                throw new NotSavedException(_Localizer["checkDelegationTechnican"]);
            }

        }

        public async Task<Guid> EditDelegation(Delegation delegation)
        {
            _DelegationRepository.Add(delegation);
            int added = await _DelegationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(delegation.Id);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<Shift> GetAllShift()
        {
            return _shiftRepository.GetAll();
        }


        public List<StoreDelegationVM> GetStore()
        {
            var tenants = _storeRepository.GetAll(x => x.IsActive == true && x.TechnicalDepartment.IsActive == true, true)
                .Include(y => y.TechnicalDepartment)
                .Include(y => y.RobbingBudget)
                .Include(y => y.StoreType)

                .Select(y => new StoreDelegationVM
                {
                    Id = y.Id,
                    Name = y.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                      (y.RobbingBudget != null ?
                  (y.StoreType.Name + " " + y.RobbingBudget.Name) : "") :
                    (y.TechnicalDepartment != null ?
                    y.StoreType.Name + " " + y.TechnicalDepartment.Name : ""),
                    userName=y.AdminId

                }).ToList().Distinct(new TenantComparer()).ToList();


            return tenants;

        }

        public async Task<bool> StopDelegation(Guid DelegationId)
        {
            var delegation = _DelegationRepository.Get(DelegationId);
            var today =DateTime.Now;
            if (delegation.DateTo.Date < today.Date ||(delegation.DateTo.Date == today.Date&& delegation.TimeTo <= today.TimeOfDay) )
            {
                throw new NotSavedException(_Localizer["StopDelegationError"]);
            }
            delegation.IsSuspended = true;
            _DelegationRepository.Update(delegation);
            int added = await _DelegationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<DelegationTrack> GetDelegationTrack()
        {
             //var techinical = _technicalDepartmentRepository.GetAll().Select(x => x.TechnicianId).Distinct();
            var techinical = _LdapAuthenticationService.GetTechnicanUserNames("").Select(x=>x.Username);

            var storeTechnicalId=_storeRepository.GetAll()
                .Include(o=>o.TechnicalDepartment)
                .FirstOrDefault().TechnicalDepartment.TechnicianId;

            IQueryable<DelegationTrack> exchangeOrderStatusTracker = _exchangeOrderStatusTrackerRepository
                .GetAll(x => x.CreatedBy != storeTechnicalId &&
                techinical.Contains(x.CreatedBy)).Include(x => x.ExchangeOrderStatus).Include(x=>x.ExchangeOrder).ThenInclude(e=>e.Operation).Select(x=>new DelegationTrack {
                Id=x.Id,
                CreationDate=x.CreationDate,
                CreationTime= new TimeSpan(x.CreationDate.Hour, x.CreationDate.Minute, x.CreationDate.Second),
                DelegationUserName=x.CreatedBy,
                Operation=( x.ExchangeOrderStatusId == (int)ExchangeOrderStatusEnum.Pending ? _Localizer["createOrder"] : x.ExchangeOrderStatusId == (int)ExchangeOrderStatusEnum.Reviewed ? _Localizer["reviewOrder"] : x.ExchangeOrderStatusId == (int)ExchangeOrderStatusEnum.Canceled ? _Localizer["cancalorder"] : _Localizer["ExchangeOrder"]) + " " + x.ExchangeOrder.Operation.Name,
                TenantId = x.TenantId,
                OperationNum = x.ExchangeOrder.Code

            } );
            IQueryable<DelegationTrack> executionOrderStatusTracker = _executionOrderStatusTrackerRepository
                .GetAll(x => x.CreatedBy != storeTechnicalId && techinical.Contains(x.CreatedBy)).Include(x => x.ExecutionOrderStatus)
                .Include(x=>x.ExecutionOrder).ThenInclude(e=>e.Operation).Select(x => new DelegationTrack
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                CreationTime = new TimeSpan(x.CreationDate.Hour, x.CreationDate.Minute, x.CreationDate.Second),
                DelegationUserName = x.CreatedBy,
                Operation =( x.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Pending ? _Localizer["createOrder"] : x.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Reviewed ? _Localizer["reviewOrder"] : x.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Canceled ? _Localizer["cancalorder"] : _Localizer["ExecutionOrder"]) + " "+ x.ExecutionOrder.Operation.Name ,
                TenantId=x.TenantId,
                OperationNum=x.ExecutionOrder.Code

                }); ;
            IQueryable<DelegationTrack> refundOrderStatusTracker = _refundOrderStatusTrackerRepository
                .GetAll(x => x.CreatedBy != storeTechnicalId && techinical.Contains(x.CreatedBy))
                .Include(x => x.RefundOrderStatus).Include(x=>x.RefundOrder).ThenInclude(r=>r.Operation).Select(x => new DelegationTrack
            {
                Id = x.Id,
                CreationDate = x.CreationDate,
                CreationTime = new TimeSpan(x.CreationDate.Hour, x.CreationDate.Minute, x.CreationDate.Second),
                DelegationUserName = x.CreatedBy,
                Operation = (x.RefundOrderStatusId==(int)RefundOrderStatusEnum.Pending? _Localizer["createOrder"] : x.RefundOrderStatusId == (int)RefundOrderStatusEnum.Reviewed ? _Localizer["reviewOrder"] : x.RefundOrderStatusId == (int)RefundOrderStatusEnum.Cancelled ? _Localizer["cancalorder"] : _Localizer["refundorder"] )+ " " + x.RefundOrder.Operation.Name,
                TenantId = x.TenantId,
                    OperationNum = x.RefundOrder.Code

                });

        return exchangeOrderStatusTracker.Concat(executionOrderStatusTracker).Concat(refundOrderStatusTracker);


        }

        internal class TenantComparer : IEqualityComparer<StoreDelegationVM>
        {
            public bool Equals(StoreDelegationVM x, StoreDelegationVM y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(StoreDelegationVM obj)
            {
                return (int)obj.Id;
            }
        }
    }


}


