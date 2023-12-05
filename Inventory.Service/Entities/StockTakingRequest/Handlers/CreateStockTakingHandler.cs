using AutoMapper;
using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Repository;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.StockTakingRequest.Handlers
{
    public class CreateStockTakingHandler : IRequestHandler<CreateStockTakingCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStockTakingBussiness _stockTakingBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IRobbedStoreItemBussiness _robbedStoreItemBussiness;

        private readonly IStoreBussiness _storeBussiness;

        public CreateStockTakingHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStockTakingBussiness stockTakingBussiness,
            IStringLocalizer<SharedResource> localizer,
            IRobbedStoreItemBussiness robbedStoreItemBussiness,
            IStoreBussiness storeBussiness
            )
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stockTakingBussiness = stockTakingBussiness;
            _Localizer = localizer;
            _robbedStoreItemBussiness = robbedStoreItemBussiness;
            _storeBussiness = storeBussiness;
        }
        public async Task<Guid> Handle(CreateStockTakingCommand request, CancellationToken cancellationToken)
        {
            //insert attachments
            var attachments = new List<StockTakingAttachment>();
            if (request.AdditionAttachment != null && request.AdditionAttachment.Count > 0)
                foreach (var attachment in request.AdditionAttachment)
                {
                    Attachment obj = _mapper.Map<Attachment>(attachment);
                    attachments.Add(new StockTakingAttachment() { Id = Guid.NewGuid(), Attachment = obj });
                }
            var newStockTakenModel =   _stockTakingBussiness.Create(request, attachments);
            if (_storeBussiness.CheckIsRobbingStore())
            {
                _robbedStoreItemBussiness.CreateStockTakingRobbing(request, newStockTakenModel);
            }
            var Result = await _stockTakingBussiness.SaveChange();
                return newStockTakenModel.Id;
        }

    }
}