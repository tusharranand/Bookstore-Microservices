using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Models
{
    public class LoginResponse
    {
        public UserModel data { get; set; }
        public string token { get; set; }
    }
}
