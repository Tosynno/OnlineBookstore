using OnlineBookstore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.IServices
{
    public interface IBookService
    {
        Task<object> CreateBook(BookRequest request);
        Task<object> GetAllBook(int pageIndex, int pageSize, bool previous, bool next);
        Task<object> DisableOrEnableBook(string BookNumber);
        Task<object> DeleteBook(string BookNumber);
    }
}
