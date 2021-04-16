using CoronaTest.Core.Entities;
using CoronaTest.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace CoronaTest.Persistence
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<VerificationToken> VerificationTokens { get; set; }
		public DbSet<Campaign> Campaigns { get; set; }
		public DbSet<Examination> Examinations { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<TestCenter> TestCenters { get; set; }
		public DbSet<AuthUser> AuthUsers { get; set; }
		public DbSet<AuthRole> AuthRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestCenter>()
				 .HasMany(u => u.AvailableInCampaigns);
			modelBuilder.Entity<Campaign>()
				  .HasMany(u => u.AvailableTestCenters);
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AuthRole>().HasData(new AuthRole { Id = 1, Name = "Admin" });
			modelBuilder.Entity<AuthRole>().HasData(new AuthRole { Id = 2, Name = "User" });
			modelBuilder.Entity<AuthUser>().HasData(new AuthUser
			{
				Id = 1,
				UserRole = "Admin",
				Email = "admin@htl.at",
				Password = AuthUtils.GenerateHashedPassword("admin@htl.at")
			});
			modelBuilder.Entity<AuthUser>().HasData(new AuthUser
			{
				Id = 2,
				UserRole = "User",
				Email = "user@htl.at",
				Password = AuthUtils.GenerateHashedPassword("user@htl.at")
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var configuration = new ConfigurationBuilder()
				 .SetBasePath(Environment.CurrentDirectory)
				 .AddJsonFile("appsettings.json")
				 .AddEnvironmentVariables()
				 .Build();

			Debug.WriteLine(configuration);

			optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
		}
	}
}
