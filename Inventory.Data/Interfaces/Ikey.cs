using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Interfaces
{
    public interface IKey<T>
    {
         T Id { get; set; }
    }
}
