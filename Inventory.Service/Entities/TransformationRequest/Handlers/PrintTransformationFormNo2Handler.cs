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
using Inventory.Service.Entities.TransformationRequest.Commands;

namespace Inventory.Service.Entities.TransformationRequest.Handlers
{
    class PrintTransformationFormNo2Handler:IRequestHandler<PrintTransformationFormNo2Command, MemoryStream>
    {
        private readonly ITransformationRequestBussiness _transformationRequestBussiness;
        private readonly IWordBusiness _wordBusiness;
        private readonly ITransformationStoreItemBussiness _transformationstoreItemBussiness;
        private readonly IStoreBussiness _storeBussiness;


        public PrintTransformationFormNo2Handler(ITransformationRequestBussiness transformationRequestBussiness,
            IWordBusiness wordBusiness,
            ITransformationStoreItemBussiness transformationstoreItemBussiness,
            IStoreBussiness storeBussiness)
        {
            _transformationRequestBussiness = transformationRequestBussiness;
            _wordBusiness = wordBusiness;
            _transformationstoreItemBussiness = transformationstoreItemBussiness;
            _storeBussiness = storeBussiness;
        }

        

        public Task<MemoryStream> Handle(PrintTransformationFormNo2Command request, CancellationToken cancellationToken)
        {
            //get RobbingOrder
            Transformation transformation = _transformationRequestBussiness.GetById(request.TransformationId);

            if (transformation != null&&transformation.Subtraction!=null && transformation.Subtraction.Count() > 0)
            {
                var subtraction = transformation.Subtraction.FirstOrDefault();
                //pobulate MainData object
                TransactionsVM MainData = new TransactionsVM();
                MainData.AdditionDocumentType = "";
                MainData.RequestDate = subtraction.RequestDate.ToString("yyyy/MM/dd");
                MainData.RequesterName = subtraction.RequesterName;
                MainData.Serial = subtraction.SubtractionNumber.ToString();
                MainData.StoreName = _storeBussiness.GetStoreName(transformation.FromStoreId);
                MainData.BudgetName = transformation.Budget.Name;
                MainData.CreationDate = subtraction.Date.ToString("yyyy/MM/dd");
                MainData.IsAddition = false;
                MainData.Currencies = new List<string>();
                //get storeitemsVm
                List<Guid> Ids = transformation.TransformationStoreItem.Select(x => x.Id).ToList();
                List<BaseItemStoreItemVM> baseItemStoreItemVMs = _transformationstoreItemBussiness.GetFormStoreItems(Ids);
                return Task.FromResult(_wordBusiness.printAdditionMultiDocument(MainData, baseItemStoreItemVMs));
            }
            return Task.FromResult(new MemoryStream());
        }

    }
}
