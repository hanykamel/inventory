using Inventory.Data.Models.Inquiry;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.InquiryRequest.Handlers
{
    class InquiryBaseItemsHandler : IRequestHandler<InquiryBaseItemsCommand, InquiryBaseItems>
    {
        private readonly IStoreItemBussiness _iStoreItemBussiness;
        public InquiryBaseItemsHandler(
            IStoreItemBussiness iStoreItemBussiness)
        {

            _iStoreItemBussiness = iStoreItemBussiness;
        }



        public async Task<InquiryBaseItems> Handle(InquiryBaseItemsCommand request, CancellationToken cancellationToken)
        {

            InquiryBaseItems result = _iStoreItemBussiness.GetInquiryBaseItems(request);
            return Task.FromResult(result).Result;

        }
    }
}

