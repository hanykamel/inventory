using Inventory.Data.Models.AttachmentVM;
using Inventory.Service.Entities.AttachmentRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.AttachmentRequest.Handlers
{
    public class DatabaseDeleteAttachmentHandler : IRequestHandler<DatabaseDeleteAttachmentCommand, bool>
    {

        private readonly IAttachmentBussiness _attachmentBussiness;
        public DatabaseDeleteAttachmentHandler(IAttachmentBussiness attachmentBussiness)
        {
            _attachmentBussiness = attachmentBussiness;

        }
        public Task<bool> Handle(DatabaseDeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_attachmentBussiness.DeleteFromDatabase(request.attachmentIds));
        }
    }
}
