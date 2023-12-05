using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Repository
{

    public interface IUnitOfWork
    {
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        Task<DbSet<TEntity>> CreateSetasync<TEntity>() where TEntity : class;
        Task<int> SaveChanges();
        int SaveChanges_notasync();
        Task<IDbContextTransaction> BeginTransaction();
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;
        void PartialUpdate<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] propsToUpdate) where TEntity : class;
        
        }
}
