using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Models
{
    public class AuthorRequest
    {
        public string AuthorName { get; set; }
        public string AuthorInfo { get; set; }
        public string Imagebase64string{ get; set; }

    }
}
