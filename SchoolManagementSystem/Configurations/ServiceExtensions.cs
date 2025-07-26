using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Students.Service;

namespace SchoolManagementSystem.Configurations
{
	public static class ServiceExtensions 
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddDatabase(configuration);
			services.AddAutoMapper(typeof(Program).Assembly);

			services.AddScoped<IStudentRepository, StudentRepository>();
			services.AddScoped<IStudentService, StudentService>();

			return services;
		}
	}
}
