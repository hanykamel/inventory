using AutoMapper;
using Inventory.Data.Enums;
using Inventory.Data.Models.Inquiry;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.InquiryRequest.Handlers
{
    class InquiryStoreItemsHandler : IRequestHandler<InquiryStoreItemsCommand, InquiryStoreItemsModel>
    {
        private readonly IStoreItemBussiness _iStoreItemBussiness;
        private readonly IMapper _mapper;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IRobbedStoreItemBussiness _robbedStoreItemBussiness;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public InquiryStoreItemsHandler(
            IStoreItemBussiness iStoreItemBussiness, IMapper mapper,
            IStoreBussiness storeBussiness,
              IRobbedStoreItemBussiness robbedStoreItemBussiness,
              IStringLocalizer<SharedResource> localizer)
        {

            _iStoreItemBussiness = iStoreItemBussiness;
            _storeBussiness= storeBussiness;
            _robbedStoreItemBussiness = robbedStoreItemBussiness;
            _mapper = mapper;
            _localizer = localizer;
        }



        public async Task<InquiryStoreItemsModel> Handle(InquiryStoreItemsCommand request, CancellationToken cancellationToken)
        {


            var InquiryStoreItems = _mapper.Map<InquiryStoreItemsRequest>(request);
            int count;
            var result = _iStoreItemBussiness.GetInquiryStoreItems(InquiryStoreItems, out count);
            InquiryStoreItemsModel model = new InquiryStoreItemsModel();
            model.Count = count;
            model.StoreItemsModel = new List<StoreItemsModel>();

            foreach (var item in result)
            {
                StoreItemsModel storeitem = new StoreItemsModel();
                storeitem.Id = item.Id;
                storeitem.itemCode = item.Code;
                storeitem.itemName = item.BaseItem.Name;
                storeitem.pageNumber = item.BookPageNumber;
                storeitem.contractNumber = item.Addition.ExaminationCommitte!=null?item.Addition.ExaminationCommitte.ContractNumber!=null? item.Addition.ExaminationCommitte.ContractNumber : "":"";
                storeitem.BaseItemId = item.BaseItemId;
                storeitem.storeItemAvalibleStatus = item.CurrentItemStatus.Name;
                storeitem.storeItemStatus = item.StoreItemStatus.Name;
                storeitem.storeItemStatusId = item.StoreItemStatusId;
                storeitem.CurrentItemStatusId = item.CurrentItemStatusId;
                storeitem.BudgetId = item.Addition?.BudgetId;
                storeitem.UnderDelete = item.UnderDelete;
                storeitem.unit = item.Unit.Name;
                model.StoreItemsModel.Add(storeitem);
            }


           
            //the robbed store item related only to kohna store and always available
            if (_storeBussiness.CheckIsRobbingStore()&& 
                (InquiryStoreItems.StoreItemAvailibilityStatusId==(int) ItemStatusEnum.Available
                || InquiryStoreItems.StoreItemAvailibilityStatusId ==null))
            {
                int RobbedStoreItemcount;
                var robbedStoreItem = _robbedStoreItemBussiness.GetRobbedStoreItemInquiry(InquiryStoreItems, out RobbedStoreItemcount);


                foreach (var item in robbedStoreItem)
                {
                    StoreItemsModel storeitem = new StoreItemsModel();
                    storeitem.Id = item.Id;
                    storeitem.itemCode = item.BaseItemId.ToString();
                    storeitem.itemName = item.BaseItem.Name;
                    storeitem.BaseItemId = item.BaseItemId;
                    storeitem.pageNumber = item.BookPageNumber;
                    storeitem.contractNumber = "";
                    storeitem.storeItemAvalibleStatus = _localizer["Available"];
                    storeitem.storeItemStatus = item.StoreItemStatus.Name;
                    storeitem.storeItemStatusId = item.StoreItemStatusId;
                    storeitem.CurrentItemStatusId = (int)ItemStatusEnum.Available;
                    storeitem.BudgetId = item.Addition?.BudgetId;
                    storeitem.unit = item.Unit.Name;
                    model.StoreItemsModel.Add(storeitem);
                }

                model.Count += RobbedStoreItemcount;

            }
            return Task.FromResult(model).Result;


        }
    }
}

