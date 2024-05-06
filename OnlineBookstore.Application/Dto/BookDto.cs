using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Dto
{
    public class BookDto
    {
        public string BookNumber { get; set; }
        public string BookName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorName { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
