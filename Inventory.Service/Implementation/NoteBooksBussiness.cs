using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Service.Implementation
{
    public class NoteBooksBussiness : INoteBooksBussiness
    {
        readonly private IRepository<Book, long> _BookRepository;
        private readonly ITenantProvider _tenantProvider;
        public NoteBooksBussiness(IRepository<Book, long> BookRepository, ITenantProvider tenantProvider)
        {
            _BookRepository = BookRepository;
            _tenantProvider = tenantProvider;
        }
        public bool AddNewBook(Book _Book)
        {
            _BookRepository.Add(_Book);
            return true;
        }

        public IQueryable<Book> GetAllBook()
        {
            var BookList = _BookRepository.GetAll(x => x.TenantId == _tenantProvider.GetTenant(), true);
            return BookList;
        }

        public IQueryable<Book> GetActiveBooks()
        {
            var BookList = _BookRepository.GetAll();
            return BookList;
        }
        public bool Activate(long BookId)
        {
            var BookEntity = _BookRepository.Get(BookId);
            BookEntity.IsActive = BookEntity.IsActive == true ? false : true;
            return true;
        }

        public bool UpdateBook(Book _Book)
        {
            _BookRepository.Update(_Book);
            return true;

        }

        public Book ViewBook(long BookId)
        {
            var BookEntity = _BookRepository.Get(BookId);
            return BookEntity;
        }
    }
}