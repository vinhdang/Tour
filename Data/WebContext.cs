using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Domain;
namespace Data
{
    public class WebContext : DbContext
    {
        public WebContext()
            : base("ApplicationServices")
        {
            Database.SetInitializer<WebContext>(null);
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<National> Nationals { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Area> Areas { get; set; }
       // public DbSet<Comfort> Comforts { get; set; }
        public DbSet<TourType> TourTypes { get; set; }
        public DbSet<TourTheme> TourThemes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourPicture> TourPictures { get; set; }
        public DbSet<TourPrice> TourPrices { get; set; }
        public DbSet<TourProvider> TourProviders { get; set; }
        public DbSet<TourActivity> TourActivities { get; set; }
        public DbSet<TourSuggestion> TourSuggestion { get; set; }
        public DbSet<TourAgenda> TourAgendas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<Contact> Contacts { get; set; }
       
        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public class FoxCommerceMVCWebContextInitializer : DropCreateDatabaseIfModelChanges<WebContext>
        {
            protected override void Seed(WebContext context)
            {

            }
        }

    }
}