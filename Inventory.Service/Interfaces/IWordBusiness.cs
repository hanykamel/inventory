using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Delegation;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.ReportVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.ReportRequest.Commands;
using Inventory.Service.Entities.TransformationRequest.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Interfaces
{
    public interface IWordBusiness
    {
        MemoryStream printAdditionMultiDocument(TransactionsVM addition, List<BaseItemStoreItemVM> baseItemStoreItemVMs);
        //MemoryStream printHandoverDocument(Form6VM stockTaking, List<StocktakingPrintObjectVM> baseItemStoreItemVMs);
        MemoryStream printStocktakingMultiDocument(Form6VM stockTaking, List<FormNo6StoreItemVM> baseItemStoreItemVMs,string TemplateName);
        //MemoryStream printStoktakingDocument(Form6VM stockTaking, List<FormNo6StoreItemVM> baseItemStoreItemVMs);
        MemoryStream printExaminationDocument(ExaminationCommitte examinationCommittes);
        MemoryStream printAdditionDocument(TransactionsVM from2Data, List<BaseItemStoreItemVM> baseItemStoreItemVMs);
        MemoryStream printExchangeDocument(TransactionsVM exhange, List<BaseItemStoreItemVM> baseItemStoreItemVMs);
        MemoryStream printTransfromationDocument(TransactionsVM exhange, List<BaseItemStoreItemVM> baseItemStoreItemVMs);
        MemoryStream printInvoiceDocument(List<Invoice> invoices, List<InvoiceStoreItemVM> invoiceStoreItemsVM, List<InvoiceStoreItem> invoiceStoreItems,bool CountAllResults);
        //MemoryStream printDeductionDocument(Form8VM deduction, List<DedcuctionStoreItemVM> baseItemStoreItemVMs);
        MemoryStream printRefundFormNo9Document(Form8VM deduction, List<DedcuctionStoreItemVM> baseItemStoreItemVMs);
        MemoryStream PrintStagnantDocument(StagnantModelVM StagnantModel);
        MemoryStream PrintInvoiceReport(List<InvoiceReportVM> invoices);
        MemoryStream PrintInvoiceStoreItemReport(List<InvoiceStoreItemsReportVM> invoices, PrintDocumentTypesEnum doc);
        MemoryStream PrintStoreItemIndexReport(List<StoreBookVM> storeItems, dynamic Filters);
        MemoryStream PrintExistingStoreItemsReport(List<ExistingStoreItemVM> storeItems, PrintDocumentTypesEnum doc, dynamic Filters);
        MemoryStream PrintTechnicianStoreItemsReport(List<PrintTechnicianStoreItemVM> technicianStoreItems, PrintDocumentTypesEnum doc, dynamic filters);
        MemoryStream PrintCustodyPersonReport(List<CustodyPersonVM> storeItems, PrintDocumentTypesEnum doc, dynamic Filters);
        MemoryStream PrintDepartmentStoreItemsReport(List<DepartmentStoreItemPrintVM> storeItems, PrintDocumentTypesEnum doc, dynamic filters);
        MemoryStream PrintDailyStoreItemsReport(List<DailyStoreItemsVM> storeItems, List<LastYearBalanceVM> lastYearBalaceList, String budget, PrintDocumentTypesEnum doc);
        MemoryStream PrintStoreItemsDistributionReport(List<StoreItemsDistributionPrintVM> storeItems, PrintDocumentTypesEnum doc, dynamic filters);
        MemoryStream PrintExaminationCommitteList(List<ExaminationCommitte> examinations, dynamic filters, PrintDocumentTypesEnum doc);
        //MemoryStream PrintExaminationCommitteListTest(List<ExaminationCommitte> examinations, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintAdditionList(List<AdditionListVM> addition, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintRefundOrdersList(List<RefundOrderVM> refundOrders, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintExchangeOrdersList(List<ExchangeOrder> exchangeOrders, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintExecutionOrdersList(List<ExecutionListVM> executionOrders, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintInvoicesList(List<Invoice> invoices, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintTransformationsList(List<Transformation> transformations, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintRobbingOrdersList(List<RobbingOrder> robbingOrders, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintStockTakingList(List<StockTaking> stockTaking, dynamic filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintDelegationsList(List<Delegation> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintDelegationsTrackList(List<DelegationTrack> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintDedcutionsList(List<Deduction> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);

        #region Lookups
        MemoryStream PrintDepartmentsList(List<Department> department, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintLocationsList(List<Location> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintBaseItemsList(List<BaseItem> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintTechnicalDepartmentsList(List<TechnicalDepartment> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintExternalEntitysList(List<ExternalEntity> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintBooksList(List<Book> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintStoresList(List<Store> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintSuppliersList(List<Supplier> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintEmployeessList(List<Employees> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintUnitsList(List<Unit> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintItemCategorysList(List<ItemCategory> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintJobTitlesList(List<JobTitle> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintRemainsList(List<Remains> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        MemoryStream PrintRemainsInquiriesList(List<RemainsDetails> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc);
        #endregion
    }
}
