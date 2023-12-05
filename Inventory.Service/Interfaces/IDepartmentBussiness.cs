using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
 public interface IDepartmentBussiness
    {

        Task<Department> AddNewDepartment(Department _Department);
        Task<bool> UpdateDepartment(Department _Department);
        Department ViewDepartment(int DepartmentId);
        Task<bool> Activate(int DepartmentId,bool ActivationType);
        IQueryable<Department> GetAllDepartment();
        IQueryable<Department> GetActiveDepartments();
        bool checkDeactivation(int DepartmentId);



    }
}
