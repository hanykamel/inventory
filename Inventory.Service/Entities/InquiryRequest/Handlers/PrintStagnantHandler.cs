using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using System.IO;
using AutoMapper;
using Inventory.Data.Models.Inquiry;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace Inventory.Service.Entities.InquiryRequest.Handlers
{
    public class PrintStagnantHandler : IRequestHandler<PrintStagnantCommand, MemoryStream>
    {
        private readonly IStagnantBussiness _iStagnantBussiness;
        private readonly IMapper _mapper;
        private readonly IWordBusiness _wordBusiness;
        private IStringLocalizer<SharedResource> _localizer;
        public PrintStagnantHandler(
            IStagnantBussiness iStagnantBussiness,
            IMapper mapper, IWordBusiness wordBusiness,
            IStringLocalizer<SharedResource> localizer)
        {
            _iStagnantBussiness = iStagnantBussiness;
            _mapper = mapper;
            _wordBusiness = wordBusiness;
            _localizer = localizer;
        }

        public Task<MemoryStream> Handle(PrintStagnantCommand request, CancellationToken cancellationToken)
        {
            StagnantModelVM StagnantModel = _mapper.Map<StagnantModelVM>(request);
            if (StagnantModel != null)
            {
                return Task.FromResult(_wordBusiness.PrintStagnantDocument(StagnantModel));
            }
            throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);
        }
    }
}

