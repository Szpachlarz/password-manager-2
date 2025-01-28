namespace PasswordManager2Api.Models
{
    public class Record
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Username { get; set; }
        public string ServiceName { get; set; }
        public string Password { get; set; }
        public string IV { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
