using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace SchoolManagementSystem.Configurations
{
	public static class SwaggerConfig 
	{
		public static void AddSwaggerWithBasicSetup(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
			});
		}

		public static void UseSwaggerWithUI(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
		}
	}
}
