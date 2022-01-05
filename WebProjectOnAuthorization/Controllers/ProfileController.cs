using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProjectOnAuthorization.Data;
using WebProjectOnAuthorization.Models;
using WebProjectOnAuthorization.ViewModels;

namespace WebProjectOnAuthorization.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.hostEnvironment = hostEnvironment;
            this.userManager = userManager;
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Update(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }
            if (id != userId)
            {
                return BadRequest();
            }
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userId);
            if (user.IsProfileComplete == false)
            {
                return RedirectToAction("welcome", "home");
            }
            //Sending Personalinfo
            var profileViewModel = new ProfileViewModel();
            profileViewModel.FirstName = user.FirstName;
            profileViewModel.LastName = user.LastName;
            profileViewModel.PhoneNo = user.PhoneNumber;
            var personalInfo = await _context.Personalinfos.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            profileViewModel.Birthdate = personalInfo.Birthdate.Date;
            profileViewModel.Profession = personalInfo.Profession;
            profileViewModel.Organisation = personalInfo.Organisation;
            profileViewModel.PresentAddress = personalInfo.PresentAddress;
            profileViewModel.About = personalInfo.About;
            profileViewModel.ProPhoto = user.ProPic;

            //Saving Contact info
            var conInfo = await _context.ContactInfos.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Github = conInfo.Github;
            profileViewModel.Twitter = conInfo.Twitter;
            profileViewModel.Facebook = conInfo.Facebook;
            profileViewModel.Linkdin = conInfo.Linkdin;
            //Hobbies info
            var hobb = await _context.Hobbies.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Hobby1 = hobb.Hobby1;
            profileViewModel.Hobby1Description = hobb.Hobby1Description;
            profileViewModel.Hobby1Image = hobb.Hobby1Image;
            profileViewModel.Hobby2 = hobb.Hobby2;
            profileViewModel.Hobby2Description = hobb.Hobby2Description;
            profileViewModel.Hobby2Image = hobb.Hobby2Image;
            profileViewModel.Hobby3 = hobb.Hobby3;
            profileViewModel.Hobby3Description = hobb.Hobby3Description;
            profileViewModel.Hobby3Image = hobb.Hobby3Image;

            if (profileViewModel == null)
            {
                return NotFound();
            }
            return View(profileViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileViewModel profileViewModel)
        {
            TryValidateModel(profileViewModel);   
                try
                {
                    var userId = userManager.GetUserId(HttpContext.User);
                    var userList = await userManager.Users.ToListAsync();
                    var user = userList.FirstOrDefault(r => r.Id == userId);
                if (user.IsProfileComplete == false)
                {
                    return RedirectToAction("welcome", "home");
                }
                //saving Personalinfo

                 var personalInfo = await _context.Personalinfos.Where(r => r.UserId == userId).FirstOrDefaultAsync();
                personalInfo.Birthdate = profileViewModel.Birthdate.Date;
                personalInfo.About = profileViewModel.About;
                personalInfo.Profession = profileViewModel.Profession;
                personalInfo.Organisation = profileViewModel.Organisation;
                personalInfo.PresentAddress = profileViewModel.PresentAddress;
                _context.Update(personalInfo);
                 await _context.SaveChangesAsync();
                 
                //Saving Contact info
                  var conInfo = await _context.ContactInfos.Where(r => r.UserId == userId).FirstOrDefaultAsync();
                conInfo.Github = profileViewModel.Github;
                conInfo.Twitter = profileViewModel.Twitter;
                conInfo.Facebook = profileViewModel.Facebook;
                conInfo.Linkdin = profileViewModel.Linkdin;
                _context.Update(conInfo);
                 await _context.SaveChangesAsync();

                    //Hobbies info
                    var hobb = await _context.Hobbies.Where(r => r.UserId == userId).FirstOrDefaultAsync();
                hobb.Hobby1 = profileViewModel.Hobby1;
                hobb.Hobby1Description = profileViewModel.Hobby1Description;
                //Image Save
                //image1
                string wwwRootPath = hostEnvironment.WebRootPath;

                if (profileViewModel.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile.FileName);
                    string extension = Path.GetExtension(profileViewModel.ImageFile.FileName);
                    profileViewModel.Hobby1Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await profileViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    hobb.Hobby1Image = profileViewModel.Hobby1Image;

                }

                //image2
                if (profileViewModel.ImageFile1 != null)
                {
                    string fileName1 = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile1.FileName);
                    string extension1 = Path.GetExtension(profileViewModel.ImageFile1.FileName);
                    profileViewModel.Hobby2Image = fileName1 = fileName1 + DateTime.Now.ToString("yymmssfff") + extension1;
                    string path1 = Path.Combine(wwwRootPath + "/Image/", fileName1);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await profileViewModel.ImageFile1.CopyToAsync(fileStream);
                    }
                    hobb.Hobby2Image = profileViewModel.Hobby2Image;

                }
                //image3
                if (profileViewModel.ImageFile2 != null)
                {
                    string fileName2 = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile2.FileName);
                    string extension2 = Path.GetExtension(profileViewModel.ImageFile2.FileName);
                    profileViewModel.Hobby3Image = fileName2 = fileName2 + DateTime.Now.ToString("yymmssfff") + extension2;
                    string path2 = Path.Combine(wwwRootPath + "/Image/", fileName2);
                    using (var fileStream = new FileStream(path2, FileMode.Create))
                    {
                        await profileViewModel.ImageFile2.CopyToAsync(fileStream);
                    }
                hobb.Hobby3Image = profileViewModel.Hobby3Image;
                }
                hobb.Hobby2 = profileViewModel.Hobby2;
                hobb.Hobby2Description = profileViewModel.Hobby2Description;
                hobb.Hobby3 = profileViewModel.Hobby3;
                hobb.Hobby3Description = profileViewModel.Hobby3Description;
                _context.Update(hobb);

                //photo gallery
                //var photos = await _context.PhotoGalleries.Where(r => r.UserId == userId).FirstOrDefaultAsync();

                //User Info
                //image
                if (profileViewModel.ProImg != null)
                {
                    string profileName = Path.GetFileNameWithoutExtension(profileViewModel.ProImg.FileName);
                    string proextension = Path.GetExtension(profileViewModel.ProImg.FileName);
                    profileViewModel.ProPhoto = profileName = profileName + DateTime.Now.ToString("yymmssfff") + proextension;
                    string propath = Path.Combine(wwwRootPath + "/Image/", profileName);
                    using (var fileStream = new FileStream(propath, FileMode.Create))
                    {
                        await profileViewModel.ProImg.CopyToAsync(fileStream);
                    }
                    user.ProPic = profileViewModel.ProPhoto;

                }
                user.IsProfileComplete = true; 
                user.FirstName = profileViewModel.FirstName;
                user.LastName = profileViewModel.LastName;
                user.PhoneNumber = profileViewModel.PhoneNo;
                await userManager.UpdateAsync(user);

                await _context.SaveChangesAsync();
                    return LocalRedirect($"/Profile/Details/{userId}");
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public async Task<IActionResult> Details(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }
            if (id != userId)
            {
                return BadRequest();
            }
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userId);
        if (user.IsProfileComplete == false)
            {
                return RedirectToAction("welcome", "home");
            }
            //Sending Personalinfo
            var profileViewModel = new ProfileViewModel();
            profileViewModel.FirstName = user.FirstName;
            profileViewModel.LastName = user.LastName;
            profileViewModel.PhoneNo = user.PhoneNumber;
            profileViewModel.Email = User.Identity.Name;
            var personalInfo = await _context.Personalinfos.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            profileViewModel.Birthdate = personalInfo.Birthdate.Date;
            profileViewModel.Profession = personalInfo.Profession;
            profileViewModel.Organisation = personalInfo.Organisation;
            profileViewModel.PresentAddress = personalInfo.PresentAddress;
            profileViewModel.About = personalInfo.About;
            profileViewModel.ProPhoto = user.ProPic;

            //Saving Contact info
            var conInfo = await _context.ContactInfos.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Github = conInfo.Github;
            profileViewModel.Twitter = conInfo.Twitter;
            profileViewModel.Facebook = conInfo.Facebook;
            profileViewModel.Linkdin = conInfo.Linkdin;
            //Hobbies info
            var hobb = await _context.Hobbies.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Hobby1 = hobb.Hobby1;
            profileViewModel.Hobby1Description = hobb.Hobby1Description;
            profileViewModel.Hobby1Image = hobb.Hobby1Image;
            profileViewModel.Hobby2 = hobb.Hobby2;
            profileViewModel.Hobby2Description = hobb.Hobby2Description;
            profileViewModel.Hobby2Image = hobb.Hobby2Image;
            profileViewModel.Hobby3 = hobb.Hobby3;
            profileViewModel.Hobby3Description = hobb.Hobby3Description;
            profileViewModel.Hobby3Image = hobb.Hobby3Image;
            if (profileViewModel == null)
                {
                    return NotFound();
                }
                return View(profileViewModel);
        }

        // GET: ProfileViewModels/Create
        public async Task<IActionResult> Create()
        {
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userManager.GetUserId(HttpContext.User));
            if (user.IsProfileComplete == true)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(HttpContext.User);
                var userList = await userManager.Users.ToListAsync();
                var user = userList.FirstOrDefault(r=> r.Id == userId);

                //saving Personalinfo
                var personalInfo = new Personalinfo();
                personalInfo.Birthdate =profileViewModel.Birthdate;
                personalInfo.About =profileViewModel.About;
                personalInfo.Profession =profileViewModel.Profession;
                personalInfo.Organisation = profileViewModel.Organisation;
                personalInfo.PresentAddress =profileViewModel.PresentAddress;
                personalInfo.UserId = userId;
                _context.Add(personalInfo);

                //Saving Contact info
                var conInfo = new ContactInfo();
                conInfo.Email = User.Identity.Name;
                conInfo.Github = profileViewModel.Github;
                conInfo.Twitter = profileViewModel.Twitter;
                conInfo.Facebook = profileViewModel.Facebook;
                conInfo.Linkdin = profileViewModel.Linkdin;
                conInfo.UserId = userId;
                _context.Add(conInfo);

                //Hobbies info
                var hobb = new Hobbies();
                hobb.UserId = userId;
                hobb.Hobby1 = profileViewModel.Hobby1;
                hobb.Hobby1Description = profileViewModel.Hobby1Description;
                //Image Save
                string wwwRootPath = hostEnvironment.WebRootPath;
                //image1
                if (profileViewModel.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile.FileName);
                    string extension = Path.GetExtension(profileViewModel.ImageFile.FileName);
                    profileViewModel.Hobby1Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await profileViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    hobb.Hobby1Image = profileViewModel.Hobby1Image;

                }
                else
                {
                    hobb.Hobby1Image = "noImage.png";
                }
                //image2
                if (profileViewModel.ImageFile1 != null)
                {
                    string fileName1 = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile1.FileName);
                    string extension1 = Path.GetExtension(profileViewModel.ImageFile1.FileName);
                    profileViewModel.Hobby2Image = fileName1 = fileName1 + DateTime.Now.ToString("yymmssfff") + extension1;
                    string path1 = Path.Combine(wwwRootPath + "/Image/", fileName1);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await profileViewModel.ImageFile1.CopyToAsync(fileStream);
                    }
                    hobb.Hobby2Image = profileViewModel.Hobby2Image;
                }
                else
                {
                    hobb.Hobby2Image = "noImage.png";

                }
                //image3
                if (profileViewModel.ImageFile2 != null)
                {
                    string fileName2 = Path.GetFileNameWithoutExtension(profileViewModel.ImageFile2.FileName);
                    string extension2 = Path.GetExtension(profileViewModel.ImageFile2.FileName);
                    profileViewModel.Hobby3Image = fileName2 = fileName2 + DateTime.Now.ToString("yymmssfff") + extension2;
                    string path2 = Path.Combine(wwwRootPath + "/Image/", fileName2);
                    using (var fileStream = new FileStream(path2, FileMode.Create))
                    {
                        await profileViewModel.ImageFile2.CopyToAsync(fileStream);
                    }
                    hobb.Hobby3Image = profileViewModel.Hobby3Image;

                }
                else
                {
                    hobb.Hobby3Image = "noImage.png";

                }

                hobb.Hobby2 = profileViewModel.Hobby2;
                hobb.Hobby2Description = profileViewModel.Hobby2Description;
                hobb.Hobby3 = profileViewModel.Hobby3;
                hobb.Hobby3Description = profileViewModel.Hobby3Description;
                _context.Add(hobb);

                //User Info
                //image
                if (profileViewModel.ImageFile2 != null)
                {
                    string profileName = Path.GetFileNameWithoutExtension(profileViewModel.ProImg.FileName);
                    string proextension = Path.GetExtension(profileViewModel.ProImg.FileName);
                    profileViewModel.ProPhoto = profileName = profileName + DateTime.Now.ToString("yymmssfff") + proextension;
                    string propath = Path.Combine(wwwRootPath + "/Image/", profileName);
                    using (var fileStream = new FileStream(propath, FileMode.Create))
                    {
                        await profileViewModel.ProImg.CopyToAsync(fileStream);
                    }
                    user.ProPic = profileViewModel.ProPhoto;

                }
                else
                {
                    user.ProPic = "noProfileImg.png";

                }

                user.PhoneNumber = profileViewModel.PhoneNo;
                user.IsProfileComplete = true;
                await userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                return LocalRedirect($"/profile/Details/{userId}");
            }
            return View(profileViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> user(string id)
        {
            string userId = id;
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userId);

            //Sending Personalinfo
            var profileViewModel = new ProfileViewModel();
            profileViewModel.FirstName = user.FirstName;
            profileViewModel.LastName = user.LastName;
            profileViewModel.PhoneNo = user.PhoneNumber;
            profileViewModel.Email = user.Email;
            var personalInfo = await _context.Personalinfos.Where(r => r.UserId == userId).FirstOrDefaultAsync();

            profileViewModel.Birthdate = personalInfo.Birthdate.Date;
            profileViewModel.Profession = personalInfo.Profession;
            profileViewModel.Organisation = personalInfo.Organisation;
            profileViewModel.PresentAddress = personalInfo.PresentAddress;
            profileViewModel.About = personalInfo.About;
            profileViewModel.ProPhoto = user.ProPic;

            //Saving Contact info
            var conInfo = await _context.ContactInfos.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Github = conInfo.Github;
            profileViewModel.Twitter = conInfo.Twitter;
            profileViewModel.Facebook = conInfo.Facebook;
            profileViewModel.Linkdin = conInfo.Linkdin;
            //Hobbies info
            var hobb = await _context.Hobbies.FirstOrDefaultAsync(r => r.UserId == userId);
            profileViewModel.Hobby1 = hobb.Hobby1;
            profileViewModel.Hobby1Description = hobb.Hobby1Description;
            profileViewModel.Hobby1Image = hobb.Hobby1Image;
            profileViewModel.Hobby2 = hobb.Hobby2;
            profileViewModel.Hobby2Description = hobb.Hobby2Description;
            profileViewModel.Hobby2Image = hobb.Hobby2Image;
            profileViewModel.Hobby3 = hobb.Hobby3;
            profileViewModel.Hobby3Description = hobb.Hobby3Description;
            profileViewModel.Hobby3Image = hobb.Hobby3Image;
            if (profileViewModel == null)
            {
                return NotFound();
            }
            return View(profileViewModel);
        }
        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = id;
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("users","home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return RedirectToAction("users","home");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> action(string id)
        {
            string userId = id;
            var userList = await userManager.Users.ToListAsync();
            var user = userList.FirstOrDefault(r => r.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    if (user.IsDisable == false)
                    {
                        user.IsDisable = true;

                    }
                    else
                    {
                        user.IsDisable = false;

                    }
                    var result = await userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    if (result.Succeeded)
                    {
                        return RedirectToAction("users","home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return RedirectToAction("users","home");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
