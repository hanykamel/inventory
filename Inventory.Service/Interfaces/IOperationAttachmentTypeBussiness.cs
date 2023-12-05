using Inventory.Data.Entities;
using System.Linq;

namespace Inventory.Service.Interfaces
{
    public interface IOperationAttachmentTypeBussiness
    {
        IQueryable<OperationAttachmentType> getAll();
    }
}
