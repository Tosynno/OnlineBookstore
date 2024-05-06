using OnlineBookstore.Application.Interfaces;
using OnlineBookstore.Application.IServices;
using OnlineBookstore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Services
{
    public class BookService : IBookService
    {
        private IBookRepo _bookRepo;

        public BookService(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public async Task<object> CreateBook(BookRequest request)
        {
            return _bookRepo.CreateBookAsync(request);
        }

        public async Task<object> DeleteBook(string BookNumber)
        {
            return await _bookRepo.DeleteBookAsync(BookNumber);
        }

        public async Task<object> DisableOrEnableBook(string BookNumber)
        {
            return _bookRepo.DisableOrEnableBookAsync(BookNumber);
        }

        public async Task<object> GetAllBook(int pageIndex, int pageSize, bool previous, bool next)
        {
            return await _bookRepo.GetAllBookAsync(pageIndex, pageSize, previous, next);
        }
    }
}
