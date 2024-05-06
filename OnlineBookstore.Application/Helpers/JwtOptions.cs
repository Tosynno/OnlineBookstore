using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Helpers
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }

        public int ExpiryMinutes { get; set; }

        public string Issuer { get; set; }
    }
}
