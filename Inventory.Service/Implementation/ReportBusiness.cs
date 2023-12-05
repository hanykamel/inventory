using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.FinancialYear;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.ReportVM;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Inventory.Service.Implementation
{
    public class IEQ : IEqualityComparer<StoreBookVM>
    {
        public bool Equals(StoreBookVM x, StoreBookVM y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(StoreBookVM obj)
        {
            return (int)obj.Id;
        }
    }
    public partial class ReportBusiness : IReportBusiness
    {
        readonly private IRepository<Invoice, Guid> _invoiceRepository;
        readonly private IRepository<InvoiceStoreItem, Guid> _invoiceStoreItemRepository;
        readonly private IRepository<StoreItem, Guid> _storeItemRepository;
        readonly private IRepository<RobbedStoreItem, Guid> _robbedStoreItemRepository;

        readonly private IRepository<BaseItem, long> _baseItemRepository;
        readonly private IRepository<Addition, Guid> _additionRepository;
        readonly private ITenantProvider _tenantProvider;
        readonly private IRepository<Transformation, Guid> _transformationRepository;
        readonly private IRepository<TransformationStoreItem, Guid> _transformationStoreItemRepository;
        readonly private IRepository<RobbingOrderStoreItem, Guid> _robbingStoreItemRepository;
        readonly private IRepository<ExecutionOrderStoreItem, Guid> _executionStoreItemRepository;

        readonly private IRepository<DeductionStoreItem, Guid> _deductionStoreItemRepository;

        readonly private IRepository<RobbingOrder, Guid> _robbingOrderRepository;
        readonly private IRepository<Subtraction, Guid> _subtractionRepository;
        readonly private IRepository<ExecutionOrder, Guid> _executionOrderRepository;
        readonly private IRepository<Deduction, Guid> _deductionRepository;
        readonly private IRepository<Currency, int> _currencyRepository;




        readonly private IStringLocalizer<SharedResource> _localizer;
        public ReportBusiness(
            IRepository<Invoice, Guid> invoiceRepository,
            IRepository<InvoiceStoreItem, Guid> invoicestoreItemRepository, IRepository<StoreItem, Guid> storeItemRepository,
            IRepository<RobbedStoreItem, Guid> robbedStoreItemRepository,
            IRepository<BaseItem, long> baseItemRepository,
            IRepository<Addition, Guid> additionRepository,
            IRepository<Transformation, Guid> transformationRepository,
            IRepository<TransformationStoreItem, Guid> transformationStoreItemRepository,
            IRepository<RobbingOrderStoreItem, Guid> robbingStoreItemRepository,

            IRepository<RobbingOrder, Guid> robbingOrderRepository,
            IRepository<ExecutionOrder, Guid> executionOrderRepository,
            IRepository<Deduction, Guid> deductionRepository,
            IRepository<Subtraction, Guid> subtractionRepository,
            IRepository<ExecutionOrderStoreItem, Guid> executionStoreItemRepository,

            IRepository<DeductionStoreItem, Guid> deductionStoreItemRepository,
            IRepository<Currency, int> currencyRepository,
        ITenantProvider tenantProvider,
            IStringLocalizer<SharedResource> localizer
            )
        {
            _invoiceRepository = invoiceRepository;
            _invoiceStoreItemRepository = invoicestoreItemRepository;
            _storeItemRepository = storeItemRepository;
            _robbedStoreItemRepository = robbedStoreItemRepository;
            _baseItemRepository = baseItemRepository;
            _additionRepository = additionRepository;
            _tenantProvider = tenantProvider;
            _transformationRepository = transformationRepository;
            _robbingOrderRepository = robbingOrderRepository;
            _executionOrderRepository = executionOrderRepository;
            _deductionRepository = deductionRepository;
            _subtractionRepository = subtractionRepository;
            _transformationStoreItemRepository = transformationStoreItemRepository;
            _robbingStoreItemRepository = robbingStoreItemRepository;
            _deductionStoreItemRepository = deductionStoreItemRepository;
            _executionStoreItemRepository = executionStoreItemRepository;
            _currencyRepository = currencyRepository;
            _localizer = localizer;
        }

        public IQueryable<DepartmentDetailsVM> GetDepartmentDetailsReport()
        {
         

            return _invoiceStoreItemRepository.GetAll()
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.BaseItem)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.ReceivedEmployee)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Department)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Location)
                .Where(x => x.IsRefunded != true)
                .GroupBy(x => new
                {
                    BaseItemId = x.StoreItem.BaseItemId,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    BookNumber = x.StoreItem.Book.BookNumber,
                    PageNumber = x.StoreItem.BookPageNumber,
                    //DepartmentId = x.Invoice.ReceivedEmployee.Department != null ? x.Invoice.ReceivedEmployee.DepartmentId : 0,
                    //DepartmentName = x.Invoice.ReceivedEmployee.Department != null ? x.Invoice.ReceivedEmployee.Department.Name : "",
                    DepartmentId = x.Invoice.Department != null ? x.Invoice.DepartmentId : 0,
                    DepartmentName = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                    ReceiverName = x.Invoice.ReceivedEmployee.Name,
                    Location = x.Invoice.Location.Name,
                    BudgetId = x.StoreItem.Addition.BudgetId,
                    TenantId = x.StoreItem.TenantId
                    // Notes=x.StoreItem.Note
                })

                .Select(c => new DepartmentDetailsVM
                {
                    Id = c.Key.BaseItemId,
                    BaseItemName = c.Key.BaseItemName,
                    DepartmentId = c.Key.DepartmentId,
                    DepartmentName = c.Key.DepartmentName,
                    ReceiverName = c.Key.ReceiverName,
                    Location = c.Key.Location,
                    BookNumber = c.Key.BookNumber,
                    PageNumber = c.Key.PageNumber,
                    BudgetId = c.Key.BudgetId,
                    // Notes = c.Key.Notes,
                    Amount = c.Count(),

                });

        }


        public IQueryable<DepartmentStoreItemVM> GetDepartmentStoreItemsReport()
        {

            return _storeItemRepository.GetAll()
                    .Include(x => x.BaseItem).ThenInclude(x => x.ItemCategory)
                    .Include(x => x.Book)
                    .Select(c => new
                    {
                        baseItemId = c.BaseItemId,
                        baseItemName = c.BaseItem.Name,
                        pageNumber = c.BookPageNumber,
                        bookNumber = c.Book.BookNumber,
                        storeID = c.TenantId,
                        BudgetId = c.Addition.BudgetId,
                        ExaminationCommitte = c.Addition.ExaminationCommitte,
                        statusStore = (c.CurrentItemStatusId == (int)ItemStatusEnum.Available || c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved),
                        statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
                        CategoryId = c.BaseItem.ItemCategoryId,
                        CategoryName = c.BaseItem.ItemCategory.Name,
                        creationDate = c.CreationDate
                    })
                    .GroupBy(c => new
                    {
                        baseItemId = c.baseItemId,
                        BaseItemName = c.baseItemName,
                        PageNumber = c.pageNumber,
                        BookNumber = c.bookNumber,
                        TenantId = c.storeID,
                        ExaminationCommitte = c.ExaminationCommitte,
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        BudgetId = c.BudgetId
                        //  CreationDate = c.creationDate
                    })
                    .Select(c => new DepartmentStoreItemVM
                    {
                        Id = c.Key.baseItemId,
                        BaseItemName = c.Key.BaseItemName,
                        BookNumber = c.Key.BookNumber,
                        PageNumber = c.Key.PageNumber,
                        // CreationDate=c.Key.CreationDate,
                        TenantId = c.Key.TenantId,
                        CategoryId = c.Key.CategoryId,
                        ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                        CategoryName = c.Key.CategoryName,
                        BudgetId = c.Key.BudgetId,
                        storeQuantity = c.Count(i => i.statusStore),
                        paidQuantity = c.Count(i => i.statusExpensed),
                        AllQuantity = c.Count()

                    });
            #region OldCode

            //    var availableQuantity =
            //         from i in _storeItemRepository.GetAll()

            //         where i.CurrentItemStatusId == (int)ItemStatusEnum.Available
            //         group i by i.BaseItemId
            //                    into grp
            //         select new
            //         {
            //             baseItemId = grp.Key,
            //             count = grp.Count()
            //         }
            //       ;




            //    var AllQuantity =
            //        from i in _storeItemRepository.GetAll()
            //        .Include("BaseItem")
            //        .Include("Unit").Include("Book").Include("InvoiceStoreItem")
            //        group i by i.BaseItemId
            //                   into grp
            //        select new
            //        {
            //            baseItemId = grp.Key,
            //            count = grp.Count(),
            //            BaseItemName = grp.First().BaseItem.Name,
            //            UnitName = grp.First().Unit == null ? "" : grp.First().Unit.Name,
            //            BookNumber = grp.First().Book.BookNumber,
            //            PageNumber = grp.First().BookPageNumber,
            //            notes = grp.First().Note,
            //            creationDate = grp.First().CreationDate,
            //            //  department= grp.First().InvoiceStoreItem.FirstOrDefault().Invoice.ReceivedEmployee.Department==null?"": grp.First().InvoiceStoreItem.FirstOrDefault().Invoice.ReceivedEmployee.Department.Name

            //        };


            //    var invoiceStoreItems =

            //        from i in _invoiceStoreItemRepository.GetAll()
            //        .Include(x => x.StoreItem)
            //        .ThenInclude(x => x.BaseItem)
            //        .Include(x => x.Invoice)
            //        .ThenInclude(x => x.ReceivedEmployee)
            //        .ThenInclude(x => x.Department)
            //        .Where(x => x.Invoice.ReceivedEmployee.Department != null)
            //        group i by i.Invoice.ReceivedEmployee.DepartmentId
            //        into grp
            //        //.Select(x => new
            //        select new
            //        {
            //            department = grp.Key,
            //            baseItemId = grp.First().StoreItem.BaseItemId,
            //            departmentName =
            //       grp.First().Invoice.ReceivedEmployee.Department != null ?
            //grp.First().Invoice.ReceivedEmployee.Department.Name : ""
            //        }
            //        ;



            //    return from i in AllQuantity
            //           join x in availableQuantity on i.baseItemId equals x.baseItemId
            //           join y in invoiceStoreItems on i.baseItemId equals y.baseItemId
            //           select new DepartmentStoreItemVM
            //           {

            //               BookNumber = i.BookNumber,
            //               CreationDate = i.creationDate,
            //               PageNumber = i.PageNumber,
            //               AllQuantity = i.count,
            //               paidQuantity = i.count - x.count,
            //               storeQuantity = x.count,
            //               //   notes = i.notes == null ? "" : i.notes,
            //               //     DepartmentDetails[] = y.departmentName == null ? "" : y.departmentName
            //           };
            #endregion
        }

        public IQueryable<DepartmentStoreItemPrintVM> GetDepartmentStoreItemsForPrintReport()
        {
            var invoiceStoreItems =
                 _invoiceStoreItemRepository.GetAll()
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.BaseItem)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.ReceivedEmployee)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Department)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Location)
                .Where(x => x.IsRefunded != true)
                .GroupBy(x => new
                {
                    BookNumber = x.StoreItem.Book.BookNumber,
                    PageNumber = x.StoreItem.BookPageNumber,
                    BudgetId = x.StoreItem.Addition.BudgetId,
                    TenantId = x.StoreItem.TenantId,
                    baseItemId = x.StoreItem.BaseItemId,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    DepartmentId = x.Invoice.Department != null ? x.Invoice.DepartmentId : 0,
                    DepartmentName = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                    ReceiverName = x.Invoice.ReceivedEmployee.Name,
                    Location = x.Invoice.Location.Name,
                    Note = x.StoreItem.Note
                    // CreationDate=x.StoreItem.CreationDate
                })
                .Select(c => new
                {
                    baseItemId = c.Key.baseItemId,
                    BaseItemName = c.Key.BaseItemName,
                    DepartmentId = c.Key.DepartmentId,
                    DepartmentName = c.Key.DepartmentName,
                    ReceiverName = c.Key.ReceiverName,
                    Location = c.Key.Location,
                    Notes = c.Key.Note,
                    BookNumber = c.Key.BookNumber,
                    PageNumber = c.Key.PageNumber,
                    BudgetId = c.Key.BudgetId,
                    TenantId = c.Key.TenantId,
                    // CreationDate = c.Key.CreationDate,
                    Amount = c.Count()
                });

            var storeItems =
                _storeItemRepository.GetAll()
                    .Include(x => x.BaseItem).ThenInclude(x => x.ItemCategory)
                    .Include(x => x.Book)
                    .Select(c => new
                    {
                        baseItemId = c.BaseItemId,
                        baseItemName = c.BaseItem.Name,
                        pageNumber = c.BookPageNumber,
                        bookNumber = c.Book.BookNumber,
                        ExaminationCommitte = c.Addition.ExaminationCommitte,
                        storeID = c.TenantId,
                        BudgetId = c.Addition.BudgetId,
                        statusAvailable = (c.CurrentItemStatusId == (int)ItemStatusEnum.Available || c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved),
                        statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
                        CategoryId = c.BaseItem.ItemCategoryId,
                        CategoryName = c.BaseItem.ItemCategory.Name,

                    })
                    .GroupBy(c => new
                    {
                        baseItemId = c.baseItemId,
                        BaseItemName = c.baseItemName,
                        PageNumber = c.pageNumber,
                        BookNumber = c.bookNumber,
                        TenantId = c.storeID,
                        ExaminationCommitte = c.ExaminationCommitte,
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName,
                        BudgetId = c.BudgetId,
                    })
                    .Select(c => new
                    {
                        baseItemId = c.Key.baseItemId,
                        BaseItemName = c.Key.BaseItemName,
                        PageNumber = c.Key.PageNumber,
                        BookNumber = c.Key.BookNumber,
                        TenantId = c.Key.TenantId,
                        ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                        CategoryId = c.Key.CategoryId,
                        CategoryName = c.Key.CategoryName,
                        BudgetId = c.Key.BudgetId,
                        storeQuantity = c.Count(i => i.statusAvailable),
                        paidQuantity = c.Count(i => i.statusExpensed),
                        AllQuantity = c.Count(),

                    });

            return from i in invoiceStoreItems
                   join s in storeItems on i.baseItemId equals s.baseItemId
                   where i.TenantId == s.TenantId &&
                         i.PageNumber == s.PageNumber &&
                         i.BookNumber == s.BookNumber &&
                         i.BudgetId == s.BudgetId
                   select new DepartmentStoreItemPrintVM
                   {
                       Id = s.baseItemId,
                       BaseItemName = s.BaseItemName,
                       BookNumber = s.BookNumber,
                       PageNumber = s.PageNumber,
                       //   CreationDate=i.CreationDate,
                       CategoryId = s.CategoryId,
                       CategoryName = s.CategoryName,
                       ContractNumber =s.ContractNumber,
                       TenantId = s.TenantId,
                       AllQuantity = s.AllQuantity,
                       paidQuantity = s.paidQuantity,
                       storeQuantity = s.storeQuantity,
                       DepartmentId = i.DepartmentId,
                       DepartmentName = i.DepartmentName,
                       ReceiverName = i.ReceiverName,
                       BudgetId = s.BudgetId,
                       Location = i.Location,
                       Amount = i.Amount,
                       Notes = i.Notes
                   };


        }




        public IQueryable<ExistingStoreItemVM> GetExistingStoreItemsReport()
        { 
            var storeItems=_storeItemRepository.GetAll()
                   .Include(i => i.BaseItem)
                   .Include(i => i.Unit)
                   .Include(i => i.Book)
                   .Select(c => new
                   {
                       baseItemId = c.BaseItemId,
                       BudgetId = c.Addition.BudgetId,
                       //   creationDate = c.CreationDate,
                       BaseItemName = c.BaseItem.Name,
                       UnitName = c.Unit != null ? c.Unit.Name : "",
                       StoreID = c.TenantId,
                       PageNumber = c.BookPageNumber,
                       BookNumber = c.Book.BookNumber,
                       ExaminationCommitte = c.Addition.ExaminationCommitte,
                       statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
                       statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
                       statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
                   })
                   .GroupBy(c => new
                   {
                       baseItemId = c.baseItemId,
                       //    creationDate = c.creationDate,
                       BaseItemName = c.BaseItemName,
                       UnitName = c.UnitName,
                       StoreID = c.StoreID,
                       ExaminationCommitte = c.ExaminationCommitte,
                       bookNumber = c.BookNumber,
                       pageNumber = c.PageNumber,
                       BudgetId = c.BudgetId
                   })

                 .Select(c => new ExistingStoreItemVM
                 {
                     Id = c.Key.baseItemId,
                     BaseItemId = c.Key.baseItemId,
                     BudgetId = c.Key.BudgetId,
                     AllQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved) + c.Count(i => i.statusExpensed),
                     availableQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved),
                     ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                     //  CreationDate = c.Key.creationDate,
                     BaseItemName = c.Key.BaseItemName,
                     UnitName = c.Key.UnitName,
                     differenceQuantity = c.Count(i => i.statusExpensed),
                     TenantId = c.Key.StoreID,
                     BookNumber = c.Key.bookNumber,
                     PageNumber = c.Key.pageNumber
                 });

            return storeItems.Union(GetExistingRobbedStoreItemsReport());
            //return x;
        }

        public IQueryable<ExistingStoreItemVM> GetExistingRobbedStoreItemsReport()
        {
            return
            _robbedStoreItemRepository.GetAll()
                   .Include(i => i.BaseItem)
                   .Include(i => i.Unit)
                   .Include(i => i.Book)
                   .Select(c => new
                   {
                       baseItemId = c.BaseItemId,
                       budgetId = c.Addition.BudgetId,
                       baseItemName = c.BaseItem.Name,
                       unitName = c.Unit != null ? c.Unit.Name : "",
                       storeID = c.TenantId,
                       pageNumber = c.BookPageNumber,
                       bookNumber = c.Book.BookNumber,
                       quantity=c.Quantity
                   })
                   .GroupBy(c => new
                   {
                       baseItemId = c.baseItemId,
                       //    creationDate = c.creationDate,
                       baseItemName = c.baseItemName,
                       unitName = c.unitName,
                       storeID = c.storeID,
                       bookNumber = c.bookNumber,
                       pageNumber = c.pageNumber,
                       budgetId = c.budgetId,
                       quantity=c.quantity
                   })

                 .Select(c => new ExistingStoreItemVM
                 {
                     Id = c.Key.baseItemId,
                     BaseItemId = c.Key.baseItemId,
                     BudgetId = c.Key.budgetId,
                     AllQuantity = c.Sum(i => i.quantity),
                     availableQuantity = c.Sum(i=>i.quantity),
                     //  CreationDate = c.Key.creationDate,
                     BaseItemName = c.Key.baseItemName,
                     UnitName = c.Key.unitName,
                     differenceQuantity = 0,
                     TenantId = c.Key.storeID,
                     BookNumber = c.Key.bookNumber,
                     PageNumber = c.Key.pageNumber
                 });
            //return x;
        }

        public IQueryable<Invoice> GetInvoiceReport()
        {
            var InvoiceList = _invoiceRepository.GetAll();
            return InvoiceList;
        }

        public IQueryable<InvoiceStoreItem> GetInvoiceStoreItemReport()
        {
            var InvoicestoreItemList =
                _invoiceStoreItemRepository.GetAll().Where(x => x.IsRefunded == false || x.IsRefunded == null);//.Include(x => x.Invoice).ThenInclude(x => x.ExchangeOrder).ThenInclude(x => x.Budget);
                                                                                                               //.Include(x=>x.StoreItem).ThenInclude(x=>x.Addition).ThenInclude(x=>x.Budget);

            return InvoicestoreItemList;
        }

        public IQueryable<CustodyPersonVM> GetCustodyPersonReport()
        {
            var CustodyPersonItemList =
                _invoiceStoreItemRepository.GetAll()
                .Where(x => x.IsRefunded == false || x.IsRefunded == null)
                .Include(x => x.StoreItem).ThenInclude(x => x.BaseItem)
                .Include(x => x.StoreItem).ThenInclude(x => x.Book)
                .Include(x => x.StoreItem).ThenInclude(x => x.Unit)
                .Include(x => x.Invoice).ThenInclude(x => x.ExchangeOrder).ThenInclude(x => x.Budget)
                .Include(x => x.Invoice).ThenInclude(x => x.ReceivedEmployee)

                .GroupBy(x => new
                {
                    BaseItemId = x.StoreItem.BaseItemId,
                    BaseName = x.StoreItem.BaseItem.Name,
                    bookNum = x.StoreItem.Book.BookNumber,
                    pagenum = x.StoreItem.BookPageNumber,
                    budget = x.Invoice.ExchangeOrder.Budget.Name,
                    unitname = x.StoreItem.Unit.Name,
                    BudgetId = x.Invoice.ExchangeOrder.BudgetId,
                    ReceivedEmployee = x.Invoice.ReceivedEmployee.Name,
                    ReceivedEmployeeId = x.Invoice.ReceivedEmployeeId,
                    ReceivedEmployeeCard = x.Invoice.ReceivedEmployee.CardCode,
                    ReceivedDate = x.Invoice.Date,
                    TenantId = x.TenantId

                })
                .Select(a => new CustodyPersonVM()
                {
                    Id = a.Key.BaseItemId,
                    BaseName = a.Key.BaseName,
                    BookNumber = a.Key.bookNum,
                    baseId = a.Key.BaseItemId,
                    BudgetName = a.Key.budget,
                    PageNumber = a.Key.pagenum,
                    CreationDate = "",
                    UnitName = a.Key.unitname,
                    BudgetId = a.Key.BudgetId,
                    ReceivedEmployee = a.Key.ReceivedEmployee,
                    ReceivedEmployeeId = a.Key.ReceivedEmployeeId,
                    ReceivedEmployeeCard = a.Key.ReceivedEmployeeCard,
                    ReceivedDate = a.Key.ReceivedDate,
                    TenantId = a.Key.TenantId,
                    StoreItemCount = a.Count()
                });


            return CustodyPersonItemList;
        }
        public IQueryable<CustodyPersonVM> GetCustodyPersonReportForPrint()
        {
            var CustodyPersonItemList =
                _invoiceStoreItemRepository.GetAll()
                .Where(x => x.IsRefunded == false || x.IsRefunded == null)
                .Include(x => x.StoreItem).ThenInclude(x => x.BaseItem)
                .Include(x => x.StoreItem).ThenInclude(x => x.Book)
                .Include(x => x.StoreItem).ThenInclude(x => x.Unit)
                .Include(x => x.Invoice).ThenInclude(x => x.ExchangeOrder).ThenInclude(x => x.Budget)
                .Include(x => x.Invoice).ThenInclude(x => x.ReceivedEmployee)
                .Include(x=>x.StoreItem).ThenInclude(x => x.Addition).ThenInclude(x => x.ExaminationCommitte)
                .GroupBy(x => new
                {
                    BaseItemId = x.StoreItem.BaseItemId,
                    BaseName = x.StoreItem.BaseItem.Name,
                    bookNum = x.StoreItem.Book.BookNumber,
                    pagenum = x.StoreItem.BookPageNumber,
                    budget = x.Invoice.ExchangeOrder.Budget.Name,
                    unitname = x.StoreItem.Unit.Name,
                    BudgetId = x.Invoice.ExchangeOrder.BudgetId,
                    ReceivedEmployee = x.Invoice.ReceivedEmployee.Name,
                    ReceivedEmployeeId = x.Invoice.ReceivedEmployeeId,
                    ReceivedEmployeeCard = x.Invoice.ReceivedEmployee.CardCode,
                    ReceivedDate = x.Invoice.Date,
                    TenantId = x.TenantId,
                    contractnum=x.StoreItem.Addition.ExaminationCommitte.ContractNumber,

                })
                .Select(a => new CustodyPersonVM()
                {
                    Id = a.Key.BaseItemId,
                    BaseName = a.Key.BaseName,
                    BookNumber = a.Key.bookNum,
                    baseId = a.Key.BaseItemId,
                    BudgetName = a.Key.budget,
                    PageNumber = a.Key.pagenum,
                    CreationDate = "",
                    UnitName = a.Key.unitname,
                    BudgetId = a.Key.BudgetId,
                    ReceivedEmployee = a.Key.ReceivedEmployee,
                    ReceivedEmployeeId = a.Key.ReceivedEmployeeId,
                    ReceivedEmployeeCard = a.Key.ReceivedEmployeeCard,
                    ReceivedDate = a.Key.ReceivedDate,
                    TenantId = a.Key.TenantId,
                    StoreItemCount = a.Count(),
                    ContractNumber=a.Key.contractnum
                });


            return CustodyPersonItemList;
        }
        public IQueryable<InvoiceStoreItem> PrintCustodyPersonReport()
        {
            var InvoicestoreItemList = _invoiceStoreItemRepository.GetAll()
                .Where(x => x.IsRefunded == false || x.IsRefunded == null)
                .Include(i => i.StoreItem).ThenInclude(c => c.BaseItem)
                .Include(i => i.StoreItem).ThenInclude(i => i.Unit)
                .Include(i => i.StoreItem).ThenInclude(i => i.Book)
                .Include(i => i.StoreItem).ThenInclude(i => i.Addition).ThenInclude(a => a.Budget)
                .Include(i => i.StoreItem).ThenInclude(i => i.Addition).ThenInclude(a => a.ExaminationCommitte)
                .Include(i => i.Invoice);
            return InvoicestoreItemList;
        }
        //public IQueryable<InvoiceStoreItem> PrintCustodyPersonReport()
        //{
        //    var InvoicestoreItemList = _invoiceStoreItemRepository.GetAll()
        //        .Where(x => x.IsRefunded == false || x.IsRefunded == null)
        //        .Include(i => i.StoreItem).ThenInclude(c => c.BaseItem)
        //        .Include(i => i.StoreItem).ThenInclude(i => i.Unit)
        //        .Include(i => i.StoreItem).ThenInclude(i => i.Book)
        //        .Include(i => i.StoreItem).ThenInclude(i => i.Addition).ThenInclude(a => a.Budget)
        //        .Include(i => i.Invoice);
        //    return InvoicestoreItemList;
        //}
        public IQueryable<InvoiceStoreItemsReportVM> PrintInvoiceStoreItemsReport()
        {
            var InvoicestoreItemList = _invoiceStoreItemRepository.GetAll()
                .Include(c => c.Invoice).ThenInclude(c => c.ReceivedEmployee)
                .Include(c => c.Invoice).ThenInclude(c => c.Location)
                .Include(c => c.StoreItem)
                .ThenInclude(c => c.Unit)
                .Include(c => c.StoreItem)
                .ThenInclude(c => c.BaseItem)
                .Select(c => new InvoiceStoreItemsReportVM
                {
                    Code = c.StoreItem.Code,
                    StoreItem = c.StoreItem.BaseItem.Name,
                    Unit = c.StoreItem.Unit.Name,
                    Price = c.StoreItem.Price,
                    CreationDate = c.CreationDate,
                    Quantity = 1,
                    TenantId = c.TenantId,
                    Invoice = new InvoiceContainerVM()
                    {
                        Code = c.Invoice.Code,
                        ReceivedEmployeeName = c.Invoice.ReceivedEmployee.Name,
                        EmployeeCardCode = c.Invoice.ReceivedEmployee.CardCode,
                        LocationName = c.Invoice.Location.Name
                    }
                });
            return InvoicestoreItemList;
        }
        public IQueryable<InvoiceStoreItem> GetInvoiceStoreItemsReport()
        {
            return _invoiceStoreItemRepository.GetAll();
        }
        public IQueryable<StoreBookVM> GetStoreBook()
        {

            var storeItems = _storeItemRepository
                 .GetAll()
                 .GroupBy(s => new
                 {
                     tenantId = s.TenantId,
                     Id = s.BaseItemId,
                     ItemName = s.BaseItem.Name,
                     ExaminationCommitte = s.Addition.ExaminationCommitte,
                     BookNumber = s.Book.BookNumber,
                     PageNumber = s.BookPageNumber,
                     UnitId = s.UnitId,
                     BudgetId = s.Addition.BudgetId
                 })
                 .Select(c => new StoreBookVM
                 {
                     Id = c.Key.Id,
                     ItemName = c.Key.ItemName,
                     BookNumber = c.Key.BookNumber,
                     PageNumber = c.Key.PageNumber,
                     ContractNumber = c.Key.ExaminationCommitte!=null ?( c.Key.ExaminationCommitte.ContractNumber!=null? c.Key.ExaminationCommitte.ContractNumber:""):"",
                     TenantId = c.Key.tenantId,
                     BudgetId = c.Key.BudgetId
                 })
                 .OrderBy(c => c.BookNumber)
                 .ThenBy(c => c.PageNumber);

            return storeItems.Union(GetRobbedStoreBook()); 
        }

        public IQueryable<StoreBookVM> GetRobbedStoreBook()
        {

            var robbedStoreItems = _robbedStoreItemRepository
                 .GetAll()
                 .GroupBy(s => new
                 {
                     tenantId = s.TenantId,
                     Id = s.BaseItemId,
                     ItemName = s.BaseItem.Name,
                     BookNumber = s.Book.BookNumber,
                     PageNumber = s.BookPageNumber,
                     UnitId = s.UnitId,
                     BudgetId = s.Addition.BudgetId
                 })
                 .Select(c => new StoreBookVM
                 {
                     Id = c.Key.Id,
                     ItemName = c.Key.ItemName,
                     BookNumber = c.Key.BookNumber,
                     PageNumber = c.Key.PageNumber,
                     TenantId = c.Key.tenantId,
                     BudgetId = c.Key.BudgetId
                 })
                 .OrderBy(c => c.BookNumber)
                 .ThenBy(c => c.PageNumber);

            return robbedStoreItems;
        }
        public IQueryable<DailyStoreItemsVM> GetAllStoreItemsDailyReport()
        {


            var AdditionResult = _additionRepository.GetAll(true)
                .Where(x => (_tenantProvider.GetTenant() == 0 || x.TenantId == _tenantProvider.GetTenant()) &&
                            x.Date >= FinancialYearProvider.CurrentYearStartDate &&
                           x.Date <= FinancialYearProvider.CurrentYearEndDate)

                .Include(w => w.Transformation).ThenInclude(x => x.FromStore).ThenInclude(s => s.RobbingBudget)
                .Include(w => w.Transformation).ThenInclude(x => x.FromStore).ThenInclude(s => s.TechnicalDepartment)
                .Include(x => x.RobbingOrder).ThenInclude(x => x.FromStore).ThenInclude(x => x.RobbingBudget)
                .Include(x => x.RobbingOrder).ThenInclude(x => x.FromStore).ThenInclude(x => x.TechnicalDepartment)
                .Include(x => x.ExaminationCommitte)
                .ThenInclude(x => x.Supplier)
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.Currency)
                .Include(x => x.RobbedStoreItem)
                .ThenInclude(x => x.Currency);

            IQueryable<DailyStoreItemsVM> additionModel = AdditionResult.Select(x => new DailyStoreItemsVM
            {

                Id = x.Id,
                Code = Convert.ToString(x.AdditionNumber),
                Date = x.Date,
                BudgetId = x.BudgetId,
                TenantId = x.TenantId,
               // Currency = x.StoreItem.FirstOrDefault().Currency!=null? x.StoreItem.FirstOrDefault().Currency.Name:"",
                Note = x.Note,
                ExaminationRequester = x.ExaminationCommitte == null ? "" : (x.ExaminationCommitte.Supplier == null ?
                (x.ExaminationCommitte.ExternalEntity == null ? "" : x.ExaminationCommitte.ExternalEntity.Name) : x.ExaminationCommitte.Supplier.Name),
                TransRequester = x.Transformation == null ? "" :
                (x.Transformation.FromStore == null ? "" : (x.Transformation.FromStore.TechnicalDepartment == null ? "" : x.Transformation.FromStore.TechnicalDepartment.Name)),
                RobbingRequester = x.RobbingOrder == null ? "" :
                (x.RobbingOrder.FromStore == null ? "" : (x.RobbingOrder.FromStore.TechnicalDepartment == null ? "" : x.RobbingOrder.FromStore.TechnicalDepartment.Name)),
                IsAddition = true, 
                CountStoreAdditionlist =
                x.StoreItem.GroupBy(t => new
                {
                    price = t.Price,
                    id = t.BaseItemId,
                    currency = t.Currency.Name
                })
                .Select(c => new priceItem
                {
                    price = c.Key.price,
                    Currency = c.Key.currency,
                    count = c.Count()
                }).Union
                (
                       x.RobbedStoreItem.GroupBy(t => new
                       {
                           price = t.Price,
                           id = t.BaseItemId,
                           currency = t.Currency.Name,
                           quantity=t.Quantity
                       })
                .Select(c => new priceItem
                {
                    price = c.Key.price *c.Key.quantity,
                    Currency = c.Key.currency,
                    count = c.Count()
                })),
                CountStoreOutgoing = 0,
                CreationDate = x.CreationDate,
            //    Currency = x.StoreItem.FirstOrDefault().Currency.Name
            }); ;

            var TransformationResult = _transformationRepository.GetAll(true)
                    .Where(x => (_tenantProvider.GetTenant() == 0 || x.TenantId == _tenantProvider.GetTenant()) &&
                                x.TransformationStatusId == (int)TransformationOrderStatusEnum.Added
                                &&
                                x.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.CurrentYearStartDate &&
                                x.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.CurrentYearEndDate
                                )
                 .Include(o => o.Budget)
                 .Include(o => o.ToStore).ThenInclude(o => o.StoreType)
                 .Include(o => o.ToStore).ThenInclude(o => o.RobbingBudget)
                 .Include(o => o.ToStore).ThenInclude(o => o.TechnicalDepartment)
                 .Include(o => o.TransformationStoreItem).ThenInclude(it => ((TransformationStoreItem)it).StoreItem)
                .Include(o => o.TransformationStoreItem).ThenInclude(it => ((TransformationStoreItem)it).StoreItem.Currency);




            IQueryable<DailyStoreItemsVM> TransformationModel =
             TransformationResult.Select(x => new DailyStoreItemsVM
             {
                 Id = x.Id,
                 Code = Convert.ToString(x.Subtraction.FirstOrDefault().SubtractionNumber),
                 Date = x.Subtraction.FirstOrDefault().RequestDate,
                 BudgetId = x.BudgetId,
                 TenantId = x.TenantId,
                 //Currency = x.TransformationStoreItem.FirstOrDefault().StoreItem.Currency!=null? x.TransformationStoreItem.FirstOrDefault().StoreItem.Currency.Name:"",
                 Note = x.Notes,
                 Requester = x.ToStore.StoreTypeId == (int)StoreTypeEnum.Robbing ?
                   (x.ToStore.StoreType.Name + " " + x.ToStore.RobbingBudget.Name) :
                        (x.ToStore.StoreType.Name + " " + x.ToStore.TechnicalDepartment.Name),
                 CountStoreAddition = 0,
                 CountStoreAdditionOutgoinglist =
                 x.TransformationStoreItem.GroupBy(t => new 
                 { price = t.StoreItem.Price, id = t.StoreItem.BaseItemId,
                 Currency=t.StoreItem.Currency.Name})
                                           .Select(c => new priceItem 
                                           { price = c.Key.price,
                                               Currency=c.Key.Currency,
                                               count = c.Count() }),

                 CreationDate = x.CreationDate,
   
             });

            var RobbingResult = _robbingOrderRepository.GetAll(true)
                .Where(x => (_tenantProvider.GetTenant() == 0 || x.TenantId == _tenantProvider.GetTenant()) &&
                            x.RobbingOrderStatusId == (int)RobbingOrderStatusEnum.Added
                                &&
                                x.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.CurrentYearStartDate &&
                                x.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.CurrentYearEndDate
                            )
                .Include(o => o.Budget)
                .Include(o => o.ToStore).ThenInclude(o => o.StoreType)
                .Include(o => o.ToStore).ThenInclude(o => o.RobbingBudget)
                .Include(o => o.ToStore).ThenInclude(o => o.TechnicalDepartment)
                .Include(o => o.RobbingOrderStoreItem).ThenInclude(it => ((RobbingOrderStoreItem)it).StoreItem)
            .Include(o => o.RobbingOrderStoreItem).ThenInclude(it => ((RobbingOrderStoreItem)it).StoreItem.Currency);


            IQueryable<DailyStoreItemsVM> RobbingModel =
                RobbingResult.Select(x => new DailyStoreItemsVM
                {
                    Id = x.Id,
                    Code = Convert.ToString(x.Subtraction.FirstOrDefault().SubtractionNumber),
                    //add subtraction
                    //Currency = x.RobbingOrderStoreItem.FirstOrDefault().StoreItem.Currency!=null? x.RobbingOrderStoreItem.FirstOrDefault().StoreItem.Currency.Name:"",
                    Date = x.Subtraction.FirstOrDefault().RequestDate,
                    BudgetId = x.BudgetId,
                    TenantId = x.TenantId,
                    Note = x.Notes,
                    Requester = x.ToStore.StoreTypeId == (int)StoreTypeEnum.Robbing ?

                    (x.ToStore.StoreType.Name + " " + x.ToStore.RobbingBudget.Name) :

                    (x.ToStore.StoreType.Name + " " + x.ToStore.TechnicalDepartment.Name),
                    CountStoreAddition = 0,
                    CountStoreAdditionOutgoinglist = x.RobbingOrderStoreItem.GroupBy(t => new
                    {
                        price = t.StoreItem.Price,
                        id = t.StoreItem.BaseItemId,
                        Currency = t.StoreItem.Currency.Name
                    })
                                           .Select(c => new priceItem
                                           {
                                               price = c.Key.price,
                                               Currency = c.Key.Currency,
                                               count = c.Count()
                                           }),
                    CreationDate = x.CreationDate,

       
                });

            var executionResult = _executionOrderRepository.GetAll(true)
             .Where(x => (_tenantProvider.GetTenant() == 0 || x.TenantId == _tenantProvider.GetTenant()) &&
                         x.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Resulted
                             &&
                             x.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.CurrentYearStartDate &&
                             x.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.CurrentYearEndDate
                         )
             .Include(o => o.Budget)
             .Include(o => o.Store).ThenInclude(o => o.StoreType)
             .Include(o => o.ExecutionOrderStoreItem).ThenInclude(it => ((ExecutionOrderStoreItem)it).StoreItem)
             .Include(o => o.ExecutionOrderStoreItem).ThenInclude(it => ((ExecutionOrderStoreItem)it).StoreItem.Currency)
            .Include(o => o.Subtraction);

            var executionModel =
                executionResult.Select(x => new DailyStoreItemsVM
                {
                    Id = x.Id,
                    Code = Convert.ToString(x.Subtraction.FirstOrDefault().SubtractionNumber),
                    Date = x.Subtraction.FirstOrDefault().RequestDate,
                    BudgetId = x.BudgetId,
                    TenantId = x.TenantId,
                    //Currency = x.ExecutionOrderStoreItem.FirstOrDefault().StoreItem.Currency!=null? x.ExecutionOrderStoreItem.FirstOrDefault().StoreItem.Currency.Name:"",
                    Note = x.Notes,
                    Requester = "",
                    CountStoreAddition = 0,
                    CountStoreAdditionOutgoinglist = x.ExecutionOrderStoreItem.
                    GroupBy(t => new
                    {
                        price = t.StoreItem.Price,
                        id = t.StoreItem.BaseItemId,
                        Currency = t.StoreItem.Currency.Name
                    })
                                           .Select(c => new priceItem
                                           {
                                               price = c.Key.price,
                                               Currency = c.Key.Currency,
                                               count = c.Count()
                                           }),
                    CreationDate = x.CreationDate,

                
                });

            var deductionResult = _deductionRepository.GetAll(true)
          .Where(x => (_tenantProvider.GetTenant() == 0 || x.TenantId == _tenantProvider.GetTenant())
                          &&
                          x.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.CurrentYearStartDate &&
                          x.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.CurrentYearEndDate
                      )
          .Include(o => o.Budget)
          .Include(o => o.DeductionStoreItem).ThenInclude(it => ((DeductionStoreItem)it).StoreItem).Include(o => o.DeductionStoreItem).ThenInclude(it => ((DeductionStoreItem)it).StoreItem.Currency);


            IQueryable<DailyStoreItemsVM> deductionModel =
                deductionResult.Select(x => new DailyStoreItemsVM
                {
                    Id = x.Id,
                    Code = Convert.ToString(x.Subtraction.FirstOrDefault().SubtractionNumber),
                    Date = x.Subtraction.FirstOrDefault().RequestDate,
                    BudgetId = x.BudgetId,
                    TenantId = x.TenantId,
                    Note = x.Notes,
                    Requester = "",
                    CountStoreAddition = 0,
                    //Currency = x.DeductionStoreItem.FirstOrDefault().StoreItem.Currency!=null? x.DeductionStoreItem.FirstOrDefault().StoreItem.Currency.Name:"",
                    CountStoreAdditionOutgoinglist = x.DeductionStoreItem.
                    GroupBy(t => new
                    {
                        price = t.StoreItem.Price,
                        id = t.StoreItem.BaseItemId,
                        Currency = t.StoreItem.Currency.Name
                    })
                                           .Select(c => new priceItem
                                           {
                                               price = c.Key.price,
                                               Currency = c.Key.Currency,
                                               count = c.Count()
                                           }),
                    CreationDate = x.CreationDate,

               
                });

            return
                additionModel
                .Union
                (TransformationModel.Union
                (
                    RobbingModel
                  .Union
                  (executionModel.Union(deductionModel))
                  )
                  )
              ;

        }
        // additionModel.Where(x=>x.CreationDate>=new date)
        public decimal GetLastYearBalance()
        {
           

               var lastYearInGoing = _storeItemRepository.GetAll(
                x => x.Addition.RequestDate >= FinancialYearProvider.LastYearStartDate&&
                           x.Addition.RequestDate <= FinancialYearProvider.LastYearEndDate)
                           .Sum(x => x.Price);

            var lastYearRobbedInGoing = _robbedStoreItemRepository.GetAll(
                x => x.Addition.RequestDate >= FinancialYearProvider.LastYearStartDate &&
                           x.Addition.RequestDate <= FinancialYearProvider.LastYearEndDate)
                           .Sum(x => x.Price*x.Quantity);

            var lastYearTransformationOutGoing = _transformationStoreItemRepository.GetAll(true)
                           .Where(x => (_tenantProvider.GetTenant() == 0 ||
                           x.Transformation.TenantId == _tenantProvider.GetTenant()) &&
                           x.Transformation.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.LastYearStartDate &&
                           x.Transformation.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                           && x.Transformation.TransformationStatusId == (int)TransformationOrderStatusEnum.Added)
                           .Sum(x => x.StoreItem.Price);

            var lastYearRobbingOutGoing = _robbingStoreItemRepository.GetAll(true)
                          .Where(x => (_tenantProvider.GetTenant() == 0 ||
                          x.RobbingOrder.TenantId == _tenantProvider.GetTenant()) &&
                          x.RobbingOrder.Subtraction.FirstOrDefault().RequestDate
                          >= FinancialYearProvider.LastYearStartDate &&
                           x.RobbingOrder.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                          && x.RobbingOrder.RobbingOrderStatusId == (int)RobbingOrderStatusEnum.Added)
                          .Sum(x => x.StoreItem.Price);

            var lastYearExecutionOutGoing = _executionStoreItemRepository.GetAll(true)
                         .Where(x => (_tenantProvider.GetTenant() == 0 ||
                         x.ExecutionOrder.TenantId == _tenantProvider.GetTenant()) &&
                         x.ExecutionOrder.Subtraction.FirstOrDefault().RequestDate
                          >= FinancialYearProvider.LastYearStartDate &&
                           x.ExecutionOrder.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                         && x.ExecutionOrder.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Resulted)
                         .Sum(x => x.StoreItem.Price);

            var lastYeardeductionOutGoing = _deductionStoreItemRepository.GetAll(true)
                       .Where(x => (_tenantProvider.GetTenant() == 0 ||
                       x.Deduction.TenantId == _tenantProvider.GetTenant()) &&
                       x.Deduction.Subtraction.FirstOrDefault().RequestDate
                          >= FinancialYearProvider.LastYearStartDate &&
                           x.Deduction.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                       )
                       .Sum(x => x.StoreItem.Price);

            return lastYearInGoing
                - lastYearRobbingOutGoing
                - lastYearTransformationOutGoing
                - lastYearExecutionOutGoing
                - lastYeardeductionOutGoing;
        }
        public List<LastYearBalanceVM> GetLastYearBalanceGroupedByCurrency()
        {
            
            var currenctList=_currencyRepository.GetAll();

            decimal lastYearInGoing=0,
                     lastYearRobbingOutGoing=0,
                     lastYearTransformationOutGoing=0,
                     lastYearExecutionOutGoing=0,
                     lastYeardeductionOutGoing=0;

            var LastYearBalanceList = new List<LastYearBalanceVM>();
            if (currenctList != null && currenctList.Count() > 0)
            {
                foreach(Currency _currency in currenctList)
                {
                     lastYearInGoing = _storeItemRepository.GetAll(
                      x => x.Addition.RequestDate >= FinancialYearProvider.LastYearStartDate &&
                      x.Addition.RequestDate <= FinancialYearProvider.LastYearEndDate
                      && x.CurrencyId==_currency.Id)
                     .Sum(x => x.Price);

                    lastYearTransformationOutGoing = _transformationStoreItemRepository.GetAll(true)
                                  .Where(x => (_tenantProvider.GetTenant() == 0 ||
                                  x.Transformation.TenantId == _tenantProvider.GetTenant()) &&
                                  x.Transformation.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.LastYearStartDate &&
                                  x.Transformation.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                                  && x.Transformation.TransformationStatusId == (int)TransformationOrderStatusEnum.Added
                                  && x.StoreItem.CurrencyId== _currency.Id)
                                  .Sum(o => o.StoreItem.Price);
                                  

                     lastYearRobbingOutGoing = _robbingStoreItemRepository.GetAll(true)
                                  .Where(x => (_tenantProvider.GetTenant() == 0 ||
                                  x.RobbingOrder.TenantId == _tenantProvider.GetTenant()) &&
                                  x.RobbingOrder.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.LastYearStartDate &&
                                  x.RobbingOrder.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                                  && x.RobbingOrder.RobbingOrderStatusId == (int)RobbingOrderStatusEnum.Added
                                && x.StoreItem.CurrencyId == _currency.Id)
                                  .Sum(o => o.StoreItem.Price);

                     lastYearExecutionOutGoing = _executionStoreItemRepository.GetAll(true)
                                 .Where(x => (_tenantProvider.GetTenant() == 0 ||
                                 x.ExecutionOrder.TenantId == _tenantProvider.GetTenant()) &&
                                 x.ExecutionOrder.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.LastYearStartDate &&
                                 x.ExecutionOrder.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                                 && x.ExecutionOrder.ExecutionOrderStatusId == (int)ExecutionOrderStatusEnum.Resulted && x.StoreItem.CurrencyId == _currency.Id)
                                  .Sum(o => o.StoreItem.Price);

                     lastYeardeductionOutGoing = _deductionStoreItemRepository.GetAll(true)
                               .Where(x => (_tenantProvider.GetTenant() == 0 ||
                               x.Deduction.TenantId == _tenantProvider.GetTenant()) &&
                               x.Deduction.Subtraction.FirstOrDefault().RequestDate >= FinancialYearProvider.LastYearStartDate &&
                               x.Deduction.Subtraction.FirstOrDefault().RequestDate <= FinancialYearProvider.LastYearEndDate
                             && x.StoreItem.CurrencyId == _currency.Id)
                                  .Sum(o => o.StoreItem.Price);


                    LastYearBalanceList.Add(new LastYearBalanceVM
                    {
                        Currency = _currency.Name,
                        Price =
                      (lastYearInGoing
                    - lastYearRobbingOutGoing
                    - lastYearTransformationOutGoing
                    - lastYearExecutionOutGoing
                    - lastYeardeductionOutGoing)
                    });
                }

                return LastYearBalanceList;
            }
            else
                return null;
        }
        public IQueryable<DailyStoreItemVM> GetDailyStoreItemsReport()
        {
            var StoreItemsCount = _storeItemRepository.GetAll().Count();
            var TotalPermissionsCount = _additionRepository.GetAll().Count();

            var expendedCount =

            from i in _storeItemRepository.GetAll().ToList()

            where i.CurrentItemStatusId == (int)ItemStatusEnum.Expenses

            group i by i.BaseItemId
                           into grp
            select new
            {
                baseItemId = grp.Key,
                count = grp.Count()
            };



            var AdditionDetails =
                  from i in _storeItemRepository.GetAll()
                  .Include("Addition")
                  group i by i.AdditionId
                             into grp
                  select new
                  {
                      baseItem = grp.First().BaseItemId,
                      additionId = grp.Key,
                      Date = grp.First().Addition.Date,
                      PermissionCode = grp.First().Addition.Code,
                      FromOrTo = grp.First().Addition.RequesterName,
                      CreationDate = grp.First().Addition.CreationDate,
                      Notes = grp.First().Addition.Note,
                      TotalPermissions = grp.Count(),

                      // Wait for Sara 
                      //shiftedCredit=123,
                      //total=22,
                  };
            return
                from a in AdditionDetails
                join e in expendedCount on a.baseItem equals e.baseItemId
                select new DailyStoreItemVM
                {
                    AdditionId = a.additionId,
                    Date = a.Date,
                    PermissionCode = a.PermissionCode,
                    FromOrTo = a.FromOrTo,
                    AddedItemsValue = StoreItemsCount,
                    Notes = a.Notes,
                    TotalPermissions = a.TotalPermissions,
                    ExpendedItemsValue = e.count,
                    CreationDate = a.CreationDate
                };
        }

        public IQueryable<DistributeStoreItemVM> GetDistributedStoreItemsReport()
        {
            //    var AllQuantity =
            return
                   from i in _invoiceRepository.GetAll()
                   .Include("ReceivedEmployee").Include("InvoiceStoreItem").Include("StoreItem")
                   group i by i.InvoiceStoreItem
                              into grp
                   select new DistributeStoreItemVM
                   {
                       // InvoiceID = grp.Key,
                       Count = grp.Count(),

                       CreationDate = grp.First().CreationDate,
                       receiverName = grp.First().InvoiceStoreItem.First().Invoice.ReceivedEmployee.Name,
                       location = grp.First().InvoiceStoreItem.First().Invoice.Location.Name,
                       TotalExpended = 0,
                       amount = 0,
                       notes = grp.First().InvoiceStoreItem.First().StoreItem.Note,

                   };
        }
        public IQueryable<PrintTechnicianStoreItemVM> GetTechnicianStoreItemsReportForPrint()
        {

            var technicalDetails = _invoiceStoreItemRepository.GetAll()
                .Where(s => s.IsRefunded != true)
                .Include(w => w.Invoice).ThenInclude(x => x.Department)
                .Include(w => w.StoreItem)
                .ThenInclude(w => w.BaseItem)
                .ThenInclude(w => w.ItemCategory)
                .GroupBy(x => new
                {
                    baseItemId = x.StoreItem.BaseItem.Id,
                    receiverName = x.Invoice.ReceivedEmployee.Name,
                    department = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                    departmentId = x.Invoice.Department != null ? x.Invoice.DepartmentId : 0,
                    invoiceDate = x.Invoice.Date,
                    invoiceCode = x.Invoice.Code,
                    //CreationDate = x.Invoice.CreationDate,
                    categoryID = x.StoreItem.BaseItem.ItemCategory.Id,
                    categoryName = x.StoreItem.BaseItem.ItemCategory.Name,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    storeId = x.StoreItem.TenantId,
                    PageNumber = x.StoreItem.BookPageNumber,
                    BookNumber = x.StoreItem.Book.BookNumber,
                    BudgetId = x.StoreItem.Addition.BudgetId
                })
                .Select(x => new
                {
                    baseItemId = x.Key.baseItemId,
                    receiverName = x.Key.receiverName,
                    department = x.Key.department,
                    invoiceDate = x.Key.invoiceDate,
                    invoiceCode = x.Key.invoiceCode,
                    //CreationDate = x.Key.CreationDate,
                    categoryID = x.Key.categoryID,
                    categoryName = x.Key.categoryName,
                    BaseItemName = x.Key.BaseItemName,
                    storeId = x.Key.storeId,
                    PageNumber = x.Key.PageNumber,
                    BookNumber = x.Key.BookNumber,
                    BudgetId = x.Key.BudgetId
                });




            var counts =
                _storeItemRepository.GetAll()
                .Select(c => new
                {
                    baseItemId = c.BaseItemId,
                    BudgetId = c.Addition.BudgetId,
                    BookNumber = c.Book.BookNumber,
                    PageNumber = c.BookPageNumber,
                    TenantId = c.TenantId,
                    ExaminationCommitte = c.Addition.ExaminationCommitte,
                    statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
                    statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
                    statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
                    statusRobbing = c.StoreItemStatusId == (int)StoreItemStatusEnum.Robbing
                })
                .GroupBy(g => new
                {
                    BaseItemId = g.baseItemId,
                    BudgetId = g.BudgetId,
                    ExaminationCommitte = g.ExaminationCommitte,
                    BookNumber = g.BookNumber,
                    PageNumber = g.PageNumber,
                    TenantId = g.TenantId
                })
                .Select(c => new
                {
                    baseItemId = c.Key.BaseItemId,
                    BudgetId = c.Key.BudgetId,
                    BookNumber = c.Key.BookNumber,
                    PageNumber = c.Key.PageNumber,
                    TenantId = c.Key.TenantId,
                    ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                    countAvailable = c.Count(i => i.statusAvailable),
                    countReserved = c.Count(i => i.statusReserved),
                    countExpensed = c.Count(i => i.statusExpensed),
                    countRobbing = c.Count(i => i.statusRobbing),
                    countTotal = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved) + c.Count(i => i.statusExpensed),
                    countStore = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved)

                });
            return from t in technicalDetails
                   join c in counts on t.baseItemId equals c.baseItemId
                   where t.BookNumber == c.BookNumber && t.PageNumber == c.PageNumber && t.storeId == c.TenantId && t.BudgetId == c.BudgetId
                   select new PrintTechnicianStoreItemVM
                   {
                       Id = t.baseItemId,
                       TotalQuantity = c.countTotal,
                       storeQuantity = c.countStore,
                       expensedQuantity = c.countExpensed,
                       ContractNumber = c.ContractNumber,
                       reservedQuantity = c.countReserved,
                       robbingQuantity = c.countRobbing,
                       receiverName = t.receiverName,
                       department = t.department,
                       invoiceDate = t.invoiceDate,
                       PageNumber = c.PageNumber,
                       invoiceCode = t.invoiceCode,
                       BaseItemID = t.baseItemId,
                       CategoryID = t.categoryID,
                       itemCategory = t.categoryName,
                       BaseItemName = t.BaseItemName,
                       BudgetId = c.BudgetId,
                       TenantId = t.storeId,
                   };

        }


        public IQueryable<TechnicianStoreItemVM> GetTechnicianStoreItemsReport()
        {
            return _storeItemRepository.GetAll()
             .Include(x => x.Addition)
             .Include(x => x.BaseItem)
             .ThenInclude(x => x.ItemCategory)
             .Select(c => new
             {
                 BaseItemId = c.BaseItemId,
                 StatusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
                 StatusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
                 StatusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
                 StatusRobbing = c.StoreItemStatusId == (int)StoreItemStatusEnum.Robbing,
                 BudgetId = c.Addition.BudgetId,
                 BookNumber = c.Book.BookNumber,
                 PageNumber = c.BookPageNumber,
                 ExaminationCommitte = c.Addition.ExaminationCommitte,
                 TenantId = c.TenantId,
                 baseItemName = c.BaseItem.Name,
                 categoryName = c.BaseItem.ItemCategory.Name,
                 categoryId = c.BaseItem.ItemCategoryId,
             })
             .GroupBy(c => new
             {
                 BaseItemId = c.BaseItemId,
                 BaseItemName = c.baseItemName,
                 CategoryId = c.categoryId,
                 CategoryName = c.categoryName,
                 ExaminationCommitte = c.ExaminationCommitte,
                 BudgetId = c.BudgetId,
                 BookNumber = c.BookNumber,
                 PageNumber = c.PageNumber,
                 TenantId = c.TenantId
             })
             .Select(c => new TechnicianStoreItemVM()
             {
                 Id = c.Key.BaseItemId,
                 // countAvailable = c.Count(i => i.statusAvailable),
                 TotalQuantity = c.Count(i => i.StatusAvailable) + c.Count(i => i.StatusReserved) + c.Count(i => i.StatusExpensed),
                 storeQuantity = c.Count(i => i.StatusAvailable) + c.Count(i => i.StatusReserved),
                 expensedQuantity = c.Count(i => i.StatusExpensed),
                 reservedQuantity = c.Count(i => i.StatusReserved),
                 robbingQuantity = c.Count(i => i.StatusRobbing),
                 TenantId = c.Key.TenantId,
                 BaseItemName = c.Key.BaseItemName,
                 ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                 BaseItemID = c.Key.BaseItemId,
                 CategoryID = c.Key.CategoryId,
                 itemCategory = c.Key.CategoryName,
                 BudgetId = c.Key.BudgetId,
                 PageNumber = c.Key.PageNumber,
                 BookNumber = c.Key.BookNumber

             });
            #region OldCode
            //var details =
            //  _storeItemRepository.GetAll()
            //  .Include(x => x.Addition)
            //  .Include(x => x.BaseItem)
            //  .ThenInclude(x => x.ItemCategory)
            //  .Select(c => new
            //  {
            //      baseItemId = c.BaseItemId,
            //      statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
            //      statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
            //      statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
            //      statusRobbing = c.StoreItemStatusId == (int)StoreItemStatusEnum.Robbing,
            //      //creationDate = c.CreationDate,
            //      budgetId = c.Addition.BudgetId,
            //      storeId = c.StoreId,
            //      baseItemName = c.BaseItem.Name,
            //      categoryName = c.BaseItem.ItemCategory.Name,
            //      categoryId = c.BaseItem.ItemCategoryId,
            //  })
            //  .GroupBy(c => new
            //  {
            //      baseItemId = c.baseItemId,
            //      storeId = c.storeId,
            //      baseItemName = c.baseItemName,
            //      categoryId = c.categoryId,
            //      categoryName = c.categoryName,
            //      budgetId = c.budgetId
            //  })
            //  .Select(c => new
            //  {
            //      Id = c.Key.baseItemId,
            //      // countAvailable = c.Count(i => i.statusAvailable),
            //      TotalQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved) + c.Count(i => i.statusExpensed),
            //      storeQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved),
            //      expensedQuantity = c.Count(i => i.statusExpensed),
            //      reservedQuantity = c.Count(i => i.statusReserved),
            //      robbingQuantity = c.Count(i => i.statusRobbing),
            //      //  CreationDate = c.First().creationDate,
            //      TenantId = c.Key.storeId,
            //      BaseItemName = c.Key.baseItemName,
            //      BaseItemID = c.Key.baseItemId,
            //      CategoryID = c.Key.categoryId,
            //      itemCategory = c.Key.categoryName,
            //      BudgetId = c.Key.budgetId

            //  });
            //var counts = _storeItemRepository.GetAll()

            //  .Select(c => new
            //  {
            //      baseItemId = c.BaseItemId,
            //      statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
            //      statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
            //      statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,
            //      statusRobbing = c.StoreItemStatusId == (int)StoreItemStatusEnum.Robbing,

            //  }).GroupBy(c => new
            //  {
            //      baseItemId = c.baseItemId,

            //  }).Select(c => new
            //  {
            //      baseItemId = c.Key.baseItemId,
            //      statusAvailable = c.Count(i => i.statusAvailable),
            //      statusReserved = c.Count(i => i.statusReserved),
            //      statusExpensed = c.Count(i => i.statusExpensed),
            //      statusRobbing = c.Count(i => i.statusRobbing),
            //      totalQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved) + c.Count(i => i.statusExpensed)

            //  });

            //return from d in details
            //       join c in counts on d.BaseItemID equals c.baseItemId

            //       select new TechnicianStoreItemVM
            //       {
            //           Id = d.BaseItemID,
            //           // countAvailable = c.Count(i => i.statusAvailable),
            //           expensedQuantity = c.statusExpensed,
            //           storeQuantity = c.statusAvailable + c.statusReserved,
            //           reservedQuantity = c.statusReserved,
            //           robbingQuantity = c.statusRobbing,
            //           TotalQuantity = c.totalQuantity,
            //           //   CreationDate= ,
            //           TenantId = d.TenantId,
            //           BaseItemName = d.BaseItemName,
            //           BaseItemID = d.BaseItemID,
            //           CategoryID = d.CategoryID,
            //           itemCategory = d.itemCategory,
            //           BudgetId = d.BudgetId
            //       };
            #endregion

        }
        public IQueryable<TechnicianStoreItemDetails> GetTechnicianStoreItemsDetailsReport()
        {
            return _invoiceStoreItemRepository.GetAll()
                .Where(s => s.IsRefunded != true)
                .Include(w => w.Invoice)
                .ThenInclude(x => x.Department)
                .Include(w => w.Invoice)
                .ThenInclude(x => x.ReceivedEmployee)
                .Include(x => x.StoreItem)
               .GroupBy(x => new
               {
                   BaseItemId = x.StoreItem.BaseItemId,
                   BudgetId = x.StoreItem.Addition.BudgetId,
                   BookNumber = x.StoreItem.Book.BookNumber,
                   PageNumber = x.StoreItem.BookPageNumber,
                   TenantId = x.TenantId,
                   receiverName = x.Invoice.ReceivedEmployee.Name,
                   departmentName = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                   departmentId = x.Invoice.Department != null ? x.Invoice.DepartmentId : 0,
                   invoiceDate = x.Invoice.Date,
                   invoiceCode = x.Invoice.Code
               })
               .Select(x => new TechnicianStoreItemDetails()
               {
                   Id = x.Key.BaseItemId,
                   receiverName = x.Key.receiverName,
                   department = x.Key.departmentName,
                   departmentId = x.Key.departmentId,
                   invoiceDate = x.Key.invoiceDate,
                   invoiceCode = x.Key.invoiceCode,
                   BookNumber = x.Key.BookNumber,
                   PageNumber = x.Key.PageNumber,
                   BudgetId = x.Key.BudgetId,
                   Count=x.Count()
                   
               });
        }
        public IQueryable<StoreItemsDistributionVM> GetStoreItemsDistributionReport()
        {

            return _storeItemRepository.GetAll()
                 .Include(x => x.BaseItem)
                 .Include(x => x.Book).Include(x => x.Addition)
                 .Include(x => x.InvoiceStoreItem)
                 .ThenInclude(x => x.Invoice)
                 .ThenInclude(x => x.ReceivedEmployee)
                 .ThenInclude(x => x.Department)

                 .Select(c => new
                 {
                     baseItemId = c.BaseItemId,
                     baseItemName = c.BaseItem.Name,
                     pageNumber = c.BookPageNumber,
                     bookNumber = c.Book.BookNumber,
                     storeID = c.StoreId,
                     categoryId = c.BaseItem.ItemCategoryId,
                     ExaminationCommitte = c.Addition.ExaminationCommitte,
                     budgetId = c.Addition.BudgetId,
                     creationDate = c.CreationDate,
                     statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
                     statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
                     statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,

                 })
                 .GroupBy(c => new
                 {
                     baseItemId = c.baseItemId,
                     baseItemName = c.baseItemName,
                     pageNumber = c.pageNumber,
                     bookNumber = c.bookNumber,
                     ExaminationCommitte = c.ExaminationCommitte,
                     storeID = c.storeID,
                     budgetId = c.budgetId,
                     categoryId = c.categoryId
                 })
                 .Select(c => new StoreItemsDistributionVM
                 {
                     Id = c.Key.baseItemId,
                     BaseItemName = c.Key.baseItemName,
                     PageNumber = c.Key.pageNumber,
                     BookNumber = c.Key.bookNumber,
                     ContractNumber = c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                     storeQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved),
                     paidQuantity = c.Count(i => i.statusExpensed),
                     AllQuantity = c.Count(i => i.statusReserved) + c.Count(i => i.statusAvailable) + c.Count(i => i.statusExpensed),
                     TenantId = c.Key.storeID,
                     BudgetId = c.Key.budgetId,
                     CategoryId = c.Key.categoryId


                 });
            #region OldCode
            //var yy = _invoiceStoreItemRepository.GetAll()
            //     .Include(x => x.StoreItem).ThenInclude(x => x.BaseItem)
            //     .Include(x => x.StoreItem).ThenInclude(x => x.Book)


            //     .Include(x => x.Invoice)
            //     .ThenInclude(x => x.ReceivedEmployee)
            //     .ThenInclude(x => x.Department)
            //     .Select(c => new
            //     {
            //         baseItemId = c.StoreItem.BaseItemId,
            //         baseItemName = c.StoreItem.BaseItem.Name,
            //         pageNumber = c.StoreItem.BookPageNumber,
            //         bookNumber = c.StoreItem.Book.BookNumber,
            //         storeID = c.StoreItem.StoreId,
            //         //  DepartmentId = c.Invoice.ReceivedEmployee.Department != null ?
            //         //c.Invoice.ReceivedEmployee.Department.Id : 0,
            //         //  DepartmentName = c.Invoice.ReceivedEmployee.Department != null ?
            //         //c.Invoice.ReceivedEmployee.Department.Name : "",
            //         creationDate = c.CreationDate,
            //         statusAvailable = c.StoreItem.CurrentItemStatusId == (int)ItemStatusEnum.Available,
            //         statusReserved = c.StoreItem.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
            //         statusExpensed = c.StoreItem.CurrentItemStatusId == (int)ItemStatusEnum.Expenses,

            //     })
            //     .GroupBy(c => c.baseItemId).ToList();
            //     yy.Select(c => new StoreItemsDistributionVM
            //     {
            //         Id = c.Key,
            //         BaseItemName = c.First().baseItemName,
            //         PageNumber = c.First().pageNumber,
            //         BookNumber = c.First().bookNumber,
            //         storeQuantity = c.Count(i => i.statusAvailable),
            //         paidQuantity = c.Count(i => i.statusExpensed),
            //         AllQuantity = c.Count(i => i.statusReserved),

            //         TenantId = c.First().storeID,
            //         //  DepartmentId = c.First().DepartmentId,
            //         //  DepartmentName = c.First().DepartmentName,
            //         CreationDate = c.First().creationDate


            //     }).ToList();
            #endregion
        }

        public IQueryable<DistributionDetailsVM> GetDistributionDetailsReport()
        {
            return _invoiceStoreItemRepository.GetAll()
              .Include(x => x.StoreItem)
              .ThenInclude(x => x.BaseItem)
              .Include(x => x.Invoice)
              .ThenInclude(x => x.Department)
              //.Include(x => x.Invoice)
              //.ThenInclude(x => x.ExchangeOrder)
              .Where(x => x.IsRefunded != true)
              .GroupBy(x => new
              {
                  BaseItemId = x.StoreItem.BaseItemId,
                  BaseItemName = x.StoreItem.BaseItem.Name,
                  PageNumber = x.StoreItem.BookPageNumber,
                  BookNumber = x.StoreItem.Book.BookNumber,
                  BudgetId = x.StoreItem.Addition.BudgetId,
                  DepartmentId = x.Invoice.Department != null ? x.Invoice.DepartmentId : 0,
                  DepartmentName = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                  // Notes=x.Invoice.ExchangeOrder.Notes
                  // Notes = x.StoreItem.Note
              })
              .Select(c => new DistributionDetailsVM
              {
                  Id = c.Key.BaseItemId,
                  BaseItemName = c.Key.BaseItemName,
                  DepartmentId = c.Key.DepartmentId,
                  DepartmentName = c.Key.DepartmentName,
                  BookNumber = c.Key.BookNumber,
                  PageNumber = c.Key.PageNumber,
                  BudgetId = c.Key.BudgetId,
                  //   Notes = c.Key.Notes,
                  Amount = c.Count()
              });
        }


        public IQueryable<StoreItemsDistributionPrintVM> GetStoreItemsDistributionForPrintReport()
        {
            var storeItems = _storeItemRepository.GetAll()
                .Include(x => x.BaseItem)
                .Include(x => x.Book)
                .Select(c => new
                {
                    baseItemId = c.BaseItem.Id,
                    baseItemName = c.BaseItem.Name,
                    pageNumber = c.BookPageNumber,
                    bookNumber = c.Book.BookNumber,
                    storeID = c.TenantId,
                    creationDate = c.CreationDate,
                    ExaminationCommitte = c.Addition.ExaminationCommitte,
                    BudgetId = c.Addition.BudgetId,
                    ItemCategoryId = c.BaseItem.ItemCategoryId,
                    statusAvailable = c.CurrentItemStatusId == (int)ItemStatusEnum.Available,
                    statusReserved = c.CurrentItemStatusId == (int)ItemStatusEnum.Reserved,
                    statusExpensed = c.CurrentItemStatusId == (int)ItemStatusEnum.Expenses
                })
                .GroupBy(c => new
                {
                    baseItemId = c.baseItemId,
                    BaseItemName = c.baseItemName,
                    PageNumber = c.pageNumber,
                    BookNumber = c.bookNumber,
                    ExaminationCommitte = c.ExaminationCommitte,
                    TenantId = c.storeID,
                    BudgetId = c.BudgetId,
                    CategoryId = c.ItemCategoryId

                })
                .Select(c => new
                {
                    baseItemId = c.Key.baseItemId,
                    storeQuantity = c.Count(i => i.statusAvailable) + c.Count(i => i.statusReserved),
                    paidQuantity = c.Count(i => i.statusExpensed),
                    AllQuantity = c.Count(i => i.statusReserved) + c.Count(i => i.statusAvailable) + c.Count(i => i.statusExpensed),
                    BaseItemName = c.Key.BaseItemName,
                    ContractNumber =c.Key.ExaminationCommitte != null ? (c.Key.ExaminationCommitte.ContractNumber != null ? c.Key.ExaminationCommitte.ContractNumber : "") : "",
                    PageNumber = c.Key.PageNumber,
                    BookNumber = c.Key.BookNumber,
                    TenantId = c.Key.TenantId,
                    BudgetId = c.Key.BudgetId,
                    CategoryId = c.Key.CategoryId
                });

            var invoiceStoreItems = _invoiceStoreItemRepository.GetAll()
                .Include(x => x.StoreItem)
                .ThenInclude(x => x.BaseItem)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Department)
                .Where(x => x.IsRefunded != true)
                .GroupBy(x => new
                {
                    baseItemId = x.StoreItem.BaseItem.Id,
                    BaseItemName = x.StoreItem.BaseItem.Name,
                    PageNumber = x.StoreItem.BookPageNumber,
                    BookNumber = x.StoreItem.Book.BookNumber,
                    TenantId = x.StoreItem.TenantId,
                    BudgetId = x.StoreItem.Addition.BudgetId,
                    DepartmentId = x.Invoice.Department != null ? x.Invoice.Department.Id : 0,
                    DepartmentName = x.Invoice.Department != null ? x.Invoice.Department.Name : "",
                    Notes = x.StoreItem.Note
                })
                .Select(c => new
                {
                    baseItemId = c.Key.baseItemId,
                    BaseItemName = c.Key.BaseItemName,
                    DepartmentId = c.Key.DepartmentId,
                    DepartmentName = c.Key.DepartmentName,
                    BookNumber = c.Key.BookNumber,
                    PageNumber = c.Key.PageNumber,
                    TenantId = c.Key.TenantId,
                    BudgetId = c.Key.BudgetId,
                    Amount = c.Count(),
                    Notes = c.Key.Notes
                });

            return from i in invoiceStoreItems
                   join s in storeItems on i.baseItemId equals s.baseItemId
                   where i.BookNumber == s.BookNumber && i.PageNumber == s.PageNumber && i.TenantId == s.TenantId && i.BudgetId == s.BudgetId
                   select new StoreItemsDistributionPrintVM
                   {
                       Id = s.baseItemId,
                       BaseItemName = s.BaseItemName,
                       BookNumber = s.BookNumber,
                       PageNumber = s.PageNumber,
                       //CreationDate = s.CreationDate,
                       TenantId = s.TenantId,
                       AllQuantity = s.AllQuantity,
                       paidQuantity = s.paidQuantity,
                       storeQuantity = s.storeQuantity,
                       DepartmentId = i.DepartmentId,
                       ContractNumber = s.ContractNumber,
                       DepartmentName = i.DepartmentName,
                       Amount = i.Amount,
                       Notes = i.Notes,
                       BudgetId = s.BudgetId,
                       CategoryId = s.CategoryId
                   };
        }

    }
}
