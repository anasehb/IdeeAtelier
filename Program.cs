using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GroupSpace23.Data;
using Microsoft.AspNetCore.Identity;
using GroupSpace23.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using GroupSpace23.Services;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using System;

namespace GroupSpace23
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("GroupSpace23Context");

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'GroupSpace23Context' not found.")));

            builder.Services.AddDefaultIdentity<GroupSpace23User>((IdentityOptions options) => options.SignIn.RequireConfirmedAccount = false)
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<MyDbContext>();

            builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();

            builder.Services.Configure<MailKitOptions>(options =>
            {
                options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
                options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
                options.Security = true;  // true zet ssl or tls aan
            });

            builder.Services.AddLocalization(options => options.ResourcesPath = "Translations");
            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            var app = builder.Build();
            Globals.App = app;

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Cookie Middleware
            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("SelectedLanguage"))
                {
                    var selectedLanguage = context.Request.Query["culture"];
                    if (!string.IsNullOrWhiteSpace(selectedLanguage))
                    {
                        context.Response.Cookies.Append("SelectedLanguage", selectedLanguage,
                            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
                    }
                }
                await next.Invoke();
            });

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                MyDbContext context = new MyDbContext(services.GetRequiredService<DbContextOptions<MyDbContext>>());
                var userManager = services.GetRequiredService<UserManager<GroupSpace23User>>();
                await MyDbContext.DataInitializer(context, userManager);
            }

            var supportedCultures = new[] { "en-US", "fr", "nl" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
