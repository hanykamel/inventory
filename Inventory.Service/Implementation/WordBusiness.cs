using DocumentFormat.OpenXml.Wordprocessing;
using inventory.Engines.WordGenerator;
using Inventory.CrossCutting.FinancialYear;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.Delegation;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.ReportVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.TransformationRequest.Handlers;
using Inventory.Service.Interfaces;
using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Inventory.Service.Implementation
{
    public class WordBusiness : IWordBusiness
    {
        private readonly IWordGenerator _wordGenerator;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;

        public WordBusiness(
            IWordGenerator wordGenerator,
            IStringLocalizer<SharedResource> Localizer,
            ITenantProvider tenantProvider,
            IStoreBussiness storeBussiness

            )
        {
            _wordGenerator = wordGenerator;
            _Localizer = Localizer;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
        }
        #region Examination
        public MemoryStream printExaminationDocument(ExaminationCommitte examinationCommitte)
        {
            Dictionary<string, string> contentReplacers = GenerateContentExaminationReplacers(examinationCommitte);
            Dictionary<string, string> repetitiveReplacers = GenerateRepetitiveExaminationReplacers(examinationCommitte);
            Dictionary<string, string> headerReplacers = GenerateExaminationHeaderReplacers(examinationCommitte);
            ArrayList tableReplacers = GenerateTableExaminationReplacers(examinationCommitte);
            return _wordGenerator.PrintDocument(PrintDocumentTypesEnum.FormNo12.ToString() + ".docx", headerReplacers: headerReplacers, tabledata: tableReplacers, contentReplacers: contentReplacers, repetitiveReplacers: repetitiveReplacers);
        }
        public Dictionary<string, string> GenerateExaminationHeaderReplacers(ExaminationCommitte examinationCommitte)
        {
            Dictionary<string, string> headerReplacers = new Dictionary<string, string>();
            headerReplacers.Add("VRItemsType", examinationCommitte.ForConsumedItems ? _Localizer["Consumed"] : _Localizer["NotConsumed"]);
            return headerReplacers;
        }
        public Dictionary<string, string> GenerateContentExaminationReplacers(ExaminationCommitte examinationCommitte)
        {
            var culture = new System.Globalization.CultureInfo("ar-Eg");
            Dictionary<string, string> contentReplacers = new Dictionary<string, string>();
            string preSupplierText = _Localizer["Based on the supply order to"];
            contentReplacers.Add("VRBudgetName", " ");
            contentReplacers.Add("VRDescNum", " ");
            contentReplacers.Add("VRSuppName", " ");
            contentReplacers.Add("VRSuppOrderNum", " ");
            contentReplacers.Add("VRComitLeader", " ");
            contentReplacers.Add("VRCurrentDate", DateTime.Now.Date.ToString("yyyy / MM / dd "));
            contentReplacers.Add("VRCurrentDay", culture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek));
            contentReplacers.Add("VRSuppOrderDate", "20  /  /  ");
            contentReplacers.Add("VRPreSuppText", preSupplierText);
            contentReplacers.Add("VRDescDate", "20  /  /  ");
            contentReplacers["VRDescDate"] = examinationCommitte.DecisionDate != null ? examinationCommitte.DecisionDate.GetValueOrDefault().Date.ToString("yyyy / MM / dd ") : "";
            contentReplacers["VRDescNum"] = examinationCommitte.DecisionNumber != null ? examinationCommitte.DecisionNumber : "";
            if (examinationCommitte.Budget == null)
            {
                return contentReplacers;
            }
            if (examinationCommitte.Supplier != null)
            {
                contentReplacers["VRSuppOrderDate"] = examinationCommitte.SupplyOrderDate.GetValueOrDefault().Date.ToString("yyyy / MM / dd ");
                string text = examinationCommitte.SupplyOrderNumber != null ? examinationCommitte.SupplyOrderNumber : " ";
                contentReplacers["VRSuppOrderNum"] = _Localizer[") number"] + text + _Localizer["on ("];
                contentReplacers["VRSuppName"] = examinationCommitte.Supplier.Name;
            }
            if (examinationCommitte.ExternalEntity != null)
            {
                contentReplacers["VRSuppName"] = examinationCommitte.ExternalEntity.Name;
                contentReplacers["VRSuppOrderNum"] = "";
                contentReplacers["VRSuppOrderDate"] = "";
            }
            int budgetId = examinationCommitte.BudgetId;
            contentReplacers["VRBudgetName"] = examinationCommitte.Budget.Name;
            switch (examinationCommitte.BudgetId)
            {
                case (int)BudgetNamesEnum.Staff:
                    preSupplierText = _Localizer["Based on the contract with"];
                    string text = examinationCommitte.ContractNumber != null ? examinationCommitte.ContractNumber : " ";
                    contentReplacers["VRSuppOrderNum"] = _Localizer[") number"] + text + " )";
                    contentReplacers["VRSuppOrderDate"] = examinationCommitte.ContractDate.GetValueOrDefault().Date.ToString("yyyy / MM / dd ");
                    break;
                case (int)BudgetNamesEnum.Presidency:
                    break;
                case (int)BudgetNamesEnum.ArmedForces:
                    preSupplierText = _Localizer["From supplier"];
                    contentReplacers["VRSuppOrderNum"] = _Localizer["issued on"];
                    break;
                case (int)BudgetNamesEnum.Housing:
                    break;
                default:
                    break;
            }
            //if (budgetId != (int)BudgetNamesEnum.Staff)
            //{
            //    if (budgetId == (int)BudgetNamesEnum.ArmedForces)
            //    {
            //        preSupplierText = _Localizer["From supplier"];
            //        contentReplacers["VRSuppOrderNum"] = _Localizer["issued on"];
            //    }
            //if (examinationCommitte.Supplier != null)
            //{
            //    contentReplacers["VRSuppOrderDate"] = examinationCommitte.SupplyOrderDate.GetValueOrDefault().Date.ToString("yyyy / MM / dd ");
            //    string text = examinationCommitte.SupplyOrderNumber != null ? examinationCommitte.SupplyOrderNumber : " ";
            //    contentReplacers["VRSuppOrderNum"] = _Localizer[") number"] + text + _Localizer["on ("];
            //    contentReplacers["VRSuppName"] = examinationCommitte.Supplier.Name;
            //}
            //if (examinationCommitte.ExternalEntity != null)
            //{
            //    contentReplacers["VRSuppName"] = examinationCommitte.ExternalEntity.Name;
            //    contentReplacers["VRSuppOrderNum"] = "";
            //    contentReplacers["VRSuppOrderDate"] = "";
            //}
            //}
            //else
            //{
            //    preSupplierText = _Localizer["Based on the contract with"];
            //    string text = examinationCommitte.ContractNumber != null ? examinationCommitte.ContractNumber : " ";
            //    contentReplacers["VRSuppOrderNum"] = _Localizer[") number"] + text + " )";
            //    contentReplacers["VRSuppOrderDate"] = examinationCommitte.ContractDate.GetValueOrDefault().Date.ToString("yyyy / MM / dd ");
            //}
            if (budgetId != (int)BudgetNamesEnum.ArmedForces)
            {
                foreach (var member in examinationCommitte.CommitteeEmployee)
                {
                    if (member.IsHead)
                    {
                        contentReplacers["VRComitLeader"] = member.JobTitle.Name + " " + member.Emplyee.Name;
                    }
                }
            }
            contentReplacers["VRPreSuppText"] = preSupplierText;
            return contentReplacers;
        }


        public Dictionary<string, string> GenerateRepetitiveExaminationReplacers(ExaminationCommitte examinationCommitte)
        {
            Dictionary<string, string> RepetitiveContentreplacers = new Dictionary<string, string>();
            var j = 1;
            if (examinationCommitte.CommitteeEmployee != null && examinationCommitte.CommitteeEmployee.Count != 0)
            {
                foreach (var member in examinationCommitte.CommitteeEmployee)
                {
                    if (!member.IsHead)
                    {
                        RepetitiveContentreplacers.Add("VRComiteeMemberName" + j, member.Emplyee.Name);
                        RepetitiveContentreplacers.Add("VRComiteeMemberJob" + j, member.JobTitle.Name);
                        j++;
                    }
                }
            }
            return RepetitiveContentreplacers;
        }
        public ArrayList GenerateTableExaminationReplacers(ExaminationCommitte examinationCommitte)
        {
            ArrayList tableReplacers = new ArrayList();
            if (examinationCommitte.CommitteeItem != null && examinationCommitte.CommitteeItem.Count != 0)
            {
                foreach (var item in examinationCommitte.CommitteeItem)
                {
                    tableReplacers.Add(new
                    {
                        Name = item.BaseItem.Name + " " + item.BaseItem.Description,
                        blank = "",
                        quantity = item.Quantity,
                        Unit = item.Unit.Name,
                        examinationPercentage = "%" + item.ExaminationPercentage,
                        accepted = item.Accepted > 0 ? "√" : "─",
                        rejected = item.Rejected > 0 ? "√" : "─",
                        reasons = item.Reasons
                    });
                }
            }
            return tableReplacers;
        }
        #endregion
        #region Addition
        public MemoryStream printAdditionDocument(TransactionsVM addition, List<BaseItemStoreItemVM> baseItemStoreItemVMs)
        {
            ArrayList tableReplacers = GenerateTableAdditionReplacers(baseItemStoreItemVMs);
            Dictionary<string, string> contentReplacers = GenerateContentAdditionReplacers(addition);
            return _wordGenerator.PrintDocument(PrintDocumentTypesEnum.FormNo2.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: contentReplacers);
        }
        public ArrayList GenerateTableAdditionReplacers(List<BaseItemStoreItemVM> baseItemStoreItemVMs)
        {
            ArrayList tableReplacers = new ArrayList();
            if (baseItemStoreItemVMs != null && baseItemStoreItemVMs.Count != 0)
            {
                foreach (var item in baseItemStoreItemVMs)
                {
                    tableReplacers.Add(new
                    {
                        //Id = item.BaseItemId,
                        Id = "",
                        Name = item.BaseItemName + " " + item.BaseItemDesc,
                        Unit = item.UnitName,
                        blank = "",
                        neededQuantity = item.Quantity,
                        blank2 = "",
                        allowedQuantity = item.Quantity,
                        blank3 = "",
                        releasedQuantity = item.Quantity,
                        itemCondition = item.ItemStatus,
                        blank4 = "",
                        price = item.UnitPrice,
                        blank5 = "",
                        value = item.FullPrice,
                        Notes = item.Notes
                    });
                }
            }
            return tableReplacers;
        }
        public Dictionary<string, string> GenerateContentAdditionReplacers(TransactionsVM mainData)
        {
            Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
            if (mainData != null)
            {
                Contentreplacers.Add("VRDepName", mainData.BudgetName);
                Contentreplacers.Add("VRStore", mainData.StoreName);
                Contentreplacers.Add("VROrderDate", mainData.RequestDate);
                Contentreplacers.Add("VRRecieverName", mainData.RequesterName);
                Contentreplacers.Add("VRExchangeNumber", mainData.Serial);
                Contentreplacers.Add("VRExchangeDate", mainData.CreationDate);
                Contentreplacers.Add("VRExchangeDocument", mainData.AdditionDocumentType);
                Contentreplacers.Add("VRDocTypeHeader", _Localizer[mainData.IsAddition ? "addition" : "Exchange"]);
                Contentreplacers.Add("VRDocType", _Localizer[mainData.IsAddition ? "the addition" : "TheExchange"]);
                Contentreplacers.Add("VRCurrency", mainData.Currency != null ? mainData.Currency : "");
            }
            return Contentreplacers;
        }
        #endregion

        #region Addition
        public MemoryStream printAdditionMultiDocument(TransactionsVM addition, List<BaseItemStoreItemVM> baseItemStoreItemVMs)
        {
            List<Form2PrintObjectVM> PrintObjectVMs = baseItemStoreItemVMs.GroupBy(b => b.Currency).Select(a => new Form2PrintObjectVM
            {
                Currency = a.Key,
                value = a.Select(x => new BaseItemStoreItemVM
                {
                    PageNumber=x.PageNumber,
                    ContractNumber=x.ContractNumber,
                    ItemStatus = x.ItemStatus,
                    BaseItemDesc = x.BaseItemDesc,
                    BaseItemName = x.BaseItemName,
                    BaseItemId = x.BaseItemId,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    FullPrice = x.UnitPrice * x.Quantity,
                    Notes = x.Notes,
                    ExaminationReport = x.ExaminationReport
                }).ToList()
            }).ToList();
            foreach (var item in PrintObjectVMs)
            {
                addition.Currencies.Add(item.Currency);
            }
            List<ArrayList> tableReplacers = GenerateTableAdditionMultiReplacers(PrintObjectVMs);
            List<Dictionary<string, string>> contentReplacers = GenerateContentAdditionMultiReplacers(addition);
            return _wordGenerator.PrintMultiplePageDocument(PrintDocumentTypesEnum.FormNo2.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: contentReplacers);
        }
        public List<ArrayList> GenerateTableAdditionMultiReplacers(List<Form2PrintObjectVM> baseItemStoreItemVMs)
        {
            List<ArrayList> tableReplacerrsList = new List<ArrayList>();
            foreach (var baseItemStoreItemVM in baseItemStoreItemVMs)
            {
                ArrayList tableReplacers = new ArrayList();
                if (baseItemStoreItemVMs != null && baseItemStoreItemVMs.Count != 0)
                {
                    foreach (var item in baseItemStoreItemVM.value)
                    {
                        tableReplacers.Add(new
                        {
                            ContractNumber=item.ContractNumber,
                            PageNumber=item.PageNumber,
                            Name = item.BaseItemName + " " + item.BaseItemDesc,
                            Unit = item.UnitName,
                            blank = "",
                            neededQuantity = item.Quantity,
                            blank2 = "",
                            allowedQuantity = item.Quantity,
                            blank3 = "",
                            releasedQuantity = item.Quantity,
                            itemCondition = item.ItemStatus,
                            blank4 = "",
                            price = item.UnitPrice,
                            blank5 = "",
                            value = item.FullPrice,
                            Notes = item.Notes
                        });
                    }
                }
                tableReplacerrsList.Add(tableReplacers);
            }
            return tableReplacerrsList;
        }
        public List<Dictionary<string, string>> GenerateContentAdditionMultiReplacers(TransactionsVM mainData)
        {
             List<Dictionary<string, string>> ContentreplacersList = new List<Dictionary<string, string>>();
            if (mainData != null)
            {
            for (int i = 0; i < mainData.Currencies.Count; i++)
            {
                Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
               
                    Contentreplacers.Add("VRDepName", mainData.BudgetName);
                    Contentreplacers.Add("VRStore", mainData.StoreName);
                    Contentreplacers.Add("VROrderDate", mainData.RequestDate);
                    Contentreplacers.Add("VRRecieverName", mainData.RequesterName);
                    Contentreplacers.Add("VRExchangeNumber", mainData.Serial);
                    Contentreplacers.Add("VRExchangeDate", mainData.CreationDate);
                    Contentreplacers.Add("VRExchangeDocument", mainData.AdditionDocumentType);
                    Contentreplacers.Add("VRDocTypeHeader", _Localizer[mainData.IsAddition ? "addition" : "Exchange"]);
                    Contentreplacers.Add("VRDocType", _Localizer[mainData.IsAddition ? "the addition" : "TheExchange"]);
                    Contentreplacers.Add("VRCurrency", mainData.Currencies[i] != null ? mainData.Currencies[i] : "");
                ContentreplacersList.Add(Contentreplacers);
                }
            }
            return ContentreplacersList;
        }
        #endregion

        #region Invoice
        public MemoryStream printInvoiceDocument(List<Invoice> invoices, List<InvoiceStoreItemVM> invoiceStoreItemsVM, List<InvoiceStoreItem> invoiceStoreItems,bool CountAllResults)
        {
            List<Dictionary<string, string>> contentReplacers =
                GenerateContentInvoiceReplacers(invoices, invoiceStoreItems, CountAllResults);
            List<ArrayList> tableReplacers = GenerateTableInvoiceReplacers
                (invoiceStoreItemsVM, invoices);
            return _wordGenerator.PrintMultiplePageDocument(
                (invoices.FirstOrDefault().ExchangeOrder.Budget.Id ==
                (int)BudgetNamesEnum.Staff ? PrintDocumentTypesEnum.StaffInvoice.ToString()
                : PrintDocumentTypesEnum.Invoice.ToString()) + ".docx",
                tabledata: tableReplacers, contentReplacers: contentReplacers);
        }

        private List<ArrayList> GenerateTableInvoiceReplacers(List<InvoiceStoreItemVM> invoiceStoreItems, List<Invoice> invoices)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            foreach (var invoice in invoices)
            {
                ArrayList replacers = new ArrayList();
                if (invoice.ExchangeOrder.Budget.Id != (int)BudgetNamesEnum.Staff)
                {
                    foreach (var item in invoiceStoreItems.Where(i => i.InvoiceId == invoice.Id))
                    {

                        replacers.Add(new
                        {
                            contractNumber=item.ContractNumber,
                            pageNumber=item.PageNumber,
                            quantity = item.Count,
                            unit = item.UnitName,
                            itemName = item.BaseItemName + " " + item.BaseItemDesc,
                            notes = item.ExchangeOrderNotes,
                        });
                    }
                }
                else
                {
                    foreach (var item in invoiceStoreItems.Where(i => i.InvoiceId == invoice.Id))
                    {
                        replacers.Add(new
                        {
                            pageNumber = item.PageNumber,
                            quantity = item.Count,
                            unit = item.UnitName,
                            itemName = item.BaseItemName,
                            contractNumber = item.ContractNumber,
                            notes = item.ExchangeOrderNotes,

                        });
                    }
                }
                tableReplacers.Add(replacers);
            }
            return tableReplacers;
        }

        private List<Dictionary<string, string>> GenerateContentInvoiceReplacers
            (List<Invoice> invoices, List<InvoiceStoreItem> invoiceStoreItems,bool CountAllResults)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var invoice in invoices)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();
                var cardCode = invoice.ReceivedEmployee.CardCode != null ? invoice.ReceivedEmployee.CardCode : "";
                if (invoice != null)
                {
                    contentReplacer.Add("VRBudget", invoice.ExchangeOrder.Budget.Name);
                    contentReplacer.Add("VRLocation", invoice.Location.Name);
                    contentReplacer.Add("VRREciever", invoice.ReceivedEmployee.Name);
                    contentReplacer.Add("VRRecieverDepartment", invoice.Department != null ? invoice.Department.Name :
                        "");
                    contentReplacer.Add("VRIdNumber", invoice.ReceivedEmployee.CardCode != null ? invoice.ReceivedEmployee.CardCode : "");
                    contentReplacer.Add("VRStoreAdmin", invoice.CreatedBy);
                    contentReplacer.Add("VRStoreName", _tenantProvider.GetTenantName());
                    contentReplacer.Add("VRTotal",
                        invoiceStoreItems.Where(x => CountAllResults == true ? x.InvoiceId == invoice.Id: x.InvoiceId == invoice.Id && x.IsRefunded != true).Count().ToString());
                }
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }
        #endregion
        #region exchange
        public MemoryStream printExchangeDocument(TransactionsVM exhange, List<BaseItemStoreItemVM> baseItemStoreItemVMs) {
            return null;
        }
        public MemoryStream printTransfromationDocument(TransactionsVM exhange, List<BaseItemStoreItemVM> baseItemStoreItemVMs)
        {
            List<Form2PrintObjectVM> printObjectList = baseItemStoreItemVMs.GroupBy(b => b.Currency).Select(a => new Form2PrintObjectVM
            {
                Currency = a.Key,
                value = a.Select(x => new BaseItemStoreItemVM
                {
                    ContractNumber=x.ContractNumber,
                    PageNumber=x.PageNumber,
                    ItemStatus = x.ItemStatus,
                    BaseItemDesc = x.BaseItemDesc,
                    BaseItemName = x.BaseItemName,
                    BaseItemId = x.BaseItemId,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    FullPrice = x.UnitPrice * x.Quantity,
                    Notes = x.Notes,
                    ExaminationReport = x.ExaminationReport
                }).ToList()
            }).ToList();
            foreach (var item in printObjectList)
            {
                exhange.Currencies.Add(item.Currency);
            }
            List<ArrayList> tableReplacers = GenerateTableExchangeReplacers(exhange, printObjectList);
            List<Dictionary<string, string>> contentReplacers = GenerateContentExchangeReplacers(exhange);
            return _wordGenerator.PrintMultiplePageDocument(PrintDocumentTypesEnum.FormNo8.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: contentReplacers);
        }

        private List<ArrayList> GenerateTableExchangeReplacers(TransactionsVM exhange, List<Form2PrintObjectVM> baseItemStoreItemVMs)
        {
            List<ArrayList> multiTableReplacers = new List<ArrayList>();
            foreach (var baseItemStoreItemVM in baseItemStoreItemVMs)
            {
                ArrayList tableReplacers = new ArrayList();
                if (baseItemStoreItemVM.value != null && baseItemStoreItemVM.value.Count != 0)
                {
                    foreach (var item in baseItemStoreItemVM.value)
                    {
                        tableReplacers.Add(new
                        {
                            //Id = item.BaseItemId,
                            ContractNumber=item.ContractNumber,
                            PageNumber=item.PageNumber,
                            Name = item.BaseItemName + " " + item.BaseItemDesc,
                            ExchangeDate = exhange.CreationDate,
                            Unit = item.UnitName,
                            blank = "",
                            neededQuantity = item.Quantity,
                            itemCondition = item.ItemStatus,
                            blank4 = "",
                            price = item.UnitPrice,
                            Notes = item.ExaminationReport
                        });
                    }
                }
                multiTableReplacers.Add(tableReplacers);
            }
            return multiTableReplacers;
        }

        private List<Dictionary<string, string>> GenerateContentExchangeReplacers(TransactionsVM exhange)
        {
            List<Dictionary<string, string>> ContentreplacersList = new List<Dictionary<string, string>>();
            for (int i = 0; i < exhange.Currencies.Count; i++)
            {
                Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
                if (exhange != null)
                {
                    Contentreplacers.Add("VRDepName", exhange.BudgetName);
                    Contentreplacers.Add("VRStore", exhange.StoreName);
                    Contentreplacers.Add("VRStoreKeeper", exhange.CustodyOwner);
                    Contentreplacers.Add("VRDate", exhange.CreationDate);
                    Contentreplacers.Add("VRCurrency", exhange.Currencies[i]!=null? exhange.Currencies[i]:"");
                }
                ContentreplacersList.Add(Contentreplacers);
            }

            return ContentreplacersList;
        }
        #endregion

        #region StoktakingMulti
        public MemoryStream printStocktakingMultiDocument(Form6VM stockTaking, List<FormNo6StoreItemVM> baseItemStoreItemVMs, string TemplateName)
        {
            List<StocktakingPrintObjectVM> PrintObjectVMs = baseItemStoreItemVMs.GroupBy(b => b.Currency).Select(a => new StocktakingPrintObjectVM
            {
                Currency = a.Key,
                value = a.Select(x => new FormNo6StoreItemVM
                {
                    ContractNumber=x.ContractNumber,
                    ItemStatus = x.ItemStatus,
                    Description = x.Description,
                    BaseItemName = x.BaseItemName,
                    BaseItemId = x.BaseItemId,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Notes = x.Notes,
                    BookNumber = x.BookNumber,
                    BookId = x.BookId,
                    PageNumber = x.PageNumber,
                    UnitId = x.UnitId,
                    CurrencyId = x.CurrencyId,
                    Currency = x.Currency
                }).ToList()
            }).ToList();
            foreach (var item in PrintObjectVMs)
            {
                stockTaking.CurrencyList.Add(item.Currency);
                decimal price = 0;
                foreach (var stocktakingItem in item.value)
                {
                    price += (stocktakingItem.Price * stocktakingItem.Quantity);
                }
                stockTaking.TotalPriceList.Add(price.ToString());
            }
            Dictionary<string, string> headerReplacers = GenerateStoktakingHeaderReplacers(stockTaking);
            List<Dictionary<string, string>> headerReplacersList = new List<Dictionary<string, string>>();
            headerReplacersList.Add(headerReplacers);
            List<Dictionary<string, string>> contentReplacers = GenerateContentStoktakingMultiReplacers(stockTaking);
            List<List<ArrayList>> tableReplacers = GenerateStoktakingMultiTableReplacers(PrintObjectVMs);
            return _wordGenerator.PrintMultiplePageDocument(TemplateName + ".docx",
                tableReplacers: tableReplacers, headerReplacers: headerReplacersList, contentReplacers: contentReplacers,fullTable:false);
        }

        public List<Dictionary<string, string>> GenerateContentStoktakingMultiReplacers(Form6VM stockTaking)
        {
            List<Dictionary<string, string>> contentReplacersList = new List<Dictionary<string, string>>();
            if (stockTaking != null)
            {
                for (int i = 0; i < stockTaking.TotalPriceList.Count; i++)
                {
                    Dictionary<string, string> contentReplacers = new Dictionary<string, string>();
                    contentReplacers.Add("VRTotalPrice", stockTaking.TotalPriceList[i]);
                    contentReplacers.Add("VRCurrency", stockTaking.CurrencyList[i]);
                    contentReplacersList.Add(contentReplacers);
                }
            }

            return contentReplacersList;
        }

        public Dictionary<string, string> GenerateStoktakingHeaderReplacers(Form6VM stockTaking)
        {
            Dictionary<string, string> headerReplacers = new Dictionary<string, string>();
            if (stockTaking != null)
            {
                headerReplacers.Add("VRBudget", stockTaking.BudgetName);
                headerReplacers.Add("VRStore", stockTaking.StoreName);
                headerReplacers.Add("VRStoreAdmin", stockTaking.StoreKeeper);
                headerReplacers.Add("VRDate", stockTaking.StockTakingDate);
            }
            return headerReplacers;
        }

        public List<List<ArrayList>> GenerateStoktakingMultiTableReplacers(List<StocktakingPrintObjectVM> baseItemStoreItemVMs)
        {
            List<List<ArrayList>> x = new List<List<ArrayList>>();
            List<ArrayList> tableReplacersList = new List<ArrayList>();
            foreach (var baseItemStoreItemVM in baseItemStoreItemVMs)
            {
            decimal FulllPrice = 0;
                ArrayList tableReplacers = new ArrayList();
                if (baseItemStoreItemVM != null && baseItemStoreItemVM.value.Count != 0)
                {
                    tableReplacers.Add(FillRow("مجموع ما قبله", FulllPrice));
                    var AddSumRows = false;
                    var lettersCount = 0;
                    foreach (var item in baseItemStoreItemVM.value)
                    {
                        FulllPrice +=(item.Price*item.Quantity);
                        var ItemFullName = item.BaseItemName + " " + item.Description;
                        ItemFullName = ItemFullName.Length > 250 ? ItemFullName.Substring(0,250) : ItemFullName;
                        lettersCount += ItemFullName.Length;
                        if (lettersCount>=250)
                        {
                            AddSumRows = true;
                            lettersCount = 0;
                        }
                        if (AddSumRows)
                        {
                            tableReplacers.Add(FillRow("مجموع ما بعده", FulllPrice - (item.Price * item.Quantity)));
                            tableReplacersList.Add(tableReplacers);
                            tableReplacers = new ArrayList();
                            tableReplacers.Add(FillRow("مجموع ما قبله", FulllPrice - (item.Price * item.Quantity)));
                            AddSumRows = false;
                        }
                        tableReplacers.Add(
                            FillRow(Convert.ToString(item.Price), (item.Price * item.Quantity),Convert.ToString(item.BookNumber),
                            Convert.ToString(item.PageNumber), item.ContractNumber,ItemFullName, item.UnitName,
                            Convert.ToString(item.Quantity),item.ItemStatus));
                    }
                }
                tableReplacersList.Add(tableReplacers);
                x.Add(tableReplacersList);
                tableReplacersList = new List<ArrayList>();
            }
            return x;
        }

        public object FillRow(string name, decimal value,string BookNumber="",string PageNumber = "",string ContractNumber="",string ItemFullName = ""
            ,string UnitName = "",string Quantity = "",string ItemStatus = "")
        {
            return new
            {
                BookNumber = BookNumber,
                PageNumber = PageNumber,
                ContractNumber=ContractNumber,
                Name = ItemFullName,
                Unit = UnitName,
                blank1 = "",
                blank2 = "",
                Count = Quantity,
                itemCondition = ItemStatus,
                blank3 = "",
                blank4 = "",
                price = name,
                FullPrice = value
            };
        }
        #endregion

        //#region Stoktaking
        //public MemoryStream printStoktakingDocument(Form6VM stockTaking, List<FormNo6StoreItemVM> baseItemStoreItemVMs)
        //{
        //    Dictionary<string, string> headerReplacers = GenerateStoktakingHeaderReplacers(stockTaking);
        //    Dictionary<string, string> contentReplacers = GenerateContentStoktakingReplacers(stockTaking);
        //    ArrayList tableReplacers = GenerateStoktakingTableReplacers(baseItemStoreItemVMs);
        //    return _wordGenerator.PrintDocument(PrintDocumentTypesEnum.StocktakingForm.ToString() + ".docx", tabledata: tableReplacers, headerReplacers: headerReplacers, contentReplacers: contentReplacers);
        //}
        //public Dictionary<string, string> GenerateContentStoktakingReplacers(Form6VM stockTaking)
        //{
        //    Dictionary<string, string> headerReplacers = new Dictionary<string, string>();
        //    if (stockTaking != null)
        //    {
        //        headerReplacers.Add("VRTotalPrice", stockTaking.TotalPrice);
        //    }
        //    return headerReplacers;
        //}


        //public ArrayList GenerateStoktakingTableReplacers(List<FormNo6StoreItemVM> baseItemStoreItemVMs)
        //{
        //    ArrayList tableReplacers = new ArrayList();
        //    if (baseItemStoreItemVMs != null && baseItemStoreItemVMs.Count != 0)
        //    {
        //        foreach (var item in baseItemStoreItemVMs)
        //        {
        //            tableReplacers.Add(new
        //            {
        //                BookNumber = item.BookNumber,
        //                PageNumber = item.PageNumber,
        //                //Code = item.BaseItemId,
        //                Code = "",
        //                Name = item.BaseItemName + " " + item.Description,
        //                Unit = item.UnitName,
        //                blank1 = "",
        //                blank2 = "",
        //                Count = item.Quantity,
        //                itemCondition = item.ItemStatus,
        //                blank3 = "",
        //                blank4 = "",
        //                price = item.Price,
        //                FullPrice = (item.Price * item.Quantity)
        //            });
        //        }
        //    }
        //    return tableReplacers;
        //}
        //#endregion
        //#region Handover
        //public MemoryStream printHandoverDocument(Form6VM handover, List<StocktakingPrintObjectVM> baseItemStoreItemVMs)
        //{
        //    Dictionary<string, string> headerReplacers = GenerateHeaderHandoverReplacers(handover);
        //    List<Dictionary<string, string>> headerReplacersList = new List<Dictionary<string, string>>();
        //    headerReplacersList.Add(headerReplacers);
        //    List<ArrayList> tableReplacers = GenerateTableHandoverReplacers(baseItemStoreItemVMs);
        //    List<Dictionary<string, string>> contentReplacers = GenerateHandoverContentReplacers(handover);
        //    return _wordGenerator.PrintMultiplePageDocument(PrintDocumentTypesEnum.HandOverForm.ToString() + ".docx"
        //        , tabledata: tableReplacers, headerReplacers: headerReplacersList, contentReplacers: contentReplacers);
        //}

        //public Dictionary<string, string> GenerateHeaderHandoverReplacers(Form6VM handover)
        //{
        //    Dictionary<string, string> headerReplacers = new Dictionary<string, string>();
        //    if (handover != null)
        //    {
        //        headerReplacers.Add("VRBudget", handover.BudgetName);
        //        headerReplacers.Add("VRStore", handover.StoreName);
        //        headerReplacers.Add("VRStoreAdmin", handover.StoreKeeper);
        //        headerReplacers.Add("VRDate", handover.StockTakingDate);
        //    }
        //    return headerReplacers;
        //}

        //public List<Dictionary<string, string>> GenerateHandoverContentReplacers(Form6VM handover)
        //{
        //    List<Dictionary<string, string>> contentReplacersList = new List<Dictionary<string, string>>();
        //    if (handover != null)
        //    {
        //        for (int i = 0; i < handover.TotalPriceList.Count; i++)
        //        {
        //            Dictionary<string, string> contentReplacers = new Dictionary<string, string>();
        //            contentReplacers.Add("VRTotalPrice", handover.TotalPriceList[i]);
        //            contentReplacers.Add("VRCurrency", handover.CurrencyList[i]);
        //            contentReplacersList.Add(contentReplacers);
        //        }
        //    }

        //    return contentReplacersList;
        //}

        //public List<ArrayList> GenerateTableHandoverReplacers(List<StocktakingPrintObjectVM> baseItemStoreItemVMs)
        //{
        //    List<ArrayList> tableReplacersList = new List<ArrayList>();
        //    foreach (var baseItemStoreItemVM in baseItemStoreItemVMs)
        //    {
        //        ArrayList tableReplacers = new ArrayList();
        //        if (baseItemStoreItemVM.value != null && baseItemStoreItemVM.value.Count != 0)
        //        {
        //            foreach (var item in baseItemStoreItemVM.value)
        //            {
        //                tableReplacers.Add(new
        //                {
        //                    BookNumber = item.BookNumber,
        //                    PageNumber = item.PageNumber,
        //                    //Code = item.BaseItemId,
        //                    Code = "",
        //                    Name = item.BaseItemName,
        //                    Unit = item.UnitName,
        //                    blank1 = "",
        //                    blank2 = "",
        //                    Count = item.Quantity,
        //                    itemCondition = item.ItemStatus,
        //                    blank3 = "",
        //                    blank4 = "",
        //                    price = item.Price,
        //                    FullPrice = (item.Price * item.Quantity)
        //                });
        //            }
        //        }
        //        tableReplacersList.Add(tableReplacers);
        //    }
        //    return tableReplacersList;
        //}

        //#endregion

        #region Stagnant
        public MemoryStream PrintStagnantDocument(StagnantModelVM StagnantModel)
        {

            Dictionary<string, string> contentReplacers = GenerateStagnantContentReplacers(Convert.ToDateTime(StagnantModel.DateFrom), StagnantModel.StoreItems);
            ArrayList tableReplacers = GenerateStagnantTableReplacers(StagnantModel.StoreItems);
            return _wordGenerator.PrintDocument(PrintDocumentTypesEnum.StagnantForm.ToString() + ".docx", tabledata: tableReplacers, contentReplacers, fullTable: false);
        }
        public Dictionary<string, string> GenerateStagnantContentReplacers(DateTime stagnantDate, List<StagnantBaseItemVM> baseItems)
        {
            String storeName = String.Empty;
            List<int> tenants = baseItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());
            Dictionary<string, string> contentReplacers = new Dictionary<string, string>();
            contentReplacers.Add("VRStagnantDate", stagnantDate.Date.ToString("yyyy / MM / dd "));
            contentReplacers.Add("VRStoreName", storeName);
            contentReplacers.Add("VRCount", baseItems.Count().ToString());
            contentReplacers.Add("VRItemsCount", baseItems.Sum(b => b.StoreItemsCount).ToString());
            return contentReplacers;
        }
        private ArrayList GenerateStagnantTableReplacers(List<StagnantBaseItemVM> storeItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (storeItems != null && storeItems.Count != 0)
            {
                for (int i = 0; i < storeItems.Count; i++)
                {
                    tableReplacers.Add(new
                    {
                        serial = i + 1,
                        BookNumber = storeItems[i].BookNumber,
                        PageNumber = storeItems[i].PageNumber,
                        Code = storeItems[i].Code,
                        StoreItem = storeItems[i].Name,
                        StoreItemCount = storeItems[i].StoreItemsCount.ToString(),
                        Unit = storeItems[i].Unit,
                        LastExchangeOrderDate = storeItems[i].LastExchangeOrderDate != null ? storeItems[i].LastExchangeOrderDate.Value.ToString("yyyy / MM / dd ") : "",
                        LastAdditionDate = storeItems[i].LastAdditionDate.ToString("yyyy / MM / dd "),
                    });
                }
            }
            return tableReplacers;
        }
        #endregion
        #region Reports
        public MemoryStream PrintInvoiceReport(List<InvoiceReportVM> invoices)
        {

            String storeName = null;
            List<int> tenants = invoices.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());
            Dictionary<string, string> contentReplacer = new Dictionary<string, string>();
            contentReplacer.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            contentReplacer.Add("VRStore", storeName);
            ArrayList tableReplacers = GenerateInvoiceReportTableReplacers(invoices);// new ArrayList(invoices);
            return _wordGenerator.PrintDocument(PrintDocumentTypesEnum.InvoiceReport.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: contentReplacer, fullTable: false);
        }

        public ArrayList GenerateInvoiceReportTableReplacers(List<InvoiceReportVM> invoices)
        {
            ArrayList tableReplacers = new ArrayList();
            if (invoices != null && invoices.Count != 0)
            {
                foreach (var item in invoices)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.Date.ToString("d  MMM  yyyy")
                    });
                }
            }
            return tableReplacers;
        }


        public MemoryStream PrintInvoiceStoreItemReport(List<InvoiceStoreItemsReportVM> invoiceStoreItems, PrintDocumentTypesEnum doc)
        {
            List<GroupedInvoiceStoreItemsVM> groupedInvoices = invoiceStoreItems
                .GroupBy(a => new
                {
                    Code = a.Invoice.Code,
                    ReceivedEmployeeName = a.Invoice.ReceivedEmployeeName,
                    LocationName = a.Invoice.LocationName,
                    EmployeeCardCode = a.Invoice.EmployeeCardCode,

                }).Select(s => new GroupedInvoiceStoreItemsVM()
                {
                    Code = s.Key.Code,
                    ReceivedEmployeeName = s.Key.ReceivedEmployeeName,
                    LocationName = s.Key.LocationName,
                    EmployeeCardCode = s.Key.EmployeeCardCode,
                    StoreItems = s.Select(g => new InvoiceStoreItemsReportVM()
                    {
                        Code = g.Code,
                        StoreItem = g.StoreItem,
                        Unit = g.Unit,
                        Price = g.Price,
                        Quantity = g.Quantity,
                        TenantId = g.TenantId,
                        CreationDate = g.CreationDate

                    }).ToList()
                }).ToList();
            Dictionary<string, string> contentReplacer = new Dictionary<string, string>();
            contentReplacer.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            List<Dictionary<string, string>> repeatedContent = GenerateInvoiceStoreItemRepeatedContent(groupedInvoices);
            List<ArrayList> tableReplacers = GenerateInvoiceStoreItemReportTableReplacers(groupedInvoices);//new ArrayList(invoiceStoreItems);
            return _wordGenerator.PrintSectionRepeaterDocument(doc.ToString() + ".docx", sectionReplacer: tableReplacers, contentRepeaterReplacers: repeatedContent, contentReplacers: contentReplacer, tableIndex: 1);
        }
        private List<Dictionary<string, string>> GenerateInvoiceStoreItemRepeatedContent(List<GroupedInvoiceStoreItemsVM> groupedInvoices)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var item in groupedInvoices)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();

                contentReplacer.Add("VRLocation", item.LocationName);
                contentReplacer.Add("VRRecEmp", item.ReceivedEmployeeName);
                contentReplacer.Add("VRCardCode", item.EmployeeCardCode);
                contentReplacer.Add("VRInvoiceCode", item.Code);
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }
        public List<ArrayList> GenerateInvoiceStoreItemReportTableReplacers(List<GroupedInvoiceStoreItemsVM> groupedInvoices)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            if (groupedInvoices != null && groupedInvoices.Count != 0)
            {
                foreach (var item in groupedInvoices)
                {
                    ArrayList lst = new ArrayList();
                    for (int i = 0; i < item.StoreItems.Count; i++)
                    {
                        lst.Add(new
                        {
                            Code = item.StoreItems[i].Code,
                            StoreItem = item.StoreItems[i].StoreItem,
                            Unit = item.StoreItems[i].Unit,
                            Quantity = item.StoreItems[i].Quantity,
                            Price = item.StoreItems[i].Price
                        });
                    }

                    tableReplacers.Add(lst);
                }
            }
            return tableReplacers;
        }


        public MemoryStream PrintStoreItemIndexReport(List<StoreBookVM> storeItems, dynamic filters)
        {
            String storeName = null;
            List<int> tenants = storeItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());

            Dictionary<string, string> contentReplacer = new Dictionary<string, string>();
            contentReplacer.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            contentReplacer.Add("VRStore", storeName);
            List<StoreItemIndexGroupVM> groupedCustomerList = storeItems
                        .GroupBy(u => u.BookNumber)
                        .Select(grp => new StoreItemIndexGroupVM()
                        {
                            BookNumber = grp.Key,
                            Result = grp.Select(c => new StoreItemIndexReportVM()
                            {
                                BookNumber = c.BookNumber,
                                BookPageNumber = c.PageNumber,
                                Name = c.ItemName,
                                CreationDate = c.CreationDate,
                                Code = c.Id.ToString()
                            }).ToList()
                        })
                        .ToList();
            List<ArrayList> tableReplacers = GenerateStoreItemIndexReportTableReplacers(groupedCustomerList);
            List<Dictionary<String, string>> booksDictionary = GenerateStoreItemIndexReportRepeatedContent(groupedCustomerList.Select(c => c.BookNumber).ToList());
            List<Dictionary<String, string>> repetitiveSectionReplacer = PrepareStoreItemIndexSectionReplacers(filters);
            return _wordGenerator.PrintSectionRepeaterDocument(
                PrintDocumentTypesEnum.StoreItemIndexReport.ToString() + ".docx",
                sectionReplacer: tableReplacers, contentRepeaterReplacers: booksDictionary,
                contentReplacers: contentReplacer,
                repetitiveSectionReplacer: repetitiveSectionReplacer,
                sectionIndex: 2,
                tableIndex: 1,
                startNewPage: true);
        }
        private List<Dictionary<String, String>> PrepareStoreItemIndexSectionReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["BaseItem"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemName"]);
                values.Add("VRValue", filters["BaseItem"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["Budget"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["BudgetName"]);
                values.Add("VRValue", filters["Budget"]?.Value);
                replacersDictionary.Add(values);
            }
            AddRepetitiveContactAndPageNumber(filters, replacersDictionary);
            return replacersDictionary;
        }

        private void AddRepetitiveContactAndPageNumber(dynamic filters, List<Dictionary<string, string>> replacersDictionary)
        {
            if (filters != null && filters["ContractNumber"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ContractNum"]);
                values.Add("VRValue", filters["ContractNumber"]?.Value);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["PageNumber"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["PageNumber"]);
                values.Add("VRValue", Convert.ToString(filters["PageNumber"]?.Value));
                replacersDictionary.Add(values);
            }
        }

        private List<ArrayList> GenerateStoreItemIndexReportTableReplacers(List<StoreItemIndexGroupVM> storeItems)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            if (storeItems != null && storeItems.Count != 0)
            {
                foreach (var item in storeItems)
                {
                    ArrayList lst = new ArrayList();

                    foreach (var subItem in item.Result)
                    {
                        lst.Add(new
                        {
                            BookPageNumber = subItem.BookPageNumber,
                            Name = subItem.Name,
                            Code = subItem.Code
                        });
                    }
                    tableReplacers.Add(lst);
                }
            }
            return tableReplacers;
        }

        private List<Dictionary<string, string>> GenerateStoreItemIndexReportRepeatedContent(List<int> books)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var book in books)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();

                contentReplacer.Add("VRBookNumber", book.ToString());
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }

        public MemoryStream PrintExistingStoreItemsReport(List<ExistingStoreItemVM> storeItems, PrintDocumentTypesEnum doc, dynamic filters)
        {
            String storeName = null;
            List<int> tenants = storeItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());
            Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();//GenerateExistingStoreItemsReport(storeItems.Count);
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRStoreName", storeName);
            replacersDictionary.Add("VRTotalCount", storeItems.Count.ToString());
            String budgetLabel = String.Empty;
            String budget = String.Empty;
            if (filters != null && filters["Budget"]?.Value != null)
            {
                budgetLabel = _Localizer["BudgetName"];
                budget = filters["Budget"]?.Value;
            }
            List<Dictionary<String, String>> repetitivvereplacersDictionary = new List<Dictionary<String, String>>();
            AddRepetitiveContactAndPageNumber(filters, repetitivvereplacersDictionary);
            replacersDictionary.Add("VRBNL", budgetLabel);
            replacersDictionary.Add("VRBN", budget);
            Dictionary<String, string> footerDictionary = new Dictionary<string, string>();
            footerDictionary.Add("VRFooterDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = GenerateIExistingStoreItemsReportTableReplacers(storeItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", repetitiveSectionReplacer: repetitivvereplacersDictionary, tabledata: tableReplacers, contentReplacers: replacersDictionary, footerReplacers: footerDictionary, fullTable: false);
        }

        //private Dictionary<string, string> GenerateExistingStoreItemsReport(int totalCount)
        //{
        //    Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
        //    Contentreplacers.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
        //    Contentreplacers.Add("VRStoreName", _tenantProvider.GetTenantName());
        //    Contentreplacers.Add("VRTotalCount", totalCount.ToString());
        //    return Contentreplacers;
        //}
        public ArrayList GenerateIExistingStoreItemsReportTableReplacers(List<ExistingStoreItemVM> storeItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (storeItems != null && storeItems.Count != 0)
            {
                foreach (var item in storeItems)
                {
                    tableReplacers.Add(new
                    {
                        //Code = item.BaseItemId,
                        Code = "",
                        StoreItem = item.BaseItemName,
                        BookNumber = item.BookNumber,
                        BookPageNumber = item.PageNumber,
                        Unit = item.UnitName,
                        sheetBalance = item.AllQuantity,
                        actualCredit = item.availableQuantity,
                        PayMovement = item.differenceQuantity,
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintTechnicianStoreItemsReport(List<PrintTechnicianStoreItemVM> technicianStoreItems, PrintDocumentTypesEnum doc, dynamic filters)
        {
            Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / dd "));
            String budgetLabel = String.Empty;
            String budget = String.Empty;
            if (filters != null && filters["Budget"]?.Value != null)
            {
                budgetLabel = _Localizer["BudgetName"];
                budget = filters["Budget"]?.Value;
            }
            replacersDictionary.Add("VRBNL", budgetLabel);
            replacersDictionary.Add("VRBN", budget);
            String contractLabel = String.Empty;
            String contract = String.Empty;
            if (filters != null && filters["ContractNumber"]?.Value != null)
            {
                contractLabel = _Localizer["ContractNum"];
                contract = filters["ContractNumber"]?.Value;
            }
            replacersDictionary.Add("VRCNL", contractLabel);
            replacersDictionary.Add("VRCN", contract);

            List<GroupedTechnicianStoreItemsVM> groupedStoreItems = technicianStoreItems
                        .GroupBy(d => d.BaseItemID)
                        .Select(g => new GroupedTechnicianStoreItemsVM()
                        {
                            BaseItemID = g.Key,
                            BaseItemName = g.FirstOrDefault()?.BaseItemName,
                            TotalQuantity = g.FirstOrDefault()?.TotalQuantity,
                            storeQuantity = g.FirstOrDefault()?.storeQuantity,
                            expensedQuantity = g.FirstOrDefault()?.expensedQuantity,
                            reservedQuantity = g.FirstOrDefault()?.reservedQuantity,
                            robbingQuantity = g.FirstOrDefault()?.robbingQuantity,
                            Details = g.ToList()
                        }).ToList();
            List<ArrayList> tableReplacers = GenerateTechnicianStoreItemsReportTable(groupedStoreItems);
            List<Dictionary<String, string>> departmentDictionary = GenerateTechnicianStoreItemsRepeatedContent(groupedStoreItems);
            return _wordGenerator.PrintSectionRepeaterDocument(doc.ToString() + ".docx", sectionReplacer: tableReplacers, contentRepeaterReplacers: departmentDictionary, contentReplacers: replacersDictionary, tableIndex: 1);

        }

        private List<ArrayList> GenerateTechnicianStoreItemsReportTable(List<GroupedTechnicianStoreItemsVM> groupedStoreItems)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            if (groupedStoreItems != null && groupedStoreItems.Count != 0)
            {
                foreach (var item in groupedStoreItems)
                {
                    ArrayList lst = new ArrayList();
                    for (int i = 0; i < item.Details.Count; i++)
                    {
                        lst.Add(new
                        {
                            RecieverName = item.Details[i].receiverName,
                            Department = item.Details[i].department,
                            InvoiceDate = item.Details[i].invoiceDate.ToString("d  MMM  yyyy"),
                            InvoiceCode = item.Details[i].invoiceCode
                        }); ;
                    }

                    tableReplacers.Add(lst);
                }
            }
            return tableReplacers;
        }

        private List<Dictionary<string, string>> GenerateTechnicianStoreItemsRepeatedContent(List<GroupedTechnicianStoreItemsVM> groupedStoreItems)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var item in groupedStoreItems)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();

                contentReplacer.Add("VRStoreItem", item.BaseItemName);
                contentReplacer.Add("VRTotalNum", item.TotalQuantity.ToString());
                contentReplacer.Add("VRStoreNum", item.storeQuantity.ToString());
                contentReplacer.Add("VRPaidNum", item.expensedQuantity.ToString());
                contentReplacer.Add("VRBookedUpNum", item.reservedQuantity.ToString());
                contentReplacer.Add("VRRobingNum", item.robbingQuantity.ToString());
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }

        public MemoryStream PrintCustodyPersonReport(List<CustodyPersonVM> baseItems, PrintDocumentTypesEnum doc, dynamic filters)
        {
            Dictionary<String, String> replacersDictionary = GenerateCustodyPersonReplacers(baseItems, filters);
            ArrayList tableReplacers = GenerateCustodyPersonTable(baseItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }

        private ArrayList GenerateCustodyPersonTable(List<CustodyPersonVM> storeItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (storeItems != null && storeItems.Count != 0)
            {
                foreach (var item in storeItems)
                {
                    tableReplacers.Add(new
                    {
                        ContractNumber= item.ContractNumber,
                        BaseItemName = item.BaseName,
                        InvoiceDate = item.ReceivedDate?.ToString("yyyy / MM / dd "),
                        BookNumber = item.BookNumber,
                        BookPageNumber = item.PageNumber,
                        UnitName = item.UnitName,
                        StoreItemCount = item.StoreItemCount,
                        BudgetName = item.BudgetName
                    });
                }
            }
            return tableReplacers;
        }
        private Dictionary<string, string> GenerateCustodyPersonReplacers(List<CustodyPersonVM> baseItems, dynamic filters)
        {
            Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
            Contentreplacers.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / dd "));
            Contentreplacers.Add("VRBaseItemCount", baseItems.Count.ToString());
            Contentreplacers.Add("VRStoreItemCount", baseItems.Sum(s => s.StoreItemCount).ToString());
            String budgetLabel = String.Empty;
            String budget = String.Empty;
            if (filters != null && filters["Budget"]?.Value != null)
            {
                budgetLabel = _Localizer["BudgetName"];
                budget = filters["Budget"]?.Value;
            }
            Contentreplacers.Add("VRBNL", budgetLabel);
            Contentreplacers.Add("VRBN", budget);
            CustodyPersonVM reciever = baseItems.FirstOrDefault();
            Contentreplacers.Add("VRRN", reciever.ReceivedEmployee);
            Contentreplacers.Add("VRRCC", reciever.ReceivedEmployeeCard);
            //Contentreplacers.Add("VRTotalCount", "");
            return Contentreplacers;
        }

        public MemoryStream PrintDepartmentStoreItemsReport(List<DepartmentStoreItemPrintVM> storeItems, PrintDocumentTypesEnum doc, dynamic filters)
        {
            String storeName = null;
            List<int> tenants = storeItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());
            DepartmentStoreItemVM StoreItem = storeItems
                .GroupBy(g => new
                {
                    BaseItemId = g.Id,
                    BaseItemName = g.BaseItemName,
                    AllQuantity = g.AllQuantity,
                    BookNumber = g.BookNumber,
                    PageNumber = g.PageNumber
                }).Select(s => new DepartmentStoreItemVM()
                {
                    BaseItemName = s.Key.BaseItemName,
                    BookNumber = s.Key.BookNumber,
                    PageNumber = s.Key.PageNumber,
                    AllQuantity = s.Key.AllQuantity,
                    DepartmentDetails = s.Select(g => new DepartmentDetailsVM()
                    {
                        DepartmentId = g.DepartmentId,
                        DepartmentName = g.DepartmentName,
                        Location = g.Location,
                        Notes = g.Notes,
                        Amount = g.Amount,
                        ReceiverName = g.ReceiverName
                    }).ToList()

                }).FirstOrDefault();
            List<GroupedDeartmentStoreItemReportVM> groupedDeparmentList = StoreItem.DepartmentDetails
                        .GroupBy(d =>
                        new
                        {
                            DepartmentId = d.DepartmentId,
                            DepartmentName = d.DepartmentName
                        }
                        )
                        .Select(g => new GroupedDeartmentStoreItemReportVM()
                        {
                            DepartmentId = g.Key.DepartmentId,
                            DepartmentName = g.Key.DepartmentName,
                            //Amount = g.FirstOrDefault()?.Amount,
                            Items = g.ToList()
                        }).ToList();
            Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRStoreItem", StoreItem.BaseItemName);
            replacersDictionary.Add("VRBookNum", StoreItem.BookNumber.ToString());
            replacersDictionary.Add("VRBPNum", StoreItem.PageNumber.ToString());
            replacersDictionary.Add("VRTotalNum", StoreItem.AllQuantity.ToString());
            replacersDictionary.Add("VRStoreName", storeName);
            int totalExpensesUnits = 0;
            foreach (var department in groupedDeparmentList)
            {
                totalExpensesUnits += department.Items.Select(a => a.Amount).Sum();
            }
            replacersDictionary.Add("VRTotal", totalExpensesUnits.ToString());
            replacersDictionary.Add("VRRemaining", (StoreItem.AllQuantity - totalExpensesUnits).ToString());
            String budgetLabel = String.Empty;
            String budget = String.Empty;
            if (filters != null && filters["Budget"]?.Value != null)
            {
                budgetLabel = _Localizer["BudgetName"];
                budget = filters["Budget"]?.Value;
            }
            replacersDictionary.Add("VRBNL", budgetLabel);
            replacersDictionary.Add("VRBN", budget);
            String contractLabel = String.Empty;
            String contract = String.Empty;
            if (filters != null && filters["ContractNumber"]?.Value != null)
            {
                contractLabel = _Localizer["ContractNum"];
                contract = filters["ContractNumber"]?.Value;
            }
            replacersDictionary.Add("VRCNL", contractLabel);
            replacersDictionary.Add("VRCN", contract);

            Dictionary<String, string> footerDictionary = new Dictionary<string, string>();
            footerDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            List<ArrayList> tableReplacers = GenerateDepartmentStoreItemsReportTable(groupedDeparmentList);
            List<Dictionary<String, string>> departmentDictionary = GenerateDepartmentStoreItemsReportRepeatedContent(groupedDeparmentList);
            return _wordGenerator.PrintSectionRepeaterDocument(doc.ToString() + ".docx",
                sectionReplacer: tableReplacers,
                contentRepeaterReplacers: departmentDictionary,
                contentReplacers: replacersDictionary,
                tableIndex: 1,
                footerReplacers: footerDictionary);

        }

        private List<ArrayList> GenerateDepartmentStoreItemsReportTable(List<GroupedDeartmentStoreItemReportVM> departments)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            if (departments != null && departments.Count != 0)
            {
                foreach (var item in departments)
                {
                    ArrayList lst = new ArrayList();
                    for (int i = 0; i < item.Items.Count; i++)
                    {
                        lst.Add(new
                        {
                            Serial = (i + 1).ToString(),
                            ReceiverName = item.Items[i].ReceiverName,
                            Amount = item.Items[i].Amount.ToString(),
                            Location = item.Items[i].Location,
                            Notes = item.Items[i].Notes
                        }); ;
                    }

                    tableReplacers.Add(lst);
                }
            }
            return tableReplacers;
        }

        private List<Dictionary<string, string>> GenerateDepartmentStoreItemsReportRepeatedContent(List<GroupedDeartmentStoreItemReportVM> departments)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var department in departments)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();
                int totalUnits = department.Items.Select(a => a.Amount).Sum();
                contentReplacer.Add("VRDepartment", department.DepartmentName.ToString());
                contentReplacer.Add("VRNum", "1");
                contentReplacer.Add("VRTotalUnit", totalUnits.ToString());
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }

        public MemoryStream PrintDailyStoreItemsReport(List<DailyStoreItemsVM> storeItems, List<LastYearBalanceVM> lastYearBalaceList, String budget, PrintDocumentTypesEnum doc)
        {
            List<DailyReportPrintObjectVM> PrintObjectVMs = storeItems.GroupBy(b => b.Currency).Select(a => new DailyReportPrintObjectVM
            {
                Currency = a.Key,
                value = a.Select(x => new DailyStoreItemsVM
                {
                    Date = x.Date,
                    Code = x.Code,
                    Requester = x.Requester,
                    _CountStoreAddition = x._CountStoreAddition,
                    CountStoreAddition = x.CountStoreAddition,
                    CountStoreAdditionlist = x.CountStoreAdditionlist,
                    CountStoreAdditionOutgoinglist =  x.CountStoreAdditionOutgoinglist,
                    CountStoreOutgoing  =  x.CountStoreOutgoing,
                    Note = x.Note
                }).ToList()
            }).ToList();
            String storeName = null;
            List<int> tenants = storeItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());

            var PrintObjectList = PrintObjectVMs.Join(lastYearBalaceList,
                PrintObject => PrintObject.Currency,
                lastYearBalace=> lastYearBalace.Currency,
                (PrintObject, lastYearBalace)=>new {Value = PrintObject.value,Price = lastYearBalace.Price, Currency = PrintObject.Currency });
            List<Dictionary<String, string>> replacersDictionaryList = new List<Dictionary<string, string>>();
            foreach (var item in PrintObjectList)
            {
                Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();
                replacersDictionary.Add("VRDepName", budget);
                replacersDictionary.Add("VRStore", storeName);
                replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / dd "));
                replacersDictionary.Add("VRLastYearBalance", item.Price.ToString());
                decimal totlatAddition = item.Value.Select(a => a.CountStoreAddition).Sum();
                decimal totlatOutgoing = item.Value.Select(a => a.CountStoreOutgoing).Sum();
                replacersDictionary.Add("VRTotatalAfter", (totlatAddition + item.Price).ToString());
                replacersDictionary.Add("VRTotal", (totlatAddition + item.Price - totlatOutgoing).ToString());
                replacersDictionary.Add("VRCurrency", item.Currency);
                replacersDictionary.Add("VRFromDate", FinancialYearProvider.CurrentYearStartDate.ToString("yyyy / MM / dd "));
                replacersDictionary.Add("VRToDate", FinancialYearProvider.CurrentYearEndDate.ToString("yyyy / MM / dd "));
                replacersDictionaryList.Add(replacersDictionary);
            }
            List<ArrayList> tableReplacers = GenerateDailyStoreItemsReportTable(PrintObjectVMs);
            return _wordGenerator.PrintMultiplePageDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionaryList, fullTable: false,numberOfHeaderRows:2);
        }

        private List<ArrayList> GenerateDailyStoreItemsReportTable(List<DailyReportPrintObjectVM> storeItemsList)
        {
            List<ArrayList> tableReplacersList =new List<ArrayList>();
            foreach (var storeItems in storeItemsList)
            {
                ArrayList tableReplacers = new ArrayList();
                if (storeItems.value != null && storeItems.value.Count != 0)
                {
                    foreach (var item in storeItems.value)
                    {
                        tableReplacers.Add(new
                        {
                            Date = item.Date.ToString("d  MMM  yyyy"),
                            Code = item.Code,
                            ToOrFromStore = item.Requester,
                            CountStoreAddition = item._CountStoreAddition.Equals(0) ? "0" : item._CountStoreAddition.ToString(),
                            CountStoreOutgoing = item.CountStoreOutgoing,
                            Note = item.Note

                        });
                    }
                }
                tableReplacersList.Add(tableReplacers);
            }
           
            return tableReplacersList;
        }


        public MemoryStream PrintStoreItemsDistributionReport(List<StoreItemsDistributionPrintVM> storeItems, PrintDocumentTypesEnum doc, dynamic filters)
        {

            String storeName = null;
            List<int> tenants = storeItems.GroupBy(g => g.TenantId).Select(s => s.Key).ToList();
            if (tenants.Count == 1)
                storeName = _storeBussiness.GetStoreName(tenants.FirstOrDefault());
            List<StoreItemsDistributionVM> groupedStoreItems = storeItems
                        .GroupBy(g => new
                        {
                            BaseItemId = g.Id,
                            BaseItemName = g.BaseItemName,
                            BookNumber = g.BookNumber,
                            PageNumber = g.PageNumber,
                            AllQuantity = g.AllQuantity,
                            PaidQuantity = g.paidQuantity,
                            StoreQuantity = g.storeQuantity,
                        })
                        .Select(g => new StoreItemsDistributionVM()
                        {
                            BaseItemName = g.Key.BaseItemName,
                            BookNumber = g.Key.BookNumber,
                            PageNumber = g.Key.PageNumber,
                            AllQuantity = g.Key.AllQuantity,
                            paidQuantity = g.Key.PaidQuantity,
                            storeQuantity = g.Key.StoreQuantity,
                            DistributionDetails = g.Select(s => new DistributionDetailsVM()
                            {
                                DepartmentName = s.DepartmentName,
                                Amount = s.Amount,
                                Notes = s.Notes
                            }).ToList()

                        }).OrderBy(o => o.BookNumber).ToList();
            List<ArrayList> tableReplacers = GenerateStoreItemsDistributionTable(groupedStoreItems);
            Dictionary<String, string> footerDictionary = new Dictionary<string, string>();
            footerDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));

            Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRStoreName", storeName);
            String budgetLabel = String.Empty;
            String budget = String.Empty;
            if (filters != null && filters["Budget"]?.Value != null)
            {
                budgetLabel = _Localizer["BudgetName"];
                budget = filters["Budget"]?.Value;
            }
            replacersDictionary.Add("VRBNL", budgetLabel);
            replacersDictionary.Add("VRBN", budget);
            String contractLabel = String.Empty;
            String contract = String.Empty;
            if (filters != null && filters["ContractNumber"]?.Value != null)
            {
                contractLabel = _Localizer["ContractNum"];
                contract = filters["ContractNumber"]?.Value;
            }
            replacersDictionary.Add("VRCNL", contractLabel);
            replacersDictionary.Add("VRCN", contract);

            List<Dictionary<String, string>> departmentDictionary = GenerateStoreItemsDistributionRepeatedContent(groupedStoreItems);
            return _wordGenerator.PrintSectionRepeaterDocument(doc.ToString() + ".docx",
                sectionReplacer: tableReplacers, contentReplacers: replacersDictionary,
                contentRepeaterReplacers: departmentDictionary,
                tableIndex: 1,
                footerReplacers: footerDictionary);
        }

        private List<ArrayList> GenerateStoreItemsDistributionTable(List<StoreItemsDistributionVM> storeItems)
        {
            List<ArrayList> tableReplacers = new List<ArrayList>();
            if (storeItems != null && storeItems.Count != 0)
            {
                foreach (var item in storeItems)
                {
                    ArrayList lst = new ArrayList();
                    for (int i = 0; i < item.DistributionDetails.Count; i++)
                    {
                        lst.Add(new
                        {
                            Serial = (i + 1).ToString(),
                            Department = item.DistributionDetails[i].DepartmentName,
                            Amount = item.DistributionDetails[i].Amount.ToString(),
                            Notes = item.DistributionDetails[i].Notes
                        }); ;
                    }

                    tableReplacers.Add(lst);
                }
            }
            return tableReplacers;
        }
        private List<Dictionary<string, string>> GenerateStoreItemsDistributionRepeatedContent(List<StoreItemsDistributionVM> storeItems)
        {
            List<Dictionary<string, string>> Contentreplacers = new List<Dictionary<string, string>>();
            foreach (var storeItem in storeItems)
            {
                Dictionary<string, string> contentReplacer = new Dictionary<string, string>();

                contentReplacer.Add("VRBookNum", storeItem.BookNumber.ToString());
                contentReplacer.Add("VRPage", storeItem.PageNumber.ToString());
                contentReplacer.Add("VRStoreItem", storeItem.BaseItemName);
                contentReplacer.Add("VRTotal", storeItem.AllQuantity.ToString());
                contentReplacer.Add("VRPaid", storeItem.paidQuantity.ToString());
                contentReplacer.Add("VRStore", storeItem.storeQuantity.ToString());
                Contentreplacers.Add(contentReplacer);
            }
            return Contentreplacers;
        }

        #endregion
        #region Refund
        public MemoryStream printRefundFormNo9Document(Form8VM deduction, List<DedcuctionStoreItemVM> baseItemStoreItemVMs)
        {
            List<Form9PrintObjectVM> PrintObjectVMs = baseItemStoreItemVMs.GroupBy(b => b.Currency).Select(a => new Form9PrintObjectVM
            {
                Currency = a.Key,
                value = a.Select(x => new DedcuctionStoreItemVM
                {
                    ContractNumber=x.ContractNumber,
                    PageNumber=x.PageNumber,
                    ItemStatus = x.ItemStatus,
                    BaseItemName = x.BaseItemName,
                    BaseItemId = x.BaseItemId,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.UnitPrice * x.Quantity,
                    Notes = x.Notes,
                    Date = x.Date
                }).ToList()
            }).ToList();
            foreach (var item in PrintObjectVMs)
            {
                deduction.Currencies.Add(item.Currency);
            }
            List<ArrayList> tableReplacers = GenerateTableRefundFormNo9Replacers(PrintObjectVMs);
            List<Dictionary<string, string>> contentReplacers = GenerateContentRefundFormNo9Replacers(deduction);
            return _wordGenerator.PrintMultiplePageDocument(PrintDocumentTypesEnum.FormNo9.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: contentReplacers);
        }
        public List<ArrayList> GenerateTableRefundFormNo9Replacers(List<Form9PrintObjectVM> dedcuctionStoreItemVMs)
        {
            List<ArrayList> tableReplacersList = new List<ArrayList>();
            foreach (var dedcuctionStoreItemVM in dedcuctionStoreItemVMs)
            {
            ArrayList tableReplacers = new ArrayList();
                if (dedcuctionStoreItemVM.value != null && dedcuctionStoreItemVM.value.Count != 0)
                {
                    foreach (var item in dedcuctionStoreItemVM.value)
                    {
                        tableReplacers.Add(new
                        {
                            ContractNumber=item.ContractNumber,
                            PageNumber=item.PageNumber,
                            Name = item.BaseItemName,
                            Unit = item.UnitName,
                            blank = "",
                            neededQuantity = item.Quantity,
                            blank2 = "",
                            price = item.UnitPrice,
                            blank3 = "",
                            value = item.TotalPrice,
                            itemCondition = item.ItemStatus,
                            exchangeDate = item.Date.ToString("yyyy / MM / dd "),
                            Notes = item.Notes
                        });
                    }
                }
                tableReplacersList.Add(tableReplacers);
            }
            return tableReplacersList;
        }
        public List<Dictionary<string, string>> GenerateContentRefundFormNo9Replacers(Form8VM deductionData)
        {
            List<Dictionary<string, string>> ContentreplacersList = new List<Dictionary<string, string>>();
            if (deductionData!=null)
            {
                for (int i = 0; i < deductionData.Currencies.Count; i++)
                {
                    Dictionary<string, string> Contentreplacers = new Dictionary<string, string>();
                    if (deductionData != null)
                    {
                        Contentreplacers.Add("VRBudget", "");
                        Contentreplacers.Add("VRStore", deductionData.StoreName);
                        Contentreplacers.Add("VRStoreAdmin", deductionData.StoreKeeper);
                        Contentreplacers.Add("VRDate", deductionData.ExchangeDate);
                        Contentreplacers.Add("VRCurrency", deductionData.Currencies[i]);
                    }
                    ContentreplacersList.Add(Contentreplacers);
                }
            }
            return ContentreplacersList;
        }
        #endregion
        #region Lists

        public MemoryStream PrintExaminationCommitteList(List<ExaminationCommitte> examinations, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");

            List<Dictionary<String, String>> repeatedReplacers = PrepareExaminationCommitteReplacers(filters);
            ArrayList tableReplacers = PrepareExaminationCommitteList(examinations);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers,
                contentReplacers: replacersDictionary, repetitiveSectionReplacer: repeatedReplacers, fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareExaminationCommitteReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExaminationNum"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String examinationDate = String.Empty;
                String[] examinationDates = (filters["Date"]?.Value as String).Split('-');
                examinationDate = Convert.ToDateTime(examinationDates[0]).ToString("yyyy / MM / d ");
                if (examinationDates.Length > 1)
                    examinationDate += $" - {Convert.ToDateTime(examinationDates[1]).ToString("yyyy / MM / d ")}";
                values.Add("VRKey", _Localizer["ExaminationDate"]);
                values.Add("VRValue", examinationDate);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["ExaminationStatusId"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExaminationStatus"]);
                values.Add("VRValue", filters["ExaminationStatusId"]?.Value);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["ContractDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String contractDate = String.Empty;
                if (filters != null && filters["ContractDate"]?.Value != null)
                {
                    String[] contractDates = (filters["ContractDate"]?.Value as String).Split('-');
                    contractDate = Convert.ToDateTime(contractDates[0]).ToString("yyyy / MM / d ");
                    if (contractDates.Length > 1)
                        contractDate += $" - {Convert.ToDateTime(contractDates[1]).ToString("yyyy / MM / d ")}";
                }
                values.Add("VRKey", _Localizer["ContractDate"]);
                values.Add("VRValue", contractDate);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["ContractNumber"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ContractNum"]);
                values.Add("VRValue", filters["ContractNumber"]?.Value);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["SupplierId"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["SuppliedTo"]);
                values.Add("VRValue", filters["SupplierId"]?.Value);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;
        }
        private ArrayList PrepareExaminationCommitteList(List<ExaminationCommitte> examinations)
        {
            ArrayList tableReplacers = new ArrayList();
            if (examinations != null && examinations.Count != 0)
            {
                foreach (var item in examinations)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.Date.ToString("d MMM yyyy"),
                        Status = item.ExaminationStatus?.Name
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintAdditionList(List<AdditionListVM> addition, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");

            List<Dictionary<String, String>> repeatedReplacers = PrepareAdditionReplacers(filters);
            ArrayList tableReplacers = PrepareAdditionList(addition);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);

        }
        List<Dictionary<String, String>> PrepareAdditionReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["AdditionNumber"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["AdditionNumber"]?.Value != null)
            {
                replacersDictionary.Add(PrepareSubtractionNumber(Convert.ToString(filters["AdditionNumber"]?.Value)));

            }
            if (filters != null && filters["AdditionDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["AdditionDate"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["AdditionDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }

            return replacersDictionary;

        }
        private ArrayList PrepareAdditionList(List<AdditionListVM> addition)
        {
            ArrayList tableReplacers = new ArrayList();
            if (addition != null && addition.Count != 0)
            {
                foreach (var item in addition)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.Date.ToString("d  MMM  yyyy"),
                        AdditionNumber = item.AdditionNumber
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintRefundOrdersList(List<RefundOrderVM> refundOrders, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");

            List<Dictionary<String, String>> repeatedReplacers = PrepareRefundOrdersReplacers(filters);
            ArrayList tableReplacers = PrepareRefundOrdersList(refundOrders);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareRefundOrdersReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["RefundNum"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["RefundDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["RefundDate"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["RefundDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }

            return replacersDictionary;
        }
        private ArrayList PrepareRefundOrdersList(List<RefundOrderVM> refundOrders)
        {
            ArrayList tableReplacers = new ArrayList();
            if (refundOrders != null && refundOrders.Count != 0)
            {
                foreach (var item in refundOrders)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.Date.ToString("d  MMM  yyyy"),
                        RefundOrderStatusName = item.RefundOrderStatus.Name
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintExchangeOrdersList(List<ExchangeOrder> exchangeOrders, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");

            List<Dictionary<String, String>> repeatedReplacers = PrepareExchangeOrdersReplacers(filters);

            ArrayList tableReplacers = PrepareExchangeOrdersList(exchangeOrders);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareExchangeOrdersReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExchangeOrderNumber"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["Date"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["ExchangeOrderDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["ExchangeOrderStatus"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExchangeOrderStatus"]);
                values.Add("VRValue", filters["ExchangeOrderStatus"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["ForEmployee"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExchangeOrderTo"]);
                values.Add("VRValue", filters["ForEmployee"]?.Value);
                replacersDictionary.Add(values);

            }
            return replacersDictionary;
        }
        private ArrayList PrepareExchangeOrdersList(List<ExchangeOrder> exchangeOrders)
        {
            ArrayList tableReplacers = new ArrayList();
            if (exchangeOrders != null && exchangeOrders.Count != 0)
            {
                foreach (var item in exchangeOrders)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.Date.ToString("d  MMM  yyyy"),
                        ExchangeOrderStatusName = item.ExchangeOrderStatus.Name,
                        ForEmployeeName = item.ForEmployee.Name,
                    });
                }
            }
            return tableReplacers;
        }


        public MemoryStream PrintExecutionOrdersList(List<ExecutionListVM> executionOrders, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");

            List<Dictionary<String, String>> repeatedReplacers = PrepareExecutionOrdersReplacers(filters);

            ArrayList tableReplacers = PrepareExecutionOrdersList(executionOrders);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareExecutionOrdersReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExecutionOrderNumber"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["SubtractionNumber"]?.Value != null)
            {
                replacersDictionary.Add(PrepareSubtractionNumber(filters["SubtractionNumber"]?.Value));
            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["Date"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["ExecutionOrderDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["ExecutionOrderStatus"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExecutionOrderStatus"]);
                values.Add("VRValue", filters["ExecutionOrderStatus"]?.Value);
                replacersDictionary.Add(values);

            }
            return replacersDictionary;
        }
        private ArrayList PrepareExecutionOrdersList(List<ExecutionListVM> executionOrders)
        {
            ArrayList tableReplacers = new ArrayList();
            if (executionOrders != null && executionOrders.Count != 0)
            {
                foreach (var item in executionOrders)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.RequestDate.ToString("yyyy / MM / dd"),
                        SubtractionNumber =Convert.ToString(item.SubtractionNum),
                        ExchangeOrderStatusName = item.ExchangeOrderStatusName,
                    });;
                }
            }
            return tableReplacers;
        }



        public MemoryStream PrintInvoicesList(List<Invoice> invoices, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));

            List<Dictionary<String, String>> repeatedReplacers = PrepareInvoicesReplacers(filters);
            ArrayList tableReplacers = PrepareInvoicesList(invoices);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareInvoicesReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["InvoiceNum"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["ExchangeOrderCode"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ExchangeOrderNumber"]);
                values.Add("VRValue", filters["ExchangeOrderCode"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["Date"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["InvoiceDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }

            if (filters != null && filters["ReceivedEmployee"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ReceivedEmployee"]);
                values.Add("VRValue", filters["ReceivedEmployee"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["ReceivedEmployeeDepartment"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ReceivedDepartment"]);
                values.Add("VRValue", filters["ReceivedEmployeeDepartment"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["BaseItemCode"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemCode"]);
                values.Add("VRValue", filters["BaseItemCode"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["BaseItemBarCode"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["BarCode"]);
                values.Add("VRValue", filters["BaseItemBarCode"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["TechDepartment"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["BaseItemDepartment"]);
                values.Add("VRValue", filters["TechDepartment"]?.Value);
                replacersDictionary.Add(values);

            }
            return replacersDictionary;

        }
        private ArrayList PrepareInvoicesList(List<Invoice> invoices)
        {
            ArrayList tableReplacers = new ArrayList();
            if (invoices != null && invoices.Count != 0)
            {
                foreach (var item in invoices)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        InvoiceDate = item.Date.ToString("d  MMM  yyyy"),
                        ReceivedEmployee = item.ReceivedEmployee?.Name,
                        Department = item.Department?.Name,
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintTransformationsList(List<Transformation> transformations, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");


            List<Dictionary<String, String>> repeatedReplacers = PrepareTransformationsReplacers(filters);
            ArrayList tableReplacers = PrepareTransformationsList(transformations);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);


        }
        private Dictionary<String, String> PrepareSubtractionNumber(string filterValue) {
            Dictionary<String, String> values = new Dictionary<String, String>();
            values.Add("VRKey", _Localizer["SubtractionNumber"]);
            values.Add("VRValue", filterValue);
            return values;
        }
        private List<Dictionary<String, String>> PrepareTransformationsReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["TransformationNumber"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["SubtractionNumber"]?.Value != null)
            {
                replacersDictionary.Add(PrepareSubtractionNumber(filters["SubtractionNumber"]?.Value));
            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["Date"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["TransformationRequestDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;

        }
        private ArrayList PrepareTransformationsList(List<Transformation> transformations)
        {
            ArrayList tableReplacers = new ArrayList();
            if (transformations != null && transformations.Count != 0)
            {
            Subtraction subtraction = new Subtraction();
                foreach (var item in transformations)
                {
                    subtraction = item.Subtraction.FirstOrDefault();
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        InvoiceDate = subtraction != null ? subtraction.RequestDate.ToString("d  MMM  yyyy") : "",
                        TransformationStatus = item.TransformationStatus?.Name,
                        FromStoreName = _storeBussiness.GetStoreName(item.FromStoreId),
                        ToStoreName = _storeBussiness.GetStoreName(item.ToStoreId),
                        subtractionNumber = subtraction != null ? Convert.ToString(subtraction.SubtractionNumber) : ""
                        // //add subtraction 
                    }); ;
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintRobbingOrdersList(List<RobbingOrder> robbingOrders, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");
            List<Dictionary<String, String>> repeatedReplacers = PrepareRobbingOrdersReplacers(filters);
            ArrayList tableReplacers = PrepareRobbingOrdersList(robbingOrders);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers,
                contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        private List<Dictionary<String, String>> PrepareRobbingOrdersReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["RobbingNum"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["SubtractionNumber"]?.Value != null)
            {
                replacersDictionary.Add(PrepareSubtractionNumber(filters["SubtractionNumber"]?.Value));
            }
            if (filters != null && filters["RobbingDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["RobbingDate"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["RobbingDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;
        }
        private ArrayList PrepareRobbingOrdersList(List<RobbingOrder> robbingOrders)
        {
            ArrayList tableReplacers = new ArrayList();
            Subtraction subtraction = new Subtraction();
            if (robbingOrders != null && robbingOrders.Count != 0)
            {
                foreach (var item in robbingOrders)
                {
                    subtraction = item.Subtraction.FirstOrDefault();
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        InvoiceDate = subtraction!=null? subtraction.RequestDate.ToString("d  MMM  yyyy"):"",
                        RobbingOrderStatus = item.RobbingOrderStatus?.Name,
                        FromStoreName = _storeBussiness.GetStoreName(item.FromStoreId),
                        ToStoreName = _storeBussiness.GetStoreName(item.ToStoreId) ,
                        SubtractionNuumber = subtraction!=null?Convert.ToString(subtraction.SubtractionNumber):"",
                    });;
                }
            }
            return tableReplacers;
        }


        public MemoryStream PrintStockTakingList(List<StockTaking> stockTaking, dynamic filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, string> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", filters != null && filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", filters != null && filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(filters["TenantId"]?.Value)) : "");
            List<Dictionary<String, String>> repeatedReplacers = PrepareStockTakingReplacers(filters);
            ArrayList tableReplacers = PrepareStockTakingList(stockTaking);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers,
                contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);


        }
        private List<Dictionary<String, String>> PrepareStockTakingReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["StockTakingCode"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["Date"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["Date"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["StockTakingDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;
        }
        private ArrayList PrepareStockTakingList(List<StockTaking> stockTaking)
        {
            ArrayList tableReplacers = new ArrayList();
            if (stockTaking != null && stockTaking.Count != 0)
            {
                foreach (var item in stockTaking)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Code,
                        Date = item.CreationDate.ToString("d  MMM  yyyy")
                    });
                }
            }
            return tableReplacers;
        }
        #endregion
        #region Lookups
        public MemoryStream PrintDepartmentsList(List<Department> departments, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDepartment", Filters != null ? Filters["DepartmentName"].Value : "");
            replacersDictionary.Add("VRDepartmentLabel", Filters != null ? _Localizer["Department"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d"));
            ArrayList tableReplacers = PrepareDepartmentsList(departments);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareDepartmentsList(List<Department> departments)
        {
            ArrayList tableReplacers = new ArrayList();
            if (departments != null && departments.Count != 0)
            {
                foreach (var item in departments)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintLocationsList(List<Location> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRLocation", Filters != null ? Filters["LocationName"].Value : "");
            replacersDictionary.Add("VRLocationLabel", Filters != null ? _Localizer["Location"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareLocationsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareLocationsList(List<Location> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintBaseItemsList(List<BaseItem> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<string, string>>();
            if (Filters != null && Filters["BaseItemCode"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["BaseItemCode"]);
                values.Add("VRValue", Convert.ToString(Filters["BaseItemCode"]?.Value));
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["BaseItemName"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemName"]);
                values.Add("VRValue", Filters["BaseItemName"]?.Value);
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["Consumed"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemType"]);
                values.Add("VRValue", Filters["Consumed"]?.Value ? _Localizer["Consumed"] : _Localizer["NotConsumed"]);
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["ItemCategory"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemCategory"]);
                values.Add("VRValue", Filters["ItemCategory"]?.Value);
                replacersDictionary.Add(values);
            }
            Dictionary<String, String> date = new Dictionary<String, String>();
            date.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareBaseItemsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: date, repetitiveSectionReplacer: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareBaseItemsList(List<BaseItem> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Limit = item.WarningLevel,
                        Unit = item.DefaultUnit.Name,
                        Category = item.ItemCategory.Name,
                        Type = item.Consumed ? _Localizer["Consumed"] : _Localizer["NotConsumed"],
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintTechnicalDepartmentsList(List<TechnicalDepartment> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRTechnicalDepartmentName", Filters != null ? Filters["TechnicalDepartmentName"].Value : "");
            replacersDictionary.Add("VRTechnicalDepartmentLabel", Filters != null ? _Localizer["TechnicalDepartment"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareTechnicalDepartmentsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareTechnicalDepartmentsList(List<TechnicalDepartment> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintExternalEntitysList(List<ExternalEntity> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRExternalEntityName", Filters != null ? Filters["ExternalEntityName"].Value : "");
            replacersDictionary.Add("VRExternalEntityLabel", Filters != null ? _Localizer["ExternalEntity"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareExternalEntitiesList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareExternalEntitiesList(List<ExternalEntity> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintBooksList(List<Book> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<string, string>>();
            if (Filters != null && Filters["storeName"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["StoreName"]);
                values.Add("VRValue", Filters["storeName"]?.Value);
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["Consumed"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["ItemType"]);
                values.Add("VRValue", Filters["Consumed"]?.Value ? _Localizer["Consumed"] : _Localizer["NotConsumed"]);
                replacersDictionary.Add(values);
            }
            Dictionary<String, String> date = new Dictionary<String, String>();
            date.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareBooksList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers,
                contentReplacers: date, repetitiveSectionReplacer: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareBooksList(List<Book> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.BookNumber,
                        PagesCount = item.PageCount,
                        //StoreName = _storeBussiness.GetAllStoreName(item.StoreId),
                        Type = item.Consumed ? _Localizer["Consumed"] : _Localizer["NotConsumed"],
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintStoresList(List<Store> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<string, string>>();
            if (Filters != null && Filters["stoerName"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["StoreName"]);
                values.Add("VRValue", Filters["stoerName"]?.Value);
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["storeType"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["StoreType"]);
                values.Add("VRValue", Filters["storeType"]?.Value);
                replacersDictionary.Add(values);
            }
            Dictionary<String, String> date = new Dictionary<String, String>();
            ArrayList tableReplacers = PrepareStoresList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: date, repetitiveSectionReplacer: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareStoresList(List<Store> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        StoreName = _storeBussiness.GetAllStoreName(item.Id),
                        StoreType = item.StoreType.Name,
                        AdminName = item.AdminId,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintSuppliersList(List<Supplier> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRSupplierName", Filters != null ? Filters["SupplierName"].Value : "");
            replacersDictionary.Add("VRSupplierLabel", Filters != null ? _Localizer["Supplier"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareSuppliersList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareSuppliersList(List<Supplier> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintEmployeessList(List<Employees> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<string, string>>();
            if (Filters != null && Filters["Name"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["EmployeeName"]);
                values.Add("VRValue", Convert.ToString(Filters["EmployeeName"]?.Value));
                replacersDictionary.Add(values);
            }
            if (Filters != null && Filters["Department"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["Department"]);
                values.Add("VRValue", Filters["Department"]?.Value);
                replacersDictionary.Add(values);
            }
            Dictionary<String, String> date = new Dictionary<String, String>();
            date.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareEmployeesList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: date, repetitiveSectionReplacer: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareEmployeesList(List<Employees> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.CardCode,
                        Name = item.Name,
                        Department = item.Department != null ? item.Department.Name : "",
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintUnitsList(List<Unit> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRUnit", Filters != null ? Filters["UnitName"].Value : "");
            replacersDictionary.Add("VRUnitLabel", Filters != null ? _Localizer["Unit"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareUnitsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareUnitsList(List<Unit> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintItemCategorysList(List<ItemCategory> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRItemCategoryName", Filters != null ? Filters["ItemCategoryName"].Value : "");
            replacersDictionary.Add("VRItemCategoryLabel", Filters != null ? _Localizer["ItemCategory"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareItemCategoriesList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareItemCategoriesList(List<ItemCategory> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintJobTitlesList(List<JobTitle> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRJobTitleName", Filters != null ? Filters["JobTitleName"].Value : "");
            replacersDictionary.Add("VRJobTitleLabel", Filters != null ? _Localizer["JobTitle"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareJobTitlesList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareJobTitlesList(List<JobTitle> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Code = item.Id,
                        Name = item.Name,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintRemainsList(List<Remains> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRRemains", Filters != null ? Filters["Name"].Value : "");
            replacersDictionary.Add("VRRemainLabel", Filters != null ? _Localizer["Remains"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareRemainsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareRemainsList(List<Remains> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.IsActive == true ? _Localizer["Active"] : _Localizer["NotActive"],
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintRemainsInquiriesList(List<RemainsDetails> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRRemains", Filters != null ? Filters["Name"].Value : "");
            replacersDictionary.Add("VRRemainLabel", Filters != null ? _Localizer["Remains"] : "");
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            ArrayList tableReplacers = PrepareRemainsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx", tabledata: tableReplacers, contentReplacers: replacersDictionary, fullTable: false);
        }
        private ArrayList PrepareRemainsList(List<RemainsDetails> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Name = item.Remains.Name,
                        book = item.BookId,
                        page = item.BookPageNumber,
                        unit = item.Unit.Name,
                        quantity = item.Quantity,
                        price = item.Price,
                        robbingName = item.RobbingName,
                    });
                }
            }
            return tableReplacers;
        }
        public MemoryStream PrintDelegationsList(List<Delegation> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", Filters != null && Filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", Filters != null && Filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(Filters["TenantId"]?.Value)) : "");
            List<Dictionary<String, String>> repeatedReplacers = PrepareDelegationReplacers(Filters);
            ArrayList tableReplacers = PrepareDelegationsList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        List<Dictionary<String, String>> PrepareDelegationReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["UserName"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["UserName"]);
                values.Add("VRValue", filters["UserName"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["DelegationDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["DelegationDate"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["DelegationDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;

        }
        private ArrayList PrepareDelegationsList(List<Delegation> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Name = item.UserName,
                        DateFrom = item.DateFrom.ToString("yyyy / MM / d "),
                        DateTo= item.DateTo.ToString("yyyy / MM / d "),
                        TimeFrom = item.TimeFrom.ToString(@"hh\:mm"),
                        TimenTo = item.TimeTo.ToString(@"hh\:mm"),
                        Store = concateStoreNames(item.DelegationStore)
                    });
                }
            }
            return tableReplacers;
        }

        private string concateStoreNames(ICollection<DelegationStore> DelegationStores)
        {
            List<string> names = new List<string>();
            var StoreNames = "";
            foreach (var store in DelegationStores)
            {
                names.Add(_storeBussiness.GetStoreName(store.StoreId));
            }
            StoreNames = string.Join("-", names);
            return StoreNames;
        }
        public MemoryStream PrintDelegationsTrackList(List<DelegationTrack> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            List<Dictionary<String, String>> repeatedReplacers = PrepareDelegationsTrackReplacers(Filters);
            ArrayList tableReplacers = PrepareDelegationsTrackList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        List<Dictionary<String, String>> PrepareDelegationsTrackReplacers(dynamic filters)
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["DelegationTrackDate"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                String date = String.Empty;
                String[] dates = (filters["DelegationTrackDate"]?.Value as String).Split('-');
                date = Convert.ToDateTime(dates[0]).ToString("yyyy / MM / d ");
                if (dates.Length > 1)
                    date += $" - {Convert.ToDateTime(dates[1]).ToString("yyyy / MM / d ")} ";
                values.Add("VRKey", _Localizer["DelegationTrackDate"]);
                values.Add("VRValue", date);
                replacersDictionary.Add(values);
            }

            return replacersDictionary;

        }
        private ArrayList PrepareDelegationsTrackList(List<DelegationTrack> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                foreach (var item in lookupItems)
                {
                    tableReplacers.Add(new
                    {
                        Date = item.CreationDate.ToString("yyyy / MM / d "),
                        Time = item.CreationTime,
                        UserName = item.DelegationUserName,
                        Operation = item.Operation,
                        OperationNumber = item.OperationNum
                    });
                }
            }
            return tableReplacers;
        }

        public MemoryStream PrintDedcutionsList(List<Deduction> lookupItems, dynamic Filters, PrintDocumentTypesEnum doc)
        {
            Dictionary<String, String> replacersDictionary = new Dictionary<string, string>();
            replacersDictionary.Add("VRDate", DateTime.Now.Date.ToString("yyyy / MM / d "));
            replacersDictionary.Add("VRSNL", Filters != null && Filters["TenantId"]?.Value != null ? _Localizer["StoreName"] : "");
            replacersDictionary.Add("VRSN", Filters != null && Filters["TenantId"]?.Value != null ? _storeBussiness.GetStoreName(Convert.ToInt32(Filters["TenantId"]?.Value)) : "");
            List<Dictionary<String, String>> repeatedReplacers = PrepareDedcutionsListReplacers(Filters);
            ArrayList tableReplacers = PrepareDedcutionsListList(lookupItems);
            return _wordGenerator.PrintDocument(doc.ToString() + ".docx",
                tabledata: tableReplacers, contentReplacers: replacersDictionary,
                repetitiveSectionReplacer: repeatedReplacers,
                fullTable: false);
        }
        List<Dictionary<String, String>> PrepareDedcutionsListReplacers(dynamic filters) 
        {
            List<Dictionary<String, String>> replacersDictionary = new List<Dictionary<String, String>>();
            if (filters != null && filters["Code"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["Code"]);
                values.Add("VRValue", filters["Code"]?.Value);
                replacersDictionary.Add(values);

            }
            if (filters != null && filters["SubtractionNumber"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["SubtractionNumber"]);
                values.Add("VRValue", filters["SubtractionNumber"]?.Value);
                replacersDictionary.Add(values);
            }
            if (filters != null && filters["Budget"]?.Value != null)
            {
                Dictionary<String, String> values = new Dictionary<String, String>();
                values.Add("VRKey", _Localizer["BudgetName"]);
                values.Add("VRValue", filters["Budget"]?.Value);
                replacersDictionary.Add(values);
            }
            return replacersDictionary;

        }
        private ArrayList PrepareDedcutionsListList(List<Deduction> lookupItems)
        {
            ArrayList tableReplacers = new ArrayList();
            if (lookupItems != null && lookupItems.Count != 0)
            {
                Subtraction subtraction = new Subtraction();
                foreach (var item in lookupItems)
                {
                    subtraction = item.Subtraction.FirstOrDefault();
                    tableReplacers.Add(new
                    {
                        code =  item.Code,
                        date = subtraction?.RequestDate.ToString("yyyy / MM / d "),
                        requesterName = subtraction?.RequesterName,
                        budget = item.Budget.Name,
                        subttractionNumber = subtraction?.SubtractionNumber
                    });;
                }
            }
            return tableReplacers;
        }
        #endregion

    }
}
