using Inventory.Data.Entities;
using Inventory.Service.Entities.NotebooksRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Inventory.Service.Entities.NotebooksRequest.Handlers
{
    public class ActivateNoteBookHandler : IRequestHandler<ActivateCommand, bool>
    {

        private readonly INoteBooksBussiness _INoteBookBussiness;
        public ActivateNoteBookHandler(INoteBooksBussiness INoteBookBussiness)
        {
            _INoteBookBussiness = INoteBookBussiness;
        }
        public Task<bool> Handle(ActivateCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_INoteBookBussiness.Activate(request.Id));
        }
    }
}
