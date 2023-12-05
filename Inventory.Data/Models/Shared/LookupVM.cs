using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.Shared
{
    public class LookupVM<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
