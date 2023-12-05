using Inventory.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Repository
{
    public interface IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
    {
        IQueryable<TEntity> GetAll(bool overridQueryFilter = false);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, bool overridQueryFilter = false);

        TEntity Get(TId id, bool overrideGlobalFilter = false);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChanges();
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, bool overrideGlobaleFilters = false);
        void Merge(TEntity persisted, TEntity current);
        int GetMax(Expression<Func<TEntity, bool>> predicate,
        Func<TEntity, int> MaxColumn, bool enableDateCheck = true);
        void PartialUpdate(TEntity entity, params Expression<Func<TEntity, object>>[] propsToUpdate);
        void Activate(TEntity entity);
        void DeActivate(TEntity entity);

    //    IQueryable<TEntity> GetActives(bool overrideGlobaleFilters = false);

    //    IQueryable<TEntity> GetActives(Expression<Func<TEntity, bool>> predicate, bool overrideGlobaleFilters = false);
    }
}
