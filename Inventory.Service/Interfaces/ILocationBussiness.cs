using System;
using System.Collections.Generic;
using System.Text;
using Inventory.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface ILocationBussiness
    {
        Task<Location> AddNewLocation(Location _Location);
        Task<bool> UpdateLocation(Location _Location);
        Location ViewLocation(int LocationId);
        Task<bool> Activate(int LocationId, bool ActivationType);
        IQueryable<Location> GetAllLocation();
        IQueryable<Location> GetActiveLocations();

        bool checkDeactivation(int LocationId);

    }
}
