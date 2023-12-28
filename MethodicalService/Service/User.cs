using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodicalService.Service
{
    public class User
    {
        public string FullName { get; set; }
        public string Role { get; set; }

        public User()
        {

        }

        public User(string fullname, string role)
        {
            FullName = fullname;
            Role = role;
        }

    }
}
