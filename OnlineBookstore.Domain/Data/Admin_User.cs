using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Domain.Data
{
    public class Admin_User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string APIkey { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Email { get; set; }
    }
}
