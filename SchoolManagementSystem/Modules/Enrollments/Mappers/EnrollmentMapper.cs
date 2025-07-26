using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using static SchoolManagementSystem.Modules.Enrollments.Dtos.EnrollmentDtos;

namespace SchoolManagementSystem.Modules.Enrollments.Mappers
{
	public class EnrollmentMapper : Profile
	{
		public EnrollmentMapper()
		{
			CreateMap<Enrollment, EnrollmentDto>()
			.ForMember(dest => dest.StudentName, opt => opt.MapFrom(
				src => string.Join(" ", new[] { src.Student.FirstName, src.Student.LastName }
					.Where(n => !string.IsNullOrWhiteSpace(n)))
			))
			.ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Student.Email))
			.ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
			.ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src => src.Class.ClassCode));

		}
	}
}
