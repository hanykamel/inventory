using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IJobTitleBussiness
    {
        Task<JobTitle> AddNewJobTitle(JobTitle _JobTitle);
        Task<bool> UpdateJobTitle(JobTitle _JobTitle);
        JobTitle ViewJobTitle(int JobTitleId);
        Task<bool> Activate(int JobTitleId, bool ActivationType);
        bool checkDeactivation(int JobTitleId);

        IQueryable<JobTitle> GetAllJobTitle();
        IQueryable<JobTitle> GetActiveJobTitles();
    }
}
