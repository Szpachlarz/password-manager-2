using System.ComponentModel.DataAnnotations;

namespace PasswordManager2Api.Dtos
{
    public class RecordDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string IV { get; set; }
    }
}
