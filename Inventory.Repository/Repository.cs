using Inventory.Data;
using Inventory.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventory.Data.Interfaces;
using Inventory.CrossCutting.FinancialYear;

namespace Inventory.Repository
{
    public class Repository<TEntity,TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>,IActive
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> SaveChanges()
        {
            return await _unitOfWork.SaveChanges();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _unitOfWork.BeginTransaction();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate,bool overrideGlobaleFilters = false)
        {
            if (predicate != null)
            {
                IQueryable<TEntity> query = GetAll(overrideGlobaleFilters)
                    .Where(predicate);
                return query.FirstOrDefault();
            }
            else
            {
                throw new ArgumentNullException("The <predicate> paramter is required.");
            }
            
            

        }
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }

        public IQueryable<TEntity> GetAll(bool overrideGlobaleFilters = false)
        {
            if (overrideGlobaleFilters)
            {
                return _unitOfWork.CreateSet<TEntity>().IgnoreQueryFilters();
            }
            return _unitOfWork.CreateSet<TEntity>();
        }

       
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, bool overrideGlobaleFilters = false)
        {
            if (overrideGlobaleFilters)
            {
                if (predicate != null)
                {
                    return GetAll().Where(predicate).IgnoreQueryFilters();
                }
                else
                {
                    throw new ArgumentNullException("The <predicate> paramter is required.");
                }
            }
            else
            {
                if (predicate != null)
                {
                    return GetAll().Where(predicate);
                }
                else
                {
                    throw new ArgumentNullException("The <predicate> paramter is required.");
                }
            }
        }
       
        public int GetMax(Expression<Func<TEntity, bool>> predicate,
           Func<TEntity, int> MaxColumn, bool enableDateCheck = true)
        {
           
            if (predicate != null)
            {
                if (enableDateCheck)
                {
                    return GetAll().Where(predicate)
                        .Where(X => X.CreationDate >= FinancialYearProvider.CurrentYearStartDate &&
                        X.CreationDate <= FinancialYearProvider.CurrentYearEndDate)
                        .Select(MaxColumn).DefaultIfEmpty(0).Max();
                }
                else

                    return GetAll().Where(predicate)
                             .Select(MaxColumn).DefaultIfEmpty(0).Max();
            }
            else
            {
                if (enableDateCheck)
                {
                    return GetAll()
                        .Where(X => X.CreationDate >= FinancialYearProvider.CurrentYearStartDate &&
                        X.CreationDate <= FinancialYearProvider.CurrentYearEndDate)
                        .Select(MaxColumn).DefaultIfEmpty(0).Max();
                }
                else
                    return GetAll().Select(MaxColumn).DefaultIfEmpty(0).Max();
            }
        }
        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var dbSet = _unitOfWork.CreateSet<TEntity>();
            dbSet.Add(entity);

        }
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var dbSet = _unitOfWork.CreateSet<TEntity>();
            dbSet.Update(entity);
        }

        public void Activate(TEntity entity)
        {
            entity.IsActive = true;

            PartialUpdate(entity, x => x.IsActive);
        }
        public void DeActivate(TEntity entity)
        {
            entity.IsActive = false;

            PartialUpdate(entity, x => x.IsActive);
        }
        public void PartialUpdate(TEntity entity, params Expression<Func<TEntity, object>>[] propsToUpdate)
        {
            _unitOfWork.PartialUpdate(entity, propsToUpdate);

        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var dbSet = _unitOfWork.CreateSet<TEntity>();
            dbSet.Remove(entity);
        }

       

        public TEntity Get(TId id,bool overrideGlobalFilter=false)
        {
            return GetAll(overrideGlobalFilter).FirstOrDefault(c => c.Id.Equals(id));
        }

        
    }
}
