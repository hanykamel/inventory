using Inventory.Data.Models.AttachmentVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AttachmentRequest.Handlers
{
    public class UploadAttachmentHandler : IRequestHandler<UploadAttachmentCommand, AttachmentOutputVM>
    {

        private readonly IAttachmentBussiness _attachmentBussiness;
        public UploadAttachmentHandler(IAttachmentBussiness attachmentBussiness)
        {
            _attachmentBussiness = attachmentBussiness;

        }
        public Task<AttachmentOutputVM> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
        {
            AttachmentOutputVM result = _attachmentBussiness.Upload(request.myFormFiles, request.myFormTypes , request.operation);
            return Task.FromResult(result);
        }
    }
}
