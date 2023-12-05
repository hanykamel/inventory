using Inventory.Service.Interfaces;
using Inventory.Data.Models.StoreItemVM;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Data.Models.PrintTemplateVM;
using System.Linq;
using System;
using Inventory.Service.Entities.RobbingOrderRequest.Commands;

namespace Inventory.Service.Entities.RobbingOrderRequest.Handlers
{
    class PrintRobbingOrderFormNo2Handler:IRequestHandler<PrintRobbingOrderFormNo2Command, MemoryStream>
    {
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IRobbingOrderStoreItemBussiness _robbingOrderStoreItemBussiness;
        private readonly IRobbingOrderRemainsBussiness _robbingOrderRemainsBussiness;
        private readonly IStoreBussiness _storeBussiness;


        public PrintRobbingOrderFormNo2Handler(IRobbingOrderBussiness robbingOrderBussiness,
            IWordBusiness wordBusiness,
            IRobbingOrderStoreItemBussiness robbingOrderStoreItemBussiness,
            IRobbingOrderRemainsBussiness robbingOrderRemainsBussiness,
            IStoreBussiness storeBussiness)
        {
            _robbingOrderBussiness = robbingOrderBussiness;
            _wordBusiness = wordBusiness;
            _robbingOrderStoreItemBussiness = robbingOrderStoreItemBussiness;
            _storeBussiness = storeBussiness;
            _robbingOrderRemainsBussiness = robbingOrderRemainsBussiness;
        }

        

        public Task<MemoryStream> Handle(PrintRobbingOrderFormNo2Command request, CancellationToken cancellationToken)
        {
            //get RobbingOrder
            RobbingOrder robbingOrder = _robbingOrderBussiness.GetById(request.RobbingOrderId);

            if (robbingOrder != null && robbingOrder.Subtraction != null && robbingOrder.Subtraction.Count() > 0)
            {
                var subtraction = robbingOrder.Subtraction.FirstOrDefault();
                //pobulate MainData object
                TransactionsVM MainData = new TransactionsVM();
                MainData.AdditionDocumentType = "";
                MainData.RequestDate = subtraction.RequestDate.ToString("yyyy/MM/dd");
                MainData.RequesterName = subtraction.RequesterName;
                MainData.Serial = subtraction.SubtractionNumber.ToString();
                MainData.StoreName = _storeBussiness.GetStoreName(robbingOrder.FromStoreId);
                MainData.BudgetName = robbingOrder.Budget.Name;
                MainData.CreationDate = subtraction.Date.ToString("yyyy/MM/dd");
                MainData.IsAddition = false;
                MainData.Currencies = new List<string>();
                List<Guid> Ids = new List<Guid>();
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = new List<BaseItemStoreItemVM>();
                //get storeitemsVm
                if (robbingOrder.RobbingOrderStoreItem!=null&& robbingOrder.RobbingOrderStoreItem.Count>0)
                {
                    Ids = robbingOrder.RobbingOrderStoreItem.Select(x => x.Id).ToList();
                    baseItemStoreItemVMs = _robbingOrderStoreItemBussiness.GetFormStoreItems(Ids);
                }
                else if (robbingOrder.RobbingOrderRemainsDetails != null && robbingOrder.RobbingOrderRemainsDetails.Count > 0)
                {
                    Ids = robbingOrder.RobbingOrderRemainsDetails.Select(x => x.Id).ToList();
                    baseItemStoreItemVMs = _robbingOrderRemainsBussiness.GetFormStoreItems(Ids);
                }
                //List<Form2PrintObjectVM> PrintObjectVMs = baseItemStoreItemVMs.GroupBy(b => b.Currency).Select(a => new Form2PrintObjectVM
                //{
                //    Currency = a.Key,
                //    value = a.Select(x => new BaseItemStoreItemVM
                //    {
                //        ItemStatus = x.ItemStatus,
                //        BaseItemDesc = x.BaseItemDesc,
                //        BaseItemName = x.BaseItemName,
                //        BaseItemId = x.BaseItemId,
                //        UnitName = x.UnitName,
                //        Quantity = x.Quantity,
                //        UnitPrice = x.UnitPrice,
                //        FullPrice = x.UnitPrice * x.Quantity,
                //        Notes = x.Notes,
                //        ExaminationReport = x.ExaminationReport
                //    }).ToList()
                //}).ToList();
                //foreach (var item in PrintObjectVMs)
                //{
                //    MainData.Currencies.Add(item.Currency);
                //}
                return Task.FromResult(_wordBusiness.printAdditionMultiDocument(MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
