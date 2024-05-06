using OnlineBookstore.Application.Models;
using OnlineBookstore.Domain.Data;
using OnlineBookstore.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Interfaces
{
    public interface IBookRepo : IRepository<BookdbContext, Book>
    {
        Task<object> CreateBookAsync(BookRequest request);
        Task<object> GetAllBookAsync(int pageIndex, int pageSize, bool previous, bool next);
        Task<object> DisableOrEnableBookAsync(string BookNumber);
        Task<object> DeleteBookAsync(string BookNumber);
    }
}
