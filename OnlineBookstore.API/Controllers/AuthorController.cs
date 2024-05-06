using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookstore.Application.IServices;
using OnlineBookstore.Application.Models;

namespace OnlineBookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        protected IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("GetAllAuthor/{pageIndex}/{pageSize}/{previous}/{next}")]
        public async Task<ActionResult> GetAllAuthors(int pageIndex, int pageSize, bool previous, bool next)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _authorService.GetAllAuthorAsync(pageIndex, pageSize, previous, next);
            if (res != null! || res?.ToString() != "")
            {
                return Ok(res);
            }
            else
            {
                return NotFound("No Records Found");
            }
        }
        [HttpGet("EnableorDisableAuthor/{requestId}")]
        public async Task<ActionResult> EnableorDisableAuthor(string requestId)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _authorService.EnableorDisableAuthorAsync(requestId);
            if (res.ToString() != "01")
            {
                return Ok(res);
            }
            else
            {
                return NotFound("No Records Found");
            }
        }

        [HttpPost("CreateAuthor")]
        public async Task<ActionResult> CreateAuthor(AuthorRequest request)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            return Ok(await _authorService.CreateAuthorAsync(request));
        }

        [HttpGet("GetAllAuthorDropdown")]
        public async Task<ActionResult> GetAllAuthorDropdown()
        {
            return Ok(await _authorService.GetAllAuthorDropdownAsync());
        }
    }
}
