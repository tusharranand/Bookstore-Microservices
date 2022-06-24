using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Models
{
    public class ResetPassword
    {
        public string password { get; set; }
        public string confirmPassword { get; set; }

    }
}
