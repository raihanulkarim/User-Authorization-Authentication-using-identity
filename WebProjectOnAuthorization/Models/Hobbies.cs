using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProjectOnAuthorization.Models
{
    public class Hobbies
    {
        public int Id { get; set; }
        public string Hobby1 { get; set; }
        public string Hobby1Description { get; set; }
        public string Hobby1Image { get; set; }
        public string Hobby2 { get; set; }
        public string Hobby2Image { get; set; }
        public string Hobby2Description { get; set; }
        public string Hobby3 { get; set; }
        public string Hobby3Description { get; set; }
        public string Hobby3Image { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
