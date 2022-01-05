namespace WebProjectOnAuthorization.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Github { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Linkdin { get; set; }
        public string UserId { get; set; }
        public ApplicationUser user { get; set; }
    }
}
