using Inventory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Reflection;
using Audit.EntityFramework;
using Audit.Core;

namespace Inventory.Data
{
    public class InventoryContext : AuditDbContext
    {
        private List<int> _tenantIds;
        private string _userName;
        private IHttpContextAccessor _httpContextAccessor;
        public InventoryContext(DbContextOptions options,
             IHttpContextAccessor httpContextAccessor)
            : base(options)
        {

            _httpContextAccessor = httpContextAccessor;

            _tenantIds = _httpContextAccessor.HttpContext?.User.Claims.Where(x =>
              x.Type == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/SelectedTenant")?
            .Select(x => int.Parse(x.Value)).ToList();

            _userName=_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
              x.Type == ClaimTypes.Name)?
            .Value;

        }
        #region Entities
        public virtual DbSet<Addition> Addition { get; set; }
        public virtual DbSet<Subtraction> Subtraction { get; set; }

        public virtual DbSet<AdditionAttachment> AdditionAttachment { get; set; }
        public virtual DbSet<AdditionDocumentType> AdditionDocumentType { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<AttachmentType> AttachmentType { get; set; }
        public virtual DbSet<BaseItem> BaseItem { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<CommitteeAttachment> CommitteeAttachment { get; set; }
        public virtual DbSet<CommitteeEmployee> CommitteeEmployee { get; set; }
        public virtual DbSet<CommitteeItem> CommitteeItem { get; set; }
        public virtual DbSet<CommitteeItemHistory> CommitteeItemHistory { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual DbSet<ExchangeOrder> ExchangeOrder { get; set; }
        public virtual DbSet<ExchangeOrderStatusTracker> ExchangeOrderStatusTracker { get; set; }
        public virtual DbSet<ExchangeOrderStatus> ExchangeOrderStatus { get; set; }
        public virtual DbSet<ExchangeOrderStoreItem> ExchangeOrderStoreItem { get; set; }
        public virtual DbSet<ExternalEntity> ExternalEntity { get; set; }
        public virtual DbSet<HistoryAction> HistoryAction { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceStoreItem> InvoiceStoreItem { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<ItemStatus> ItemStatus { get; set; }
        public virtual DbSet<JobTitle> JobTitle { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Operation> Operation { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreItem> StoreItem { get; set; }
        public virtual DbSet<RobbedStoreItem> RobbedStoreItem { get; set; }

        public virtual DbSet<StoreItemCopy> StoreItemCopy { get; set; }
        public virtual DbSet<StoreItemHistory> StoreItemHistory { get; set; }
        public virtual DbSet<StoreItemStatus> StoreItemStatus { get; set; }
        public virtual DbSet<StoreType> StoreType { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<TechnicalDepartment> TechnicalDepartment { get; set; }
        public virtual DbSet<Transformation> Transformation { get; set; }
        public virtual DbSet<TransformationAttachment> TransformationAttachment { get; set; }
        public virtual DbSet<TransformationStatus> TransformationStatus { get; set; }
        public virtual DbSet<TransformationStoreItem> TransformationStoreItem { get; set; }


        public virtual DbSet<RefundOrder> RefundOrder { get; set; }
        public virtual DbSet<RefundOrderAttachment> RefundOrderAttachment { get; set; }
        public virtual DbSet<RefundOrderStatus> RefundOrderStatus { get; set; }
        public virtual DbSet<RefundOrderStoreItem> RefundOrderStoreItem { get; set; }
        public virtual DbSet<RefundOrderStatusTracker> RefundOrderStatusTracker { get; set; }
        public virtual DbSet<RobbingOrder> RobbingOrder { get; set; }
        public virtual DbSet<RobbingOrderAttachment> RobbingOrderAttachment { get; set; }
        public virtual DbSet<RobbingOrderStatus> RobbingOrderStatus { get; set; }
        public virtual DbSet<RobbingOrderStoreItem> RobbingOrderStoreItem { get; set; }
        public virtual DbSet<RobbingOrderRemainsDetails> RobbingOrderRemainsDetails { get; set; }
        public virtual DbSet<Deduction> Deduction { get; set; }
        public virtual DbSet<DeductionAttachment> DeductionAttachment { get; set; }
        public virtual DbSet<DeductionStoreItem> DeductionStoreItem { get; set; }


        public virtual DbSet<Unit> Unit { get; set; }

        public virtual DbSet<OperationAttachmentType> OperationAttachmentType { get; set; }
        public virtual DbSet<ExaminationStatus> ExaminationStatus { get; set; }
        public virtual DbSet<DelegationType> DelegationType { get; set; }
        public virtual DbSet<UserConnection> UserConnection { get; set; }

        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationHandler> NotificationHandler { get; set; }
        public virtual DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public virtual DbSet<NotificationValues> NotificationValues { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }

        public virtual DbSet<StockTaking> StockTaking { get; set; }
        public virtual DbSet<StockTakingStoreItem> StockTakingStoreItem { get; set; }
        public virtual DbSet<StockTakingRobbedStoreItem> StockTakingRobbedStoreItem { get; set; }

        public virtual DbSet<StockTakingAttachment> StockTakingAttachment { get; set; }

        public virtual DbSet<Delegation> Delegation { get; set; }
        public virtual DbSet<DelegationStore> DelegationStore { get; set; }

        public virtual DbSet<Stagnant> Stagnant { get; set; }
        public virtual DbSet<StagnantStoreItem> StagnantStoreItem { get; set; }
        public virtual DbSet<StagnantAttachment> StagnantAttachment { get; set; }

        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }

       // public virtual DbSet<RobbingStoreItemStatus> RobbingStoreItemStatus { get; set; }
        public virtual DbSet<ExecutionOrderStatus> ExecutionOrderStatus { get; set; }
        public virtual DbSet<ExecutionOrderResultItem> ExecutionOrderResultItem { get; set; }
        public virtual DbSet<ExecutionOrderResultRemain> ExecutionOrderResultRemain { get; set; }
        public virtual DbSet<ExecutionOrderStatusTracker> ExecutionOrderStatusTracker { get; set; }
        public virtual DbSet<Remains> Remains { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            //store history of specific tables

            Audit.Core.Configuration.Setup()
           .UseEntityFramework(ef => ef
           .AuditTypeExplicitMapper(m => m
           .Map<StoreItem, StoreItemCopy>((source, copy) =>
             {
                 copy.HistoryId = source.Id;
                 copy.Id = Guid.NewGuid();
                 copy.AuditDate = DateTime.Now;
                 foreach (PropertyInfo propertyInfo in copy.GetType().GetProperties())
                 {
                     var prop = source.GetType().GetProperty(propertyInfo.Name);
                     if (prop != null)
                         if (propertyInfo.Name.ToLower() != "id")
                             propertyInfo.SetValue(copy, prop.GetValue(source));
                 }
             })
           .Map<ExchangeOrder,ExchangeOrderStatusTracker>((source, tracker) =>
           {
               tracker.ExchangeOrderId = source.Id;
               tracker.Id = Guid.NewGuid();
               tracker.CreationDate = DateTime.Now;
               tracker.ModifiedBy = null;
               tracker.ModificationDate = null;

               var prop = source.GetType().GetProperty("ModifiedBy");
               if (prop != null)
               {
                   var propValue = prop.GetValue(source);
                   if (propValue != null)
                       tracker.CreatedBy =Convert.ToString( propValue);
               }
                 
           })
             .Map<RefundOrder, RefundOrderStatusTracker>((source, tracker) =>
             {
                 tracker.RefundOrderId = source.Id;
                 tracker.Id = Guid.NewGuid();
                 tracker.CreationDate = DateTime.Now;
                 tracker.ModifiedBy = null;
                 tracker.ModificationDate = null;

                 var prop = source.GetType().GetProperty("ModifiedBy");
                 if (prop != null)
                 {
                     var propValue = prop.GetValue(source);
                     if (propValue != null)
                         tracker.CreatedBy = Convert.ToString(propValue);
                 }

             })
              .Map<ExecutionOrder, ExecutionOrderStatusTracker>((source, tracker) =>
              {
                  tracker.ExecutionOrderId = source.Id;
                  tracker.Id = Guid.NewGuid();
                  tracker.CreationDate = DateTime.Now;
                  tracker.ModifiedBy = null;
                  tracker.ModificationDate = null;

                  var prop = source.GetType().GetProperty("ModifiedBy");
                  if (prop != null)
                  {
                      var propValue = prop.GetValue(source);
                      if (propValue != null)
                          tracker.CreatedBy = Convert.ToString(propValue);
                  }

              })
             .AuditEntityAction((AuditEvent, EventEntry, Copy) =>
             {
                 var AuditAction = Copy.GetType().GetProperty("AuditAction");
                 if (AuditAction != null)
                     AuditAction.SetValue(Copy, EventEntry.Action);
                               
             })
         )
         );
          

            modelBuilder.Entity<Addition>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                //entity.Property(e => e.AdditionDocumentNumber).IsRequired();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.AdditionDocumentType)
                    .WithMany(p => p.Addition)
                    .HasForeignKey(d => d.AdditionDocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addition_AdditionDocumentType");

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.Addition)
                    .HasForeignKey(d => d.BudgetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addition_Budget");

                entity.HasOne(d => d.ExaminationCommitte)
                    .WithMany(p => p.Addition)
                    .HasForeignKey(d => d.ExaminationCommitteId)
                    .HasConstraintName("FK_Addition_ExaminationCommitte");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Addition)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addition_Operation");

                entity.HasOne(d => d.Transformation)
                    .WithMany(p => p.Addition)
                    .HasForeignKey(d => d.TransformationId)
                    .HasConstraintName("FK_Addition_Transformation");
            });
            modelBuilder.Entity<Subtraction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Subtraction)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subtraction_Operation");

                entity.HasOne(d => d.Transformation)
                    .WithMany(p => p.Subtraction)
                    .HasForeignKey(d => d.TransformationId)
                    .HasConstraintName("FK_Subtraction_Transformation");


                entity.HasOne(d => d.ExecutionOrder)
                    .WithMany(p => p.Subtraction)
                    .HasForeignKey(d => d.ExecutionOrderId)
                    .HasConstraintName("FK_Subtraction_ExecutionOrder");


                entity.HasOne(d => d.RobbingOrder)
                    .WithMany(p => p.Subtraction)
                    .HasForeignKey(d => d.RobbingOrderId)
                    .HasConstraintName("FK_Subtraction_RobbingOrder");
                entity.HasOne(d => d.Deduction)
                  .WithMany(p => p.Subtraction)
                  .HasForeignKey(d => d.DeductionId)
                  .HasConstraintName("FK_Subtraction_Deduction");
            });
            modelBuilder.Entity<AdditionAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Addition)
                    .WithMany(p => p.AdditionAttachment)
                    .HasForeignKey(d => d.AdditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionAttachment_Addition");

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.AdditionAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionAttachment_Attachment");
            });

            modelBuilder.Entity<AdditionDocumentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FileExtention).IsRequired();

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.FileUrl).IsRequired();

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.Attachment)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attachment_AttachmentType1");
            });

            modelBuilder.Entity<AttachmentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<BaseItem>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.DefaultUnit)
                    .WithMany(p => p.BaseItem)
                    .HasForeignKey(d => d.DefaultUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BaseItem_Unit");

                entity.HasOne(d => d.ItemCategory)
                    .WithMany(p => p.BaseItem)
                    .HasForeignKey(d => d.ItemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BaseItem_ItemCategory");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_Store");
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CommitteeAttachment>(entity =>
            {
                // entity.Property(e => e.Id).val.ValueGeneratedNever();

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.CommitteeAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeAttachment_Attachment");

                entity.HasOne(d => d.Committee)
                    .WithMany(p => p.CommitteeAttachment)
                    .HasForeignKey(d => d.CommitteeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeAttachment_ExaminationCommitte");
            });
            modelBuilder.Entity<OperationAttachmentType>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.AttachmentType)
                    .WithMany(p => p.OperationAttachmentType)
                    .HasForeignKey(d => d.AttachmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperationAttachmentType_AttachmentType");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.OperationAttachmentType)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperationAttachmentType_Operation");
            });
            modelBuilder.Entity<CommitteeEmployee>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Emplyee)
                    .WithMany(p => p.CommitteeEmployee)
                    .HasForeignKey(d => d.EmplyeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeEmployee_Employees");

                entity.HasOne(d => d.ExaminationCommitte)
                    .WithMany(p => p.CommitteeEmployee)
                    .HasForeignKey(d => d.ExaminationCommitteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeEmployee_ExaminationCommitte");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.CommitteeEmployee)
                    .HasForeignKey(d => d.JobTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeEmployee_JobTitle");

            });

            modelBuilder.Entity<CommitteeItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.BaseItem)
                    .WithMany(p => p.CommitteeItem)
                    .HasForeignKey(d => d.BaseItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeItem_BaseItem");



                entity.HasOne(d => d.ExaminationCommitte)
                    .WithMany(p => p.CommitteeItem)
                    .HasForeignKey(d => d.ExaminationCommitteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeItem_ExaminationCommitte");
            });

            modelBuilder.Entity<CommitteeItemHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CommitteeItem)
                    .WithMany(p => p.CommitteeItemHistory)
                    .HasForeignKey(d => d.CommitteeItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeItemHistory_CommitteeItem");

                entity.HasOne(d => d.HistoryAction)
                    .WithMany(p => p.CommitteeItemHistory)
                    .HasForeignKey(d => d.HistoryActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommitteeItemHistory_HistoryAction");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employees_Department");
            });

            modelBuilder.Entity<ExaminationCommitte>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.ContractDate).HasColumnType("date");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DecisionDate).HasColumnType("date");

                entity.Property(e => e.SupplyOrderDate).HasColumnType("date");

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.BudgetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Budget");

                entity.HasOne(d => d.ExternalEntity)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.ExternalEntityId)
                    .HasConstraintName("FK_ExaminationCommitte_ExternalEntity");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Operation");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Store");

                entity.HasOne(d => d.Supplier)
                     .WithMany(p => p.ExaminationCommitte)
                     .HasForeignKey(d => d.SupplierId)
                     .HasConstraintName("FK_ExaminationCommitte_Supplier");

                entity.HasOne(d => d.ExaminationStatus)
                 .WithMany(p => p.ExaminationCommitte)
                 .HasForeignKey(d => d.ExaminationStatusId)
                 .HasConstraintName("FK_ExaminationCommitte_ExaminationStatus");
            });
            modelBuilder.Entity<ExaminationCommitte>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.ContractDate).HasColumnType("date");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DecisionDate).HasColumnType("date");

                entity.Property(e => e.SupplyOrderDate).HasColumnType("date");

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.BudgetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Budget");

                entity.HasOne(d => d.ExternalEntity)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.ExternalEntityId)
                    .HasConstraintName("FK_ExaminationCommitte_ExternalEntity");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Operation");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ExaminationCommitte)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExaminationCommitte_Store");

                entity.HasOne(d => d.Supplier)
                     .WithMany(p => p.ExaminationCommitte)
                     .HasForeignKey(d => d.SupplierId)
                     .HasConstraintName("FK_ExaminationCommitte_Supplier");

                entity.HasOne(d => d.ExaminationStatus)
                 .WithMany(p => p.ExaminationCommitte)
                 .HasForeignKey(d => d.ExaminationStatusId)
                 .HasConstraintName("FK_ExaminationCommitte_ExaminationStatus");
            });

            modelBuilder.Entity<ExchangeOrder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");


                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.ExchangeOrder)
                    .HasForeignKey(d => d.BudgetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrder_Budget");

                entity.HasOne(d => d.ExchangeOrderStatus)
                    .WithMany(p => p.ExchangeOrder)
                    .HasForeignKey(d => d.ExchangeOrderStatusId)
                    .HasConstraintName("FK_ExchangeOrder_ExchangeOrderStatus");

                entity.HasOne(d => d.ForEmployee)
                    .WithMany(p => p.ExchangeOrder)
                    .HasForeignKey(d => d.ForEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrder_Employees");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ExchangeOrder)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrder_Operation");
            });

            modelBuilder.Entity<ExchangeOrderStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });
            modelBuilder.Entity<ExaminationStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });
            modelBuilder.Entity<DelegationType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });
            modelBuilder.Entity<ExchangeOrderStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.ExchangeOrderStoreItem)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrderStoreItem_ExchangeOrder");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.ExchangeOrderStoreItem)
                    .HasForeignKey(d => d.StoreItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrderStoreItem_StoreItem");
            });

            modelBuilder.Entity<ExternalEntity>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<HistoryAction>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.ReceivedEmployee)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ReceivedEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Employees");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Departments");

                entity.HasOne(d => d.Location)
                  .WithMany(p => p.Invoice)
                  .HasForeignKey(d => d.LocationId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Invoice_Locations");

                entity.HasOne(d => d.ExchangeOrder)
                 .WithMany(p => p.Invoice)
                 .HasForeignKey(d => d.ExchangeOrderId)
                 .HasConstraintName("FK_Invoice_ExchangeOrder");
            });

            modelBuilder.Entity<InvoiceStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceStoreItem)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceStoreItem_Invoice");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.InvoiceStoreItem)
                    .HasForeignKey(d => d.StoreItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceStoreItem_StoreItem");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ItemStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.AdminId).HasMaxLength(450);



                entity.HasOne(d => d.RobbingBudget)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.RobbingBudgetId)
                    .HasConstraintName("FK_Store_Budget");

                entity.HasOne(d => d.StoreType)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.StoreTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_StoreType");

                entity.HasOne(d => d.TechnicalDepartment)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.TechnicalDepartmentId)
                    .HasConstraintName("FK_Store_TechnicalDepartment");
            });

            modelBuilder.Entity<StoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();


                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                //entity.Property(e => e.RobbingPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Addition)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.AdditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_Addition");

                entity.HasOne(d => d.BaseItem)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.BaseItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_BaseItem");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_Book");

                entity.HasOne(d => d.CurrentItemStatus)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.CurrentItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_ItemStatusId");


                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_Store");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_StoreItem_Unit");

                entity.HasOne(d => d.StoreItemStatus)
                    .WithMany(p => p.StoreItem)
                    .HasForeignKey(d => d.StoreItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItem_StoreItemStatus");
            });
            modelBuilder.Entity<RobbedStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();



                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Addition)
                    .WithMany(p => p.RobbedStoreItem)
                    .HasForeignKey(d => d.AdditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingStoreItem_Addition");

                entity.HasOne(d => d.BaseItem)
                    .WithMany(p => p.RobbedStoreItem)
                    .HasForeignKey(d => d.BaseItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingStoreItem_BaseItem");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.RobbedStoreItem)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingStoreItem_Book");

              
                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.RobbedStoreItem)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_RobbingStoreItem_Unit");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.RobbedStoreItem)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingStoreItem_Currency");
            });
            modelBuilder.Entity<StoreItemHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OperationCode).IsRequired();

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.StoreItemHistory)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItemHistory_Operation");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.StoreItemHistory)
                    .HasForeignKey(d => d.StoreItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItemHistory_StoreItem");
            });

            modelBuilder.Entity<StoreItemStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<StoreType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TechnicalDepartment>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.TechnicianId).HasMaxLength(450);

            });

            modelBuilder.Entity<Transformation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

               

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.Transformation)
                    .HasForeignKey(d => d.BudgetId)
                    .HasConstraintName("FK_Transformation_Budget");

                entity.HasOne(d => d.FromStore)
                    .WithMany(p => p.FromTransformation)
                    .HasForeignKey(d => d.FromStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_Store");

                entity.HasOne(d => d.ToStore)
                   .WithMany(p => p.ToTransformation)
                   .HasForeignKey(d => d.ToStoreId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Transformation_ToStore");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Transformation)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transformation_Operation");

                entity.HasOne(d => d.ToExternalEntity)
                    .WithMany(p => p.Transformation)
                    .HasForeignKey(d => d.ToExternalEntityId)
                    .HasConstraintName("FK_Transformation_ExternalEntity");

                entity.HasOne(d => d.TransformationStatus)
                    .WithMany(p => p.Transformation)
                    .HasForeignKey(d => d.TransformationStatusId)
                    .HasConstraintName("FK_Transformation_TransformationStatus");
            });

            modelBuilder.Entity<TransformationAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.TransformationAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransformationAttachment_Attachment");

                entity.HasOne(d => d.Transformation)
                    .WithMany(p => p.TransformationAttachment)
                    .HasForeignKey(d => d.TransformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransformationAttachment_Transformation");
            });

            modelBuilder.Entity<TransformationStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TransformationStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NoteCreatorId).HasMaxLength(450);

                entity.HasOne(d => d.StoreItem)
                .WithMany(p => p.TransformationStoreItem)
                .HasForeignKey(d => d.StoreItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransformationStoreItem_StoreItem");

                entity.HasOne(d => d.Transformation)
                    .WithMany(p => p.TransformationStoreItem)
                    .HasForeignKey(d => d.TransformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransformationStoreItem_Transformation");

            });

            modelBuilder.Entity<StockTaking>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.StockTaking)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockTacking_Operation");



            });
            modelBuilder.Entity<StockTakingStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.StockTaking)
                    .WithMany(p => p.StockTakingStoreItem)
                    .HasForeignKey(d => d.StockTakingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockTakingStoreItem_StockTaking");
            });

            modelBuilder.Entity<StockTakingRobbedStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.StockTaking)
                    .WithMany(p => p.StockTakingRobbedStoreItem)
                    .HasForeignKey(d => d.StockTakingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockTakingRobbedStoreItem_StockTaking");
            });
            modelBuilder.Entity<StockTakingAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.StockTakingAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockTakingAttachment_Attachment");

                entity.HasOne(d => d.StockTaking)
                    .WithMany(p => p.StockTakingAttachment)
                    .HasForeignKey(d => d.StockTakingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockTakingAttachment_StockTaking");
            });
            modelBuilder.Entity<RefundOrder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");


                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.RefundOrder)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrder_Operation");


                entity.HasOne(d => d.RefundOrderStatus)
                    .WithMany(p => p.RefundOrder)
                    .HasForeignKey(d => d.RefundOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrder_RefundOrderStatus");


                entity.HasOne(d => d.RefundOrderEmployee)
                    .WithMany(p => p.EmployeeRefundOrder)
                    .HasForeignKey(d => d.RefundOrderEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrder_RefundOrderEmployee");



                entity.HasOne(d => d.ExaminationEmployee)
                    .WithMany(p => p.ExaminationRefundOrder)
                    .HasForeignKey(d => d.ExaminationEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrder_examinationemployeeRefundOrder");

            });

            modelBuilder.Entity<RefundOrderAttachment>(entity =>
            {
                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.RefundOrderAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderAttachment_Attachment");

                entity.HasOne(d => d.RefundOrder)
                    .WithMany(p => p.RefundOrderAttachment)
                    .HasForeignKey(d => d.RefundOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderAttachment_RefundOrder");
            });

            modelBuilder.Entity<RefundOrderStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RefundOrderStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.StoreItemStatus)
                    .WithMany(p => p.RefundOrderStoreItem)
                    .HasForeignKey(d => d.StoreItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderStoreItem_StoreItemStatus");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.RefundOrderStoreItem)
                    .HasForeignKey(d => d.StoreItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderStoreItem_StoreItem");
            });

            modelBuilder.Entity<StoreItemCopy>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.RobbingPrice).HasColumnType("decimal(18, 0)");
                entity.HasOne(d => d.StoreItemStatus)
                    .WithMany(p => p.StoreItemCopy)
                    .HasForeignKey(d => d.StoreItemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItemCopy_StoreItemStatus");

                entity.HasOne(d => d.History)
                    .WithMany(p => p.StoreItemCopy)
                    .HasForeignKey(d => d.HistoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreItemCopy_StoreItem");
            });
            modelBuilder.Entity<ExchangeOrderStatusTracker>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ExchangeOrderStatus)
                    .WithMany(p => p.ExchangeOrderStatusTracker)
                    .HasForeignKey(d => d.ExchangeOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrderStatusTracker_ExchangeOrderStatus");

                entity.HasOne(d => d.ExchangeOrder)
                    .WithMany(p => p.ExchangeOrderStatusTracker)
                    .HasForeignKey(d => d.ExchangeOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeOrderStatusTracker_ExchangeOrder");
            });

            modelBuilder.Entity<RefundOrderStatusTracker>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.RefundOrderStatus)
                    .WithMany(p => p.RefundOrderStatusTracker)
                    .HasForeignKey(d => d.RefundOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderStatusTracker_RefundOrderStatus");

                entity.HasOne(d => d.RefundOrder)
                    .WithMany(p => p.RefundOrderStatusTracker)
                    .HasForeignKey(d => d.RefundOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefundOrderStatusTracker_RefundOrder");
            });
            modelBuilder.Entity<ExecutionOrderStatusTracker>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ExecutionOrderStatus)
                    .WithMany(p => p.ExecutionOrderStatusTracker)
                    .HasForeignKey(d => d.ExecutionOrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderStatusTracker_ExecutionOrderStatus");

                entity.HasOne(d => d.ExecutionOrder)
                    .WithMany(p => p.ExecutionOrderStatusTracker)
                    .HasForeignKey(d => d.ExecutionOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderStatusTracker_ExecutionOrder");
            });

            modelBuilder.Entity<RobbingOrder>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

                //entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.RobbingOrder)
                    .HasForeignKey(d => d.BudgetId)
                    .HasConstraintName("FK_RobbingOrder_Budget");



                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.RobbingOrder)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingOrder_Operation");


                entity.HasOne(d => d.RobbingOrderStatus)
                    .WithMany(p => p.RobbingOrder)
                    .HasForeignKey(d => d.RobbingOrderStatusId)
                    .HasConstraintName("FK_RobbingOrder_RobbingOrderStatus");


                entity.HasOne(d => d.FromStore)
                    .WithMany(p => p.FromRobbingOrder)
                    .HasForeignKey(d => d.FromStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingOrder_Store");

                entity.HasOne(d => d.ToStore)
                  .WithMany(p => p.ToRobbingOrder)
                  .HasForeignKey(d => d.ToStoreId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_RobbingOrder_ToStore");
            });

            modelBuilder.Entity<RobbingOrderAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.RobbingOrderAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingOrderAttachment_Attachment");

                entity.HasOne(d => d.RobbingOrder)
                    .WithMany(p => p.RobbingOrderAttachment)
                    .HasForeignKey(d => d.RobbingOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobbingOrderAttachment_RobbingOrder");
            });
            modelBuilder.Entity<ExecutionOrderAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.ExecutionOrderAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderAttachment_Attachment");

                entity.HasOne(d => d.ExecutionOrder)
                    .WithMany(p => p.ExecutionOrderAttachment)
                    .HasForeignKey(d => d.ExecutionOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderAttachment_ExecutionOrder");
            });
            modelBuilder.Entity<ExecutionOrderResultItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

              
                entity.HasOne(d => d.BaseItem)
                .WithMany(p => p.ExecutionOrderResultItem)
                .HasForeignKey(d => d.BaseItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExecutionOrderResultItem_BaseItem");

                entity.HasOne(d => d.ExecutionOrder)
                    .WithMany(p => p.ExecutionOrderResultItem)
                    .HasForeignKey(d => d.ExecutionOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderResultItem_ExecutionOrder");

            });

            modelBuilder.Entity<ExecutionOrderResultRemain>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();


                entity.HasOne(d => d.Remains)
                .WithMany(p => p.ExecutionOrderResultRemain)
                .HasForeignKey(d => d.RemainsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExecutionOrderResultRemain_Remains");

                entity.HasOne(d => d.ExecutionOrder)
                    .WithMany(p => p.ExecutionOrderResultRemain)
                    .HasForeignKey(d => d.ExecutionOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExecutionOrderResultRemain_ExecutionOrder");

            });
            modelBuilder.Entity<RobbingOrderStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RobbingOrderStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                entity.HasOne(d => d.StoreItemStatus)
                   .WithMany(p => p.RobbingOrderStoreItem)
                   .HasForeignKey(d => d.StoreItemStatusId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_RobbingOrderStoreItem_StoreItemStatus");
            });
            modelBuilder.Entity<RobbingOrderRemainsDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Deduction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).IsRequired();

              

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Deduction)
                    .HasForeignKey(d => d.OperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Deduction_Operation");

            });

            modelBuilder.Entity<DeductionAttachment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.DeductionAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeductionAttachment_Attachment");

                entity.HasOne(d => d.Deduction)
                    .WithMany(p => p.DeductionAttachment)
                    .HasForeignKey(d => d.DeductionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeductionAttachment_Deduction");
            });


            modelBuilder.Entity<DeductionStoreItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });
            modelBuilder.Entity<UserConnection>(entity =>
            {
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.ConnectionId).IsRequired();

            });
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(d => d.NotificationTemplate)
                   .WithMany(p => p.Notification)
                   .HasForeignKey(d => d.NotificationTemplateId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Notification_NotificationTemplate");


            });

            modelBuilder.Entity<NotificationHandler>(entity =>
            {

                entity.HasOne(d => d.NotificationTemplate)
                    .WithMany(p => p.NotificationHandler)
                    .HasForeignKey(d => d.NotificationTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationHandler_NotificationTemplate");

            });

            modelBuilder.Entity<NotificationValues>(entity =>
            {
                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationValues)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationValues_Notification");

            });
            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.UserNotification)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserNotification_Notification");

            });

            modelBuilder.Entity<NotificationTemplate>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

            });

            modelBuilder.Entity<DelegationStore>(entity =>
            {

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DelegationStore)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DelegationStore_Store");

                entity.HasOne(d => d.Delegation)
                    .WithMany(p => p.DelegationStore)
                    .HasForeignKey(d => d.DelegationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DelegationStore_Delegation");


            });
            modelBuilder.Entity<StagnantAttachment>(entity =>
            {

                entity.HasOne(d => d.Stagnant)
                    .WithMany(p => p.StagnantAttachment)
                    .HasForeignKey(d => d.StagnantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StagnantAttachment_Stagnant");

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.StagnantAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StagnantAttachment_Attachment");


            });
            modelBuilder.Entity<StagnantStoreItem>(entity =>
            {

                entity.HasOne(d => d.Stagnant)
                    .WithMany(p => p.StagnantStoreItem)
                    .HasForeignKey(d => d.StagnantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StagnantStoreItem_Stagnant");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.StagnantStoreItem)
                    .HasForeignKey(d => d.StoreItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StagnantStoreItem_StoreItem");


            });
            modelBuilder.Entity<Delegation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.DelegationType)
               .WithMany(p => p.Delegation)
               .HasForeignKey(d => d.DelegationTypeId)
               .HasConstraintName("FK_Delegation_DelegationType");

            });
            modelBuilder.Entity<Stagnant>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

            });
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

            });
            //modelBuilder.Entity<RobbingStoreItemStatus>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //});
            modelBuilder.Entity<ExecutionOrderStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

            });
            //modelBuilder.Entity<Remains>(entity =>
            //{
            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //});
            modelBuilder.Entity<RemainsDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            });
            //get entities implement IActive
            var configureActiveMethod = GetType().GetTypeInfo()
                .DeclaredMethods.Single(m => m.Name == nameof(ConfigureActiveFilter));
            var argsActive = new object[] { modelBuilder };
            var ActiveEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(IActive).IsAssignableFrom(t.ClrType));
            foreach (var entityType in ActiveEntityTypes)
                configureActiveMethod.MakeGenericMethod(entityType.ClrType).Invoke(this, argsActive);

            //get entities implement ITenant and IActive

            var configureTenantMethod = GetType().GetTypeInfo()
                .DeclaredMethods.Single(m => m.Name == nameof(ConfigureTenantFilter));
            var args = new object[] { modelBuilder };
            var tenantEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(ITenant).IsAssignableFrom(t.ClrType));
            foreach (var entityType in tenantEntityTypes)
                configureTenantMethod.MakeGenericMethod(entityType.ClrType).Invoke(this, args);

            //get entities implement IDelete

            var configureDeleteMethod = GetType().GetTypeInfo()
                .DeclaredMethods.Single(m => m.Name == nameof(ConfigureDeleteFilter));
            var argsDelete = new object[] { modelBuilder };
            var DeleteEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(IDelete).IsAssignableFrom(t.ClrType));
            foreach (var entityType in DeleteEntityTypes)
                configureTenantMethod.MakeGenericMethod(entityType.ClrType).Invoke(this, args);


        }
        private void ConfigureTenantFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, ITenant, IActive
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(t => (_tenantIds.Count == 0 || _tenantIds.Any(c => c == t.TenantId))
                && t.IsActive == true);
        }

        private void ConfigureActiveFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, IActive
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(t => t.IsActive == true);
        }
        private void ConfigureDeleteFilter<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, IDelete
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(t => t.IsDelete == null);
        }
    }

}
