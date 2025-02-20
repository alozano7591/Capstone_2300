using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CircuitShare.Entities
{
	public class CircuitShareDbContext : IdentityDbContext<User>
	{

		public CircuitShareDbContext(DbContextOptions<CircuitShareDbContext> options) : base(options)
		{

		}


		public static async Task CreateAdminUser(IServiceProvider serviceProvider)
		{

			
			UserManager<User> userManager =
				serviceProvider.GetRequiredService<UserManager<User>>();
			RoleManager<IdentityRole> roleManager = serviceProvider
				.GetRequiredService<RoleManager<IdentityRole>>();

			string username = "admin";
			string password = "Sesame123#";
			string roleName = "Admin";

			// if role doesn't exist, create it
			if (await roleManager.FindByNameAsync(roleName) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}

			User adminUser = await userManager.FindByNameAsync(username);

			
			// if username doesn't exist, create it and add it to role
			
			if (await userManager.FindByNameAsync(username) == null)
			{
				User user = new User
				{
					UserName = username,
					FirstName = "John",
					LastName = "Jones",
					//Birthday = new DateTime(1990, 10, 31, 00, 00, 00),	// Old MSSql datetime
					Birthday = new DateTime(1990, 10, 31, 00, 00, 00, DateTimeKind.Utc),		// Postgres requires declaring as UTC
					Email = "bones@email.com",
					PhoneNumber = "555-888-9999",
					EmailConfirmed = true,
				};

				var result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, roleName);
				}
			}
			
		}

		public static async Task CreateUsers(IServiceProvider serviceProvider)
		{
			await CreateUser(serviceProvider, "Bob", "Pass1!", "bobby@fakeemail.com");
			await CreateUser(serviceProvider, "John", "Pass1!", "timmy@fakeemail.com");
			await CreateUser(serviceProvider, "Sarah", "Pass1!", "Sally@fakeemail.com");
			await CreateUser(serviceProvider, "Kate", "Pass1!", "wilma@fakeemail.com");
			await CreateUser(serviceProvider, "Sam", "Pass1!", "aleks@fakeemail.com");
		}

		public static async Task CreateUser(IServiceProvider serviceProvider, string username, string password, string email)
		{
			UserManager<User> userManager =
				serviceProvider.GetRequiredService<UserManager<User>>();
			RoleManager<IdentityRole> roleManager = serviceProvider
				.GetRequiredService<RoleManager<IdentityRole>>();

			// if username doesn't exist, create it and add it to role
			if (await userManager.FindByNameAsync(username) == null)
			{
				User user = new User
				{
					UserName = username,
					Email = email,
					EmailConfirmed = true,
				};

				var result = await userManager.CreateAsync(user, password);

				if (result.Succeeded)
				{
					Console.WriteLine(string.Format("User Created: Username {0}, email: {1}", username, email));
				}
				else
				{
					Console.WriteLine(string.Format("Failed to create User: Username {0}, email: {1}", username, email));
				}
			}
		}

		// Db sets
		public DbSet<Charger> Chargers { get; set; }
		public DbSet<BillingAddress> BillingAddresses { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Charger>().HasData(
			   new Charger()
			   {
					ChargerId = 1,
					Name = "Potato Charger",
				   Description = "This is a moderate charger that charges for a low price.",
				   HourlyRate = 4.99,
			   },
			   new Charger()
			   {
				   ChargerId = 2,
				   Name = "Berry Charger",
				   Description = "The medium performance charger with a good price.",
				   HourlyRate = 6.00,
			   },
			   new Charger()
			   {
				   ChargerId = 3,
				   Name = "Apple Charger",
				   Description = "It you want the fastest and the best then here it is.",
				   HourlyRate = 14.99,
			   },
			   new Charger()
			   {
				   ChargerId = 4,
				   Name = "Banana Charger",
				   Description = "It works and chargers really well. Believe you me.",
				   HourlyRate = 11.99,
			   },
			   new Charger()
			   {
				   ChargerId = 5,
				   Name = "Tesla Charger 83",
				   Description = "Brought to you by the same technology as the space man company.",
				   HourlyRate = 13.49,
			   }
			);
		}

		
	}
}
