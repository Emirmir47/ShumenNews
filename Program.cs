using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShumenNews.Data;
using ShumenNews.Data.Models;
using ShumenNews.Data.Seeding;
using ShumenNews.Extensions;
using ShumenNews.Services;

namespace ShumenNews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ShumenNewsDbContext>(options =>
                options.UseSqlServer(connectionString));
            // Seeding
            builder.Services.AddScoped<ShumenNewsSeeder>();
            builder.Services.AddTransient<IArticleService, ArticleService>();
            builder.Services.AddTransient<IImageService, ImageService>();
            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                //Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6; //128
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;

            })
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ShumenNewsDbContext>()
              .AddDefaultUI()
              .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Extension
            app.UseDatabaseSeeding();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}