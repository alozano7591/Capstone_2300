using CircuitShare.Entities;
using CircuitShare.Services;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using React.AspNet;


namespace CircuitShare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			var connStr = builder.Configuration.GetConnectionString("CircuitShareDB");
			builder.Services.AddDbContext<CircuitShareDbContext>(options => options.UseNpgsql(connStr));

            builder.Services.AddScoped<ChargerManager>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddReact();
            builder.Services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
                .AddV8();

			// Login stuff
			builder.Services.AddIdentity<User, IdentityRole>(options => {
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 3;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
				options.SignIn.RequireConfirmedEmail = true;           // will need to change this to true at some point. False for now for easier testing
			}).AddEntityFrameworkStores<CircuitShareDbContext>().AddDefaultTokenProviders();

			//Reset password token
			builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
				options.TokenLifespan = TimeSpan.FromMinutes(30));

            // cookies and session tracking
			builder.Services.AddMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(5);
				options.Cookie.HttpOnly = false;
				options.Cookie.IsEssential = true;
			});

			var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                config
                    .AddScript("~/js/HelloReact.jsx");
                config
                    .AddScript("~/js/LoginForm.jsx");
                config
                    .AddScript("~/js/RegisterForm.jsx");
                config
                    .AddScript("~/js/Chargers.jsx");
                
            });
            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
            
            app.UseStaticFiles();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

			var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
			using (var scope = scopeFactory.CreateScope())
			{
				await CircuitShareDbContext.CreateAdminUser(scope.ServiceProvider);
				await CircuitShareDbContext.CreateUsers(scope.ServiceProvider);
			}

			app.Run();
        }
    }
}
