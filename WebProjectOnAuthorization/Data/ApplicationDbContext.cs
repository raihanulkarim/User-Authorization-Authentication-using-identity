using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebProjectOnAuthorization.Models;
using WebProjectOnAuthorization.ViewModels;

namespace WebProjectOnAuthorization.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Hobbies> Hobbies { get; set; }
        public DbSet<Personalinfo> Personalinfos { get; set; }
        //public DbSet<PhotoGallery> PhotoGalleries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity("WebProjectOnAuthorization.Models.ContactInfo", b =>
            {
                b.HasOne("WebProjectOnAuthorization.Models.ApplicationUser", "user")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.Navigation("user");
            });

            modelBuilder.Entity("WebProjectOnAuthorization.Models.Hobbies", b =>
            {
                b.HasOne("WebProjectOnAuthorization.Models.ApplicationUser", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
               

                b.Navigation("User");
            });

            modelBuilder.Entity("WebProjectOnAuthorization.Models.Personalinfo", b =>
            {
                b.HasOne("WebProjectOnAuthorization.Models.ApplicationUser", "user")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);


                b.Navigation("user");
            });

            /*modelBuilder.Entity("WebProjectOnAuthorization.Models.PhotoGallery", b =>
           // {
             //   b.HasOne("WebProjectOnAuthorization.Models.ApplicationUser", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);


                b.Navigation("User");
            });*/
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1",
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    NormalizedUserName = "ADMIN@mail.COM",
                    NormalizedEmail = "ADMIN@mail.COM",
                    EmailConfirmed = true,
                    IsProfileComplete = true,
                    IsDisable = false,
                    PasswordHash = hasher.HashPassword(null, "admin123!"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            ) ;
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { 
                    Id = "1",
                    Name = "admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp="1",
                }, 
                new IdentityRole { 
                    Id = "2",
                    Name = "user",
                    NormalizedName = "user",
                    ConcurrencyStamp="1",
                }
                );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "1",
                   UserId = "1"
               }
               );


        }
    }
}
