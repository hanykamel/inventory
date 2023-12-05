using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
  public   interface ITechnicalDepartmentBussiness
    {

        Task<TechnicalDepartment> AddNewTechnicalDepartment(TechnicalDepartment _TechnicalDepartment);
        Task<bool> UpdateTechnicalDepartment(TechnicalDepartment _TechnicalDepartment);
        TechnicalDepartment ViewTechnicalDepartment(int TechnicalDepartmentId);
        Task<bool> Activate(int TechnicalDepartmentId, bool ActivationType);
        IQueryable<TechnicalDepartment> GetAllTechnicalDepartment();
        IQueryable<TechnicalDepartment> GetActiveTechnicalDepartments();
        bool checkDeactivation(int TechDepartmnetId);

    }
}
