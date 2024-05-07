using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Domain.Data
{
    public class Admin_Activity_log
    {
        public long Id { get; set; }
        public long AdminId { get; set; }
        public string Messagex { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
