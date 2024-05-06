using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Models
{
    public class BookRequest
    {
        public string BookName { get; set; }
        public string Description { get; set; }
        public string Imagestring { get; set; }
        public long AuthorId { get; set; }
    }
}
