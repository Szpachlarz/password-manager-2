using Microsoft.AspNetCore.Identity;

namespace PasswordManager2Api.Models
{
    public class Account : IdentityUser
    {
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
