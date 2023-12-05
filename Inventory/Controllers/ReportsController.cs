using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using Microsoft.AspNet.OData;
using Inventory.Data.Enums;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Data.Models.ReportVM;
using Microsoft.AspNet.OData.Query;
using System.Reflection;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Web.Attributes;
using Inventory.Data.Models.Delegation;

namespace Inventory.Web.Controllers
{

    [Route("api/[controller]")]
    // [ODataRoutePrefix("Reports")]
    public class ReportsController : Controller
    {
        private readonly IReportBusiness _reportBusiness;
        private readonly IDepartmentBussiness _departmentBussiness;
        private readonly IBaseItemBussiness _baseItemBussiness;
        private readonly ILocationBussiness _locationBussiness;
        private readonly ITechnicalDepartmentBussiness _technicalDepartmentBussiness;
        private readonly ISupplierBussiness _supplierBussiness;
        private readonly IUnitBussiness _unitBussiness;
        private readonly IItemCategoryBussiness _itemCategoryBusiness;
        private readonly IJobTitleBussiness _jobTitleBussiness;
        private readonly IExternalEntityBussiness _externalEntityBussiness;
        private readonly IExaminationBusiness _examinationBussiness;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IBookBussiness _booksBussiness;
        private readonly IAdditionBussiness _additionBussiness;
        private readonly IEmployeeBussiness _employeeBussiness;
        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IExecutionOrderBussiness _executionOrderBussiness;
        private readonly IInvoiceBussiness _invoiceBussiness;
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IStockTakingBussiness _stockTakingBussiness;
        private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IRemainsBussiness _remainsBussiness;
        private readonly IDelegationBussiness _delegationBusiness;
        private readonly IDeductionBusiness _deductionBusiness;
        
        public ReportsController(IReportBusiness reportBusiness,
            IDepartmentBussiness departmentBussiness,
            IBaseItemBussiness baseItemBussiness,
            ILocationBussiness locationBussiness,
            ITechnicalDepartmentBussiness technicalDepartmentBussiness,
            ISupplierBussiness supplierBussiness,
            IUnitBussiness unitBussiness,
            IItemCategoryBussiness itemCategoryBusiness,
            IJobTitleBussiness jobTitleBussiness,
            IExternalEntityBussiness externalEntityBussiness,
            IExaminationBusiness examinationBussiness,
            IStoreBussiness storeBussiness,
            IBookBussiness booksBussiness,
            IAdditionBussiness additionBussiness,
            IEmployeeBussiness employeeBussiness,
            IRefundOrderBussiness refundOrderBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IExecutionOrderBussiness executionOrderBussiness,
            IInvoiceBussiness invoiceBussiness,
            ITransformationRequestBussiness TransformationRequestBussiness,
            IRobbingOrderBussiness robbingOrderBussiness,
            IStockTakingBussiness stockTakingBussiness,
            IStringLocalizer<SharedResource> Localizer,
            IRemainsBussiness remainsBussiness,
            IDelegationBussiness delegationBusiness,
            IDeductionBusiness deductionBusiness
            )
        {
            _departmentBussiness = departmentBussiness;
            _reportBusiness = reportBusiness;
            _baseItemBussiness = baseItemBussiness;
            _locationBussiness = locationBussiness;
            _technicalDepartmentBussiness = technicalDepartmentBussiness;
            _supplierBussiness = supplierBussiness;
            _unitBussiness = unitBussiness;
            _itemCategoryBusiness = itemCategoryBusiness;
            _jobTitleBussiness = jobTitleBussiness;
            _externalEntityBussiness = externalEntityBussiness;
            _examinationBussiness = examinationBussiness;
            _storeBussiness = storeBussiness;
            _booksBussiness = booksBussiness;
            _additionBussiness = additionBussiness;
            _employeeBussiness = employeeBussiness;
            _refundOrderBussiness = refundOrderBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _executionOrderBussiness = executionOrderBussiness;
            _invoiceBussiness = invoiceBussiness;
            _transformationRequestBussiness = TransformationRequestBussiness;
            _robbingOrderBussiness = robbingOrderBussiness;
            _stockTakingBussiness = stockTakingBussiness;
            _Localizer = Localizer;
            _remainsBussiness = remainsBussiness;
            _delegationBusiness = delegationBusiness;
            _deductionBusiness = deductionBusiness;
        }
        #region InvoiceReport
        [EnableQuery]
        [HttpGet]
        [Route("GetInvoiceForReport")]
        public IQueryable<Invoice> GetInvoiceForReport() => _reportBusiness.GetInvoiceReport();

        [EnableQuery]
        [HttpGet]
        [Route("PrintInvoiceReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.InvoiceReport })]
        public IQueryable<Invoice> PrintInvoiceReport() => _reportBusiness.GetInvoiceReport();
        #endregion

        #region InvoiceStoreItemsReports
        [EnableQuery]
        [HttpGet]
        [Route("GetInvoiceStoreItemsForReport")]
        public IQueryable<InvoiceStoreItem> GetInvoiceStoreItemsForReport() => _reportBusiness.GetInvoiceStoreItemsReport();

        [EnableQuery]
        [HttpGet]
        [Route("PrintInvoiceStoreItemsReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.InvoiceStoreItemsReport })]
        public IQueryable<InvoiceStoreItemsReportVM> PrintInvoiceStoreItemsReport() => _reportBusiness.PrintInvoiceStoreItemsReport();
        #endregion


        #region StoreItemIndexReport

        [EnableQuery(MaxExpansionDepth = 4, MaxAnyAllExpressionDepth = 2)]
        [HttpGet]
        [Route("GetStoreBook")]
        public IQueryable<StoreBookVM> GetStoreBook() => _reportBusiness.GetStoreBook();


        [EnableQuery]
        [HttpGet]
        [Route("PrintStoreBook")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.StoreItemIndexReport })]
        public IQueryable<StoreBookVM> PrintStoreBook(String paramters) => _reportBusiness.GetStoreBook();
        #endregion

        #region ExistingStoreItemsReport
        [EnableQuery]
        [HttpGet]
        [Route("GetExistingStoreItemsForReport")]
        public IQueryable<ExistingStoreItemVM> GetExistingStoreItemsForReport() => _reportBusiness.GetExistingStoreItemsReport();

        [EnableQuery]
        [HttpGet]
        [Route("PrintExistingStoreItemsReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExistingStoreItemsReport })]
        public IQueryable<ExistingStoreItemVM> PrintExistingStoreItemsReport(String paramters) => _reportBusiness.GetExistingStoreItemsReport();

        #endregion

        #region StoreItemsDistributionReport
        [EnableQuery]
        [HttpGet]
        [Route("GetStoreItemsDistributionForReport")]
        public IQueryable<StoreItemsDistributionVM> GetStoreItemsDistributionForReport() => _reportBusiness.GetStoreItemsDistributionReport();


        [EnableQuery]
        [HttpGet]
        [Route("GetDistributionDetailsForReport")]
        public IQueryable<DistributionDetailsVM> GetDistributionDetailsForReport() => _reportBusiness.GetDistributionDetailsReport();


        [EnableQuery]
        [HttpGet]
        [Route("PrintStoreItemsDistributionReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.StoreItemsDistributionReport })]
        public IQueryable<StoreItemsDistributionPrintVM> PrintStoreItemsDistributionReport(String paramters) => _reportBusiness.GetStoreItemsDistributionForPrintReport();

        #endregion

        #region TechnicianStoreItemsReport

        [EnableQuery]
        [HttpGet]
        [Route("GetTechnicianStoreItemsForReport")]
        public IQueryable<TechnicianStoreItemVM> GetTechnicianStoreItemsForReport() => _reportBusiness.GetTechnicianStoreItemsReport();

        [EnableQuery]
        [HttpGet]
        [Route("GetTechnicianStoreItemsDetailsForReport")]
        public IQueryable<TechnicianStoreItemDetails> GetTechnicianStoreItemsDetailsForReport() => _reportBusiness.GetTechnicianStoreItemsDetailsReport();



        [EnableQuery]
        [HttpGet]
        [Route("PrintTechnicianStoreItemsReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.TechnicianStoreItemsReport })]
        public IQueryable<PrintTechnicianStoreItemVM> PrintTechnicianStoreItemsReport(String paramters) => _reportBusiness.GetTechnicianStoreItemsReportForPrint();
        #endregion


        #region CustodyPersonReport
        [EnableQuery]
        [HttpGet]
        [Route("GetCustodyPersonVMReport")]
        public IQueryable<CustodyPersonVM> GetCustodyPersonVMReport() => _reportBusiness.GetCustodyPersonReport();

        [EnableQuery(MaxExpansionDepth = 6)]
        [HttpGet]
        [Route("PrintCustodyPersonReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.CustodyPersonReport })]
        public IQueryable<CustodyPersonVM> PrintCustodyPersonReport(String paramters)
        {
            if (!String.IsNullOrEmpty(paramters) && paramters.Contains("Reciever"))
                return _reportBusiness.GetCustodyPersonReportForPrint();
            //_reportBusiness.PrintCustodyPersonReport();
            throw new InvalidModelStateException(_Localizer["MustSelectReciever"]);
        }

        #endregion

        #region DepartmentStoreItemsReport
        [EnableQuery]
        [HttpGet]
        [Route("GetDepartmentStoreItemsForReport")]
        public IQueryable<DepartmentStoreItemVM> GetDepartmentStoreItemsForReport() => _reportBusiness.GetDepartmentStoreItemsReport();

        [EnableQuery]
        [HttpGet]
        [Route("GetDepartmentDetailsForReport")]
        public IQueryable<DepartmentDetailsVM> GetDepartmentDetailsForReport() => _reportBusiness.GetDepartmentDetailsReport();

        [EnableQuery]
        [HttpGet]
        [Route("PrintDepartmentStoreItemsReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DepartmentStoreItemsReport })]
        public IQueryable<DepartmentStoreItemPrintVM> PrintDepartmentStoreItemsReport(String paramters)
        {
            if (!String.IsNullOrEmpty(paramters) && paramters.Contains("BaseItemId"))
                return _reportBusiness.GetDepartmentStoreItemsForPrintReport();
            throw new InvalidModelStateException(_Localizer["MustSelectBaseItemId"]);
        }

        #endregion

        #region DailyStoreItemsReport
        [EnableQuery]
        [HttpGet]
        [Route("GetDailyStoreItemsForReport")]
        public IQueryable<DailyStoreItemsVM> GetDailyStoreItemsForReport() => _reportBusiness.GetAllStoreItemsDailyReport();

        [EnableQuery]
        [HttpGet]
        [Route("PrintDailyStoreItemsReport")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DailyStoreItemsReport })]
        public IQueryable<DailyStoreItemsVM> PrintDailyStoreItemsReport(String paramters) => _reportBusiness.GetAllStoreItemsDailyReport();
        #endregion

        #region Lookups

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [Route("PrintDepartmentsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DepartmentsList })]
        public IQueryable<Department> PrintDepartmentsList(String paramters) => _departmentBussiness.GetAllDepartment();

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintBaseItemsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.BaseItemsList })]
        public IActionResult PrintBaseItemsList(String paramters) => Ok(_baseItemBussiness.GetAllPrintBaseItem());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintLocationsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.LocationsList })]
        public IActionResult PrintLocationsList(String paramters) => Ok(_locationBussiness.GetAllLocation());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintTechnicalDepartmentsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.TechnicalDepartmentsList })]
        public IActionResult PrintTechnicalDepartmentsList(String paramters) => Ok(_technicalDepartmentBussiness.GetAllTechnicalDepartment());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintExternalEntitiesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExternalEntityList })]
        public IActionResult PrintExternalEntitiesList(String paramters) => Ok(_externalEntityBussiness.GetAllExternalEntity());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintSuppliersList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.SuppliersList })]
        public IActionResult PrintSuppliersList(String paramters) => Ok(_supplierBussiness.GetAllSupplier());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintUnitsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.UnitsList })]
        public IActionResult PrintUnitsList(String paramters) => Ok(_unitBussiness.GetAllUnit());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintItemCategoriesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ItemsCategoriesList })]
        public IActionResult PrintItemCategoriesList(String paramters) => Ok(_itemCategoryBusiness.GetAllItemCategory());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintJobTitlesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.JobsTitlesList })]
        public IActionResult PrintJobTitlesList(String paramters) => Ok(_jobTitleBussiness.GetAllJobTitle());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintStoresList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.StoresList })]
        public IActionResult PrintStoresList(string paramters) => Ok(_storeBussiness.GetAllPrintStore());

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintBooksList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.BooksList })]
        public IActionResult PrintBooksList(string paramters) => Ok(_booksBussiness.GetALLStoreBooks());


        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        [Route("PrintEmployeesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.EmployeesList })]
        public IActionResult PrintEmployeesList(string paramters) => Ok(_employeeBussiness.GetAllPrintEmployees());
        #endregion
        #region Lists
        [EnableQuery]
        [HttpGet]
        [Route("PrintExaminationCommitteListTest")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExaminationList })]
        public IActionResult PrintExaminationCommitteListTest(string paramters) => Ok(_examinationBussiness.PrintExamination());

        [EnableQuery]
        [HttpGet]
        [Route("PrintExaminationCommitteList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExaminationList })]
        public IActionResult PrintExaminationCommitteList(string paramters) => Ok(_examinationBussiness.PrintExamination());

        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("PrintAdditionList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.AdditionList })]
        public IQueryable<AdditionListVM> PrintAdditionList(string paramters) => _additionBussiness.PrintAdditionList();

        [EnableQuery(MaxExpansionDepth = 7, MaxAnyAllExpressionDepth = 3)]
        [HttpGet]
        [Route("PrintRefundOrdersList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.RefundOrdersList })]
        public IQueryable<RefundOrderVM> PrintRefundOrdersList(string paramters)
        {
            return _refundOrderBussiness.PrintRefundOrdersList();
        }
        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("PrintExchangeOrdersList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExchangeOrdersList })]
        public IQueryable<ExchangeOrder> PrintExchangeOrdersList(string paramters) => _exchangeOrderBussiness.PrintExchangeOrders();


        [EnableQuery(MaxExpansionDepth = 3)]
        [HttpGet]
        [Route("PrintInvoicesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.InvoicesList })]
        public IQueryable<Invoice> PrintInvoicesList(string paramters) => _invoiceBussiness.PrintInvoicesList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintTransformationsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.TransformationsList })]
        public IQueryable<Transformation> PrintTransformationsList(string paramters) => _transformationRequestBussiness.PrintTransformationsList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintRobbingOrdersList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.RobbingOrdersList })]
        public IQueryable<RobbingOrder> PrintRobbingOrdersList(string paramters) => _robbingOrderBussiness.PrintRobbingOrdersList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintStockTakingList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.StockTakingList })]
        public IQueryable<StockTaking> PrintStockTakingList(string paramters) => _stockTakingBussiness.GetAllStockTaking();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintDeductionList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DeductionList })]
        public IQueryable<Deduction> PrintDeductionList(string paramters) => _deductionBusiness.PrintDeductionsList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintExecutionOrderList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.ExecutionOrdersList })]
        public IQueryable<ExecutionListVM> PrintExecutionOrderList(string paramters) => _executionOrderBussiness.GetExecutionOrdersList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintRemainsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.RemainsList })]
        public IQueryable<Remains> PrintRemainsList(string paramters) => _remainsBussiness.GetAllRemains();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintRemainsInquiriesList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.RemainsInquiriesList })]
        public IQueryable<RemainsDetails> PrintRemainsInquiriesList(string paramters) => _remainsBussiness.GetAllRemainsDetailsList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintDelegationsList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DelegationsList })]
        public IQueryable<Delegation> PrintDelegationsList(string paramters) => _delegationBusiness.GetAllDelegationList();

        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet]
        [Route("PrintDelegationsTrackList")]
        [TypeFilter(typeof(WordActionFilterAttribute), Arguments = new[] { (object)PrintDocumentTypesEnum.DelegationsTrackList })]
        public IQueryable<DelegationTrack> PrintDelegationsTrackList(string paramters) => _delegationBusiness.GetDelegationTrack();
        

        #endregion
    }



}