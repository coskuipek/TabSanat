using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TabSanat.Dal.Configuration;
using TabSanat.Model;

namespace TabSanat.Dal.Data
{
    public class TabDbContext : IdentityDbContext<AppUser>
    {
        public TabDbContext()
        {

        }
        public TabDbContext(DbContextOptions<TabDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppSettings>().HasData(
                new AppSettings()
            {
                Id = 1,
                SettingName = "Firma Adı",
                Value = "Tab Sanat"
            },
               new AppSettings()
               {
                   Id = 2,
                   SettingName = "Ana Sayfada geç ödeme listesi için minimum sayı",
                   Value = "1"
               });

            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new ExtraConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new SaleItemConfiguration());
            modelBuilder.ApplyConfiguration(new SeasonConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());

        }
    }
}
