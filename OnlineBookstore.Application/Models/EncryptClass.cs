using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Models
{
    public class EncryptClass
    {
        [Required]
        public string Data { get; set; }
    }
}
