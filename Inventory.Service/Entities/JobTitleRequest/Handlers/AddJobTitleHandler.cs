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
    class AddJobTitleHandler : IRequestHandler<AddJobTitleCommand, JobTitle>
    {
        private readonly IJobTitleBussiness _IJobTitleBussiness;
        public AddJobTitleHandler(IJobTitleBussiness IJobTitleBussiness)
        {
            _IJobTitleBussiness = IJobTitleBussiness;
        }
        public Task<JobTitle> Handle(AddJobTitleCommand request, CancellationToken cancellationToken)
        {
            JobTitle entity = new JobTitle();
            entity.Name = request.Name;
            return _IJobTitleBussiness.AddNewJobTitle(entity);
        }
    }
}