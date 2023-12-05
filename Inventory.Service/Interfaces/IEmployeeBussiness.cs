using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface IEmployeeBussiness
    {
        Task<Employees> AddNewEmployees(Employees _Employees);
        Task<bool> UpdateEmployees(Employees _Employees);
        Employees ViewEmployees(int EmployeesId);
        Task<bool> Activate(int EmployeesId, bool ActivationType);
        bool checkDeactivation(int EmployeeId);

        IQueryable<Employees> GetAllEmployees();
        Employees IsCardCodeExist(String emp);
        IQueryable<Employees> GetActiveEmployees();
        Employees GetEmployees(string CardNum);
        IQueryable<Employees> GetAllPrintEmployees();

    }
}
