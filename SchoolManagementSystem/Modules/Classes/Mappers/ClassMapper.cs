using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Classes.Entities;
using static SchoolManagementSystem.Modules.Classes.Dtos.ClassDtos;

namespace SchoolManagementSystem.Modules.Classes.Mappers
{
	public class ClassMapper : Profile
	{
		public ClassMapper()
		{
			CreateMap<ClassCreateDto, Class>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
				.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

			CreateMap<Class, ClassDto>()
				.ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(
						src => src.Teacher != null
							? $"{src.Teacher.FirstName} {src.Teacher.LastName}"
							: null
					))
				.ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Enrollments.Count));

			CreateMap<Class, ClassSummaryDto>()
			.ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(
				src => src.Teacher != null
					? $"{src.Teacher.FirstName} {src.Teacher.LastName}"
					: null
			))
			.ForMember(dest => dest.Students, opt => opt.MapFrom(
				src => src.Enrollments.Select(e => e.Student)
			));
		}
	}
}
