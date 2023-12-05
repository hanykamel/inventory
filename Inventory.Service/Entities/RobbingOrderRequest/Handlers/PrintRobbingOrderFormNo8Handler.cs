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
    class PrintRobbingOrderFormNo8Handler:IRequestHandler<PrintRobbingOrderFormNo8Command, MemoryStream>
    {
        private readonly IRobbingOrderBussiness _robbingOrderBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly IRobbingOrderStoreItemBussiness _robbingOrderStoreItemBussiness;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IRobbingOrderRemainsBussiness _robbingOrderRemainsBussiness;

        public PrintRobbingOrderFormNo8Handler(IRobbingOrderBussiness robbingOrderBussiness,
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

        

        public Task<MemoryStream> Handle(PrintRobbingOrderFormNo8Command request, CancellationToken cancellationToken)
        {
            //get RobbingOrder
            RobbingOrder robbingOrder = _robbingOrderBussiness.GetById(request.RobbingOrderId);

            if (robbingOrder != null)
            {
                //pobulate MainData object
                TransactionsVM MainData = new TransactionsVM();
                MainData.StoreName = _storeBussiness.GetStoreName(robbingOrder.FromStoreId);
                MainData.BudgetName = robbingOrder.Budget.Name;
                MainData.CreationDate = robbingOrder.Subtraction.FirstOrDefault().Date.ToString("yyyy/MM/dd");
                MainData.CustodyOwner = robbingOrder.FromStore.AdminId;
                MainData.Currencies = new List<string>();
                //get storeitemsVm
                List<Guid> Ids = new List<Guid>();
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = new List<BaseItemStoreItemVM>();
                //get storeitemsVm
                if (robbingOrder.RobbingOrderStoreItem != null && robbingOrder.RobbingOrderStoreItem.Count > 0)
                {
                    Ids = robbingOrder.RobbingOrderStoreItem.Select(x => x.Id).ToList();
                    baseItemStoreItemVMs = _robbingOrderStoreItemBussiness.GetFormStoreItems(Ids);
                }
                else if (robbingOrder.RobbingOrderRemainsDetails != null && robbingOrder.RobbingOrderRemainsDetails.Count > 0)
                {
                    Ids = robbingOrder.RobbingOrderRemainsDetails.Select(x => x.Id).ToList();
                    baseItemStoreItemVMs = _robbingOrderRemainsBussiness.GetFormStoreItems(Ids);
                }
                return Task.FromResult(_wordBusiness.printTransfromationDocument(MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
