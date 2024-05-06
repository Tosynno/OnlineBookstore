using Microsoft.EntityFrameworkCore;
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
    public interface IAuthorRepo: IRepository<BookdbContext, Author>
    {
        Task<object> CreateAuthorAsync(AuthorRequest request);
        Task<IQueryable<Author>> GetAllAuthorAsync();
        Task<object> GetAllAuthorAsync(int pageIndex, int pageSize, bool previous, bool next);
        Task<object> EnableorDisableAuthorAsync(string requestId);
    }
}
