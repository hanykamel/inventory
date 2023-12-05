using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
 public interface IBookBussiness
    {
        Task<Book> AddNewBook(Book _Book);
        Task<bool> UpdateBook(Book _Book);
        Book ViewBook(int BookId);
        Task<bool> Activate(int BookId, bool ActivationType);
        bool checkValidEdit(long BookId);
        IQueryable<Book> GetAllBook();
        IQueryable<Book> GetActiveBooks();
        IQueryable<Book> GetALLStoreBooks();
        IQueryable<Book> CheckBookExistane(Book _Book);
    }
}
