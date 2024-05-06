using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Domain.Data
{
    public class Book
    {
        public Guid Id { get; set; }
        public string BookNumber { get; set; }
        public string BookName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public long AuthorId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set;}
        public int LastUpdatedBy { get; set;}
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Author author { get; set; }

    }
}
