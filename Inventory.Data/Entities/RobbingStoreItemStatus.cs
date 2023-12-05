//using System;
//using System.Collections.Generic;

//namespace Inventory.Data.Entities
//{
//    public  class RobbingStoreItemStatus
//    {
//        public RobbingStoreItemStatus()
//        {
//            RobbingOrderStoreItem = new HashSet<RobbingOrderStoreItem>();
//            RobbedStoreItem = new HashSet<RobbedStoreItem>();
//            StoreItem = new HashSet<StoreItem>();

//        }

//        public int Id { get; set; }
//        public string Name { get; set; }

//        public virtual ICollection<RobbingOrderStoreItem> RobbingOrderStoreItem { get; set; }
//        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

//        public virtual ICollection<StoreItem> StoreItem { get; set; }


//    }
//}
