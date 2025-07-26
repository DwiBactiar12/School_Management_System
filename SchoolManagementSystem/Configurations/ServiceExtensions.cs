using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Classes.Service;
using SchoolManagementSystem.Modules.Enrollments.Repositories;
using SchoolManagementSystem.Modules.Enrollments.Service;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Students.Service;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using SchoolManagementSystem.Modules.Teachers.Service;

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

			services.AddScoped<ITeacherRepository, TeacherRepository>();
			services.AddScoped<ITeacherService, TeacherService>();

			services.AddScoped<IClassRepository, ClassRepository>();
			services.AddScoped<ICalssService, ClassService>();

			services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
			services.AddScoped<IEnrollmentService, EnrollmentService>();
			return services;
		}
	}
}
