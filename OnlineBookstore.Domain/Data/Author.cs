using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Domain.Data
{
    public class Author
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorInfo { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public string QueryString { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Book> book { get; set; } = new List<Book>();
    }
}
