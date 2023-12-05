
using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Subtraction:Entity<Guid>,IActive,ITenant,ICode
    {
      
        public Subtraction()
        {
        }

        public Guid? TransformationId { get; set; }
        public Guid? RobbingOrderId { get; set; }
        public Guid? ExecutionOrderId { get; set; }
        
        public Guid? DeductionId { get; set; }

        public int OperationId { get; set; }
        public string RequesterName { get; set; }
        public DateTime Date { get; set; }
        public DateTime RequestDate { get; set; }
        public int? SubtractionNumber { get; set; }

        
        public virtual Operation Operation { get; set; }
        public virtual Transformation Transformation { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
        public virtual RobbingOrder RobbingOrder { get; set; }
        public virtual Deduction Deduction { get; set; }


        public int TenantId { get; set; }
        public int Year { get; set; }
        public int Serial { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
    }
}
