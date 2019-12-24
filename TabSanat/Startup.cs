using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Dal.Uow;
using TabSanat.Helpers;
using TabSanat.Model;
using TabSanat.Services.Implementations;
using TabSanat.Services.Interfaces;

namespace TabSanat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TabDbContext>(options => options.UseSqlServer
                (Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("TabSanat.Dal")));

            TabSanatServices(services);

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<TabDbContext>();

            TabSanatClaims(services);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/YetkisizGiris";
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithRedirects("/Hata/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Hata/{0}");
                app.UseHsts();
            }
            var defaultCulture = new CultureInfo("tr-TR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            bool x = await RoleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                await RoleManager.CreateAsync(adminRole);

                foreach (var item in ClaimData.Claims)
                    await RoleManager.AddClaimAsync(adminRole, new Claim(item, item));


                //Here we create a Admin super user who will maintain the website                   

                var user = new AppUser
                {
                    UserName = "cosku",
                    FirstName = "Coşku",
                    LastName = "İpek"
                };


                string userPWD = "cosku";

                IdentityResult chkUser = await UserManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(user, "Admin");
                }

            }
        }

        private void TabSanatServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISaveService, SaveService>();

            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddTransient<IExtraRepository, ExtraRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IHistoryRepository, HistoryRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();

            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IDiscountService, DiscountService>();
            services.AddTransient<IExtraService, ExtraService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IPaymentTypeService, PaymentTypeService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<IGroupService, GroupService>();


            services.AddTransient<IAppSettingsRepository, AppSettingsRepository>();
            services.AddTransient<IAppSettingsService, AppSettingsService>();

        }

        private static void TabSanatClaims(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var item in ClaimData.Claims)
                    options.AddPolicy(item, policy => policy.RequireClaim(item, item));

            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<TabDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
