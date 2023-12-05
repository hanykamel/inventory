using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
  public  class JobTitleBussiness: IJobTitleBussiness
    {
        readonly private IRepository<JobTitle, int> _JobTitleRepository;
        public JobTitleBussiness(IRepository<JobTitle, int> JobTitleRepository)
        {
            _JobTitleRepository = JobTitleRepository;
        }
        public async Task<JobTitle> AddNewJobTitle(JobTitle _JobTitle)
        {
            _JobTitleRepository.Add(_JobTitle);
            int added = await _JobTitleRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_JobTitle);
            else
                return await Task.FromResult<JobTitle>(null);
        }

        public IQueryable<JobTitle> GetAllJobTitle()
        {
            var JobTitleList = _JobTitleRepository.GetAll(true);
            return JobTitleList;
        }

        public IQueryable<JobTitle> GetActiveJobTitles()
        {
            var JobTitleList = _JobTitleRepository.GetAll();
            return JobTitleList;
        }
        public bool checkDeactivation(int JobTitleId)
        {
            var checkConnections =
                _JobTitleRepository.GetAll()
                .Where(x => x.Id == JobTitleId && x.CommitteeEmployee.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int JobTitleId,bool ActivationType)
        {
            if (ActivationType)
                _JobTitleRepository.Activate(new JobTitle() { Id = JobTitleId });
            else
                _JobTitleRepository.DeActivate(new JobTitle() { Id = JobTitleId });

            var added = await _JobTitleRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> UpdateJobTitle(JobTitle _JobTitle)
        {
            _JobTitleRepository.PartialUpdate(_JobTitle,j=>j.Name);
            int added = await _JobTitleRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public JobTitle ViewJobTitle(int JobTitleId)
        {
            var JobTitleEntity = _JobTitleRepository.Get(JobTitleId);
            return JobTitleEntity;
        }
    }
}