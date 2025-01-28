using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Models.Password
{
    public class CreatePasswordDto
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Website { get; set; }
        public string Password { get; set; }        
        public string IV { get; set; }
    }
}
