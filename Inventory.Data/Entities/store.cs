using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Store:Entity<int>,IActive,ITenant
    {
        public Store()
        {
            Book = new HashSet<Book>();
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
            StoreItem = new HashSet<StoreItem>();
            FromTransformation = new HashSet<Transformation>();
            FromRobbingOrder = new HashSet<RobbingOrder>();
            ToTransformation = new HashSet<Transformation>();
            ToRobbingOrder = new HashSet<RobbingOrder>();
            DelegationStore = new HashSet<DelegationStore>();
        }


        public string AdminId { get; set; }
        public int StoreTypeId { get; set; }
        public int? TechnicalDepartmentId { get; set; }
        public int? RobbingBudgetId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Budget RobbingBudget { get; set; }
        public virtual StoreType StoreType { get; set; }
        public virtual TechnicalDepartment TechnicalDepartment { get; set; }
        public virtual ICollection<Book> Book { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<Transformation> FromTransformation { get; set; }
        public virtual ICollection<RobbingOrder> FromRobbingOrder { get; set; }
        public virtual ICollection<Transformation> ToTransformation { get; set; }
        public virtual ICollection<RobbingOrder> ToRobbingOrder { get; set; }
        public virtual ICollection<DelegationStore> DelegationStore { get; set; }
        public int TenantId { get; set; }
    }
}
