using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebProjectOnAuthorization.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")] 
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string ProPic { get; set; }
        public bool IsProfileComplete { get; set; }
        public bool IsDisable { get; set; }

    }
}
