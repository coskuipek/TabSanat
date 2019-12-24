﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TabSanat.Dal.Data;

namespace TabSanat.Dal.Migrations
{
    [DbContext(typeof(TabDbContext))]
    [Migration("20190924141023_HistoryDateTime")]
    partial class HistoryDateTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TabSanat.Model.AppSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SettingName");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("AppSettings");

                    b.HasData(
                        new { Id = 1, SettingName = "Firma Adı", Value = "Tab Sanat" },
                        new { Id = 2, SettingName = "Ana Sayfada geç ödeme listesi için minimum sayı", Value = "1" }
                    );
                });

            modelBuilder.Entity("TabSanat.Model.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TabSanat.Model.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<int>("DayOfWeek");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<decimal>("OneLessonPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid>("SeasonId");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("TotalNumberOfLessons");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("SeasonId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TabSanat.Model.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AmountOfDiscount");

                    b.Property<string>("AppUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFixedAmount");

                    b.Property<string>("Name");

                    b.Property<bool>("OnlyOnce");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("TabSanat.Model.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<Guid?>("ExtraId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<decimal>("PriceEach")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ExtraId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("TabSanat.Model.Extra", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<decimal>("PriceToBuy")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("PriceToSell")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Extras");
                });

            modelBuilder.Entity("TabSanat.Model.History", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("TabSanat.Model.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsGiveBack");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("date");

                    b.Property<Guid>("PaymentTypeId");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid>("RegistrationId");

                    b.Property<Guid>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("RegistrationId");

                    b.HasIndex("StudentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TabSanat.Model.PaymentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("TabSanat.Model.Registration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<Guid>("CourseId");

                    b.Property<Guid?>("DiscountId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LeaveDate")
                        .HasColumnType("date");

                    b.Property<int>("NrOfLessonStudentWillJoin");

                    b.Property<decimal>("PaymentLeft")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StartToCourseDate")
                        .HasColumnType("date");

                    b.Property<Guid>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CourseId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("StudentId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("TabSanat.Model.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("PaymentTypeId");

                    b.Property<Guid?>("StudentId");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("StudentId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("TabSanat.Model.SaleItem", b =>
                {
                    b.Property<Guid>("ExtraId");

                    b.Property<Guid>("SaleId");

                    b.Property<int>("Amount");

                    b.Property<decimal>("PriceEach")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("ExtraId", "SaleId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleItem");
                });

            modelBuilder.Entity("TabSanat.Model.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("TabSanat.Model.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("AppUserId");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<Guid?>("DiscountId");

                    b.Property<string>("Email");

                    b.Property<string>("FatherFullName");

                    b.Property<string>("FatherPhoneNo");

                    b.Property<string>("FirstName");

                    b.Property<string>("Institution");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("MotherFullName");

                    b.Property<string>("MotherPhoneNo");

                    b.Property<string>("PhoneNo");

                    b.Property<string>("PhotoPath");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("date");

                    b.Property<string>("TcKimlikNo");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("DiscountId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TabSanat.Model.Course", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.Season", "Season")
                        .WithMany("Courses")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TabSanat.Model.Discount", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TabSanat.Model.Expense", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.Extra", "Extra")
                        .WithMany()
                        .HasForeignKey("ExtraId");
                });

            modelBuilder.Entity("TabSanat.Model.Extra", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TabSanat.Model.History", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TabSanat.Model.Payment", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.Registration", "Registration")
                        .WithMany("Payments")
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.Student", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TabSanat.Model.PaymentType", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TabSanat.Model.Registration", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.Course", "Course")
                        .WithMany("Registrations")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");

                    b.HasOne("TabSanat.Model.Student", "Student")
                        .WithMany("Registers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TabSanat.Model.Sale", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.Student", "Student")
                        .WithMany("Sales")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("TabSanat.Model.SaleItem", b =>
                {
                    b.HasOne("TabSanat.Model.Extra", "Extra")
                        .WithMany()
                        .HasForeignKey("ExtraId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TabSanat.Model.Sale", "Sale")
                        .WithMany("SaleItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TabSanat.Model.Season", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("TabSanat.Model.Student", b =>
                {
                    b.HasOne("TabSanat.Model.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("TabSanat.Model.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");
                });
#pragma warning restore 612, 618
        }
    }
}
