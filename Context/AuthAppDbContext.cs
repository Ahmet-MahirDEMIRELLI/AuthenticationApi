using AuthenticationApi.Classes;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Context
{
	public class AuthAppDbContext : DbContext
	{
		public AuthAppDbContext(DbContextOptions<AuthAppDbContext> options) : base(options) { }
		public DbSet<User> Users { get; set; }
		public DbSet<LoggedUser> LoggedUsers { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasKey(rh => new { rh.UserId });

			modelBuilder.Entity<LoggedUser>()
				.HasKey(rh => new { rh.LogId });
		}
	}
}
