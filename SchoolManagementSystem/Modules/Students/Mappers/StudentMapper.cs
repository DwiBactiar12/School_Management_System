using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Students.Entities;
using static SchoolManagementSystem.Modules.Students.Dtos.TeacherDtos;

namespace SchoolManagementSystem.Modules.Students.Mappers
{
	public class StudentMapper : Profile
	{
		public StudentMapper()
		{
			CreateMap<CreateStudentDto, Student>()
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

			CreateMap<UpdateStudentDto, Student>()
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.StudentId, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.Enrollments, opt => opt.Ignore())
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null))
				;

			CreateMap<Student, StudentDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
				.ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count));

			CreateMap<Student, StudentSummaryDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
		}
	}
}
