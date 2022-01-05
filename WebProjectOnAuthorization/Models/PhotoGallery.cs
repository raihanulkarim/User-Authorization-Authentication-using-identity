namespace WebProjectOnAuthorization.Models
{
    public class PhotoGallery
    {
        public int Id { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
