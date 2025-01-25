using System.ComponentModel.DataAnnotations;

namespace PasswordManager2Api.Dtos
{
    public class RecordDto
    {
        [Required]
        public string ServiceName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string IV { get; set; }
    }
}
