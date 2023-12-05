using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Inventory.Data.Models.Inquiry;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Service.Entities.ReportRequest.Commands;
using System.Linq;
using Inventory.Data.Entities;
using System.Collections.Generic;
using Inventory.Data.Models.PrintTemplateVM;
using AutoMapper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System;
using Inventory.Data.Models.ReportVM;
using Newtonsoft.Json.Linq;
using Inventory.Data.Models.Delegation;

namespace Inventory.Service.Entities.ReportRequest.Handlers
{
    public class PrintReportHandler : IRequestHandler<PrintReportCommand, MemoryStream>
    {
        private readonly IReportBusiness _reportBusiness;
        private readonly IWordBusiness _wordBusiness;
        private IStringLocalizer<SharedResource> _localizer;
        private readonly IMapper _mapper;
        public PrintReportHandler(
            IReportBusiness reportBusiness,
            IStringLocalizer<SharedResource> localizer,
            IWordBusiness wordBusiness,
            IMapper mapper
            )
        {
            _wordBusiness = wordBusiness;
            _localizer = localizer;
            _reportBusiness = reportBusiness;
            _mapper = mapper;
        }

        public Task<MemoryStream> Handle(PrintReportCommand request, CancellationToken cancellationToken)
        {
            if (request != null && request.Result != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                switch (request.ReportType)
                {
                    case Data.Enums.PrintDocumentTypesEnum.InvoiceReport:
                        {
                            List<Invoice> list = (request.Result.Value as IQueryable<Invoice>)?.ToList();
                            if (list == null || list.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            List<InvoiceReportVM> invoiceList = new List<InvoiceReportVM>();
                            for (int i = 0; i < list.Count; i++)
                            {
                                InvoiceReportVM obj = _mapper.Map<InvoiceReportVM>(list[i]);
                                invoiceList.Add(obj);
                            }
                            memoryStream = _wordBusiness.PrintInvoiceReport(invoiceList);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.InvoiceStoreItemsReport:
                        {

                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            List<InvoiceStoreItemsReportVM> invoiceStoreItemList = (request.Result.Value as IQueryable<InvoiceStoreItemsReportVM>)?.ToList();
                            if (invoiceStoreItemList == null || invoiceStoreItemList.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintInvoiceStoreItemReport(invoiceStoreItemList, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.StoreItemIndexReport:
                        {

                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            List<StoreBookVM> invoiceStoreItemList = (request.Result.Value as IQueryable<StoreBookVM>)?.ToList();
                            if (invoiceStoreItemList == null || invoiceStoreItemList.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintStoreItemIndexReport(invoiceStoreItemList, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ExistingStoreItemsReport:
                        {

                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            List<ExistingStoreItemVM> existingStoreItems = (request.Result.Value as IQueryable<ExistingStoreItemVM>)?.ToList();
                            if (existingStoreItems == null || existingStoreItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintExistingStoreItemsReport(existingStoreItems, request.ReportType, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.TechnicianStoreItemsReport:
                        {
                            var x = request.Result.Value as IQueryable<PrintTechnicianStoreItemVM>;
                            List<PrintTechnicianStoreItemVM> technicianStoreItems = (request.Result.Value as IQueryable<PrintTechnicianStoreItemVM>)?.ToList();
                            if (technicianStoreItems == null || technicianStoreItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataAsNoExchange"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintTechnicianStoreItemsReport(technicianStoreItems, request.ReportType, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.CustodyPersonReport:
                        {
                            List<CustodyPersonVM> storeItems = (request.Result.Value as IQueryable<CustodyPersonVM>)?.ToList();
                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            if (storeItems == null || storeItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintCustodyPersonReport(storeItems, request.ReportType, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.DepartmentStoreItemsReport:
                        {

                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            List<DepartmentStoreItemPrintVM> storeItems = (request.Result.Value as IQueryable<DepartmentStoreItemPrintVM>)?.ToList();
                            if (storeItems == null || storeItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataAsNoExchange"]);

                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintDepartmentStoreItemsReport(storeItems, request.ReportType, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.DailyStoreItemsReport:
                        {

                            //var invoiceStoreItemList = request.Result.Value as dynamic;
                            List<DailyStoreItemsVM> storeItems = (request.Result.Value as IQueryable<DailyStoreItemsVM>)?.ToList();
                            if (storeItems == null || storeItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            String budget = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var lastYearBalanceList = _reportBusiness.GetLastYearBalanceGroupedByCurrency();
                            //decimal lastYearBalance = _reportBusiness.GetLastYearBalance();
                            memoryStream = _wordBusiness.PrintDailyStoreItemsReport(storeItems, lastYearBalanceList, budget, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.StoreItemsDistributionReport:
                        {
                            List<StoreItemsDistributionPrintVM> storeItems = (request.Result.Value as IQueryable<StoreItemsDistributionPrintVM>)?.ToList();
                            if (storeItems == null || storeItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataAsNoExchange"]);
                            //for (int i = 0; i < storeItems.Count; i++)
                            //{
                            //    storeItems[i].DistributionDetails = _reportBusiness.GetDistributionDetailsReport().Where(c => c.Id == storeItems[i].Id).ToList();
                            //}
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintStoreItemsDistributionReport(storeItems, request.ReportType, filters);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ExaminationList:
                        {
                            List<ExaminationCommitte> examinations = (request.Result.Value as IQueryable<ExaminationCommitte>)?.ToList();
                            if (examinations == null || examinations.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintExaminationCommitteList(examinations, filters, request.ReportType);
                            break;

                        }
                    case Data.Enums.PrintDocumentTypesEnum.AdditionList:
                        {
                            List<AdditionListVM> addition = (request.Result.Value as IQueryable<AdditionListVM>)?.ToList();
                            if (addition == null || addition.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintAdditionList(addition, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.RefundOrdersList:
                        {
                            List<RefundOrderVM> refundOrders = (request.Result.Value as IQueryable<RefundOrderVM>)?.ToList();
                            if (refundOrders == null || refundOrders.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintRefundOrdersList(refundOrders, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ExchangeOrdersList:
                        {
                            List<ExchangeOrder> exchangeOrders = (request.Result.Value as IQueryable<ExchangeOrder>)?.ToList();
                            if (exchangeOrders == null || exchangeOrders.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintExchangeOrdersList(exchangeOrders, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ExecutionOrdersList:
                        {
                            List<ExecutionListVM> executionOrders = (request.Result.Value as IQueryable<ExecutionListVM>)?.ToList();
                            if (executionOrders == null || executionOrders.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintExecutionOrdersList(executionOrders, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.InvoicesList:
                        {
                            List<Invoice> invoices = (request.Result.Value as IQueryable<Invoice>)?.ToList();
                            if (invoices == null || invoices.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintInvoicesList(invoices, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.TransformationsList:
                        {
                            List<Transformation> transformations = (request.Result.Value as IQueryable<Transformation>)?.ToList();
                            if (transformations == null || transformations.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintTransformationsList(transformations, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.RobbingOrdersList:
                        {
                            List<RobbingOrder> robbingOrders = (request.Result.Value as IQueryable<RobbingOrder>)?.ToList();
                            if (robbingOrders == null || robbingOrders.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintRobbingOrdersList(robbingOrders, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.StockTakingList:
                        {
                            List<StockTaking> stockTaking = (request.Result.Value as IQueryable<StockTaking>)?.ToList();
                            if (stockTaking == null || stockTaking.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            dynamic filters = JsonConvert.DeserializeObject(request.Params.Split('=')[1].Replace("'", "").Replace(")", ""));
                            memoryStream = _wordBusiness.PrintStockTakingList(stockTaking, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.DelegationsList:
                        {
                            List<Delegation> lookupItems = (request.Result.Value as IQueryable<Delegation>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintDelegationsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.DelegationsTrackList:
                        {
                            List<DelegationTrack> lookupItems = (request.Result.Value as IQueryable<DelegationTrack>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintDelegationsTrackList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.DeductionList:
                        {
                            List<Deduction> lookupItems = (request.Result.Value as IQueryable<Deduction>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintDedcutionsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    #region Lookups
                    case Data.Enums.PrintDocumentTypesEnum.DepartmentsList:
                        {
                            List<Department> department = (request.Result.Value as IQueryable<Department>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (department == null || department.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintDepartmentsList(department, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.LocationsList:
                        {
                            List<Location> lookupItems = (request.Result.Value as IQueryable<Location>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintLocationsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.BaseItemsList:
                        {
                            List<BaseItem> lookupItems = (request.Result.Value as IQueryable<BaseItem>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintBaseItemsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.TechnicalDepartmentsList:
                        {
                            List<TechnicalDepartment> lookupItems = (request.Result.Value as IQueryable<TechnicalDepartment>)?.ToList();
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            memoryStream = _wordBusiness.PrintTechnicalDepartmentsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ExternalEntityList:
                        {
                            List<ExternalEntity> lookupItems = (request.Result.Value as IQueryable<ExternalEntity>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintExternalEntitysList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.BooksList:
                        {
                            List<Book> lookupItems = (request.Result.Value as IQueryable<Book>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintBooksList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.StoresList:
                        {
                            List<Store> lookupItems = (request.Result.Value as IQueryable<Store>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintStoresList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.SuppliersList:
                        {
                            List<Supplier> lookupItems = (request.Result.Value as IQueryable<Supplier>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintSuppliersList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.EmployeesList:
                        {
                            List<Employees> lookupItems = (request.Result.Value as IQueryable<Employees>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintEmployeessList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.UnitsList:
                        {
                            List<Data.Entities.Unit> lookupItems = (request.Result.Value as IQueryable<Data.Entities.Unit>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintUnitsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.ItemsCategoriesList:
                        {
                            List<ItemCategory> lookupItems = (request.Result.Value as IQueryable<ItemCategory>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintItemCategorysList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.JobsTitlesList:
                        {
                            List<JobTitle> lookupItems = (request.Result.Value as IQueryable<JobTitle>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintJobTitlesList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.RemainsList:
                        {
                            List<Remains> lookupItems = (request.Result.Value as IQueryable<Remains>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintRemainsList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    case Data.Enums.PrintDocumentTypesEnum.RemainsInquiriesList:
                        {
                            List<RemainsDetails> lookupItems = (request.Result.Value as IQueryable<RemainsDetails>)?.ToList();
                            String filtersstring = request.Params.Split('=')[1].Replace("'", "").Replace(")", "");
                            var filters = JsonConvert.DeserializeObject(filtersstring);
                            if (lookupItems == null || lookupItems.Count == 0)
                                throw new NoDataException(_localizer["NoDataException"]);
                            memoryStream = _wordBusiness.PrintRemainsInquiriesList(lookupItems, filters, request.ReportType);
                            break;
                        }
                    #endregion
                    default:
                        throw new NoDataException(_localizer["NoDataException"]);
                }
                return Task.FromResult(memoryStream);

            }
            throw new NoDataException(_localizer["NoDataException"]);
        }

    }
}

