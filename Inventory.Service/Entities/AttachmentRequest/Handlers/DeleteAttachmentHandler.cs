using Inventory.Data.Models.AttachmentVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AttachmentRequest.Handlers
{
    public class DeleteAttachmentHandler : IRequestHandler<DeleteAttachmentCommand, bool>
    {

        private readonly IAttachmentBussiness _attachmentBussiness;
        public DeleteAttachmentHandler(IAttachmentBussiness attachmentBussiness)
        {
            _attachmentBussiness = attachmentBussiness;

        }
        public Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_attachmentBussiness.Delete(request.attachmentIds , request.operation));
        }
    }
}
