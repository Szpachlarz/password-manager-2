using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager2.Model
{
    internal class RecordModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Website { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AES_IV { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
