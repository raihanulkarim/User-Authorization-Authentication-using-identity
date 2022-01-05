using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebProjectOnAuthorization.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        //Personal Info
        public string FirstName { get; set; }
      
        public string LastName { get; set; }

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
        public string ProPhoto { get; set; }
        //Contact Info
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Github { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Linkdin { get; set; }
        //Hobbies
        public string Hobby1 { get; set; }
        public string Hobby1Description { get; set; }
        public string Hobby1Image { get; set; }
        public string Hobby2 { get; set; }
        public string Hobby2Image { get; set; }
        public string Hobby2Description { get; set; }
        public string Hobby3 { get; set; }
        public string Hobby3Description { get; set; }
        public string Hobby3Image { get; set; }
        //PhotoGallery
        public IFormFile ImageFile { get; set; }
        public IFormFile ImageFile1 { get; set; }
        public IFormFile ImageFile2 { get; set; }
        public IFormFile ProImg { get; set; }
    }
}
