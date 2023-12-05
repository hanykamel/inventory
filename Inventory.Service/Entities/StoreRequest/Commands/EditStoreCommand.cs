using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.StoreRequest.Commands
{
    public class EditStoreCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Admin { get; set; }
        public int StoreType { get; set; }
        public int? RobbingBudgetId { get; set; }
        public int? TechnicalDepartmentId { get; set; }
    }
}
