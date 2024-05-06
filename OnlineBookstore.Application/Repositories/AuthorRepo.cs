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
    public class AuthorRepo : BaseRepository<BookdbContext, Author>, IAuthorRepo
    {
        private AzureBlogService _blogService;
        public AuthorRepo(BookdbContext dbContext, AzureBlogService blogService) : base(dbContext)
        {
            _blogService = blogService;
        }

        public async Task<object> CreateAuthorAsync(AuthorRequest request)
        {
            Author author = new Author();
            author.AuthorName = request.AuthorName;
            author.AuthorInfo = request.AuthorInfo;
            author.IsActive = true;
            author.ImageUrl = await _blogService.UploadBlob(request.AuthorName, request.Imagebase64string);
            author.QueryString = Guid.NewGuid().ToString(); 
            AddAsync(author);
            return "SUCCESSFUL";
        }

        public async Task<object> EnableorDisableAuthorAsync(string requestId)
        {
            var res = await GetAllAsync();
            var result = res.FirstOrDefault(c => c.QueryString == requestId);
            if (result != null)
            {
                if (result.IsActive == true)
                {
                    result.IsActive = false;
                    await UpdateAsync(result);
                    return "Author has been Deactivated";
                }
                else
                {
                    result.IsActive = false;
                    await UpdateAsync(result);
                    return "Author has been Activated";
                }
            }
            return "01";
        }

        public async Task<object> GetAllAuthorAsync(int pageIndex, int pageSize, bool previous, bool next)
        {
           var res = await GetAllAsync();
            var result = res.Select(c => new AuthorDto{
                AuthorName = c.AuthorName,
                AuthorInfo = c.AuthorInfo,
                ImageUrl = c.ImageUrl,
                RequestId = c.QueryString,
                Status = c.IsActive == true ? "ACTIVE" : "INACTIVE",
                CreatedDate = c.CreatedDate.ToString("dd MMM yyyy")
            }).Distinct().ToList();
            if (result.Count() > 0)
            {
                if (previous)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.AuthorName).LastPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.AuthorName).CountOfPages(pageSize),
                    };
                    return metadata;

                }
                else if (next)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.AuthorName).FirstPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.AuthorName).CountOfPages(pageSize),
                    };
                    return metadata;
                }
                else
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                    .ThenBy(c => c.AuthorName).Page(pageIndex, pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => Convert.ToDateTime(c.CreatedDate))
                  .ThenBy(c => c.AuthorName).CountOfPages(pageSize),
                    };
                    return metadata;

                }

            }
            else
            {
                return "";
            }
        }

        public async Task<IQueryable<Author>> GetAllAuthorAsync()
        {
            var res = await GetAllAsync();
            return res.AsQueryable();
            
        }
    }
}
