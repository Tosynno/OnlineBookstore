using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBookstore.Application.IServices;
using OnlineBookstore.Application.Models;

namespace OnlineBookstore.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        protected IBookService _bookService;
        protected IHttpContextAccessor _httpContextAccessor;

        public BookController(IBookService bookService, IHttpContextAccessor httpContextAccessor)
        {
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("GetAllBooks/{pageIndex}/{pageSize}/{previous}/{next}")]
        //[ValidateAuthRequestAttribute]
        //[ServiceFilter(typeof(EncryptionActionFilter))]
        public async Task<ActionResult> GetAllBooks(int pageIndex, int pageSize, bool previous, bool next)
        {
            string[] auth = this.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _bookService.GetAllBook(pageIndex, pageSize, previous, next);
            if (res != null! || res.ToString() != "")
            {
                return Ok(res);
            }
            else
            {
                return NotFound("No Records Found");
            }
        }

        [HttpPost("Create-Book")]
        public async Task<ActionResult> CreateBook(BookRequest request)
        {
            return Ok(await _bookService.CreateBook(request));
        }

        [HttpGet("Delete-Book/{BookNumber}")]
        public async Task<ActionResult> DeleteBook(string BookNumber)
        {
            return Ok(await _bookService.DeleteBook(BookNumber));
        }
        [HttpGet("Disable-Enable-Book/{BookNumber}")]
        public async Task<ActionResult> DisableOrEnableBook(string BookNumber)
        {
            return Ok(await _bookService.DisableOrEnableBook(BookNumber));
        }
    }
}
