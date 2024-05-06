using Azure;
using OnlineBookstore.Application.Dto;
using OnlineBookstore.Application.Helpers;
using OnlineBookstore.Application.Interfaces;
using OnlineBookstore.Application.Models;
using OnlineBookstore.Application.Utilies;
using OnlineBookstore.Domain.Data;
using OnlineBookstore.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Repositories
{
    public class BookRepo : BaseRepository<BookdbContext, Book>, IBookRepo
    {
        private AzureBlogService _blogService;
        private readonly IRepository<BookdbContext, Author> _authrepository;
        public BookRepo(BookdbContext dbContext, AzureBlogService blogService, IRepository<BookdbContext, Author> authrepository) : base(dbContext)
        {
            _blogService = blogService;
            _authrepository = authrepository;
        }

        public async Task<object> CreateBookAsync(BookRequest request)
        {
            Book sa = new Book();
            sa.BookName = request.BookName;
            sa.Description = request.Description;
            sa.ImageUrl = await _blogService.UploadBlob(request.BookName, request.Imagestring);
            sa.AuthorId = request.AuthorId;
            sa.IsActive = true;
            sa.IsDeleted = false;
            sa.CreatedBy = 1;
            await AddAsync(sa);
            return "Successful";
        }

        public async Task<object> DeleteBookAsync(string BookNumber)
        {
            var res = await GetAllAsync();
            var result = res.FirstOrDefault(c => c.BookNumber == BookNumber);
            if (result != null)
            {
                result.IsDeleted = true;
                result.LastUpdatedDate = DateTime.Now;
                result.LastUpdatedBy = 1;
                await UpdateAsync(result);

                return "Book deleted successfully";
            }
            return "Invalid Book number";
        }

        public async Task<object> DisableOrEnableBookAsync(string BookNumber)
        {
            var res = await GetAllAsync();
            var result = res.FirstOrDefault(c => c.BookNumber == BookNumber);
            if (result != null)
            {
                if (result.IsActive == true)
                {
                    result.IsActive = false;
                    result.LastUpdatedDate = DateTime.Now;
                    result.LastUpdatedBy = 1;
                    await UpdateAsync(result);

                    return "Book Deactivated successfully";
                }
                else
                {
                    result.IsActive = true;
                    result.LastUpdatedDate = DateTime.Now;
                    result.LastUpdatedBy = 1;
                    await UpdateAsync(result);

                    return "Book Activated successfully";
                }
                
            }
            return "Invalid Book number";
        }

        public async Task<object> GetAllBookAsync(int pageIndex, int pageSize, bool previous, bool next)
        {
            var res = await GetAllAsync();
            var auth = await _authrepository.GetAllAsync();
            var result = (from u in res.AsQueryable()
                          join s in auth on u.AuthorId equals s.Id into auths
                          from s in auths.DefaultIfEmpty()
                          where u.IsDeleted == false 
                          select new BookDto
                          {
                              BookNumber = u.BookNumber,
                              BookName = u.BookName,
                              Description =u.Description,
                              ImageUrl = u.ImageUrl,
                              AuthorName = s.AuthorName,
                              CreatedDate = u.CreatedDate.ToString("dd MMM yyyy"),
                              LastUpdatedDate = u.LastUpdatedDate.ToString("dd MMM yyyy"),
                              Status = u.IsActive == true ? "InStock" :"OutStock",
                          }).ToList();

            if (result.Count() > 0)
            {
                if (previous)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.BookName).LastPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.BookName).CountOfPages(pageSize),
                    };
                    return metadata;
                    
                }
                else if (next)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.BookName).FirstPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.BookName).CountOfPages(pageSize),
                    };
                    return metadata;
                }
                else
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.BookName).Page(pageIndex, pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                  .ThenBy(c => c.BookName).CountOfPages(pageSize),
                    };
                    return metadata;
                    
                }

            }
            else
            {
                return "";
            }
        }
    }
}
