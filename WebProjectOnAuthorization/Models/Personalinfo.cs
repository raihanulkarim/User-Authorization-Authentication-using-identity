using System;
using System.ComponentModel.DataAnnotations;

namespace WebProjectOnAuthorization.Models
{
    public class Personalinfo
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        public DateTime Birthdate { get; set; }
        [Required(ErrorMessage = "About is required")]
        [Display(Name = "About")]
        public string About { get; set; }
        [Required(ErrorMessage = "Profession is required")]
        [Display(Name = "Profession")]
        public string Profession { get; set; }
        [Required(ErrorMessage = "Organisation is required")]
        [Display(Name = "Organisation")]
        public string Organisation { get; set; }
        public string PresentAddress { get; set; }
        public string UserId { get; set; }
        public ApplicationUser user { get; set; }
    }
}
