using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Dto
{
    public class AuthorDto
    {
        public string AuthorName { get; set; }
        public string AuthorInfo { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public string RequestId { get; set; }
        public string CreatedDate { get; set; }
    }
}
