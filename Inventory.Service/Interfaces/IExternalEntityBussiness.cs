using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
   public interface IExternalEntityBussiness
    {
        Task<ExternalEntity> AddNewExternalEntity(ExternalEntity _ExternalEntity);
        Task<bool> UpdateExternalEntity(ExternalEntity _ExternalEntity);
        ExternalEntity ViewExternalEntity(int ExternalEntityId);
        Task<bool> Activate(int ExternalEntityId, bool ActivationType);
        IQueryable<ExternalEntity> GetAllExternalEntity();
        IQueryable<ExternalEntity> GetActiveExternalEntities();

        bool checkDeactivation(int ExternalEntityId);

    }
}
