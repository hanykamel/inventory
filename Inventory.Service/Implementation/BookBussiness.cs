using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.CrossCutting.Tenant;

namespace Inventory.Service.Implementation
{
    public class BookBussiness : IBookBussiness
    {
        readonly private IRepository<Book, long> _BookRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        readonly private ITenantProvider _tenantProvider;
        public BookBussiness(IRepository<Book, long> BookRepository,
            IStringLocalizer<SharedResource> Localizer, ITenantProvider tenantProvider)
        {
            _BookRepository = BookRepository;
            _Localizer = Localizer;
            _tenantProvider = tenantProvider;
        }
        public async Task<Book> AddNewBook(Book _Book)
        {
            _BookRepository.Add(_Book);
            int added = await _BookRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Book);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public IQueryable<Book> GetAllBook()
        {
            var BookList = _BookRepository.GetAll(true);
            return BookList;
        }

        public IQueryable<Book> GetActiveBooks()
        {
            var BookList = _BookRepository.GetAll();
            return BookList;
        }
        public IQueryable<Book> GetALLStoreBooks()
        {
            return _BookRepository.GetAll(x => x.TenantId == _tenantProvider.GetTenant(), true);
        }
        public IQueryable<Book> CheckBookExistane(Book _Book)
        {
            if (_Book.Id != 0)
                return _BookRepository.GetAll
                (b => b.Id != _Book.Id && b.BookNumber == _Book.BookNumber && b.StoreId == _Book.StoreId && b.Consumed == _Book.Consumed, true);
            else
                return _BookRepository.GetAll
                (b => b.BookNumber == _Book.BookNumber && b.StoreId == _Book.StoreId && b.Consumed == _Book.Consumed, true);

        }

        public bool checkValidEdit(long BookId)
        {
            var checkConnections = _BookRepository.GetAll()
                .Where(x => x.Id == BookId && x.StoreItem.Any()).Count();

            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int BookId, bool ActivationType)
        {
            if (ActivationType)
                _BookRepository.Activate(new Book() { Id = BookId });
            else
                _BookRepository.DeActivate(new Book() { Id = BookId });

            var added = await _BookRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);
        }

        public async Task<bool> UpdateBook(Book _Book)
        {
            _BookRepository.PartialUpdate(_Book, d => d.PageCount, d => d.Consumed, d => d.StoreId, d => d.BookNumber);
            int added = await _BookRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                throw new NotSavedException(_Localizer["NotSavedException"]);

        }

        public Book ViewBook(int BookId)
        {
            var BookEntity = _BookRepository.Get(BookId);
            if (BookEntity != null)
            {
                return BookEntity;
            }
            else
                throw new InvalidException(_Localizer["InvalidException"]);
        }
    }
}