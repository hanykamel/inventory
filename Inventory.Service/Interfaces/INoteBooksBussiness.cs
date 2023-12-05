using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Interfaces
{
  public  interface INoteBooksBussiness
    {

        bool AddNewBook(Book _Book);
        bool UpdateBook(Book _Book);
        Book ViewBook(long BookId);
        bool Activate(long BookId);
        IQueryable<Book> GetAllBook();
        IQueryable<Book> GetActiveBooks();
    }
}
