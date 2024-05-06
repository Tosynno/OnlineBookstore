using OnlineBookstore.Application.Dto;
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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _author;

        public AuthorService(IAuthorRepo author)
        {
            _author = author;
        }

        public async Task<object> CreateAuthorAsync(AuthorRequest request)
        {
            return await _author.CreateAuthorAsync(request);
        }

        public async Task<object> EnableorDisableAuthorAsync(string requestId)
        {
            return await _author.EnableorDisableAuthorAsync(requestId);
        }

        public async Task<object> GetAllAuthorAsync(int pageIndex, int pageSize, bool previous, bool next)
        {
            return _author.GetAllAuthorAsync(pageIndex, pageSize, previous, next);
        }

        public async Task<object> GetAllAuthorDropdownAsync()
        {
            var res = await _author.GetAllAuthorAsync();
            var result = res.Select(c => new DropDownDto
            {
                Text = c.AuthorName,
                Value = c.Id.ToString()
            }).ToList();

            return result;
        }
    }
}
