using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AdditionRequest.Handlers
{
    public class GenerateStoreItemsBarCodeHandler : IRequestHandler<GenerateStoreItemsBarCodeCommand, List<StoreItemVM>>
    {
        private readonly IMediator _mediator;
        private readonly IAdditionBussiness additionBussiness;
        private readonly ICommiteeItemBussiness _commiteeItemBussiness;
        private readonly IExaminationBusiness _examinationBusiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly ILogger<CteateAdditionHandler> _logger;

        public IUnitOfWork UnitOfWork { get; }

        public GenerateStoreItemsBarCodeHandler(
            IMediator _mediator,
            IStoreItemBussiness storeItemBussiness,
            IUnitOfWork unitOfWork,
            ILogger<CteateAdditionHandler> logger
            )
        {
            this._mediator = _mediator;
            _storeItemBussiness = storeItemBussiness;
            UnitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<List<StoreItemVM>> Handle(GenerateStoreItemsBarCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await Task.FromResult(_storeItemBussiness.GenerateBarcodeImages(request.AdditionId));
            }
            catch (InventoryExceptionBase)
            {
                _logger.LogError("generating barcode image error");
                throw new InvalidException();
            }

        }

    }
}