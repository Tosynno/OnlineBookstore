using OnlineBookstore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.IServices
{
    public interface IAuthorService
    {
        Task<object> CreateAuthorAsync(AuthorRequest request);
        Task<object> GetAllAuthorDropdownAsync();
        Task<object> GetAllAuthorAsync(int pageIndex, int pageSize, bool previous, bool next);
        Task<object> EnableorDisableAuthorAsync(string requestId);
    }
}
