using Inventory.CrossCutting.FinancialYear;
using Inventory.CrossCutting.Identity;
using Inventory.CrossCutting.Tenant;
using Inventory.Data;
using Inventory.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        #region Members

        private readonly InventoryContext _context;
        private readonly IIdentityProvider _identityProvider;
        private readonly ITenantProvider _tenantProvider;


        #endregion

        #region Constructor

        public UnitOfWork(InventoryContext context,
            IIdentityProvider identityProvider,
            ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _identityProvider = identityProvider;
        }

        #endregion

        #region IUnitOfWork Members     

        public async Task<int> SaveChanges()
        {
            SetIEntityFields();
            return await _context.SaveChangesAsync();
        }
        public int SaveChanges_notasync()
        {
            SetIEntityFields();
            return  _context.SaveChanges();
        }

        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            var set = _context.Set<TEntity>();
            return set;
        }

        #endregion

        #region IDisposable Members

        ////public void Dispose()
        ////{

        ////    //_context.Dispose();
        ////}

        #endregion

        #region Private Methods

        private void SetIEntityFields()
        {
            var now = DateTime.Now;

            _context.ChangeTracker.Entries<IEntity<int>>()
            .Where(e => e.State == EntityState.Added)
            .ToList()
            .ForEach(e =>
            {
                e.Entity.CreationDate = now;
                e.Entity.CreatedBy = _identityProvider.GetUserName();
                // e.Entity.IsActive = true;
            });
            _context.ChangeTracker.Entries<IEntity<Guid>>()
            .Where(e => e.State == EntityState.Added)
            .ToList()
            .ForEach(e =>
            {
                e.Entity.CreationDate = now;
                e.Entity.CreatedBy = _identityProvider.GetUserName();
                // e.Entity.IsActive = true;
            });
            _context.ChangeTracker.Entries<IEntity<long>>()
           .Where(e => e.State == EntityState.Added)
           .ToList()
           .ForEach(e =>
           {
               e.Entity.CreationDate = now;
               e.Entity.CreatedBy = _identityProvider.GetUserName();
               // e.Entity.IsActive = true;
           });
            _context.ChangeTracker.Entries<ITenant>()
            .Where(e => e.State == EntityState.Added)
            .ToList()
            .ForEach(e =>
            {
                var tenantId = _tenantProvider.GetTenant();
                if (tenantId != 0)
                    e.Entity.TenantId = tenantId;
            });

           
            _context.ChangeTracker.Entries<IEntity<int>>()
            .Where(e => e.State == EntityState.Modified)
            .ToList()
            .ForEach(e =>
            {
                e.Entity.ModificationDate = now;
                e.Entity.ModifiedBy = _identityProvider.GetUserName();
            });
            _context.ChangeTracker.Entries<IEntity<Guid>>()
              .Where(e => e.State == EntityState.Modified)
              .ToList()
              .ForEach(e =>
              {
                  e.Entity.ModificationDate = now;
                  e.Entity.ModifiedBy = _identityProvider.GetUserName();
              });
                    _context.ChangeTracker.Entries<IEntity<long>>()
            .Where(e => e.State == EntityState.Modified)
            .ToList()
            .ForEach(e =>
            {
                e.Entity.ModificationDate = now;
                e.Entity.ModifiedBy = _identityProvider.GetUserName();
            });
            

            _context.ChangeTracker.Entries<ICode>()
           .Where(e => e.State == EntityState.Added)
           .ToList()
           .ForEach(e =>
           {

               e.Entity.Year = FinancialYearProvider.CurrentYear;
           });

            _context.ChangeTracker.Entries<IActive>()
          .Where(e => e.State == EntityState.Added)
          .ToList()
          .ForEach(e =>
          {
              e.Entity.IsActive = true;
          });
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            //((IEntity<int>)original).ModificationDate = DateTime.Now;
            ((IEntity<object>)original).ModificationDate = DateTime.Now;
            //  ((IEntity)original).LastUpdatedUsername = _identityProvider.GetUsername();
            //  ((IEntity)original).LastUpdateBy = _identityProvider.GetUserId();
            //((IEntity)original).IsActive = true;

            //if it is not attached, attach original and set current values
            _context.Entry(original).CurrentValues.SetValues(current);
        }
        #endregion

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<DbSet<TEntity>> CreateSetasync<TEntity>() where TEntity : class
        {
            var set = _context.Set<TEntity>();
            return await Task.Run(() => set);
        }

      

        public void UpdateWithoutGet<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = CreateSet<TEntity>();
            dbSet.Attach(entity);
            var entry = _context.Entry(entity);
        
        }


        public void PartialUpdate<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] propsToUpdate) where TEntity : class
        {
            var dbSet = CreateSet<TEntity>();
            dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            foreach (var prop in propsToUpdate)
                entry.Property(prop).IsModified = true;

        }

        public void updateAll<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] propsToUpdate) where TEntity : class
        {
          //  _context.BaseItem.Where(x=>x.Id==4).Update(c=>c.Id=)

        }

    }
}

