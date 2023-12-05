using inventory.Engines.CodeGenerator;
using inventory.Engines.LdapAuth;
using inventory.Engines.WordGenerator;
using Inventory.CrossCutting.Identity;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Models.Delegation;
using Inventory.Data.Models.ExecutionOrderVM;
using Inventory.Data.Models.ReportVM;
using Inventory.Repository;
using Inventory.Service.Implementation;
using Inventory.Service.Interfaces;
using Inventory.Web.Identity;
using Inventory.Web.Tenant;
using Microsoft.AspNet.OData.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Inventory.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddOdataApiConfig(this ODataConventionModelBuilder builder)
        {
            builder.EntitySet<ExistingStoreItemVM>("ExistingStoreItemVM");

            builder.EntitySet<TechnicianStoreItemVM>("TechnicianStoreItemVM");
            builder.EntitySet<TechnicianStoreItemDetails>("TechnicianStoreItemDetails");
            builder.EntitySet<PrintTechnicianStoreItemVM>("PrintTechnicianStoreItemVM");

            builder.EntitySet<DepartmentStoreItemVM>("DepartmentStoreItemVM");
            builder.EntitySet<DepartmentDetailsVM>("DepartmentDetailsVM");
            builder.EntitySet<DepartmentStoreItemPrintVM>("DepartmentStoreItemPrintVM");


            builder.EntitySet<StoreItemsDistributionVM>("StoreItemsDistributionVM");
            builder.EntitySet<DistributionDetailsVM>("DistributionDetailsVM");
            builder.EntitySet<StoreItemsDistributionPrintVM>("StoreItemsDistributionPrintVM");

            // builder.EntitySet<DailyStoreItemVM>("DailyStoreItemVM");
            builder.EntitySet<DistributeStoreItemVM>("DistributeStoreItemVM");
            builder.EntitySet<StoreBookVM>("StoreBookVM");

            builder.EntitySet<DailyStoreItemsVM>("DailyStoreItemsVM");
            //b/*uilder.EntitySet<DeductionVM>("DeductionVM");*/
            builder.EntitySet<Deduction>("Deduction");
            builder.EntitySet<Budget>("Budget");
            builder.EntitySet<BaseItem>("BaseItem");
            builder.EntitySet<Addition>("Addition");
            builder.EntitySet<Invoice>("Invoice");
            builder.EntitySet<Location>("Location");
            // builder.EntitySet<Department>("Department");
            builder.EntitySet<ExchangeOrder>("ExchangeOrder");
            builder.EntitySet<RefundOrder>("RefundOrder");
            builder.EntitySet<Inventory.Data.Entities.Unit>("Unit");
            builder.EntitySet<Supplier>("Supplier");
            builder.EntitySet<ExternalEntity>("ExternalEntity");
            builder.EntitySet<Department>("Department");
            builder.EntitySet<Location>("Location");
            builder.EntitySet<JobTitle>("JobTitle");
            builder.EntitySet<ItemCategory>("ItemCategory");
            builder.EntitySet<ExaminationCommitte>("Examination");
            builder.EntitySet<Employees>("Employees");
            builder.EntitySet<Store>("Store");
            builder.EntitySet<TechnicalDepartment>("TechnicalDepartment");
            builder.EntitySet<Book>("Book");
            builder.EntitySet<StoreItem>("StoreItem");
            builder.EntitySet<InvoiceStoreItem>("InvoiceStoreItem");
            builder.EntitySet<Transformation>("Transformation");
            builder.EntitySet<RobbingOrder>("RobbingOrder");
            builder.EntitySet<StockTaking>("StockTaking");
            //var StockTaking = builder.EntitySet<StockTaking>("StockTaking").EntityType;
            //StockTaking.Property(x => x.CreationDate).();
            builder.EntitySet<Delegation>("Delegation");
            builder.EntitySet<DelegationTrack>("DelegationTrack");
            builder.EntitySet<UserNotification>("Notification");
            builder.EntitySet<Currency>("Currency");
            builder.EntitySet<CustodyPersonVM>("CustodyPersonVM");
            builder.EntitySet<ExecutionOrder>("ExecutionOrder");
            builder.EntitySet<Remains>("Remains");
            builder.EntitySet<RemainsDetails>("RemainsDetails");
            builder.EntitySet<CustomeExecutionOrderVM>("CustomeExecutionOrderVM");




            builder.Namespace = "EntitiesService";
            #region Lookups

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDepartmentsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintBaseItemsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintLocationsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintTechnicalDepartmentsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintSuppliersList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintUnitsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintItemCategoriesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintJobTitlesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExternalEntitiesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintStoresList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintBooksList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintEmployeesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintRemainsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintRemainsInquiriesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Department>("Department").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Department>("Department");

            builder.EntitySet<Remains>("Remains").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Remains>("Remains");

            builder.EntitySet<BaseItem>("BaseItem").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<BaseItem>("BaseItem");
            builder.EntitySet<Location>("Location").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Location>("Location");

            builder.EntitySet<Supplier>("Supplier").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Supplier>("Supplier");

            builder.EntitySet<ItemCategory>("ItemCategory").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<ItemCategory>("ItemCategory");


            builder.EntitySet<Employees>("Employees").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Employees>("Employees");

            builder.EntitySet<Store>("Store").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Store>("Store");
            //builder
            //    .Function("GetAll")
            //    .ReturnsCollectionFromEntitySet<Store>("Store").SupportedInFilter = true;

            builder.EntitySet<TechnicalDepartment>("TechnicalDepartment").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<TechnicalDepartment>("TechnicalDepartment");

            builder.EntitySet<Inventory.Data.Entities.Unit>("Unit").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Inventory.Data.Entities.Unit>("Unit");

            builder.EntitySet<JobTitle>("JobTitle").EntityType.Collection
               .Function("GetAll")
               .ReturnsCollectionFromEntitySet<JobTitle>("JobTitle");

            builder.EntitySet<ExternalEntity>("ExternalEntity").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<ExternalEntity>("ExternalEntity");

            builder.EntitySet<Book>("Book").EntityType.Collection
                .Function("GetAll")
                .ReturnsCollectionFromEntitySet<Book>("Book");

            builder.EntitySet<StoreItem>("StoreItem").EntityType.Collection
                .Function("GetAvailable")
                .ReturnsCollectionFromEntitySet<StoreItem>("StoreItem");

            builder.EntitySet<StoreItem>("StoreItem").EntityType.Collection
                .Function("GetRobbingStoreItems")
                .ReturnsCollectionFromEntitySet<StoreItem>("StoreItem");
            #endregion

            #region Inquiry
            builder.EntitySet<StoreItem>("Inquiry").EntityType.Collection
                .Function("GetStagnantStoreItems")
                .ReturnsCollectionFromEntitySet<StoreItem>("Inquiry").Parameter<string>("stagnantDate");

            builder.EntitySet<StoreItem>("Inquiry").EntityType.Collection
                .Function("getStagnantStoreItemsByBaseItemId")
                .ReturnsCollectionFromEntitySet<StoreItem>("Inquiry");//.Parameter<string>("stagnantDate");


            builder.EntitySet<Remains>("Remains").EntityType.Collection
              .Function("GetRemainsDetails")
              .ReturnsCollectionFromEntitySet<Remains>("Remains");

            #endregion


            #region Lists

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExaminationCommitteList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExaminationCommitteListTest")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintAdditionList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintRefundOrdersList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExchangeOrdersList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExecutionOrderList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintInvoicesList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintTransformationsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintRobbingOrdersList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
               .Function("PrintStockTakingList")
               .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Addition>("Addition").EntityType.Collection
                .Function("GetALL")
                .ReturnsCollectionFromEntitySet<Addition>("Addition");
            builder.EntitySet<Addition>("Addition").EntityType.Collection
                .Function("GetAddition")
                .ReturnsCollectionFromEntitySet<Addition>("Addition");

            builder.EntitySet<Transformation>("Transformation").EntityType.Collection
                .Function("GetAllTransformationView")
                .ReturnsCollectionFromEntitySet<Transformation>("Transformation");

            builder.EntitySet<RefundOrder>("RefundOrder").EntityType.Collection
                .Function("GetList")
                .ReturnsCollectionFromEntitySet<RefundOrder>("RefundOrder");

            builder.EntitySet<ExchangeOrder>("ExchangeOrder").EntityType.Collection
                .Function("GetList")
                .ReturnsCollectionFromEntitySet<ExchangeOrder>("ExchangeOrder");

            builder.EntitySet<ExecutionOrder>("ExecutionOrder").EntityType.Collection
                .Function("GetList")
                .ReturnsCollectionFromEntitySet<ExecutionOrder>("ExecutionOrder");

            builder.EntitySet<ExecutionOrder>("ExecutionOrder").EntityType.Collection
                .Function("GetExecutionOrderList")
                .ReturnsCollectionFromEntitySet<ExecutionOrder>("ExecutionOrder");

            builder.EntitySet<RobbingOrder>("RobbingOrder").EntityType.Collection
                .Function("GetRobbingOrderList")
                .ReturnsCollectionFromEntitySet<RobbingOrder>("RobbingOrder");
            #endregion
            #region Reports
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetTechnicianStoreItemsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");


            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetExistingStoreItemsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetTechnicianStoreItemsDetailsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetDepartmentStoreItemsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetDepartmentDetailsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetStoreItemsDistributionForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetDistributionDetailsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetStoreBook")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetCustodyPersonVMReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetInvoiceForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reportstore").EntityType.Collection
                .Function("GetInvoiceStoreItemsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reportstore");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("GetDailyStoreItemsForReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDailyStoreItemsReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintStoreBook")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");

            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintExistingStoreItemsReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDepartmentStoreItemsReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintTechnicianStoreItemsReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintCustodyPersonReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintStoreItemsDistributionReport")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDelegationsList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDelegationsTrackList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            builder.EntitySet<Invoice>("Reports").EntityType.Collection
                .Function("PrintDeductionList")
                .ReturnsCollectionFromEntitySet<Invoice>("Reports").Parameter<string>("paramters");
            #endregion

            #region Delegation




            builder.EntitySet<Delegation>("Delegation").EntityType.Collection
                .Function("GetDelegationTrack")
                .ReturnsCollectionFromEntitySet<Delegation>("Delegation");
            #endregion


        }

        public static void AddEntitiesScope(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRobbingOrderBussiness), typeof(RobbingOrderBussiness));
            services.AddTransient(typeof(INotificationBussiness), typeof(NotificationBussiness));
            services.AddTransient(typeof(IStockTakingBussiness), typeof(StockTakingBussiness));
            services.AddTransient(typeof(ITenantProvider), typeof(TenantProvider));
            services.AddTransient(typeof(IExaminationBusiness), typeof(ExaminationBusiness));
            services.AddTransient(typeof(IAttachmentBussiness), typeof(AttachmentBussiness));
            services.AddTransient(typeof(IAttachmentTypeBussiness), typeof(AttachmentTypeBussiness));
            services.AddTransient(typeof(IOperationAttachmentTypeBussiness), typeof(OpertaionAttachmentTypeBussiness));
            services.AddTransient(typeof(IBaseItemBussiness), typeof(BaseItemBussiness));
            services.AddTransient(typeof(IItemCategoryBussiness), typeof(ItemCategoryBussiness));
            services.AddTransient(typeof(IUnitBussiness), typeof(UnitBussiness));
            services.AddTransient(typeof(IBudgetBussiness), typeof(BudgetBussiness));
            services.AddTransient(typeof(ISupplierBussiness), typeof(SupplierBussiness));
            services.AddTransient(typeof(IExternalEntityBussiness), typeof(ExternalEntityBussiness));
            services.AddTransient(typeof(IJobTitleBussiness), typeof(JobTitleBussiness));
            services.AddTransient(typeof(IEmployeeBussiness), typeof(EmployeeBussiness));
            services.AddTransient(typeof(IUserBusiness), typeof(UserBusiness));
            services.AddTransient(typeof(IAdditionBussiness), typeof(AdditionBussiness));
            services.AddTransient(typeof(ICommiteeItemBussiness), typeof(CommiteeItemBussiness));
            services.AddScoped(typeof(IStoreItemBussiness), typeof(StoreItemBussiness));
            services.AddScoped(typeof(IStoreItemCopyBussiness), typeof(StoreItemCopyBussiness));
            services.AddScoped(typeof(IRobbingOrderStoreItemBussiness), typeof(RobbingOrderStoreItemBussiness));
            services.AddScoped(typeof(ITransformationStoreItemBussiness), typeof(TransformationStoreItemBussiness));
            services.AddScoped(typeof(IStoreBussiness), typeof(StoreBussiness));
            services.AddTransient(typeof(INoteBooksBussiness), typeof(NoteBooksBussiness));
            services.AddTransient(typeof(ICodeGenerator), typeof(CodeGenerator));
            services.AddTransient(typeof(IWordGenerator), typeof(WordGenerator));
            services.AddTransient(typeof(IWordBusiness), typeof(WordBusiness));
            services.AddScoped(typeof(IIdentityProvider), typeof(IdentityProvider));
            services.AddScoped(typeof(ILdapAuthenticationService), typeof(LdapAuthenticationService));
            services.AddScoped(typeof(IInvoiceBussiness), typeof(InvoiceBussiness));
            services.AddScoped(typeof(IExchangeOrderBussiness), typeof(ExchangeOrderBussiness));
            services.AddScoped(typeof(IRefundOrderBussiness), typeof(RefundOrderBussiness));
            services.AddScoped(typeof(IDepartmentBussiness), typeof(DepartmentBussiness));
            services.AddScoped(typeof(ILocationBussiness), typeof(LocationBussiness));
            services.AddScoped(typeof(ITechnicalDepartmentBussiness), typeof(TechnicalDepartmentBussiness));
            services.AddScoped(typeof(IBookBussiness), typeof(BookBussiness));
            services.AddScoped(typeof(IInvoiceStoreItemBussiness), typeof(InvoiceStoreItemBussiness));
            services.AddScoped(typeof(ITransformationRequestBussiness), typeof(TransformationRequestBussiness));
            services.AddScoped(typeof(IRefundOrderBussiness), typeof(RefundOrderBussiness));
            services.AddScoped(typeof(IStockTakingBussiness), typeof(StockTakingBussiness));
            services.AddScoped(typeof(IStagnantBussiness), typeof(StagnantBussiness));
            services.AddScoped(typeof(IReportBusiness), typeof(ReportBusiness));
            services.AddScoped(typeof(IDelegationBussiness), typeof(DelegationBussiness));
            services.AddScoped(typeof(IDeductionBusiness), typeof(DeductionBusiness));
            services.AddScoped(typeof(ICurrencyBussiness), typeof(CurrencyBussiness));
            services.AddScoped(typeof(IExecutionOrderBussiness), typeof(ExecutionOrderBussiness));
            services.AddScoped(typeof(IRemainsBussiness), typeof(RemainsBussiness));
            services.AddScoped(typeof(IRobbingOrderRemainsBussiness), typeof(RobbingOrderRemainsBussiness));
            services.AddScoped(typeof(ISubtractionBussiness), typeof(SubtractionBussiness));
            services.AddScoped(typeof(IRobbedStoreItemBussiness), typeof(RobbedStoreItemBussiness));
            

        }


    }
}
