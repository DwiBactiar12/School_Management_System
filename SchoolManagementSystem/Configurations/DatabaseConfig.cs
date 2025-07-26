using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Common.Helpers;
using System;
using System.Diagnostics;

namespace SchoolManagementSystem.Configurations
{
	public static class DatabaseConfig 
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
						   $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
						   $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
						   $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
						   $"Password={Environment.GetEnvironmentVariable("DB_PASS")};" +
						   "SSL Mode=Require;Trust Server Certificate=true";

			Debug.WriteLine("PostgreSQL Connection String: " + connectionString);

			services.AddDbContext<SchoolDbContext>(options =>
				options.UseNpgsql(connectionString)
			);

			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // Untuk PostgreSQL

			return services;
		}
	}
}
