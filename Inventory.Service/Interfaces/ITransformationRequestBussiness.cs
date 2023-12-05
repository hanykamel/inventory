using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface ITransformationRequestBussiness
    {
        IQueryable<Transformation> GetAllTransformation();
        IQueryable<Transformation> PrintTransformationsList();
        Transformation ViewTransformation(Guid TransformationId);
        string GetCode();
        string GetLastCode();
        IQueryable<Transformation> GetAllTransformationView();
        void UpdateTransformationStatus(Guid requestId);
        Task<bool> AddNewTransformation(Transformation transformation);
        Transformation GetById(Guid Id);
        bool CancelTransformation(Transformation transformation);
        List<Guid> GetTransformationStoreItem(Guid Id);
        Task<bool> Save();
    }
}
