using Inventory.Data.Entities;
using Inventory.Service.Entities.JobTitleRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.JobTitleRequest.Handlers
{
    public class EditJobTitleHandler : IRequestHandler<EditJobTitleCommand, bool>
    {
        private readonly IJobTitleBussiness _IJobTitleBussiness;
        public EditJobTitleHandler(IJobTitleBussiness IJobTitleBussiness)
        {
            _IJobTitleBussiness = IJobTitleBussiness;
        }
        public Task<bool> Handle(EditJobTitleCommand request, CancellationToken cancellationToken)
        {
            return _IJobTitleBussiness.UpdateJobTitle(new JobTitle { Id = request.Id, Name = request.Name });
        }
    }
}
