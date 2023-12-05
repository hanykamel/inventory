using Inventory.Data.Entities;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.ReportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Interfaces
{
    public interface IReportBusiness
    {
        
        IQueryable<Invoice> GetInvoiceReport();
        IQueryable<InvoiceStoreItem> GetInvoiceStoreItemsReport();
        IQueryable<InvoiceStoreItemsReportVM> PrintInvoiceStoreItemsReport();
        IQueryable<DailyStoreItemVM> GetDailyStoreItemsReport();
        //IQueryable<DistributeStoreItemVM> GetDistributedStoreItemsReport();

        IQueryable<ExistingStoreItemVM> GetExistingStoreItemsReport();
        IQueryable<DepartmentStoreItemVM> GetDepartmentStoreItemsReport();

        IQueryable<DepartmentDetailsVM> GetDepartmentDetailsReport();

        IQueryable<DepartmentStoreItemPrintVM> GetDepartmentStoreItemsForPrintReport();

        IQueryable<StoreItemsDistributionVM> GetStoreItemsDistributionReport();
        IQueryable<DistributionDetailsVM> GetDistributionDetailsReport();
        IQueryable<StoreItemsDistributionPrintVM> GetStoreItemsDistributionForPrintReport();



        IQueryable<PrintTechnicianStoreItemVM> GetTechnicianStoreItemsReportForPrint();
        IQueryable<TechnicianStoreItemVM> GetTechnicianStoreItemsReport();
        IQueryable<TechnicianStoreItemDetails> GetTechnicianStoreItemsDetailsReport();

        IQueryable<StoreBookVM> GetStoreBook();
        //IQueryable<StoreItemIndexReportVM> PrintStoreBook();

        IQueryable<InvoiceStoreItem> GetInvoiceStoreItemReport();

        IQueryable<DailyStoreItemsVM> GetAllStoreItemsDailyReport();
        IQueryable<InvoiceStoreItem> PrintCustodyPersonReport();
        decimal GetLastYearBalance();
        IQueryable<CustodyPersonVM> GetCustodyPersonReport();
        IQueryable<CustodyPersonVM> GetCustodyPersonReportForPrint();
        List<LastYearBalanceVM> GetLastYearBalanceGroupedByCurrency();
    }
}
